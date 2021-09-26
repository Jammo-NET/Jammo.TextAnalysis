using System.Xml.Linq;
using Jammo.TextAnalysis.Xml.Inspection;

namespace Jammo.TextAnalysis.Xml
{
    public class XmlAnalysisCompilation : AnalysisCompilation
    {
        public XDocument Document { get; private set; }
        public XElement Root => Document.Root;

        public override void GenerateCompilation()
        {
            Document = XDocument.Parse(RawText);
        }
    }
}