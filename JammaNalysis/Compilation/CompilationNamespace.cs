using System.Collections.Generic;
using System.Linq;
using JammaNalysis.CsFileAnalysis;

namespace JammaNalysis.Compilation
{
    public class CompilationNamespace
    {
        public readonly string Name;

        public List<TypeDeclaration> Types;
        public List<CompilationNamespace> Namespaces = new();

        public CompilationNamespace(string name)
        {
            Name = name;
        }
        
        public CompilationNamespace(NamespaceDeclaration ns)
        {
            Name = ns.NamespaceName;
            Types = ns.Types.ToList();
        }

        public bool Contains(CompilationNamespace other)
        {
            return other.Name.StartsWith(Name);
        }

        public CompilationNamespace Merge(params CompilationNamespace[] others)
        {
            var newComp = new CompilationNamespace("Root");
            newComp.Namespaces.Add(this);
            
            foreach (var comp in others)
                newComp.Namespaces.Add(comp);

            return newComp;
        }

        public bool TryGetNamespace(string name, out CompilationNamespace result)
        {
            if (!Name.Contains(name))
            {
                result = null;
                return false;
            }

            CompilationNamespace currentSearch = this;
            
            foreach (var namePoint in name.Substring(0, Name.Length).Split('.'))
            {
                var matched = false;
                
                foreach (var ns in Namespaces)
                {
                    if (ns.Name == currentSearch.Name + "." + namePoint)
                    {
                        currentSearch = ns;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    result = null;
                    return false;
                }
            }

            result = currentSearch;
            return true;
        }

        public IEnumerable<CompilationNamespace> EnumerateTree()
        {
            for (var iteration = 0; iteration < Namespaces.Count; iteration++)
            {
                var comp = Namespaces[iteration];
                yield return comp;

                foreach (var nestedComp in comp.EnumerateTree())
                    yield return nestedComp;
            }
        }
    }
}