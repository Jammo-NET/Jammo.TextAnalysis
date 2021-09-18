using System;
using System.Linq;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.TextAnalysis.DotNet.CSharp
{
    public class CSharpAnalysisCompilation : AnalysisCompilation<CSharpInspection, CSharpInspectionRule, CSharpAnalysisCompilation>
    {
        public CSharpCompilation Compilation { get; private set; }
        public INamespaceSymbol GlobalNamespace => Compilation?.GlobalNamespace;

        public void CreateInspection(SyntaxNode node, CSharpInspectionRule rule)
        {
            CreateInspection(
                new CSharpInspection(node.ToString(), IndexSpanHelper.FromTextSpan(node.Span), rule));
        }

        public override void CreateInspection(CSharpInspection cSharpInspection)
        {
            InternalInspector.AddInspection(cSharpInspection);
        }

        public override void GenerateInspections()
        {
            if (Compilation == null)
                GenerateCompilation();
            
            InternalInspector?.Inspect(this);
        }
        
        public override void GenerateCompilation()
        {
            var trees = InternalRawText.Select(t => CSharpSyntaxTree.ParseText(t));

            Compilation = CSharpCompilation.Create($"JAMMO_COMP_{Guid.NewGuid()}", trees.ToArray());
        }
    }
}