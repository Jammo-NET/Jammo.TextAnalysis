using System;
using System.Collections.Generic;
using System.Linq;

namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public partial class SlnParser
    {
        public readonly SlnFileData Result;
        
        public SlnParser(string text)
        {
            var slnData = new SlnFileData();
            
            var stateQueue = new List<ParserState>();
            var tokens = new List<BasicToken>();

            stateQueue.Add(ParserState.Any);

            var tokenizer = new Tokenizer(text);
            var parseIndex = tokenizer.Index;
            
            BasicToken token;
            while ((token = tokenizer.Next()) != null)
            {
                tokens.Add(token);
                
                switch (stateQueue.LastOrDefault())
                {
                    case ParserState.Any:
                    {
                        switch (token.Text)
                        {
                            case "Version":
                                stateQueue.Add(ParserState.Version);
                                break;
                            case "Project":
                                stateQueue.Add(ParserState.ProjectDef);
                                break;
                            case "Global":
                                stateQueue.Add(ParserState.GlobalDef);
                                break;
                            case "GlobalSection":
                                stateQueue.Add(ParserState.GlobalSection);
                                break;
                        }

                        break;
                    }
                    case ParserState.Version:
                    {
                        var versionNum = string.Empty;
                        
                        BasicToken versionToken;
                        while ((versionToken = tokenizer.Next()) != null)
                        {
                            if (versionToken.Type == BasicTokenType.Newline)
                                break;
                            
                            if (versionToken.Type == BasicTokenType.Whitespace)
                                continue;
                            
                            versionNum += versionToken.Text;
                        }

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

            Result = slnData;
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