using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jammo.TextAnalysis.DotNet.MsBuild
{
    public interface IProjectFileSystemEntry
    {
        public FileSystemInfo Info { get; }

        public IEnumerable<IProjectFileSystemEntry> EnumerateTree();
        public IEnumerable<ProjectFile> EnumerateFiles();
        public IEnumerable<ProjectDirectory> EnumerateDirectories();
    }
    
    public class ProjectDirectory : IProjectFileSystemEntry
    {
        public FileSystemInfo Info { get; }

        public readonly List<IProjectFileSystemEntry> Children = new();

        internal ProjectDirectory(DirectoryInfo info)
        {
            Info = info;
        }

        public IProjectFileSystemEntry TraverseRelativePath(string path)
        {
            path = path.Replace("\\", "/");
            
            return TraverseRelativePath(path.Split("/"));
        }

        private IProjectFileSystemEntry TraverseRelativePath(string[] path)
        {
            var current = path.FirstOrDefault();

            if (current is null)
                return null;
            
            foreach (var entry in Children)
            {
                if (current.StartsWith(entry.Info.Name))
                {
                    if (current == entry.Info.Name)
                        return entry;

                    if (entry is ProjectDirectory projectDirectory)
                    {
                        return projectDirectory.TraverseRelativePath(path.Skip(1).ToArray());
                    }
                }
            }

            return null;
        }

        public IEnumerable<IProjectFileSystemEntry> EnumerateTree()
        {
            foreach (var child in Children)
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
            foreach (var dir in Children.OfType<ProjectDirectory>())
            {
                yield return dir;
                
                foreach (var nestedDir in dir.EnumerateDirectories())
                    yield return nestedDir;
            }
        }
    }

    public class ProjectFile : IProjectFileSystemEntry
    {
        public FileSystemInfo Info { get; }
        public readonly ProjectDirectory Directory;

        internal readonly List<IProjectFileSystemEntry> dependantFiles = new();
        public IEnumerable<IProjectFileSystemEntry> DependantFiles => dependantFiles;

        internal ProjectFile(FileInfo info, ProjectDirectory directory)
        {
            Info = info;
            Directory = directory;
        }
        
        public IEnumerable<IProjectFileSystemEntry> EnumerateTree()
        {
            yield return this;
        }

        public IEnumerable<ProjectFile> EnumerateFiles()
        {
            yield break;
        }

        public IEnumerable<ProjectDirectory> EnumerateDirectories()
        {
            yield break;
        }
    }
}