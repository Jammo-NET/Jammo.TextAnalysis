using Jammo.ParserTools;

namespace Jammo.CsAnalysis.Helpers
{
    public abstract class SyntaxMatch
    {
        public readonly string Text;
        public readonly IndexSpan Span;

        public SyntaxMatch(string text, IndexSpan span)
        {
            Text = text;
            Span = span;
        }
    }
    
    public class KeywordSyntax : SyntaxMatch
    {
        public readonly bool IsContextual;

        public KeywordSyntax(string text, IndexSpan span, bool isContextual) : base(text, span)
        {
            IsContextual = isContextual;
        }
    }
    
    public class NumericLiteral : SyntaxMatch
    {
        public readonly bool HasMantissa;

        public NumericLiteral(string text, IndexSpan span, bool hasMantissa) : base(text, span)
        {
            HasMantissa = hasMantissa;
        }
    }

    public class StringLiteral : SyntaxMatch
    {
        public readonly bool IsVerbatim;
        public readonly bool IsFormatted;

        public StringLiteral(string text, IndexSpan span, bool isVerbatim, bool isFormatted) : base(text, span)
        {
            IsVerbatim = isVerbatim;
            IsFormatted = isFormatted;
        }
    }
}