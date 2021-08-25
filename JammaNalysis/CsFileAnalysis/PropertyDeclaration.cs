using System.Collections.Generic;
using System.Linq;
using JammaNalysis.Compilation;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis.CsFileAnalysis
{
    public class PropertyDeclaration : Statement, IMember
    {
        public string Name { get; set; }
        public List<MemberModifier> Modifiers { get; set; }
        public string[] Attributes { get; set; }

        public PropertyDeclaration(IndexSpan span, PropertyDeclarationSyntax propertyDeclaration) : base(span)
        {
            Modifiers = IMember.GetModifiersFromSyntaxTokens(propertyDeclaration.Modifiers);

            Attributes = propertyDeclaration.AttributeLists
                .Select(list => list.Attributes.Select(a => a.Name.ToString()))
                .SelectMany(i => i)
                .ToArray();
        }
    }
}