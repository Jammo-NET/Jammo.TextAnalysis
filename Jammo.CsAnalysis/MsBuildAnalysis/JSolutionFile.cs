using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jammo.CsAnalysis.MsBuildAnalysis.Solutions;

namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public class JSolutionFile : IDisposable
    {
        private readonly FileInfo info;

        public readonly SolutionStream Stream;
        public IEnumerable<JProjectFile> ProjectFiles { get; private set; }
        
        public JSolutionFile(string path)
        {
            info = new FileInfo(path);
            
            if (!info.Exists)
                throw new ArgumentException($"File '{info.Name}' does not exist.");

            if (info.Extension != ".sln")
                throw new ArgumentException("Expected a .sln file.");
            
            Stream = new SolutionStream(info.Open(FileMode.Open, FileAccess.Read, FileShare.Read));
            
            UpdateProjects();
        }

        public void UpdateProjects()
        {
            Stream.Parse();
            
            ProjectFiles = Stream.Projects
                .Select(p => new JProjectFile(Path.Join(info.FullName, p.RelativePath)));
        }

        public void Dispose()
        {
            Stream.Dispose();
        }
    }
}