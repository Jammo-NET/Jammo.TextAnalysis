using System;
using System.Linq;
using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.CodeInspection.Rules
{
    public class UnusedFieldInspection : InspectionRule
    {
        public override InspectionInfo GetInspectionInfo()
        {
            return new InspectionInfo(
                "JAMMO_0001",
                "UnusedFieldInspection", 
                "Field is never used within the target compilation.");
        }

        public override Inspection TestFieldDeclaration(FieldDeclarationSyntax syntax, CompilationWrapper context)
        {
            foreach (var tree in context.Compilation.SyntaxTrees)
            {
                var root = tree.GetRoot();

                foreach (var node in root.DescendantNodes().OfType<IdentifierNameSyntax>())
                {
                    if (node.Identifier.Text == syntax.Declaration.Variables.First().ToString())
                        return null;
                }
            }

            return RuleHelper.CreateInspection(syntax, this);
        }
    }
}