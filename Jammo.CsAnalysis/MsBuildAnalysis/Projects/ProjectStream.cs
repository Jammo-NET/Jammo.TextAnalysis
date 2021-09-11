using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Jammo.CsAnalysis.MsBuildAnalysis.Projects
{
    public class ProjectStream : IParserStream, IDisposable
    {
        private FileStream stream;

        public string Sdk;
        
        public Dictionary<string, string> Properties = new();
        public List<ItemGroup> ItemGroups = new();

        public void Parse()
        {
            if (stream == null)
                throw new IOException("Cannot parse an uninitialized file stream");

            var document = XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None).Result;

            if (document.Root == null)
                return;

            Sdk = document.Root.Attribute("Sdk")?.Value;
            
            foreach (var element in document.Root.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "PropertyGroup":
                        foreach (var property in element.Elements())
                            Properties[property.Name.ToString()] = property.Value;
                        
                        break;
                    case "ItemGroup":
                        var group = new ItemGroup();
                        
                        foreach (var item in element.Elements())
                            group.Add(Item.FromElement(item));
                            
                        ItemGroups.Add(group);
                        
                        break;
                }
            }
        }

        public void Write()
        {
            if (stream == null)
            {
                var working = Directory.GetCurrentDirectory();

                Console.WriteLine("The current stream is null, a new file will be created in the working directory." +
                                  $"Current working directory: {working}");
                
                stream = File.Create(Path.Join(Directory.GetCurrentDirectory(), "Jammo_SolutionStream.sln"));
            }
            
            using var writer = new StreamWriter(stream);
            stream.SetLength(0);

            writer.Write(ToString());
        }
        
        public void WriteTo(string path)
        {
            using var file = File.Create(path);
            using var writer = new StreamWriter(file);
            
            file.SetLength(0);
            writer.Write(ToString());
        }

        public IEnumerable<Item> CompiledFiles =>
            ItemGroups.SelectMany(g => g.Where(i => string.IsNullOrEmpty(i.Remove)));

        public IEnumerable<PackageReferenceItem> PackageReferences =>
            ItemGroups.SelectMany(g => g.OfType<PackageReferenceItem>());

        public void Dispose()
        {
            stream?.Dispose();
        }

        public override string ToString()
        {
            return ToDocument().ToString();
        }

        public XDocument ToDocument()
        {
            var root = new XElement("Project");
            root.SetAttributeValue("Sdk", Sdk);
            
            root.Add(new XElement(
                "PropertyGroup", Properties.Select(p => new XElement(p.Key, p.Value))));
            
            foreach (var group in ItemGroups)
                root.Add(group.ToString());

            return new XDocument(root);
        }
    }

    public class ItemGroup : Collection<Item>
    {
        public XElement ToXElement()
        {
            return new XElement("ItemGroup", this.Select(i => i.ToXElement()));
        }
    }

    public enum OutputDirectoryCopy
    {
        None = 0,
        
        PreserveNewest,
        Always
    }
}