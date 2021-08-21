using Microsoft.CodeAnalysis;

namespace JammaNalysis
{
    public class CompilationTypeMember : ICompilationTypeMember
    {
        public string Name { get; set; }
        
        public Accessibility Modifier { get; set; }
        public CompilationNamespaceType ParentType { get; set; }
        
        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsVirtual { get; set; }
        public string Type { get; set; }
        
        public CompilationTypeMember(IFieldSymbol member, CompilationNamespaceType type)
        {
            Name = member.Name;
            Modifier = member.DeclaredAccessibility;
            ParentType = type;
            IsStatic = member.IsStatic;
            IsAbstract = member.IsAbstract;
            IsVirtual = member.IsVirtual;
            Type = member.Type.Name;
        }
    }
}