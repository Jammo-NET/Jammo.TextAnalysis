using Jammo.ParserTools;
using Jammo.TextAnalysis.Xml.Inspection.Rules;

namespace Jammo.TextAnalysis.Xml.Inspection
{
    public class XmlDiagnostic : TextAnalysis.Diagnostic
    {
        public XmlDiagnostic(string rawText, IndexSpan span, XmlInspectionRule rule) : base(rawText, span, rule)
        {
            
        }
    }
}