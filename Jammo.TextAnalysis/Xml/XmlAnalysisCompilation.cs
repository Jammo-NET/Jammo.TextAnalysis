using System.Xml.Linq;

namespace Jammo.TextAnalysis.Xml
{
    public class XmlAnalysisCompilation : FileAnalysisCompilation
    {
        public XDocument Document { get; private set; }
        public XElement Root => Document.Root;

        public override void GenerateCompilation()
        {
            Document = XDocument.Parse(RawText);
        }
    }
}