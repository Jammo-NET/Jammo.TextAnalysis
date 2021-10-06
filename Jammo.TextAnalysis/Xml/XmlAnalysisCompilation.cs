using System.Xml.Linq;

namespace Jammo.TextAnalysis.Xml
{
    public class XmlAnalysisCompilation : FileAnalysisCompilation
    {
        public XDocument Document { get; private set; }
        public XElement Root => Document.Root;

        public virtual void GenerateDocument()
        {
            Document = XDocument.Parse(RawText);
        }
    }
}