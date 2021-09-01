using System;

namespace Jammo.CsAnalysis.CsFileAnalysis
{
    public enum DeclarationModifier
    {
        None = 0,

        Private,
        Internal,
        Protected,
        Public,

        Abstract,
        Virtual,
        Sealed,
        Static,
        Override,
        Readonly,
        Const,
        New,
        Partial,

        Extern,
        Volatile,
        Unsafe,
        Async,
        Ref,

        VisibilityMask = Private | Internal | Protected | Public,
    }
}