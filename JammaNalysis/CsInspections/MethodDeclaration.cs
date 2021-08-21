using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis.CsInspections
{
    public class MethodDeclaration : BlockStatement, IMember
    {
        public string Name { get; set; }
        public DeclarationAccessibility Accessibility { get; set; }
        public IMember[] Members;

        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsOverride { get; set; }
        
        public bool IsAsync;
        public bool IsUnsafe;
        public bool IsVolatile;

        public string[] Attributes { get; set; }

        public MethodDeclaration(IndexSpan span, MethodDeclarationSyntax method) : base(span)
        {
            Name = method.Identifier.Text;

            foreach (var modifier in method.Modifiers)
            {
                switch (modifier.Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        Accessibility |= DeclarationAccessibility.Public;
                        break;
                    case SyntaxKind.PrivateKeyword:
                        Accessibility |= DeclarationAccessibility.Private;
                        break;
                    case SyntaxKind.ProtectedKeyword:
                        Accessibility |= DeclarationAccessibility.Protected;
                        break;
                    case SyntaxKind.InternalKeyword:
                        Accessibility |= DeclarationAccessibility.Internal;
                        break;
                    case SyntaxKind.StaticKeyword:
                        IsStatic = true;
                        break;
                    case SyntaxKind.AsyncKeyword:
                        IsAsync = true;
                        break;
                    case SyntaxKind.AbstractKeyword:
                        IsAbstract = true;
                        break;
                    case SyntaxKind.VirtualKeyword:
                        IsVirtual = true;
                        break;
                    case SyntaxKind.UnsafeKeyword:
                        IsUnsafe = true;
                        break;
                    case SyntaxKind.VolatileKeyword:
                        IsVolatile = true;
                        break;
                    case SyntaxKind.OverrideKeyword:
                        IsOverride = true;
                        break;
                }
            }

            Attributes = method.AttributeLists
                .Select(list => list.Attributes.Select(a => a.Name.ToString()))
                .SelectMany(i => i)
                .ToArray();
        }
    }
}