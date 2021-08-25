using System.Collections.Generic;
using System.Linq;
using JammaNalysis.Compilation;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis.CsFileAnalysis
{
    public class MethodDeclaration : BlockStatement, IMember
    {
        public string Name { get; set; }
        public List<MemberModifier> Modifiers { get; set; }
        public string ReturnType;

        public string[] Attributes { get; set; }

        public MethodDeclaration(IndexSpan span, MethodDeclarationSyntax method) : base(span)
        {
            Name = method.Identifier.Text;
            ReturnType = method.ReturnType.ToString();
            Modifiers = IMember.GetModifiersFromSyntaxTokens(method.Modifiers);

            Attributes = method.AttributeLists
                .Select(list => list.Attributes.Select(a => a.Name.ToString()))
                .SelectMany(i => i)
                .ToArray();
        }
    }
}