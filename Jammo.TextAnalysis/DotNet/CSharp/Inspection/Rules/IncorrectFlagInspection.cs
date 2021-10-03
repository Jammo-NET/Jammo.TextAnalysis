using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

    public class IncorrectFlagDiagnostic : CSharpDiagnostic
    {
        public IncorrectFlagDiagnostic(EnumDeclarationSyntax syntax, CSharpInspectionRule rule) : base(syntax, rule) { }

        public override IEnumerable<CSharpDiagnosticFix> Fix(CSharpAnalysisCompilation context)
        {
            var syntax = (EnumDeclarationSyntax)Syntax;
            var power = 0;

            var members = new SeparatedSyntaxList<EnumMemberDeclarationSyntax>();
            
            foreach (var member in syntax.Members)
            {
                members.Add(member.WithEqualsValue(
                        SyntaxFactory.EqualsValueClause(SyntaxFactory.ParseExpression($"1<<{power++}"))));
            }

            var newSyntax = syntax.WithMembers(members);

            yield return new CSharpDiagnosticFix(IndexSpanHelper.FromTextSpan(syntax.Span), newSyntax.ToFullString());
        }
    }
}