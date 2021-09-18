namespace Jammo.TextAnalysis
{
    public abstract class InspectionRule
    {
        public abstract InspectionInfo GetInspectionInfo();
    }
    
    public readonly struct InspectionInfo
    {
        public readonly string InspectionCode;
        public readonly string InspectionName;
        public readonly string InspectionMessage;

        public InspectionInfo(string code, string name, string message)
        {
            InspectionCode = code;
            InspectionName = name;
            InspectionMessage = message;
        }
    }
}