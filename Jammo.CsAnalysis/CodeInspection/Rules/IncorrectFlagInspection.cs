using System;
using System.Linq;
using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.CodeInspection.Rules
{
    public class IncorrectFlagInspection : InspectionRule
    {
        public override InspectionInfo GetInspectionInfo()
        {
            return new InspectionInfo(
                "JAMMO_0002",
                "IncorrectFlagInspection",
                "This flag enumeration has incorrect indices");
        }

        public override void TestEnumDeclaration(EnumDeclarationSyntax syntax, CompilationWrapper context)
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
                    context.CreateInspection(member, this);
            }
        }
    }
}