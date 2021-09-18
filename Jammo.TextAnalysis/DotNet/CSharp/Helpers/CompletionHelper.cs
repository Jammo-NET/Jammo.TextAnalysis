using System.Linq;

namespace Jammo.TextAnalysis.DotNet.CSharp.Helpers
{
    public class CompletionHelper
    {
        private static readonly string[] Keywords =
        {
            "abstract", "as", "and",
            "base", "bool", "break", "by", "byte",
            "case", "catch", "char", "checked", "class", "const", "continue",
            "decimal", "default", "delegate", "do", "double", "descending", "dynamic",
            "explicit", "event", "extern", "else", "enum",
            "false", "finally", "fixed", "float", "for", "foreach", "from",
            "goto", "group",
            "if", "implicit", "in", "int", "interface", "internal", "into", "is",
            "lock", "long",
            "new", "null", "namespace",
            "object", "operator", "out", "override", "orderby", "or",
            "params", "private", "protected", "public", 
            "readonly", "ref", "return",
            "switch", "struct", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "select",
            "this", "throw", "true", "try", "typeof",
            "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
            "var", "virtual", "volatile", "void",
            "while", "where",
            "yield",
        };


        public static string[] MatchKeywordsByPartial(string partial)
        {
            return Keywords.Where(k => k.StartsWith(partial)).ToArray();
        }
    }
}