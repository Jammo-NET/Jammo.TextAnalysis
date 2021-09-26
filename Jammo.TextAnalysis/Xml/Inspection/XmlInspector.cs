using System;
using System.IO;
using System.Linq;
using System.Xml;
using Jammo.TextAnalysis.Xml.Inspection.Rules;

namespace Jammo.TextAnalysis.Xml.Inspection
{
    public class XmlInspector : Inspector<XmlInspectionRule, XmlDiagnostic, XmlAnalysisCompilation>
    {
        public override async void Inspect(XmlAnalysisCompilation context)
        {
            var settings = new XmlReaderSettings
            {
                Async = true
            };

            var reader = XmlReader.Create(new StringReader(context.Document.ToString()), settings);
                
            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        break;
                    case XmlNodeType.Text:
                        await reader.GetValueAsync();
                        break;
                    case XmlNodeType.EndElement:
                        break;
                    case XmlNodeType.Attribute:
                        break;
                    case XmlNodeType.DocumentType:
                        break;
                    case XmlNodeType.Comment:
                        break;
                    case XmlNodeType.XmlDeclaration:
                        break;
                    case XmlNodeType.CDATA:
                        break;
                    default:
                        break;
                }
            }
        }
        
        private void InvokeRule(Action<XmlInspectionRule> factory)
        {
            foreach (var rule in InternalRules.Cast<XmlInspectionRule>())
                factory.Invoke(rule);
        }
    }
}