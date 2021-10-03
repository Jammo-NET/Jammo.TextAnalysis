using System.IO;
using Jammo.ParserTools;

namespace Jammo.TextAnalysis
{
    public class DiagnosticFix
    {
        public IndexSpan OldTextSpan;
        public string NewText;

        public DiagnosticFix(IndexSpan oldTextSpan, string newText)
        {
            OldTextSpan = oldTextSpan;
            NewText = newText;
        }
    }
}