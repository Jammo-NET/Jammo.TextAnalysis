using Microsoft.CodeAnalysis;

namespace JammaNalysis
{
    public class CompilationTypeMethod : ICompilationTypeMember
    {
        public string Name { get; set; }
        
        public Accessibility Modifier { get; set; }
        public CompilationNamespaceType ParentType { get; set; }

        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsVirtual { get; set; }
        public string Type { get; set; }
        
        public CompilationTypeMethod(IMethodSymbol method, CompilationNamespaceType type)
        {
            Name = method.Name;
            Modifier = method.DeclaredAccessibility;
            ParentType = type;
            IsStatic = method.IsStatic;
            IsAbstract = method.IsAbstract;
            IsVirtual = method.IsVirtual;
            Type = method.ReturnType.Name;
        }
    }
}