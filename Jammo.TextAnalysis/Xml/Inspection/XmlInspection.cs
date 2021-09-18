using Jammo.ParserTools;
using Jammo.TextAnalysis.Xml.Inspection.Rules;

namespace Jammo.TextAnalysis.Xml.Inspection
{
    public class XmlInspection : Inspection<XmlInspectionRule>
    {
        public XmlInspection(string rawText, IndexSpan span, XmlInspectionRule rule) : base(rawText, span, rule)
        {
            
        }
    }
}