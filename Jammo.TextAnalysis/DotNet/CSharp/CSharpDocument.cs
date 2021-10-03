using System.Collections.Generic;
using System.IO;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.TextAnalysis.DotNet.CSharp
{
    public class CSharpDocument
    {
        public readonly FileInfo File;
        public readonly string RawText;
        public readonly SyntaxTree SyntaxTree;
        
        public readonly List<CSharpDiagnostic> Diagnostics = new();

        public CSharpDocument(FileInfo file)
        {
            File = file;

            using var reader = file.OpenText();
            RawText = reader.ReadToEndAsync().Result;
            
            SyntaxTree = CSharpSyntaxTree.ParseText(RawText);
        }
    }
}