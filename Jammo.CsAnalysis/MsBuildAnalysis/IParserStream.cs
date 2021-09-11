using System;

namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public interface IParserStream : IDisposable
    {
        public void Parse();
        public void Write();
        public void WriteTo(string path);
    }
}