using Jammo.CsAnalysis.CodeInspection.Rules;

namespace Jammo.CsAnalysis.CodeInspection
{
    public class Inspection
    {
        private readonly string rawText;
        
        public readonly IndexSpan Span;
        public readonly InspectionRule Rule;

        public Inspection(string rawText, IndexSpan span, InspectionRule rule)
        {
            this.rawText = rawText;
            Span = span;
            Rule = rule;
        }

        public override string ToString()
        {
            return rawText;
        }
    }
}