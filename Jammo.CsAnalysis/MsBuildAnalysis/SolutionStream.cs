using System;
using System.IO;

namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public sealed class SolutionStream : IDisposable
    {
        private FileStream stream;
        
        public SlnFileData Data;

        public SolutionStream(FileStream stream)
        {
            this.stream = stream;
        }

        public bool TryParse()
        {
            var reader = new StreamReader(stream);
            var data = SlnParser.Parse(reader.ReadToEndAsync().Result);

            switch (data.FormattedVersion.Version)
            {
                case "12.00":
                    
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        public bool TryWrite()
        {
            return true;
        }

        public void Dispose()
        {
            stream?.Dispose();
        }
    }
}