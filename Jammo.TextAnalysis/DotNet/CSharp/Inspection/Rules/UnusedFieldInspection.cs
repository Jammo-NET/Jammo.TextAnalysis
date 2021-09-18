using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules
{
    public class UnusedFieldInspection : CSharpInspectionRule
    {
        private readonly List<VariableDeclaratorSyntax> fields = new();
        
        public override InspectionInfo GetInspectionInfo()
        {
            return new InspectionInfo(
                "JAMMO_0001",
                "UnusedFieldInspection", 
                "Field is never used within the target compilation.");
        }

        public override void TestFieldDeclaration(FieldDeclarationSyntax syntax, CSharpAnalysisCompilation context)
        {
            var matchedVars = new Dictionary<VariableDeclaratorSyntax, bool>();

            foreach (var variable in syntax.Declaration.Variables)
                matchedVars[variable] = false;
            
            foreach (var tree in context.Compilation.SyntaxTrees)
            {
                foreach (var node in tree.GetRoot().DescendantNodes().OfType<IdentifierNameSyntax>())
                {
                    foreach (var variable in matchedVars)
                    {
                        if (matchedVars.All(k => k.Value))
                            return;
                        
                        if (variable.ToString() == node.Identifier.Text)
                            matchedVars[variable.Key] = true;
                    }
                }
            }

            foreach (var variable in matchedVars
                .Where(v => v.Value == false))
            {
                context.CreateInspection(variable.Key, this);
                fields.Add(variable.Key);
            }
        }
    }
}