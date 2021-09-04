using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jammo.CsAnalysis.MsBuildAnalysis.Solutions;
using Microsoft.Build.Construction;

namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public class JSolutionFile
    {
        private FileInfo info;

        public readonly FormatVersion Version;
        public IEnumerable<CsProjectFile> Projects { get; }
        
        public JSolutionFile(string path)
        {
            info = new FileInfo(path);

            if (!info.Exists)
                throw new ArgumentException($"File '{info.Name}' does not exist.");

            if (info.Extension != ".sln")
                throw new ArgumentException("Expected a .sln file.");

            Projects = SolutionFile.Parse(path).ProjectsInOrder
                .Select(p => new CsProjectFile(p.AbsolutePath));
        }
    }

    public class SlnFileData
    {
        public FormatVersion FormattedVersion;
    }
}