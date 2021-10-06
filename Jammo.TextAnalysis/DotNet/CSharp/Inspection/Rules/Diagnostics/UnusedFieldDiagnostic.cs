using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules.Diagnostics
{
    public class UnusedFieldDiagnostic : CSharpDiagnostic
    {
        public UnusedFieldDiagnostic(VariableDeclaratorSyntax syntax, CSharpInspectionRule rule) : base(syntax, rule) { }
        
        public override IEnumerable<CSharpDiagnosticFix> Fix(CSharpAnalysisCompilation context)
        {
            yield break;
        }
    }
}