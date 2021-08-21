using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis.CsInspections
{
    public class NamespaceDeclaration : Statement
    {
        public string NamespaceName;
        public IMember[] Members;
        
        public NamespaceDeclaration(IndexSpan span, NamespaceDeclarationSyntax ns) : base(span)
        {
            NamespaceName = ns.Name.ToString();
            
            var members = new List<IMember>();

            foreach (var member in ns.Members)
            {
                switch (member.Kind())
                {
                    case SyntaxKind.ClassDeclaration:
                        members.Add(
                            new ClassDeclaration(
                                IndexSpan.FromTextSpan(member.Span), 
                                (ClassDeclarationSyntax)member));
                        break;
                    case SyntaxKind.InterfaceDeclaration:
                        members.Add(
                            new InterfaceDeclaration(
                                IndexSpan.FromTextSpan(member.Span), 
                                (InterfaceDeclarationSyntax)member));
                        break;
                    case SyntaxKind.RecordDeclaration:
                        members.Add(
                            new RecordDeclaration(
                                IndexSpan.FromTextSpan(member.Span), 
                                (RecordDeclarationSyntax)member));
                        break;
                    case SyntaxKind.StructDeclaration:
                        members.Add(
                            new StructDeclaration(
                                IndexSpan.FromTextSpan(member.Span), 
                                (StructDeclarationSyntax)member));
                        break;
                    case SyntaxKind.MethodDeclaration:
                        members.Add(
                            new MethodDeclaration(
                                IndexSpan.FromTextSpan(member.Span),
                                (MethodDeclarationSyntax)member));
                        break;
                    case SyntaxKind.FieldDeclaration:
                        break;
                    case SyntaxKind.PropertyDeclaration:
                        break;
                }
            }

            Members = members.ToArray();
        }
    }
}