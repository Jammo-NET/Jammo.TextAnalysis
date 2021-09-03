using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Jammo.CsAnalysis.MsBuildAnalysis
{
    public partial class SlnParser
    {
        private class Tokenizer
        {
            private string text;

            public int Index { get; private set; }

            public Tokenizer(string input)
            {
                text = input;
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
                        currentTokenType = BasicTokenType.Alphabetical;
                        currentRead += character;
                    }
                    else
                    {
                        return new BasicToken(currentRead, currentTokenType, new IndexSpan(Index, charIndex));
                    }
                }

                if (string.IsNullOrEmpty(currentRead))
                    return null;
                
                return new BasicToken(currentRead, currentTokenType, new IndexSpan(Index, text.Length - 1));
            }
        }

        private class BasicToken
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

        private enum BasicTokenType
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