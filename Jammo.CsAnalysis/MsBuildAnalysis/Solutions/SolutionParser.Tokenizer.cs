using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jammo.CsAnalysis.MsBuildAnalysis.Solutions
{
    public static partial class SolutionParser 
    { 
        internal class Tokenizer : IEnumerable<BasicToken>
        {
            private readonly string text;
            private TokenizerOptions options;

            public int Index { get; private set; }

            public Tokenizer(string input, TokenizerOptions options)
            {
                text = input;
                this.options = options ?? new TokenizerOptions();
            }

            public static IEnumerable<BasicToken> Tokenize(string input, TokenizerOptions options)
            {
                var tokens = new List<BasicToken>();
                var tokenizer = new Tokenizer(input, options);
                
                BasicToken token;
                while ((token = tokenizer.Next()) != null)
                    tokens.Add(token);

                return tokens;
            }

            public BasicToken Next()
            {
                var token = PeekNext();

                Index += token?.Text.Length ?? 0;
                
                return token;
            }

            public BasicToken PeekNext()
            {
                var currentRead = string.Empty;
                var currentTokenType = BasicTokenType.Unhandled;

                for (var charIndex = Index; charIndex < text.Length; charIndex++)
                {
                    var character = text[charIndex];

                    if (char.IsWhiteSpace(character) &&
                        (char.IsWhiteSpace(currentRead.LastOrDefault()) || string.IsNullOrEmpty(currentRead)))
                    {
                        currentTokenType = BasicTokenType.Whitespace;
                        currentRead += character;

                        if (currentRead == Environment.NewLine)
                            return new BasicToken(
                                currentRead,
                                BasicTokenType.Newline,
                                new IndexSpan(Index, charIndex));
                    }
                    else if (char.IsPunctuation(character) && string.IsNullOrEmpty(currentRead))
                    {
                        return new BasicToken(
                            character.ToString(),
                            BasicTokenType.Punctuation,
                            new IndexSpan(Index, charIndex));
                    }
                    else if (char.IsSymbol(character) && string.IsNullOrEmpty(currentRead))
                    {
                        return new BasicToken(
                            character.ToString(),
                            BasicTokenType.Symbol,
                            new IndexSpan(Index, charIndex));
                    }
                    else if (char.IsLetter(character) &&
                        char.IsLetter(currentRead.LastOrDefault()) || string.IsNullOrEmpty(currentRead))
                    {
                        currentTokenType = BasicTokenType.Alphabetical;
                        currentRead += character;
                    }
                    else if (char.IsNumber(character) &&
                             (char.IsLetter(currentRead.LastOrDefault()) || 
                             char.IsNumber(currentRead.LastOrDefault()) ||
                             string.IsNullOrEmpty(currentRead)) &&
                             !char.IsPunctuation(character))
                    { // Allow abc123
                        if (char.IsLetter(currentRead.LastOrDefault()))
                            currentTokenType = BasicTokenType.Alphabetical;
                        else
                            currentTokenType = BasicTokenType.Numerical;
                        
                        currentRead += character;
                    }
                    else
                    {
                        if (options.Ignorable.Contains(currentTokenType))
                        {
                            Index += currentRead.Last();
                            
                            return PeekNext();
                        }

                        return new BasicToken(currentRead, currentTokenType, new IndexSpan(Index, charIndex));
                    }
                }

                if (string.IsNullOrEmpty(currentRead))
                    return null;
                
                if (options.Ignorable.Contains(currentTokenType))
                {
                    Index += currentRead.Last();
                            
                    return PeekNext();
                }
                
                return new BasicToken(currentRead, currentTokenType, new IndexSpan(Index, text.Length - 1));
            }

            public IEnumerator<BasicToken> GetEnumerator()
            {
                BasicToken token;
                while ((token = Next()) != null)
                    yield return token;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class BasicTokenCollection : Collection<BasicToken>
        {
            public override string ToString()
            {
                return string.Concat(this.Select(token => token.Text));
            }
        }

        internal class BasicToken
        {
            public readonly string Text;
            public readonly BasicTokenType Type;
            public readonly IndexSpan Span;

            public BasicToken(string text, BasicTokenType type, IndexSpan span)
            {
                Text = text;
                Type = type;
                Span = span;
            }

            public override string ToString()
            {
                return Text;
            }
        }
        
        internal class TokenizerOptions
        {
            public readonly BasicTokenType[] Ignorable;

            public TokenizerOptions(params BasicTokenType[] ignorable)
            {
                Ignorable = ignorable;
            }
        }
    
        internal enum BasicTokenType
        {
            Unhandled = 0,
            
            Alphabetical,
            Numerical,
            Symbol,
            Punctuation,
            Whitespace,
            Newline
        }
    }
}