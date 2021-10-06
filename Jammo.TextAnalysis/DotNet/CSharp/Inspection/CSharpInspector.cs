using System.Collections.Generic;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection
{
    public class CSharpInspector : Inspector<CSharpInspectionRule, CSharpDiagnostic, CSharpAnalysisCompilation>
    {
        private readonly List<Microsoft.CodeAnalysis.Diagnostic> compilationDiagnostics = new();
        public IEnumerable<Microsoft.CodeAnalysis.Diagnostic> CompilationDiagnostics => compilationDiagnostics;

        public override void Inspect(CSharpAnalysisCompilation context)
        {
            InternalDiagnostics.Clear();
            
            var walker = new RuleWalker(InternalRules, context);

            foreach (var tree in context.Trees)
            {
                var root = tree.GetCompilationUnitRoot();
                compilationDiagnostics.AddRange(root.GetDiagnostics());
                
                walker.Visit(root);
            }

            InternalDiagnostics.AddRange(walker.Result);
        }
    }
}