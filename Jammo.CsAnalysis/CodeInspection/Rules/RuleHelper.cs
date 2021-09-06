using Microsoft.CodeAnalysis;

namespace Jammo.CsAnalysis.CodeInspection.Rules
{
    public static class RuleHelper
    {
        public static Inspection CreateInspection(SyntaxNode node, InspectionRule rule)
        {
            return new Inspection(node.ToString(), IndexSpan.FromTextSpan(node.Span), rule);
        }
    }
}