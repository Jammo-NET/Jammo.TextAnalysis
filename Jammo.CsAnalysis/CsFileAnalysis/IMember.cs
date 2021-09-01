using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.CsAnalysis.CsFileAnalysis
{
    public interface IMember
    {
        public string Name { get; set; }
        public List<MemberModifier> Modifiers { get; set; }

        internal static List<MemberModifier> GetModifiersFromSyntaxTokens(SyntaxTokenList kinds)
        {
            var modifiers = new List<MemberModifier>();
            
            foreach (var modifier in kinds)
            {
                var memberModifier = modifier.Kind() switch
                {
                    SyntaxKind.PublicKeyword =>
                        new MemberModifier(DeclarationModifier.Public, modifier.Span),
                    
                    SyntaxKind.PrivateKeyword =>
                        new MemberModifier(DeclarationModifier.Private, modifier.Span),
                    
                    SyntaxKind.ProtectedKeyword =>
                        new MemberModifier(DeclarationModifier.Protected, modifier.Span),
                    
                    SyntaxKind.InternalKeyword =>
                        new MemberModifier(DeclarationModifier.Internal, modifier.Span),
                    
                    SyntaxKind.StaticKeyword =>
                        new MemberModifier(DeclarationModifier.Static, modifier.Span),
                    
                    SyntaxKind.AbstractKeyword =>
                        new MemberModifier(DeclarationModifier.Abstract, modifier.Span),
                    
                    SyntaxKind.VirtualKeyword =>
                        new MemberModifier(DeclarationModifier.Virtual, modifier.Span),
                    
                    SyntaxKind.SealedKeyword =>
                        new MemberModifier(DeclarationModifier.Sealed, modifier.Span),
                    
                    SyntaxKind.OverrideKeyword =>
                        new MemberModifier(DeclarationModifier.Override, modifier.Span),
                    
                    SyntaxKind.ReadOnlyKeyword =>
                        new MemberModifier(DeclarationModifier.Readonly, modifier.Span),
                    
                    SyntaxKind.ConstKeyword =>
                        new MemberModifier(DeclarationModifier.Const, modifier.Span),
                    
                    SyntaxKind.NewKeyword =>
                        new MemberModifier(DeclarationModifier.New, modifier.Span),
                    
                    SyntaxKind.PartialKeyword =>
                        new MemberModifier(DeclarationModifier.Partial, modifier.Span),
                    
                    SyntaxKind.ExternKeyword =>
                        new MemberModifier(DeclarationModifier.Extern, modifier.Span),
                    
                    SyntaxKind.VolatileKeyword =>
                        new MemberModifier(DeclarationModifier.Volatile, modifier.Span),
                    
                    SyntaxKind.UnsafeKeyword =>
                        new MemberModifier(DeclarationModifier.Unsafe, modifier.Span),
                    
                    SyntaxKind.AsyncKeyword =>
                        new MemberModifier(DeclarationModifier.Async, modifier.Span),
                    
                    SyntaxKind.ReferenceKeyword =>
                        new MemberModifier(DeclarationModifier.Ref, modifier.Span),
                    
                    _ => null
                };
                
                if (memberModifier != null)
                    modifiers.Add(memberModifier);
            }

            return modifiers;
        }
    }
}