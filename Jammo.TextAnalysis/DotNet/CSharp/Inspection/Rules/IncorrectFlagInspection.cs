using System.Linq;
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

        public override void TestEnumDeclaration(EnumDeclarationSyntax syntax, CSharpAnalysisCompilation context)
        {
            if (!syntax.AttributeLists
                .Any(list => list.Attributes
                    .Any(a => a.Name.ToString() == "Flags")))
                return;

            foreach (var member in syntax.Members)
            {
                var value = member.EqualsValue?.Value.ToString();
                
                if (value == null)
                    continue;

                if (int.TryParse(value, out var intValue) && intValue % 2 != 0)
                    context.CreateDiagnostic(new IncorrectFlagDiagnostic(member, this));
            }
        }
    }

    public class IncorrectFlagDiagnostic : CSharpDiagnostic<EnumMemberDeclarationSyntax>
    {
        public IncorrectFlagDiagnostic(EnumMemberDeclarationSyntax syntax, CSharpInspectionRule rule) : base(syntax, rule) { }

        public override void Fix(CSharpAnalysisCompilation context)
        {
            throw new System.NotImplementedException();
        }
    }
}