using Jammo.ParserTools;
using Microsoft.CodeAnalysis.Text;

namespace Jammo.CsAnalysis
{
    public static class IndexSpanHelper
    {
        public static IndexSpan FromTextSpan(TextSpan textSpan)
        {
            return new IndexSpan(textSpan.Start, textSpan.End);
        }
    }
}