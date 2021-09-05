using System.Collections.Generic;
using System.IO;
using Jammo.CsAnalysis.MsBuildAnalysis.Solutions;

namespace Jammo.CsAnalysis.Compilation
{
    public class CSharpFileAnalysisWrapper
    {
        public readonly CompilationWrapper CompilationWrapper;
        
        public static CSharpFileAnalysisWrapper Create(string filePath, AnalysisType analysisType)
        {
            return new CSharpFileAnalysisWrapper(filePath, analysisType);
        }

        private CSharpFileAnalysisWrapper(string path, AnalysisType analysisType)
        {
            CompilationWrapper = new CompilationWrapper();
            
            switch (analysisType)
            {
                case AnalysisType.File:
                {
                    CompilationWrapper.AppendFile(new FileInfo(path));
                    
                    break;
                }
                case AnalysisType.Directory:
                {
                    CompilationWrapper.AppendFileRange(
                        GetDirectoryFiles(new DirectoryInfo(path), SearchOption.AllDirectories));
                    
                    break;
                }
                case AnalysisType.Solution:
                {
                    CompilationWrapper.AppendFileRange(GetSolutionFiles(new FileInfo(path)));

                    break;
                }
                case AnalysisType.Project:
                {
                    CompilationWrapper.AppendFileRange(GetProjectFiles(new FileInfo(path)));
                    
                    break;
                }
            }
        }

        private IEnumerable<FileInfo> GetSolutionFiles(FileInfo file)
        {
            using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            using var stream = new SolutionStream(fileStream);
            var files = new List<FileInfo>();

            foreach (var project in stream.Projects)
                files.AddRange(
                    GetProjectFiles(new FileInfo(Path.Join(stream.FilePath, project.RelativePath))));
            
            return files;
        }

        private IEnumerable<FileInfo> GetProjectFiles(FileInfo file)
        {
            return GetDirectoryFiles(file.Directory, SearchOption.AllDirectories); 
        }
        
        private IEnumerable<FileInfo> GetDirectoryFiles(DirectoryInfo directory, SearchOption query)
        {
            return directory.GetFiles("*", query);
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