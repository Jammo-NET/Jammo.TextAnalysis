using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules.Diagnostics
{
    public class IncorrectFlagDiagnostic : CSharpDiagnostic
    {
        public IncorrectFlagDiagnostic(EnumDeclarationSyntax syntax, CSharpInspectionRule rule) : base(syntax, rule) { }

        public override IEnumerable<DiagnosticFix> Fix(CSharpAnalysisCompilation context)
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

            yield return new DiagnosticFix(IndexSpanHelper.FromTextSpan(syntax.Span), newSyntax.ToFullString());
        }
    }
}