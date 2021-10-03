using System;
using System.Collections.Generic;
using System.Linq;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection
{
    public class CSharpInspector : Inspector<CSharpInspectionRule, CSharpDiagnostic, CSharpAnalysisCompilation>
    {
        public override void Inspect(CSharpAnalysisCompilation context)
        {
            InternalDiagnostics.Clear();
            
            var walker = new RuleWalker(InternalRules, context);

            foreach (var tree in context.Trees)
                walker.Visit(tree.GetRoot());
            
            InternalDiagnostics.AddRange(walker.Result);
        }
    }
}