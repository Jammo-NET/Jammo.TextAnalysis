using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jammo.CsAnalysis.MsBuildAnalysis.Solutions
{
    public static partial class SolutionParser
    {
        public static SolutionStream Parse(string text)
        {
            var stream = new SolutionStream();
            var stateQueue = new List<ParserState>();

            stateQueue.Insert(0, ParserState.Any);
            
            var tokenizer = new Tokenizer(text, new TokenizerOptions(BasicTokenType.Newline));
            var parseIndex = tokenizer.Index;
            
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
                                case "GlobalSection":
                                    stateQueue.Insert(0, ParserState.GlobalSection);
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

                            versionNum.Append(versionToken.Text);
                        } while ((versionToken = tokenizer.Next()) != null);

                        stream.Version = new FormatVersion(versionNum.ToString());

                        stateQueue.Remove(stateQueue.First());
                        
                        break;
                    }
                    case ParserState.ProjectDef:
                    {
                        var defState = ProjectDefState.Guid;
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
                                        case ProjectDefState.Guid:
                                            projectDef.Guid = builder.ToString();
                                            defState = ProjectDefState.Name;
                                            break;
                                        case ProjectDefState.Name:
                                            projectDef.Name = builder.ToString();
                                            defState = ProjectDefState.ConfigGuid;
                                            break;
                                        case ProjectDefState.RelativePath:
                                            projectDef.RelativePath = builder.ToString();
                                            break;
                                        case ProjectDefState.ConfigGuid:
                                            projectDef.ConfigGuid = builder.ToString();
                                            defState = ProjectDefState.RelativePath;
                                            break;
                                    }

                                    builder.Clear();
                                }
                                
                                continue;
                            }
                            
                            if (isBuilding)
                            {
                                builder.Append(projectDefToken.Text);
                            }
                            
                            if (projectDefToken.Text == "EndProject")
                                break;
                        } while ((projectDefToken = tokenizer.Next()) != null);
                        
                        stream.AddProject(projectDef);
                        
                        break;
                    }
                    case ParserState.GlobalDef:
                    {
                        break;
                    }
                    case ParserState.GlobalSection:
                    {
                        break;
                    }
                    case ParserState.Configuration:
                    {
                        break;
                    }
                }

                parseIndex = tokenizer.Index;
            }

            return stream;
        }

        private enum ParserState
        {
            Any = 0,
            
            Version,
            ProjectDef,
            GlobalDef,
            GlobalSection,
            Configuration
        }

        private enum ProjectDefState
        {
            Guid, Name, RelativePath, ConfigGuid
        }
    }
}