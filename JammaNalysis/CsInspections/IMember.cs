namespace JammaNalysis.CsInspections
{
    public interface IMember
    {
        public string Name { get; set; }
        
        public DeclarationAccessibility Accessibility { get; set; }
        
        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsOverride { get; set; }
        
        public string[] Attributes { get; set; }
    }
}