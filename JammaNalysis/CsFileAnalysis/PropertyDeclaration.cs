using System.Linq;
using JammaNalysis.Compilation;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis.CsFileAnalysis
{
    public class PropertyDeclaration : Statement, IMember
    {
        public string Name { get; set; }
        
        public DeclarationAccessibility Accessibility { get; set; }
        
        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsOverride { get; set; }
        
        public string[] Attributes { get; set; }

        public PropertyDeclaration(IndexSpan span, PropertyDeclarationSyntax propertyDeclaration) : base(span)
        {
            foreach (var modifier in propertyDeclaration.Modifiers)
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
                    case SyntaxKind.AbstractKeyword:
                        IsAbstract = true;
                        break;
                    case SyntaxKind.VirtualKeyword:
                        IsVirtual = true;
                        break;
                    case SyntaxKind.OverrideKeyword:
                        IsOverride = true;
                        break;
                }
            }
            
            Attributes = propertyDeclaration.AttributeLists
                .Select(list => list.Attributes.Select(a => a.Name.ToString()))
                .SelectMany(i => i)
                .ToArray();
        }
    }
}