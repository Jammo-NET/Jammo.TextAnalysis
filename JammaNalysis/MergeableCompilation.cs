using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis
{
    public class MergeableCompilation
    {
        public readonly bool Success;

        public CompilationNamespace GlobalNamespace;
// TODO: Make a wrapper for roslyn analysis on files
        public readonly FileInfo Info;

        public MergeableCompilation(CompilationNamespace ns)
        {
            GlobalNamespace = ns;
            Success = true;
        }
        
        public MergeableCompilation(FileInfo file, params FileInfo[] references)
        {
            Info = file;
            Info.Refresh();

            try
            {
                using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(fileStream);
                
                var tree = CSharpSyntaxTree.ParseText(reader.ReadToEndAsync().Result);
                var root = (CompilationUnitSyntax)tree.GetRoot();
                
                var compilation = CSharpCompilation.Create("Compilation")
                    .AddReferences(references.Select(r => MetadataReference.CreateFromFile(r.FullName)))
                    .AddSyntaxTrees(tree);

                GlobalNamespace = ParseNamespace(compilation.GlobalNamespace);
                
                Success = true;
            }
            catch (IOException e)
            {
                Success = false;
            }
        }

        private CompilationNamespace ParseNamespace(INamespaceSymbol namespaceSymbol)
        {
            var compNamespace = new CompilationNamespace(namespaceSymbol.Name);

            foreach (var type in namespaceSymbol.GetTypeMembers())
                compNamespace.Types.Add(ParseType(type, compNamespace));

            foreach (var childNamespace in namespaceSymbol.GetNamespaceMembers())
                compNamespace.Namespaces.Add(ParseNamespace(childNamespace));
            
            return compNamespace;
        }

        private CompilationNamespaceType ParseType(INamedTypeSymbol type, CompilationNamespace compNamespace)
        {
            var compType = new CompilationNamespaceType(type, compNamespace);

            foreach (var member in type.GetMembers())
                compType.Members.Add(ParseMember(member, compType));

            foreach (var nestedType in type.GetTypeMembers())
                compType.Types.Add(ParseType(nestedType, compNamespace));

            return compType;
        }

        private ICompilationTypeMember ParseMember(ISymbol member, CompilationNamespaceType type)
        {
            if (member.Kind == SymbolKind.Method)
                return ParseMethod(member, type);
            
            return new CompilationTypeMember((IFieldSymbol)member, type);
        }

        private CompilationTypeMethod ParseMethod(ISymbol member, CompilationNamespaceType type)
        {
            var method = (IMethodSymbol)member;
            
            var compMethod = new CompilationTypeMethod((IMethodSymbol)member, type);

            return compMethod;
        }

        public MergeableCompilation Merge(params MergeableCompilation[] others)
        {
            if (others.Length == 0)
                return this;
            
            CompilationNamespace compNamespace;

            if (others.All(c => c.GlobalNamespace.Name == GlobalNamespace.Name))
            {
                compNamespace = GlobalNamespace;

                foreach (var comp in others)
                {
                    compNamespace.Types.AddRange(comp.GlobalNamespace.Types);
                    
                    foreach (var ns in others)
                    {
                        if (compNamespace.Namespaces.All(n => !n.TryGetNamespace(ns.GlobalNamespace.Name, out _)))
                            compNamespace.Namespaces.Add(ns.GlobalNamespace);
                    } // TODO: Cover every case
                }
            }
            else
            {
                compNamespace = new CompilationNamespace("Root");
                
                compNamespace.Namespaces.Add(GlobalNamespace);
                
                foreach (var comp in others)
                {
                    compNamespace.Namespaces.Add(comp.GlobalNamespace);
                }
            }

            return new MergeableCompilation(compNamespace);
        }
    }
}