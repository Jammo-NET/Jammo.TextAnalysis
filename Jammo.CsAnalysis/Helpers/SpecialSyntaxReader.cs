using System.Collections.Generic;
using System.Linq;
using Jammo.ParserTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.Helpers
{
    public class SpecialSyntaxReader
    {
        public readonly IEnumerable<SyntaxMatch> Matches;
        public IEnumerable<KeywordSyntax> Keywords => Matches.OfType<KeywordSyntax>();
        public IEnumerable<StringLiteral> StringLiterals => Matches.OfType<StringLiteral>();
        public IEnumerable<NumericLiteral> NumericLiterals => Matches.OfType<NumericLiteral>();

        public SpecialSyntaxReader(string text)
        {
            var tree = CSharpSyntaxTree.ParseText(text);
            
            var walker = new SpecialWalker();
            walker.Visit(tree.GetRoot());

            Matches = walker.Matches;
        }
        
        private class SpecialWalker : CSharpSyntaxWalker
        {
            private readonly List<SyntaxMatch> matches = new();
            public IEnumerable<SyntaxMatch> Matches => matches;
            
            public SpecialWalker() : base(SyntaxWalkerDepth.Token) { }

            public override void VisitToken(SyntaxToken token)
            {
                if (token.IsKeyword())
                    matches.Add(new KeywordSyntax(
                        token.ValueText,
                        new IndexSpan(token.SpanStart, token.Span.End),
                        token.IsContextualKeyword()));
            }

            private const char FormattedStringKeyword = '$';

            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                if (node.IsKind(SyntaxKind.NumericLiteralExpression))
                    matches.Add(
                        new NumericLiteral(
                            node.ToString(), 
                            new IndexSpan(node.SpanStart, node.Span.End),
                            node.ToString().Contains('.')));    
                else if (node.IsKind(SyntaxKind.StringLiteralExpression))
                    matches.Add(
                        new StringLiteral(
                            node.ToString(), 
                            new IndexSpan(node.SpanStart, node.Span.End),
                            node.Token.IsVerbatimStringLiteral(),
                            node.Token.ToFullString().StartsWith(FormattedStringKeyword)));
            }
        }
    }
}