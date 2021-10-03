using System;
using System.IO;
using System.Linq;
using Jammo.TextAnalysis.DotNet.CSharp;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
using Jammo.TextAnalysis.DotNet.MsBuild;
using NUnit.Framework;

namespace JammaNalysis_UnitTests
{
    [TestFixture]
    public class CSharpProjectAnalysisTests
    {
        [Test]
        public void TestDocumentAnalysis()
        {
            var directory = Path.Join(Directory.GetCurrentDirectory(), "project-test");
            var projectFile = new JProjectFile(Path.Join(directory, "test.csproj"));
            var testFile = new FileInfo(Path.Join(directory, "CSharpTest.cs"));
            
            var compilation = new CSharpProjectAnalysisCompilation(projectFile);
            compilation.AppendFile(testFile);
            
            var inspector = new CSharpProjectInspector();
            inspector.AddRule(new IncorrectFlagInspection());
            inspector.Inspect(compilation);

            Assert.True(compilation.Documents.First().Diagnostics.Count == 1);
        }
    }
}