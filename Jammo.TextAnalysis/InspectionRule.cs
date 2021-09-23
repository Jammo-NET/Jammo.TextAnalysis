namespace Jammo.TextAnalysis
{
    public abstract class InspectionRule
    {
        public abstract DiagnosticInfo GetInspectionInfo();
    }
    
    public readonly struct DiagnosticInfo
    {
        public readonly string InspectionCode;
        public readonly string InspectionName;
        public readonly string InspectionMessage;

        public DiagnosticInfo(string code, string name, string message)
        {
            InspectionCode = code;
            InspectionName = name;
            InspectionMessage = message;
        }
    }
}