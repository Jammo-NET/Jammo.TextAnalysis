using Jammo.ParserTools;

namespace Jammo.TextAnalysis
{
    public abstract class Inspection<TInspectionRule>
    {
        protected readonly string InternalRawText;
        public readonly IndexSpan Span;
        
        public readonly TInspectionRule Rule;

        protected Inspection(string rawText, IndexSpan span, TInspectionRule rule)
        {
            InternalRawText = rawText;
            Span = span;
            Rule = rule;
        }

        public override string ToString()
        {
            return InternalRawText;
        }
    }
}