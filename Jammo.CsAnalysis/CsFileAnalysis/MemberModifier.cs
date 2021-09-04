namespace Jammo.CsAnalysis.CsFileAnalysis
{
    public class MemberModifier
    {
        public readonly DeclarationModifier Modifier;
        public readonly IndexSpan Span;

        public MemberModifier(DeclarationModifier modifier, IndexSpan span)
        {
            Modifier = modifier;
            Span = span;
        }
    }
}