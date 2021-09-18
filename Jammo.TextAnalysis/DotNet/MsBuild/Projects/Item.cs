using System.Collections.Generic;
using System.Xml.Linq;

namespace Jammo.TextAnalysis.DotNet.MsBuild.Projects
{
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
                "Content" => new ContentItem(),
                "Page" => new PageItem(),
                "ApplicationDefinition" => new ApplicationDefinitionItem(),
                "Resource" => new ResourceItem(),
                "SpecFlowFeatures" => new SpecFlowFeaturesFilesItem(),
                "OriginalXamlResource" => new OriginalXamlResourceItem(),
                "CIInclude" => new CIIncludeItem(),
                "CICompile" => new CICompileItem(),
                "InterfaceDefinition" => new InterfaceDefinitionItem(),
                "TypeScriptCompile" => new TypeScriptCompileItem(),
                "AdditionalFiles" => new AdditionalFilesItem(),
                "None" => new NoneItem(),
                _ => new UnknownItem()
            };

            item.Update = element.Attribute("Update")?.Value;
            item.Include = element.Attribute("Include")?.Value;
            item.Remove = element.Attribute("Remove")?.Value;
            item.DependsOn = element.Element("DependantUpon")?.Value;

            item.LoadFromElement(element);
            
            return item;
        }

        public virtual void LoadFromElement(XElement element) { }
        public virtual XElement ToXElement() => FromItem(this);

        public static XElement FromItem(Item item)
        {
            var element = new XElement(item.GetType().Name);
            
            if (item.Update != null)
                element.SetAttributeValue("Update", item.Update);
            
            if (item.Include != null)
                element.SetAttributeValue("Include", item.Include);
            
            if (item.Remove != null)
                element.SetAttributeValue("Remove", item.Remove);

            return element;
        }
    }
    
    public class UnknownItem : Item
    {
        
    }

    public class PackageReferenceItem : Item
    {
        public string Version;
        
        public override void LoadFromElement(XElement element)
        {
            Version = element.Attribute("Version")?.Value;
        }

        public override XElement ToXElement()
        {
            var element = FromItem(this);

            element.SetAttributeValue("Version", Version);
            
            return element;
        }
    }
    
    public class CompileItem : Item
    {
        
    }

    public class ContentItem : Item
    {
        
    }
    
    public class ResourceItem : Item
    {
        
    }
    
    public class ApplicationDefinitionItem : Item
    {
        
    }
    
    public class SpecFlowFeaturesFilesItem : Item
    {
        
    }
    
    public class OriginalXamlResourceItem : Item
    {
        
    }
    
    public class CIIncludeItem : Item
    {
        
    }
    
    public class CICompileItem : Item
    {
        
    }
    
    public class PageItem : Item
    {
        
    }
    
    public class InterfaceDefinitionItem : Item
    {
        
    }
    
    public class TypeScriptCompileItem : Item
    {
        
    }
    
    public class AdditionalFilesItem : Item
    {
        
    }
    
    public class NoneItem : Item
    {
        
    }
}