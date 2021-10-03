using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jammo.TextAnalysis
{
    public abstract class FileAnalysisCompilation : AnalysisCompilation
    {
        protected readonly Dictionary<FileInfo, string> InternalFileText = new();
        public string FileText => string.Join(Environment.NewLine, InternalFileText);

        protected IEnumerable<string> InternalFullRawText => InternalFileText
            .Select(t => t.Value)
            .Concat(new[] { Environment.NewLine })
            .Concat(InternalRawText);

        public virtual void AppendFile(FileInfo file)
        {
            using var stream = file.OpenRead();
            using var reader = new StreamReader(stream);
            
            InternalFileText.Add(file, reader.ReadToEndAsync().Result);
        }
        
        public virtual void AppendFileRange(IEnumerable<FileInfo> fileRange)
        {
            foreach (var file in fileRange)
                AppendFile(file);
        }
    }
}