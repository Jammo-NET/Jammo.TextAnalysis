using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace JammaNalysis.MsBuildAnalysis
{
    public class CsProjectFile
    {
        private const string RootName = "Project";
        private const string ItemGroupName = "ItemGroup";
        private const string PropertyGroupName = "PropertyGroup";

        public readonly string FilePath;
        public ProjectFileSystem FileSystem;
        public readonly XDocument ProjectHead;
        
        public IEnumerable<XElement> ItemGroups => ProjectHead.Root
            .Elements()
            .Where(e => e.Name == ItemGroupName);

        public IEnumerable<XElement> PropertyGroups => ProjectHead.Root
            .Elements()
            .Where(e => e.Name == PropertyGroupName);
        
        public CsProjectFile(string path)
        {
            var info = new FileInfo(path);
            
            if (!info.Exists)
                throw new ArgumentException($"File '{info.Name}' does not exist.");

            if (info.Extension != ".csproj")
                throw new ArgumentException("Expected a .csproj file.");
            
            if (info.Directory == null)
                return;

            var reader = new StreamReader(info.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            var document = XDocument.Parse(reader.ReadToEndAsync().Result);

            var root = document.Root;
            
            if (root?.Name != RootName)
                return;

            FilePath = path;
            ProjectHead = document;

            var filePathAttributes = new[] { "Include", "Update", "Remove" };
            
            var specialItems = ItemGroups
                .Select(g => g
                    .Elements()
                    .Where(e => filePathAttributes.Contains(e.Attributes().FirstOrDefault()?.Value.ToString())))
                .SelectMany(e => e)
                .ToArray();

            var system = new ProjectFileSystem(ReadDirectory(info.Directory));

            foreach (var item in specialItems)
            {
                var removeAttribute = item.Attributes().First().Name == "Remove" ? item.Attributes().First() : null;

                if (removeAttribute != null)
                {
                    system.TopLevel.TraverseRelativePath(removeAttribute.Value);
                } // TODO: Come back to this crap
            }

            FileSystem = system;
        }

        private ProjectDirectory ReadDirectory(DirectoryInfo info)
        {
            var dir = new ProjectDirectory(info);
            
            foreach (var entry in info.EnumerateFileSystemInfos())
            {
                if (entry is FileInfo file)
                    dir.Children.Add(new ProjectFile(file));
                else if (entry is DirectoryInfo directory)
                    dir.Children.Add(ReadDirectory(directory));
            }

            return dir;
        }
    }

    public class ProjectFileSystem
    {
        public readonly ProjectDirectory TopLevel;

        public ProjectFileSystem(ProjectDirectory directory)
        {
            TopLevel = directory;
        }

        public IEnumerator<IProjectFileSystemEntry> EnumerateTree()
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
        
        public IEnumerator<ProjectDirectory> EnumerateDirectories()
        {
            yield return TopLevel;
        }
    }
}