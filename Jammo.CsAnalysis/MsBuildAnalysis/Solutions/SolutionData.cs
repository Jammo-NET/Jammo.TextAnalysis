namespace Jammo.CsAnalysis.MsBuildAnalysis.Solutions
{
    public abstract class SolutionData
    {
        public abstract string ToFormattedString();
    }

    public class FormatVersion : SolutionData
    {
        public string Value { get; }
        
        public FormatVersion(string value)
        {
            Value = value;
        }

        public override string ToFormattedString()
        {
            return $"Microsoft Visual Studio Solution File, Format Version {Value}";
        }
    }

    public class ProjectDefinition : SolutionData
    {
        public string Name;
        public string RelativePath;
        public string Guid;
        public string ConfigGuid;

        public ProjectDefinition()
        {
            
        }

        public override string ToFormattedString()
        {
            throw new System.NotImplementedException();
        }
    }
}