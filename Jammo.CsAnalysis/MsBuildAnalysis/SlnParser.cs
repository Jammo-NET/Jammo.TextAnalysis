using System;
using System.Collections.Generic;
using System.Linq;

namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public static partial class SlnParser
    {
        public static SlnFileData Parse(string text)
        {
            var slnData = new SlnFileData();
            var stateQueue = new List<ParserState>();

            stateQueue.Insert(0, ParserState.Any);
            
            var tokenizer = new Tokenizer(text);
            var parseIndex = tokenizer.Index;
            
            foreach (var token in tokenizer)
            {
                if (token.Type == BasicTokenType.Newline)
                    continue;

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
                        var versionNum = string.Empty;
                        
                        var versionToken = token;
                        do
                        {
                            if (versionToken.Type == BasicTokenType.Newline)
                                break;

                            if (versionToken.Type == BasicTokenType.Whitespace)
                                continue;

                            versionNum += versionToken.Text;
                        } while ((versionToken = tokenizer.Next()) != null);

                        slnData.FormattedVersion = new FormatVersion(versionNum, new IndexSpan(parseIndex, tokenizer.Index));

                        stateQueue.Remove(stateQueue.Last());
                        
                        break;
                    }
                    case ParserState.ProjectDef:
                    {
                        
                        
                        break;
                    }
                    case ParserState.FolderDef:
                    {
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

            return slnData;
        }

        private enum ParserState
        {
            Any = 0,
            
            Version,
            ProjectDef,
            FolderDef,
            GlobalDef,
            GlobalSection,
            Configuration
        }
    }
}