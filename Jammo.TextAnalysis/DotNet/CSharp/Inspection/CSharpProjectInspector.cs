using System;
using System.Collections.Generic;
using System.Linq;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                
                document.Diagnostics.AddRange(walker.Result);
                InternalDiagnostics.AddRange(walker.Result);
                
                walker.ClearResult();
            }

            var rawTree = CSharpSyntaxTree.ParseText(context.RawText);
            walker.Visit(rawTree.GetRoot());
            
            InternalDiagnostics.AddRange(walker.Result);
        }
    }
}