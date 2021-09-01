using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.CsFileAnalysis
{
    public abstract class Statement
    {
        public readonly IndexSpan Span;

        protected Statement(IndexSpan span)
        {
            Span = span;
        }
    }

    public class UsingStatement : Statement
    {
        public string NamespaceName;
        public string DeclarationName;
        
        public bool IsDeclaration;
        public bool IsStaticDeclaration;

        public UsingStatement(IndexSpan span, UsingDirectiveSyntax statement) : base(span)
        {
            NamespaceName = statement.Name.ToString();
            DeclarationName = statement.Alias?.Name.ToString();
            IsDeclaration = statement.Alias != null;
            IsStaticDeclaration = statement.StaticKeyword.Span.Length > 0;
        }
    }

    public class ForLoop : BlockStatement
    {
        public ForLoop(IndexSpan span) : base(span)
        {
            
        }
    }

    public class ForeachLoop : BlockStatement
    {
        public ForeachLoop(IndexSpan span) : base(span)
        {
            
        }
    }

    public class WhileLoop : BlockStatement
    {
        public WhileLoop(IndexSpan span) : base(span)
        {
            
        }
    }

    public class DoWhileLoop : BlockStatement
    {
        public DoWhileLoop(IndexSpan span) : base(span)
        {
            
        }
    }
}