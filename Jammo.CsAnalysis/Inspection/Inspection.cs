using Jammo.CsAnalysis.Inspection.Rules;
using Microsoft.CodeAnalysis;

namespace Jammo.CsAnalysis.Inspection
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