using System.Collections.Generic;
using System.Linq;
using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.CsFileAnalysis
{
    public abstract class TypeDeclaration : BlockStatement, IMember
    {
        public string Name { get; set; }
        public List<MemberModifier> Modifiers { get; set; }
        public string[] BaseTypes;
        public IMember[] Members;
        
        public string[] Attributes { get; set; }
        
        protected TypeDeclaration(IndexSpan span, TypeDeclarationSyntax typeDeclaration) : base(span)
        {
            Name = typeDeclaration.Identifier.Text;
            Modifiers = IMember.GetModifiersFromSyntaxTokens(typeDeclaration.Modifiers);

            var members = new List<IMember>();

            foreach (var member in typeDeclaration.Members)
            {
                var memberSpan = (IndexSpan)member.Span;
                
                switch (member.Kind())
                {
                    case SyntaxKind.ClassDeclaration:
                        members.Add(
                            new ClassDeclaration(
                                memberSpan, 
                                (ClassDeclarationSyntax)member));
                        break;
                    case SyntaxKind.InterfaceDeclaration:
                        members.Add(
                            new InterfaceDeclaration(
                                memberSpan, 
                                (InterfaceDeclarationSyntax)member));
                        break;
                    case SyntaxKind.RecordDeclaration:
                        members.Add(
                            new RecordDeclaration(
                                memberSpan, 
                                (RecordDeclarationSyntax)member));
                        break;
                    case SyntaxKind.StructDeclaration:
                        members.Add(
                            new StructDeclaration(
                                memberSpan, 
                                (StructDeclarationSyntax)member));
                        break;
                    case SyntaxKind.MethodDeclaration:
                        members.Add(
                            new MethodDeclaration(
                                memberSpan,
                                (MethodDeclarationSyntax)member));
                        break;
                    case SyntaxKind.FieldDeclaration:
                        members.Add(
                            new FieldDeclaration(
                                memberSpan,
                                (FieldDeclarationSyntax)member));
                        break;
                    case SyntaxKind.PropertyDeclaration:
                        members.Add(
                            new PropertyDeclaration(
                                memberSpan,
                                (PropertyDeclarationSyntax)member));
                        break;
                }
            }

            Members = members.ToArray();

            Attributes = typeDeclaration.AttributeLists
                .Select(list => list.Attributes.Select(a => a.Name.ToString()))
                .SelectMany(i => i)
                .ToArray();
        }
    }
    
    public class ClassDeclaration : TypeDeclaration, IMember
    {
        public ClassDeclaration(IndexSpan span, ClassDeclarationSyntax classDeclaration) 
            : base(span, classDeclaration)
        {
            
        }
    }

    public class InterfaceDeclaration : TypeDeclaration
    {
        public InterfaceDeclaration(IndexSpan span, InterfaceDeclarationSyntax interfaceDeclaration) 
            : base(span, interfaceDeclaration)
        {
            
        }
    }

    public class RecordDeclaration : TypeDeclaration
    {
        public RecordDeclaration(IndexSpan span, RecordDeclarationSyntax recordDeclaration)
            : base(span, recordDeclaration)
        {
            
        }
    }

    public class StructDeclaration : TypeDeclaration
    {
        public StructDeclaration(IndexSpan span, StructDeclarationSyntax structDeclaration) 
            : base(span, structDeclaration)
        {
            
        }
    }
}