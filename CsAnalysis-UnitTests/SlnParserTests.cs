using System;
using System.IO;
using System.Linq;
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
        public class TestGlobalDefinition
        {
            private string testString = "Global GlobalSection(Hello) = World {A}.B.C = E\r\nEndSection EndGlobal";
            private SolutionStream testStream;
            
            [SetUp]
            public void SetUp()
            {
                testStream = SolutionParser.Parse(testString);
            }
            
            [Test]
            public void TestGlobalSectionConfig()
            {
                Assert.True(testStream.Globals.First().Sections.First().ConfigurationType == "Hello");
            }
            
            [Test]
            public void TestGlobalSectionRunTime()
            {
                Assert.True(testStream.Globals.First().Sections.First().RunTime == "World");
            }

            [Test]
            public void TestConfiguration()
            {
                Console.WriteLine(testStream.Globals.First().Sections.First().Configurations.First().ProjectGlobalGuid);
                Console.WriteLine(testStream.Globals.First().Sections.First().Configurations.First().Type);
                Console.WriteLine(testStream.Globals.First().Sections.First().Configurations.First().Config);
                Console.WriteLine(testStream.Globals.First().Sections.First().Configurations.First().AssignedConfig);
                Assert.True(testStream.Globals.First().Sections.First().Configurations.First().AssignedConfig == "E");
            }
        }

        [TestFixture]
        public class TestProjectDefinition
        {
            private string testString = $"Project(\"MyGuid\") = \"MyProject\", \"MyPath\", \"MyConfigGuid\" EndProject";
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