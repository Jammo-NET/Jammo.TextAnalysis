using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection
{
    public abstract class CSharpDiagnostic : Diagnostic
    {
        internal readonly CSharpSyntaxNode Syntax;

        internal CSharpDiagnostic(CSharpSyntaxNode syntax, CSharpInspectionRule rule) : 
            base(syntax.ToString(), IndexSpanHelper.FromTextSpan(syntax.Span), rule)
        {
            Syntax = syntax;
        }

        public abstract void Fix(CSharpAnalysisCompilation context);
    }
}