using System.Text;
using Jammo.ParserTools;

namespace Jammo.CsAnalysis.MsBuildAnalysis.Solutions.ParserExtensions
{
    internal static class PrivateTokenizerExtensions
    {
        public static string ReadUntilOrEnd(this Tokenizer tokenizer, string match)
        {
            var result = new StringBuilder();
            
            foreach (var token in tokenizer)
            {
                if (token.Text == match)
                    return result.ToString();

                result.Append(token.Text);
            }

            return result.ToString();
        }
    }
}