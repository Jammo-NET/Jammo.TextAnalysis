using System.Xml.Linq;

namespace Jammo.TextAnalysis.Xml.Inspection.Rules
{
    public abstract class XmlInspectionRule : InspectionRule
    {
        public virtual void TestRootElement(XElement element, XmlAnalysisCompilation compilation) { }
        public virtual void TestElement(XElement element, bool isRoot, XmlAnalysisCompilation compilation) { }
        public virtual void TestAttribute(XAttribute attribute, XmlAnalysisCompilation compilation) { }
        public virtual void TestElementTextContent(string text, XmlAnalysisCompilation compilation) { }
        
        public virtual void TestDocType(string text, XmlAnalysisCompilation compilation) { }
        public virtual void TestXmlDeclaration(string text, XmlAnalysisCompilation compilation) { }
        // ReSharper disable once InconsistentNaming
        public virtual void TestCDATA(string text, XmlAnalysisCompilation compilation) { }

        public virtual void TestComment(string text, XmlAnalysisCompilation compilation) { }
    }
}