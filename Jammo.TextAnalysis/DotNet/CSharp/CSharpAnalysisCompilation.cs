using System;
using System.Collections.Generic;
using System.Linq;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.TextAnalysis.DotNet.CSharp
{
    public class CSharpAnalysisCompilation : AnalysisCompilation
    {
        public CSharpCompilation Compilation { get; private set; }
        public INamespaceSymbol GlobalNamespace => Compilation?.GlobalNamespace;
        
        public IEnumerable<TNode> FindNodes<TNode>(Func<TNode, bool> predicate)
            where TNode : CSharpSyntaxNode
        {
            foreach (var tree in Compilation.SyntaxTrees)
            {
                foreach (var node in tree.GetRoot().DescendantNodes().OfType<TNode>())
                {
                    if (predicate.Invoke(node))
                        yield return node;
                }
            }
        }

        public void CreateDiagnostic<TSyntax>(CSharpDiagnostic<TSyntax> diagnostic)
            where TSyntax : CSharpSyntaxNode
        {
            InternalInspector.AddInspection(diagnostic);
        }

        public override void GenerateInspections()
        {
            if (Compilation == null)
                GenerateCompilation();
            
            InternalInspector?.Inspect(this);
        }
        
        public override void GenerateCompilation()
        {
            var trees = InternalRawText.Select(t => CSharpSyntaxTree.ParseText(t));

            Compilation = CSharpCompilation.Create($"JAMMO_COMP_{Guid.NewGuid()}", trees.ToArray());
        }
    }
}