using System;
using System.Collections.Generic;
using System.Linq;
using Jammo.TextAnalysis.DotNet.MsBuild;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.TextAnalysis.DotNet.CSharp
{
    public class CSharpSolutionAnalysisCompilation : CSharpAnalysisCompilation
    {
        public readonly List<CSharpProjectAnalysisCompilation> projectCompilations = new();
        public IEnumerable<CSharpProjectAnalysisCompilation> ProjectCompilations => projectCompilations;

        public readonly JSolutionFile SolutionFile;

        public CSharpSolutionAnalysisCompilation(JSolutionFile solutionFile)
        {
            throw new NotImplementedException();
            
            SolutionFile = solutionFile;
        }

        public override void GenerateCompilation()
        {
            Compilation = CSharpCompilation.Create($"JAMMO_COMP_{Guid.NewGuid()}", 
                ProjectCompilations.Select(c => CSharpSyntaxTree.ParseText(c.RawText)));
        }
    }
}