using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jammo.TextAnalysis.DotNet.MsBuild.Solutions;

namespace Jammo.TextAnalysis.DotNet.MsBuild
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

            using var fileStream = info.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            Stream = new SolutionStream(fileStream);
            
            UpdateProjects();
        }

        public void UpdateProjects()
        {
            Stream.Parse();
            
            ProjectFiles = Stream.Projects
                .Select(p => new JProjectFile(Path.Join(info.DirectoryName, p.RelativePath)));
        }

        public void Dispose()
        {
            Stream.Dispose();
        }
    }
}