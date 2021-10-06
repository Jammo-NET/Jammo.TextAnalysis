using System;
using System.Collections.Generic;
using System.IO;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Jammo.TextAnalysis.DotNet.CSharp
{
    public class CSharpDocument
    {
        private readonly List<CSharpDiagnostic> diagnostics = new();

        public readonly FileInfo File;
        public string RawText { get; private set; }
        public SyntaxTree SyntaxTree { get; private set; }

        public IEnumerable<CSharpDiagnostic> Diagnostics => diagnostics;

        public CSharpDocument(FileInfo file)
        {
            File = file ?? throw new ArgumentNullException(nameof(file));

            Update();
        }

        public void AddDiagnostic(CSharpDiagnostic diagnostic)
        {
            diagnostics.Add(diagnostic);
        }

        public void AddDiagnostics(IEnumerable<CSharpDiagnostic> newDiagnostics)
        {
            foreach (var diagnostic in newDiagnostics)
                AddDiagnostic(diagnostic);
        }

        public void Update()
        {
            File.Refresh();
            
            using var reader = File.OpenText();
            RawText = reader.ReadToEndAsync().Result;
            
            SyntaxTree = CSharpSyntaxTree.ParseText(RawText);
        }
    }
}