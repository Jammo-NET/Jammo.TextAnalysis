using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection
{
    public class CSharpProjectInspector : Inspector<CSharpInspectionRule, CSharpDiagnostic, CSharpProjectAnalysisCompilation>
    {
        public override void Inspect(CSharpProjectAnalysisCompilation context)
        {
            InternalDiagnostics.Clear();

            var walker = new RuleWalker(InternalRules, context);

            foreach (var document in context.Documents)
            {
                walker.Visit(document.SyntaxTree.GetRoot());
                
                document.AddDiagnostics(walker.Result);
                InternalDiagnostics.AddRange(walker.Result);
                
                walker.ClearResult();
            }

            var rawTree = CSharpSyntaxTree.ParseText(context.RawText);
            walker.Visit(rawTree.GetRoot());
            
            InternalDiagnostics.AddRange(walker.Result);
        }
    }
}