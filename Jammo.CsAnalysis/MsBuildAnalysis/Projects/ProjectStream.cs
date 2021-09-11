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
            
            var writer = new StreamWriter(stream);
            stream.SetLength(0);

            writer.Write(ToString());
        }
        
        public void WriteTo(string path)
        {
            var file = File.Create(path);
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
            var document = new XDocument();

            return document.ToString();
        }
    }

    public class ItemGroup : Collection<Item>
    {
        
    }

    public abstract class Item
    {
        public string Update;
        public string Include;
        public string Remove;

        public string DependsOn;
        public OutputDirectoryCopy CopyMode;

        public string Generator;
        public string CustomToolNamespace;
        
        public List<Item> Items;

        public static Item FromElement(XElement element)
        {
            Item item = element.Name.ToString() switch
            {
                "PackageReference" => new PackageReferenceItem(),
                "Compile" => new CompileItem(),
                "None" => new NoneItem(),
                _ => new UnknownItem()
            };

            item.Update = element.Attribute("Update")?.Value;
            item.Include = element.Attribute("Include")?.Value;
            item.Remove = element.Attribute("Remove")?.Value;
            
            item.LoadFromElement(element);

            return item;
        }

        public abstract void LoadFromElement(XElement element);
    }

    public enum OutputDirectoryCopy
    {
        None = 0,
        
        PreserveNewest,
        Always
    }

    public class UnknownItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }

    public class PackageReferenceItem : Item
    {
        public string Version;
        
        public override void LoadFromElement(XElement element)
        {
            Version = element.Attribute("Version")?.Value;
        }
    }
    
    public class CompileItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            
        }
    }

    public class ContentItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class ResourceItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class AppDefinitionItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class SpecFlowFeaturesFilesItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class OriginalXamlResourceItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class CIIncludeItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class CICompileItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class PageItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class InterfaceDefinitionItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class TypeScriptCompileItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class AdditionalFilesItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            throw new NotImplementedException();
        }
    }
    
    public class NoneItem : Item
    {
        public override void LoadFromElement(XElement element)
        {
            
        }
    }
}