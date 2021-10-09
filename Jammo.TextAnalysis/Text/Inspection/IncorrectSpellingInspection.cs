using System.Collections.Generic;
using Jammo.ParserTools;

namespace Jammo.TextAnalysis.Text.Inspection
{
    public class IncorrectSpellingInspection : TextInspectionRule
    {
        public IncorrectSpellingInspection(TextDictionary dictionary) : base(dictionary)
        {
            
        }
        
        public override DiagnosticInfo GetInspectionInfo()
        {
            return new DiagnosticInfo();
        }

        public override IEnumerable<TextDiagnostic> TestAlphabetical(BasicToken token, TextAnalysisCompilation context)
        {
            yield break;
        }
    }
}