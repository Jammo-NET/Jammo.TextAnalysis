using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jammo.TextAnalysis.DotNet.MsBuild;
using Jammo.TextAnalysis.DotNet.MsBuild.Solutions;

namespace Jammo.TextAnalysis.DotNet.CSharp
{
    public class CSharpAnalysisCompilationHelper
    {
        public static CSharpAnalysisCompilation Create(string path, AnalysisType analysisType)
        {
            var compilation = new CSharpAnalysisCompilation();
            
            switch (analysisType)
            {
                case AnalysisType.File:
                {
                    compilation.AppendFile(new FileInfo(path));
                    
                    break;
                }
                case AnalysisType.Directory:
                {
                    compilation.AppendFileRange(
                        GetDirectoryFiles(new DirectoryInfo(path), SearchOption.AllDirectories));
                    
                    break;
                }
                case AnalysisType.Solution:
                {
                    compilation.AppendFileRange(GetSolutionFiles(new FileInfo(path)));

                    break;
                }
                case AnalysisType.Project:
                {
                    compilation.AppendFileRange(GetProjectFiles(new FileInfo(path)));
                    
                    break;
                }
            }

            return compilation;
        }

        private static IEnumerable<FileInfo> GetSolutionFiles(FileInfo file)
        {
            using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            using var stream = new SolutionStream(fileStream);
            var slnDirectory = new FileInfo(stream.FilePath).Directory;
            var files = new List<FileInfo>();

            foreach (var project in stream.Projects)
                files.AddRange(
                    GetProjectFiles(new FileInfo(Path.Join(slnDirectory?.FullName, project.RelativePath))));
            
            return files;
        }

        private static IEnumerable<FileInfo> GetProjectFiles(FileInfo file)
        {
            return new JProjectFile(file.FullName).FileSystem.EnumerateFiles().Select(f => (FileInfo)f.Info); 
        }
        
        private static IEnumerable<FileInfo> GetDirectoryFiles(DirectoryInfo directory, SearchOption query)
        {
            return directory.GetFiles("*.cs", query);
        }
    }

    public enum AnalysisType
    {
        File,
        Solution,
        Project,
        Directory,
    }
}