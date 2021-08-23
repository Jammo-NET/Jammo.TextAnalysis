using System;
using System.Collections.Generic;
using JammaNalysis.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis.CsFileAnalysis
{
    public class NamespaceDeclaration : Statement
    {
        public string NamespaceName;
        public TypeDeclaration[] Types;
        
        public NamespaceDeclaration(IndexSpan span) : base(span)
        {
            
        }

        public NamespaceDeclaration(IndexSpan span, NamespaceDeclarationSyntax ns) : base(span)
        {
            var nsMembers = ns?.Members ?? new SyntaxList<MemberDeclarationSyntax>();
            NamespaceName = ns?.Name.ToString();
            
            var members = new List<TypeDeclaration>();

            foreach (var member in nsMembers)
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
                }
            }

            Types = members.ToArray();
        }
    }
}