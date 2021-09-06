using System;
using System.Collections.Generic;
using System.Linq;
using Jammo.CsAnalysis.CodeInspection.Rules;
using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.CodeInspection
{
    public class CodeInspector
    {
        private readonly List<InspectionRule> rules = new();
        private readonly List<Inspection> inspections = new();

        public IEnumerable<Inspection> Inspections => inspections;

        public void Inspect(string text, CompilationWrapper context)
        {
            var tree = CSharpSyntaxTree.ParseText(text);
            var root = tree.GetRoot();
            var walker = new RuleWalker(rules, context);

            walker.Visit(root);

            inspections.AddRange(walker.Result);
        }

        public void WithRules(params IEnumerable<InspectionRule>[] sets)
        {
            foreach (var ruleSet in sets)
                rules.AddRange(ruleSet);
        }

        private class RuleWalker : CSharpSyntaxWalker
        {
            private readonly CompilationWrapper context;
            
            private readonly IEnumerable<InspectionRule> rules;
            private readonly List<Inspection> result = new();
            
            public IEnumerable<Inspection> Result => result;

            public RuleWalker(IEnumerable<InspectionRule> rules, CompilationWrapper context) : base(SyntaxWalkerDepth.Trivia)
            {
                this.rules = rules;
                this.context = context;
            }

            public override void VisitUsingDirective(UsingDirectiveSyntax node)
            {
                AddInspections(r => r.TestUsingDirective(node, context));
                
                base.VisitUsingDirective(node);
            }

            public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
            {
                AddInspections(r => r.TestNamespaceDeclaration(node, context));
                
                base.VisitNamespaceDeclaration(node);
            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                AddInspections(r => r.TestClassDeclaration(node, context));
                
                base.VisitClassDeclaration(node);
            }

            public override void VisitStructDeclaration(StructDeclarationSyntax node)
            {
                AddInspections(r => r.TestStructDeclaration(node, context));
                
                base.VisitStructDeclaration(node);
            }

            public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
            {
                AddInspections(r => r.TestInterfaceDeclaration(node, context));
                
                base.VisitInterfaceDeclaration(node);
            }

            public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
            {
                AddInspections(r => r.TestRecordDeclaration(node, context));
                
                base.VisitRecordDeclaration(node);
            }

            public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
            {
                AddInspections(r => r.TestFieldDeclaration(node, context));
                
                base.VisitFieldDeclaration(node);
            }

            public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
            {
                AddInspections(r => r.TestPropertyDeclaration(node, context));
                
                base.VisitPropertyDeclaration(node);
            }

            public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                AddInspections(r => r.TestMethodDeclaration(node, context));
                
                base.VisitMethodDeclaration(node);
            }

            public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
            {
                AddInspections(r => r.TestVariableAssignment(node, context));
                
                base.VisitVariableDeclaration(node);
            }

            public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
            {
                AddInspections(r => r.TestVariableDeclaration(node, context));
                
                base.VisitVariableDeclarator(node);
            }

            public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
            {
                AddInspections(r => r.TestMemberAccess(node, context));
                
                base.VisitMemberAccessExpression(node);
            }

            public override void VisitExpressionStatement(ExpressionStatementSyntax node)
            {
                Func<InspectionRule, Inspection> flag;

                switch (node.Kind())
                {
                    case SyntaxKind.EqualsExpression:
                        flag = r => r.TestValueEquals(node, context);
                        break;
                    case SyntaxKind.NotEqualsExpression:
                        flag = r => r.TestValueNotEqual(node, context);
                        break;
                    case SyntaxKind.ExclamationToken:
                        flag = r => r.TestNot(node, context);
                        break;
                    case SyntaxKind.LessThanExpression:
                        flag = r => r.TestLessThan(node, context);
                        break;
                    case SyntaxKind.GreaterThanExpression:
                        flag = r => r.TestGreaterThan(node, context);
                        break;
                    case SyntaxKind.LessThanOrEqualExpression:
                        flag = r => r.TestLessThanOrEqual(node, context);
                        break;
                    case SyntaxKind.GreaterThanOrEqualExpression:
                        flag = r => r.TestMoreThanOrEqual(node, context);
                        break;
                    default:
                        return;
                }
                
                AddInspections(flag);
                
                base.VisitExpressionStatement(node);
            }

            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                Func<InspectionRule, Inspection> flag;

                switch (node.Kind())
                {
                    case SyntaxKind.StringLiteralExpression:
                        flag = r => r.TestStringLiteral(node, context);
                        break;
                    case SyntaxKind.NumericLiteralExpression:
                        flag = r => r.TestNumericLiteral(node, context);
                        break;
                    default:
                        return;
                }
                
                AddInspections(flag);
                
                base.VisitLiteralExpression(node);
            }

            public override void VisitTrivia(SyntaxTrivia trivia)
            {
                Func<InspectionRule, Inspection> flag;

                switch (trivia.Kind())
                {
                    case SyntaxKind.SingleLineCommentTrivia:
                        flag = r => r.TestSingleLineComment(trivia, context);
                        break;
                    case SyntaxKind.MultiLineCommentTrivia:
                        flag = r => r.TestMultiLineComment(trivia, context);
                        break;
                    case SyntaxKind.SingleLineDocumentationCommentTrivia:
                        flag = r => r.TestDocumentationComment(trivia, context);
                        break;
                    case SyntaxKind.MultiLineDocumentationCommentTrivia:
                        flag = r => r.TestDocumentationComment(trivia, context);
                        break;
                    default:
                        return;
                }
                
                AddInspections(flag);
                
                base.VisitTrivia(trivia);
            }
            
            private void AddInspections(Func<InspectionRule, Inspection> factory)
            {
                result.AddRange(rules.Select(factory).Where(r => r != null));
            }
        }
    }
}