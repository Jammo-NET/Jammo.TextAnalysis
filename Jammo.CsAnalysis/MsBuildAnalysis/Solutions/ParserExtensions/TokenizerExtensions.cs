using System.Text;

namespace Jammo.CsAnalysis.MsBuildAnalysis.Solutions.ParserExtensions
{
    internal static class PrivateTokenizerExtensions
    {
        public static string ReadUntilOrEnd(this SolutionParser.Tokenizer tokenizer, string match)
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