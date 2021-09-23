using Jammo.ParserTools;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection
{
    public abstract class CSharpDiagnostic<TSyntax> : Diagnostic
        where TSyntax : CSharpSyntaxNode
    {
        internal readonly TSyntax Syntax;

        internal CSharpDiagnostic(TSyntax syntax, CSharpInspectionRule rule) : 
            base(syntax.ToString(), IndexSpanHelper.FromTextSpan(syntax.Span), rule)
        {
            Syntax = syntax;
        }

        public abstract void Fix(CSharpAnalysisCompilation context);
    }
}