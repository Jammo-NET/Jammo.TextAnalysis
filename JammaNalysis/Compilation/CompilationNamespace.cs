using System;
using System.Collections.Generic;
using System.Linq;
using JammaNalysis.CsFileAnalysis;

namespace JammaNalysis.Compilation
{
    public class CompilationNamespace
    {
        public readonly string Name;

        public List<IMember> Members;
        public List<CompilationNamespace> Namespaces = new();

        public CompilationNamespace(string name)
        {
            Name = name;
            Members = new List<IMember>();
        }
        
        public CompilationNamespace(NamespaceDeclaration ns)
        {
            Name = ns.Name;
            Members = ns.Members.ToList();
        }

        public bool Contains(CompilationNamespace other)
        {
            return other.Name.StartsWith(Name);
        }

        public CompilationNamespace Merge(params CompilationNamespace[] others)
        {
            const string fallbackRootName = "Root";
            var root = this;

            foreach (var ns in others)
            {
                if (root.Name == ns.Name)
                {
                    var found = false;
                    
                    foreach (var nestedNs in root.Namespaces)
                    {
                        if (ns == nestedNs)
                            continue;
                        
                        if (ns.Name == nestedNs.Name)
                        {
                            nestedNs.Namespaces.Add(ns);
                            found = true;
                            break;
                        }
                    }
                    
                    if (!found)
                        root.Members.AddRange(ns.Members);
                }
                else if (root.Contains(ns))
                {
                    var found = false;

                    foreach (var nestedNs in root.Namespaces)
                    {
                        if (ns.Contains(nestedNs))
                        {
                            nestedNs.Namespaces.Add(ns);
                            found = true;
                            break;
                        }
                    }
                    
                    if (!found)
                        root.Namespaces.Add(ns);
                }
                else
                {
                    if (root.Name == fallbackRootName)
                        root.Namespaces.Add(ns);
                    else
                        root = new CompilationNamespace(fallbackRootName) { Namespaces = { root } };
                }
            }
            
            return root;
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