using System;
using System.Collections.Generic;
using System.IO;

namespace Jammo.TextAnalysis
{
    public abstract class AnalysisCompilation
    {
        protected readonly List<string> InternalRawText = new();
        
        public string RawText => string.Join(Environment.NewLine, InternalRawText);
        
        public void AppendFile(FileInfo file)
        {
            using var stream = file.OpenRead();
            using var reader = new StreamReader(stream);
            
            InternalRawText.Add(reader.ReadToEndAsync().Result);
        }
        
        public void AppendFileRange(IEnumerable<FileInfo> fileRange)
        {
            foreach (var file in fileRange)
                AppendFile(file);
        }

        public void AppendText(string text)
        {
            InternalRawText.Add(text);
        }

        public void AppendTextRange(IEnumerable<string> strings)
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