using System;
using System.Collections.Generic;
using System.Text;

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
        public string ProjectGuid;
        public string GlobalGuid;

        public override string ToFormattedString()
        {
            return $"Project({ProjectGuid}) = \"{Name}\", \"{RelativePath}\", \"{GlobalGuid}\"{Environment.NewLine}EndProject";
        }
    }

    public class GlobalDefinition : SolutionData
    {
        private readonly List<GlobalSectionDefinition> sections = new();
        public GlobalSectionDefinition[] Sections => sections.ToArray();
        
        public void AddSection(GlobalSectionDefinition section)
        {
            sections.Add(section);
        }

        public void RemoveSection(int index)
        {
            sections.RemoveAt(index);
        }

        public override string ToFormattedString()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Global");

            foreach (var section in sections)
            {
                builder.AppendLine("\t" + section.ToFormattedString());
            }

            builder.Append("EndGlobal");

            return builder.ToString();
        }
    }

    public class GlobalSectionDefinition : SolutionData
    {
        private readonly List<GlobalConfiguration> configurations = new();
        public IEnumerable<GlobalConfiguration> Configurations => configurations.ToArray();

        public string ConfigurationType;
        public string RunTime;

        public void AddConfiguration(GlobalConfiguration configuration)
        {
            configurations.Add(configuration);
        }

        public void RemoveConfiguration(int index)
        {
            configurations.RemoveAt(index);
        }

        public override string ToFormattedString()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"GlobalSection({ConfigurationType}) = {RunTime}");

            foreach (var config in configurations)
            {
                builder.AppendLine("\t" + config.ToFormattedString());
            }

            builder.Append("EndGlobalSection");
            
            return builder.ToString();
        }
    }
    
    public class GlobalConfiguration : SolutionData
    {
        public string ProjectGlobalGuid;
        public string Config;
        public string Type;
        public string AssignedConfig;
        
        public override string ToFormattedString()
        {
            return $"{ProjectGlobalGuid}.{Config}.{Type} = {AssignedConfig}";
        }
    }
}