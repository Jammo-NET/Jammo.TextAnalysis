using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jammo.CsAnalysis.MsBuildAnalysis.Solutions.ParserExtensions;

namespace Jammo.CsAnalysis.MsBuildAnalysis.Solutions
{
    public static partial class SolutionParser
    {
        public static SolutionStream Parse(string text)
        {
            var stream = new SolutionStream();
            var stateQueue = new List<ParserState>();

            stateQueue.Insert(0, ParserState.Any);

            var tokenizer = new Tokenizer(text, default);

            foreach (var token in tokenizer)
            {
                var currentState = stateQueue.FirstOrDefault();
                
                switch (currentState)
                {
                    case ParserState.Any:
                    {
                        var anyTokens = new BasicTokenCollection();

                        var anyToken = token;

                        do
                        {
                            if (anyToken.Type == BasicTokenType.Newline)
                                continue;
                            
                            anyTokens.Add(anyToken);
                            
                            switch (anyTokens.ToString())
                            {
                                case "Microsoft Visual Studio Solution File, Format Version":
                                    stateQueue.Insert(0, ParserState.Version);
                                    break;
                                case "Project":
                                    stateQueue.Insert(0, ParserState.ProjectDef);
                                    break;
                                case "Global":
                                    stateQueue.Insert(0, ParserState.GlobalDef);
                                    break;
                            }

                            if (currentState != stateQueue.FirstOrDefault())
                                break;
                        } while ((anyToken = tokenizer.Next()) != null);

                        break;
                    }
                    case ParserState.Version:
                    {
                        var versionNum = new StringBuilder();

                        var versionToken = token;
                        do
                        {
                            if (versionToken.Type == BasicTokenType.Whitespace)
                                continue;
                            
                            if (versionToken.Type != BasicTokenType.Numerical &&
                                versionToken.Type != BasicTokenType.Punctuation)
                                break;

                            versionNum.Append(versionToken.Text);
                        } while ((versionToken = tokenizer.Next()) != null);

                        stream.Version = new FormatVersion(versionNum.ToString());
                        
                        stateQueue.Remove(stateQueue.First());

                        break;
                    }
                    case ParserState.ProjectDef:
                    {
                        var defState = ProjectDefState.ProjectGuid;
                        var projectDef = new ProjectDefinition();
                        var builder = new StringBuilder();
                        var isBuilding = false;

                        var projectDefToken = token;
                        do
                        {
                            if (projectDefToken.Text == "\"")
                            {
                                isBuilding = !isBuilding;

                                if (!isBuilding)
                                {
                                    switch (defState)
                                    {
                                        case ProjectDefState.ProjectGuid:
                                            projectDef.ProjectGuid = builder.ToString();
                                            defState = ProjectDefState.Name;
                                            break;
                                        case ProjectDefState.Name:
                                            projectDef.Name = builder.ToString();
                                            defState = ProjectDefState.RelativePath;
                                            break;
                                        case ProjectDefState.RelativePath:
                                            projectDef.RelativePath = builder.ToString();
                                            defState = ProjectDefState.GlobalGuid;
                                            break;
                                        case ProjectDefState.GlobalGuid:
                                            projectDef.GlobalGuid = builder.ToString();
                                            break;
                                    }

                                    builder.Clear();
                                }

                                continue;
                            }

                            if (isBuilding) builder.Append(projectDefToken.Text);

                            if (projectDefToken.Text == "EndProject")
                            {
                                stateQueue.Remove(stateQueue.First());
                                break;
                            }
                        } while ((projectDefToken = tokenizer.Next()) != null);

                        stream.AddProject(projectDef);

                        break;
                    }
                    case ParserState.GlobalDef:
                    {
                        var globalDef = new GlobalDefinition();
                        
                        BasicToken globalToken;
                        while ((globalToken = tokenizer.Next()) != null)
                        {
                            if (globalToken.Text == "EndGlobal")
                                break;

                            if (globalToken.Text == "GlobalSection")
                            {
                                var sectionState = GlobalSectionState.ConfigType;
                                var section = new GlobalSectionDefinition();
                                var builder = new StringBuilder();

                                BasicToken globalSectionToken;
                                while ((globalSectionToken = tokenizer.Next()) != null)
                                {
                                    if (globalSectionToken.Text == "EndGlobalSection")
                                        break;

                                    switch (sectionState)
                                    {
                                        case GlobalSectionState.ConfigType:
                                            if (globalSectionToken.Text == "(")
                                            {
                                                sectionState = GlobalSectionState.Intermediate;
                                                builder.Append(tokenizer.ReadUntilOrEnd(")"));

                                                section.ConfigurationType = builder.ToString();

                                                builder.Clear();
                                            }

                                            break;
                                        case GlobalSectionState.Intermediate:
                                            if (globalSectionToken.Text == "=")
                                                sectionState = GlobalSectionState.RunTime;

                                            break;
                                        case GlobalSectionState.RunTime:
                                            if (globalSectionToken.Type == BasicTokenType.Whitespace)
                                                continue;

                                            sectionState = GlobalSectionState.Configurations;
                                            builder.Append(globalSectionToken.Text);

                                            section.RunTime = builder.ToString();

                                            builder.Clear();

                                            break;
                                        case GlobalSectionState.Configurations:
                                            var configState = GlobalConfigState.ProjectGlobalGuid;
                                            var config = new GlobalConfiguration();
                                            var configBuilder = new StringBuilder();
                                            
                                            var configToken = globalSectionToken;
                                            do
                                            {
                                                switch (configState)
                                                {
                                                    case GlobalConfigState.ProjectGlobalGuid:
                                                        if (configToken.Text != ".")
                                                        {
                                                            configBuilder.Append(configToken.Text);
                                                            break;
                                                        }

                                                        config.ProjectGlobalGuid = configBuilder.ToString().TrimStart();
                                                        configState = GlobalConfigState.Type;

                                                        configBuilder.Clear();
                                                        break;
                                                    case GlobalConfigState.Type:
                                                        if (configToken.Text != ".")
                                                        {
                                                            configBuilder.Append(configToken.Text);
                                                            break;
                                                        }

                                                        config.Type = configBuilder.ToString();
                                                        configState = GlobalConfigState.Config;
                                                        
                                                        configBuilder.Clear();
                                                        break;
                                                    case GlobalConfigState.Config:
                                                        if (configToken.Text != "=")
                                                        {
                                                            configBuilder.Append(configToken.Text);
                                                            break;
                                                        }

                                                        config.Config = configBuilder.ToString().Trim();
                                                        configState = GlobalConfigState.AssignedConfig;
                                                        
                                                        configBuilder.Clear();
                                                        break;
                                                    case GlobalConfigState.AssignedConfig:
                                                        if (configToken.Type == BasicTokenType.Newline)
                                                            configState = GlobalConfigState.Complete;
                                                        else
                                                            config.AssignedConfig += configToken.Text;

                                                        break;
                                                }

                                                if (configState == GlobalConfigState.Complete)
                                                {
                                                    config.AssignedConfig = config.AssignedConfig.Trim();
                                                    break;
                                                }
                                            } while ((configToken = tokenizer.Next()) != null);

                                            section.AddConfiguration(config);

                                            break;
                                    }
                                }

                                globalDef.AddSection(section);
                            }
                        }
                        // Parse global sections

                        stream.AddGlobal(globalDef);

                        break;
                    }
                }
            }

            return stream;
        }

        private enum ParserState
        {
            Any = 0,

            Version,
            ProjectDef,
            GlobalDef,
        }

        private enum ProjectDefState
        {
            ProjectGuid,
            Name,
            RelativePath,
            GlobalGuid
        }

        private enum GlobalSectionState
        {
            ConfigType,
            RunTime,
            Configurations,
            Intermediate
        }

        private enum GlobalConfigState
        {
            ProjectGlobalGuid,
            Config,
            Type,
            AssignedConfig,
            Complete
        }
    }
}