using System.Collections.Generic;
using Jammo.ParserTools;

namespace Jammo.TextAnalysis.Text
{
    public abstract class TextInspectionRule : InspectionRule
    {
        private readonly TextDictionary dictionary;

        protected TextInspectionRule(TextDictionary dictionary)
        {
            this.dictionary = dictionary;
        }

        public virtual IEnumerable<TextDiagnostic> TestAlphabetical(BasicToken token, TextAnalysisCompilation context) => null;
        public virtual IEnumerable<TextDiagnostic> TestNumerical(BasicToken token, TextAnalysisCompilation context) => null;
        public virtual IEnumerable<TextDiagnostic> TestNewline(BasicToken token, TextAnalysisCompilation context) => null;
        public virtual IEnumerable<TextDiagnostic> TestWhitespace(BasicToken token, TextAnalysisCompilation context) => null;
        public virtual IEnumerable<TextDiagnostic> TestSymbol(BasicToken token, TextAnalysisCompilation context) => null;
        public virtual IEnumerable<TextDiagnostic> TestPunctuation(BasicToken token, TextAnalysisCompilation context) => null;
        public virtual IEnumerable<TextDiagnostic> TestUnhandled(BasicToken token, TextAnalysisCompilation context) => null;
    }
}