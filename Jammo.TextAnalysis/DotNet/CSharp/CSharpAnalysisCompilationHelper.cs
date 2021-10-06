using System.Collections.Generic;
using System.IO;
using Jammo.TextAnalysis.DotNet.MsBuild;

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
                    compilation = new CSharpSolutionAnalysisCompilation(new JSolutionFile(path));

                    break;
                }
                case AnalysisType.Project:
                {
                    compilation = new CSharpProjectAnalysisCompilation(new JProjectFile(path));

                    break;
                }
            }

            return compilation;
        }

        private static IEnumerable<FileInfo> GetDirectoryFiles(DirectoryInfo directory, SearchOption option)
        {
            return directory.GetFiles("*.cs", option);
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