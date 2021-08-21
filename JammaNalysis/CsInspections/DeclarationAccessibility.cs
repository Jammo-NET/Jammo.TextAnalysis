using System;

namespace JammaNalysis.CsInspections
{
    [Flags]
    public enum DeclarationAccessibility
    {
        None = 1<<0,
        Public = 1<<1,
        Private = 1<<2,
        Protected = 1<<3,
        Internal = 1<<4
    }
}