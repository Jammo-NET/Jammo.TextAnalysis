using System.Xml.Linq;
using Jammo.TextAnalysis.Xml.Inspection;
using Jammo.TextAnalysis.Xml.Inspection.Rules;

namespace Jammo.TextAnalysis.Xml
{
    public class XmlAnalysisCompilation : AnalysisCompilation<XmlInspection, XmlInspectionRule, XmlAnalysisCompilation>
    {
        public XDocument Document { get; private set; }
        public XElement Root => Document.Root;
        
        public override void CreateInspection(XmlInspection inspection)
        {
            InternalInspector.AddInspection(inspection);
        }

        public override void GenerateInspections()
        {
            if (Document == null)
                GenerateCompilation();
            
            InternalInspector.Inspect(this);
        }

        public override void GenerateCompilation()
        {
            Document = XDocument.Parse(RawText);
        }
    }
}