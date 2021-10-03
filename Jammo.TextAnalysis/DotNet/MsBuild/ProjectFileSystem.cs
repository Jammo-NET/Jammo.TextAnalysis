using System.Collections.Generic;
using System.Linq;

namespace Jammo.TextAnalysis.DotNet.MsBuild
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
            return EnumerateDirectories().SelectMany(directory => directory.EnumerateFiles());
        }
        
        public IEnumerable<ProjectDirectory> EnumerateDirectories()
        {
            yield return TopLevel;

            foreach (var nestedDir in TopLevel.EnumerateDirectories())
                yield return nestedDir;
        }
    }
}