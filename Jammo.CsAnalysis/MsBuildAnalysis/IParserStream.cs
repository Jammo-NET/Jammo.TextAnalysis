namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public interface IParserStream
    {
        public void Parse();
        public void Write();
        public void WriteTo(string path);
    }
}