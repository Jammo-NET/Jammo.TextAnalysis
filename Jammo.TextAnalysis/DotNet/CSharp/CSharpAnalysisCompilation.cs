using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.TextAnalysis.DotNet.CSharp
{
    public class CSharpAnalysisCompilation : FileAnalysisCompilation
    {
        public IEnumerable<CSharpSyntaxTree> Trees => 
            InternalFullRawText.Select(t => (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(t));

        public CSharpCompilation Compilation { get; protected set; }
        public INamespaceSymbol GlobalNamespace => Compilation?.GlobalNamespace;
        
        public virtual IEnumerable<TNode> FindNodes<TNode>(Func<TNode, bool> predicate)
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

        public override void GenerateCompilation()
        {
            Compilation = CSharpCompilation.Create($"JAMMO_COMP_{Guid.NewGuid()}", Trees);
        }
    }
}