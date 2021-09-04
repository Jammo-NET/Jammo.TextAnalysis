using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.CsFileAnalysis
{
    public class FieldDeclaration : Statement, IMember
    {
        public string Name { get; set; }
        public List<MemberModifier> Modifiers { get; set; }
        
        public string[] Attributes { get; set; }
        
        public FieldDeclaration(IndexSpan span, FieldDeclarationSyntax fieldDeclaration) : base(span)
        {
            Name = fieldDeclaration.Declaration.Variables.First().Identifier.Text;
            Modifiers = IMember.GetModifiersFromSyntaxTokens(fieldDeclaration.Modifiers);
            
            Attributes = fieldDeclaration.AttributeLists
                .Select(list => list.Attributes.Select(a => a.Name.ToString()))
                .SelectMany(i => i)
                .ToArray();
        }
    }
}