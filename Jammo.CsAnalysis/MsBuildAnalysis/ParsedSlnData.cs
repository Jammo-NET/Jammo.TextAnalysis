namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public abstract class ParsedSlnData
    {
        public readonly IndexSpan Span;

        protected ParsedSlnData(IndexSpan span)
        {
            Span = span;
        }
        
        public abstract string ToFormattedString();
    }

    public class FormatVersion : ParsedSlnData
    {
        public string Version { get; }
        
        public FormatVersion(string version, IndexSpan span) : base(span)
        {
            Version = version;
        }

        public override string ToFormattedString()
        {
            return $"Microsoft Visual Studio Solution File, Format Version {Version}";
        }
    }
}