using Jammo.ParserTools;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection
{
    public class CSharpDiagnosticFix : DiagnosticFix
    {
        public CSharpDiagnosticFix(IndexSpan oldTextSpan, string newText) : base(oldTextSpan, newText)
        {
            
        }
    }
}