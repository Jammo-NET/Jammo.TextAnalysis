using Jammo.ParserTools;

namespace Jammo.TextAnalysis
{
    public abstract class Diagnostic
    {
        protected readonly string InternalRawText;
        
        public readonly IndexSpan Span;
        public readonly DiagnosticInfo Info;

        protected Diagnostic(string rawText, IndexSpan span, InspectionRule rule)
        {
            InternalRawText = rawText;
            Span = span;
            Info = rule.GetInspectionInfo();
        }

        public override string ToString()
        {
            return InternalRawText;
        }
    }
}