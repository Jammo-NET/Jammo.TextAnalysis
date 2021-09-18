using System.Collections.Generic;
using System.IO;

namespace Jammo.TextAnalysis
{
    public abstract class AnalysisCompilationBase { }
    
    public abstract class AnalysisCompilation<TInspection, TInspectionRule, TAnalysisCompilation> : AnalysisCompilationBase
        where TInspection : Inspection<TInspectionRule>
        where TInspectionRule : InspectionRule
        where TAnalysisCompilation : AnalysisCompilationBase
    {
        protected readonly List<string> InternalRawText = new();
        protected Inspector<TInspection, TInspectionRule, TAnalysisCompilation>
            InternalInspector;

        public IEnumerable<TInspection> Inspections => InternalInspector?.Inspections;
        
        public string RawText => string.Join('\n', InternalRawText);
        
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

        public void SetInspector(Inspector<TInspection, TInspectionRule, TAnalysisCompilation> 
            newInspector)
        {
            InternalInspector = newInspector;
        }

        public abstract void CreateInspection(TInspection inspection);

        public abstract void GenerateInspections();

        public abstract void GenerateCompilation();
    }
}