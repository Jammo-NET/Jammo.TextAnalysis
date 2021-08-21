using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace JammaNalysis
{
    public class CompilationNamespaceType
    {
        public readonly string Name;
        
        public readonly Accessibility Modifier;
        public readonly CompilationNamespace Namespace;

        public readonly List<CompilationNamespaceType> Types = new();
        public readonly List<ICompilationTypeMember> Members = new();
        
        public bool Static;
        public bool Abstract;
        public bool Sealed;

        public CompilationNamespaceType(INamedTypeSymbol type, CompilationNamespace ns)
        {
            Name = type.Name;
            Modifier = type.DeclaredAccessibility;
            Static = type.IsStatic;
            Abstract = type.IsAbstract;
            Sealed = type.IsSealed;
            Namespace = ns;
        }
    }
}