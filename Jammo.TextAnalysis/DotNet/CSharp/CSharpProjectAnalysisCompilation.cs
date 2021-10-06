using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jammo.TextAnalysis.DotNet.MsBuild;

namespace Jammo.TextAnalysis.DotNet.CSharp
{
    public class CSharpProjectAnalysisCompilation : CSharpAnalysisCompilation
    {
        private readonly List<CSharpDocument> documents = new();
        public IEnumerable<CSharpDocument> Documents => documents;
        
        public CSharpVersion CSharpVersion;
        public readonly JProjectFile ProjectFile;

        public CSharpProjectAnalysisCompilation(JProjectFile projectFile)
        {
            var version = projectFile.GetNamedProperties("LangVersion").FirstOrDefault();

            CSharpVersion = version?.Value switch
            {
                "default" => CSharpVersion.Default,
                "latestmajor" => CSharpVersion.LatestMajor,
                "latestminor" => CSharpVersion.LatestMinor,
                "preview" => CSharpVersion.Preview,
                "10" => CSharpVersion.Version10,
                "9" => CSharpVersion.Version9,
                "8" => CSharpVersion.Version8,
                "7.3" => CSharpVersion.Version73,
                "7.2" => CSharpVersion.Version72,
                "7.1" => CSharpVersion.Version71,
                "7" => CSharpVersion.Version7,
                "6" => CSharpVersion.Version6,
                "5" => CSharpVersion.Version5,
                "4" => CSharpVersion.Version4,
                "3" => CSharpVersion.Version3,
                "ISO-2" => CSharpVersion.Iso1,
                "ISO-1" => CSharpVersion.Iso2,
                _ => CSharpVersion.Default
            };

            ProjectFile = projectFile;

            foreach (var file in ProjectFile.FileSystem.EnumerateFiles().Where(f => f.Info.Extension == ".cs"))
                documents.Add(new CSharpDocument((FileInfo)file.Info));
        }

        public override void AppendFile(FileInfo file)
        {
            documents.Add(new CSharpDocument(file));
            
            base.AppendFile(file);
        }

        public override void AppendFileRange(IEnumerable<FileInfo> fileRange)
        {
            foreach (var file in fileRange)
                AppendFile(file);
        }

        public void ClearRawFiles()
        {
            InternalFileText.Clear();
        }
    }
    
    public enum CSharpVersion
    {
        Default,
        LatestMinor,
        LatestMajor,
        Preview,
        
        Version10,
        Version9,
        Version8,
        Version73,
        Version72,
        Version71,
        Version7,
        Version6,
        Version5,
        Version4,
        Version3,
        Iso1,
        Iso2,
    }
}