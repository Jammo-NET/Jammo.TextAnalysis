using System.Collections.Generic;

namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public class ProjectFileSystem
    {
        public readonly ProjectDirectory TopLevel;

        public ProjectFileSystem(ProjectDirectory directory)
        {
            TopLevel = directory;
        }

        public IEnumerable<IProjectFileSystemEntry> EnumerateTree()
        {
            yield return TopLevel;

            foreach (var child in TopLevel.EnumerateTree())
            {
                yield return child;

                foreach (var nested in child.EnumerateTree())
                    yield return nested;
            }
        }
        
        public IEnumerable<ProjectFile> EnumerateFiles()
        {
            yield break;
        }
        
        public IEnumerable<ProjectDirectory> EnumerateDirectories()
        {
            yield return TopLevel;
        }
    }
}