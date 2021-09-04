using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.CsFileAnalysis
{
    public class NamespaceDeclaration : Statement, IMember
    {
        public string Name { get; set; } = "";
        public List<MemberModifier> Modifiers { get; set; }
        public IMember[] Members;
        
        public NamespaceDeclaration(IndexSpan span) : base(span)
        {
            
        }

        public NamespaceDeclaration(IndexSpan span, NamespaceDeclarationSyntax ns) : base(span)
        {
            var nsMembers = ns?.Members ?? new SyntaxList<MemberDeclarationSyntax>();
            Name = ns.Name.ToString();
            
            var members = new List<IMember>();

            foreach (var member in nsMembers)
            {
                switch (member.Kind())
                {
                    case SyntaxKind.NamespaceDeclaration:
                    {
                        members.Add(
                            new NamespaceDeclaration(member.Span, (NamespaceDeclarationSyntax)member));
                        break;
                    }
                    case SyntaxKind.ClassDeclaration:
                        members.Add(
                            new ClassDeclaration(member.Span, (ClassDeclarationSyntax)member));
                        break;
                    case SyntaxKind.InterfaceDeclaration:
                        members.Add(
                            new InterfaceDeclaration(member.Span, (InterfaceDeclarationSyntax)member));
                        break;
                    case SyntaxKind.RecordDeclaration:
                        members.Add(
                            new RecordDeclaration(member.Span, (RecordDeclarationSyntax)member));
                        break;
                    case SyntaxKind.StructDeclaration:
                        members.Add(
                            new StructDeclaration(member.Span, (StructDeclarationSyntax)member));
                        break;
                }
            }

            Members = members.ToArray();
        }
    }
}