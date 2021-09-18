using Jammo.ParserTools;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection
{
    public class CSharpInspection : TextAnalysis.Inspection<CSharpInspectionRule>
    {
        public CSharpInspection(string rawText, IndexSpan span, CSharpInspectionRule rule) : base(rawText, span, rule)
        {
            
        }
    }
}