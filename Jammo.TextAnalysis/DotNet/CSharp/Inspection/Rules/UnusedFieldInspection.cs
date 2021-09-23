using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules
{
    public class UnusedFieldInspection : CSharpInspectionRule
    {
        public override DiagnosticInfo GetInspectionInfo()
        {
            return new DiagnosticInfo(
                "JAMMO_0001",
                "UnusedFieldInspection", 
                "Field is never used within the target compilation.");
        }

        public override void TestFieldDeclaration(FieldDeclarationSyntax syntax, CSharpAnalysisCompilation context)
        {
            foreach (var variable in syntax.Declaration.Variables)
            {
                var nodes = context
                    .FindNodes<IdentifierNameSyntax>(n => variable.ToString() == n.Identifier.ToString());

                if (!nodes.Any())
                {
                    context.CreateDiagnostic(new UnusedFieldDiagnostic(variable, this));
                }
            }
        }
    }
    
    public class UnusedFieldDiagnostic : CSharpDiagnostic<VariableDeclaratorSyntax>
    {
        public UnusedFieldDiagnostic(VariableDeclaratorSyntax syntax, CSharpInspectionRule rule) : base(syntax, rule) { }
        
        public override void Fix(CSharpAnalysisCompilation context)
        {
            
        }
    }
}