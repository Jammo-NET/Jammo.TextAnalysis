using System.Collections.Generic;
using System.Linq;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules
{
    public class IncorrectFlagInspection : CSharpInspectionRule
    {
        public override DiagnosticInfo GetInspectionInfo()
        {
            return new DiagnosticInfo(
                "JAMMO_0002",
                "IncorrectFlagInspection",
                "This flag enumeration has incorrect indices");
        }

        public override IEnumerable<CSharpDiagnostic> TestEnumDeclaration(EnumDeclarationSyntax syntax, CSharpAnalysisCompilation context)
        {
            if (!syntax.AttributeLists
                .Any(list => list.Attributes
                    .Any(a => a.Name.ToString() == "Flags")))
                yield break;

            foreach (var member in syntax.Members)
            {
                var value = member.EqualsValue?.Value.ToString();
                
                if (value == null)
                    continue;
                
                if (int.TryParse(value, out var intValue) && intValue % 2 != 0)
                {
                    yield return new IncorrectFlagDiagnostic(syntax, this);
                    break;
                }
            }
        }
    }
}