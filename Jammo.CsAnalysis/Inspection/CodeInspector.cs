using System.Collections.Generic;
using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.CsAnalysis.Inspection
{
    public class CodeInspector
    {
        public IEnumerable<Inspection> Inspections { get; }

        private CodeInspector(IEnumerable<Inspection> inspections)
        {
            Inspections = inspections;
        }

        public static CodeInspector Inspect(string text, CompilationWrapper context)
        {
            var tree = CSharpSyntaxTree.ParseText(text);
            var root = tree.GetRoot();
            var inspections = new List<Inspection>();

            foreach (var direct in root.ChildNodes())
            {
                
            }

            return new CodeInspector(inspections);
        }
    }
}