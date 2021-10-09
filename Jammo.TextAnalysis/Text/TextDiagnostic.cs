using Jammo.ParserTools;

namespace Jammo.TextAnalysis.Text
{
    public class TextDiagnostic : Diagnostic
    {
        public TextDiagnostic(string rawText, IndexSpan span, InspectionRule rule) : base(rawText, span, rule)
        {
            
        }
    }
}