using System;
using System.Collections.Generic;
using System.IO;

namespace Jammo.TextAnalysis
{
    public abstract class AnalysisCompilation
    {
        protected readonly List<string> InternalRawText = new();
        
        public string RawText => string.Join(Environment.NewLine, InternalRawText);

        public virtual void AppendText(string text)
        {
            InternalRawText.Add(text);
        }

        public virtual void AppendTextRange(IEnumerable<string> strings)
        {
            foreach (var raw in strings)
                AppendText(raw);
        }

        public void ClearRaws()
        {
            InternalRawText.Clear();
        }

        public abstract void GenerateCompilation();
    }
}