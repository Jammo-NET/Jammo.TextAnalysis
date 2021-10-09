using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Jammo.ParserTools;
using Jammo.TextAnalysis.Text.Inspection;

namespace Jammo.TextAnalysis.Text
{
    public class TextInspector : Inspector<TextInspectionRule, TextDiagnostic, TextAnalysisCompilation>
    {
        private readonly List<TextDiagnostic> diagnostics = new List<TextDiagnostic>();
        
        public readonly TextDictionary Dictionary;

        public TextInspector(TextDictionary dictionary)
        {
            Dictionary = dictionary;
        }

        public override void Inspect(TextAnalysisCompilation context)
        {
            diagnostics.Clear();
            
            foreach (var token in Tokenizer.Tokenize(context.Text))
            {
                switch (token.Type)
                {
                    case BasicTokenType.Alphabetical:
                        InvokeRule(r => r.TestAlphabetical(token, context));
                        break;
                    case BasicTokenType.Numerical:
                        InvokeRule(r => r.TestNumerical(token, context));
                        break;
                    case BasicTokenType.Symbol:
                        InvokeRule(r => r.TestSymbol(token, context));
                        break;
                    case BasicTokenType.Punctuation:
                        InvokeRule(r => r.TestPunctuation(token, context));
                        break;
                    case BasicTokenType.Whitespace:
                        InvokeRule(r => r.TestWhitespace(token, context));
                        break;
                    case BasicTokenType.Newline:
                        InvokeRule(r => r.TestNewline(token, context));
                        break;
                    case BasicTokenType.Unhandled:
                        InvokeRule(r => r.TestUnhandled(token, context));
                        break;
                }
            }
        }

        private void InvokeRule(Func<TextInspectionRule, IEnumerable<TextDiagnostic>> factory)
        {
            foreach (var rule in InternalRules)
                diagnostics.AddRange(factory.Invoke(rule) ?? Enumerable.Empty<TextDiagnostic>());
        }
    }
    
    public enum TextDictionary
    {
            
    }
}