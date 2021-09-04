namespace Jammo.CsAnalysis.CsFileAnalysis
{
    public class Block
    {
        public IndexSpan Span;
        
        public Statement[] Statements;
        public int ScopeLevel;
    }
    
    public abstract class BlockStatement : Statement
    {
        public Block Block;

        protected BlockStatement(IndexSpan span) : base(span)
        {
            
        }
    }
}