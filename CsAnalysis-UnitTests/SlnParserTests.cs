using System;
using System.Linq;
using Jammo.CsAnalysis.MsBuildAnalysis;
using Jammo.CsAnalysis.MsBuildAnalysis.Solutions;
using NUnit.Framework;

namespace JammaNalysis_UnitTests
{
    [TestFixture]
    public class SolutionParserTests
    {
        [Test]
        public void TestVersion()
        {
            var result = SolutionParser.Parse("Microsoft Visual Studio Solution File, Format Version 12.00");
            
            Assert.True(result.Version.Value == "12.00");
        }

        [TestFixture]
        public class TestProjectDefinition
        {
            private string testString = "Project(\"MyGuid\") = \"MyProject\", \"MyPath\", \"MyConfigGuid\" EndProject";
            private SolutionStream testStream;
            
            [SetUp]
            public void SetUp()
            {
                testStream = SolutionParser.Parse(testString);
            }
            
            [Test]
            public void TestProjectName()
            {
                Assert.True(testStream.Projects.First().Name == "MyProject");
            }
        }
    }
}