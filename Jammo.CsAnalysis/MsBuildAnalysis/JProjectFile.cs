using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public class JProjectFile
    {
        private const string RootName = "Project";

        public readonly FileInfo FileInfo;
        public readonly ProjectFileSystem FileSystem;
        public readonly XDocument ProjectHead;
        
        private IEnumerable<XElement> ItemGroups => ProjectHead.Root?
            .Elements()
            .Where(e => e.Name == "ItemGroup");

        private IEnumerable<XElement> PropertyGroups => ProjectHead.Root?
            .Elements()
            .Where(e => e.Name == "PropertyGroup");

        public IEnumerable<XElement> GetNamedItems(string name) => ItemGroups
            .Descendants()
            .Where(x => x.Name == name);

        public IEnumerable<XElement> GetNamedProperties(string name) => PropertyGroups
            .Descendants()
            .Where(x => x.Name == name);

        public JProjectFile(string path)
        {
            FileInfo = new FileInfo(path);
            
            if (!FileInfo.Exists)
                throw new ArgumentException($"File '{FileInfo.Name}' does not exist.");

            if (FileInfo.Extension != ".csproj")
                throw new ArgumentException("Expected a .csproj file.");
            
            if (FileInfo.Directory == null)
                return;

            using var fileStream = FileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fileStream);
            var document = XDocument.Parse(reader.ReadToEndAsync().Result);

            var root = document.Root;
            
            if (root?.Name != RootName)
                return;
            
            ProjectHead = document;

            var system = new ProjectFileSystem(ReadDirectory(FileInfo.Directory));
            
            FileSystem = system;
        }

        public void Write()
        {
            using var fileStream = FileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            ProjectHead.Save(fileStream);
        }

        private ProjectDirectory ReadDirectory(DirectoryInfo info)
        {
            var dir = new ProjectDirectory(info);
            
            foreach (var entry in info.EnumerateFileSystemInfos())
            {
                if (entry is FileInfo file)
                    dir.Children.Add(new ProjectFile(file, dir));
                else if (entry is DirectoryInfo directory)
                    dir.Children.Add(ReadDirectory(directory));
            }

            return dir;
        }
    }
}