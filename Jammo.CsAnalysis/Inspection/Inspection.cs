using Jammo.CsAnalysis.Inspection.Rules;

namespace Jammo.CsAnalysis.Inspection
{
    public class Inspection
    {
        private string rawText;
        
        public IndexSpan Span;
        public InspectionRule Rule;
        
        public override string ToString()
        {
            return rawText;
        }
    }
}