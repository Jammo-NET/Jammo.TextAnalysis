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
                Assert.True(testStream.Globals.First().Sections.First().Configurations.First().AssignedConfig == "E");
            }
        }

        [TestFixture]
        public class TestProjectDefinition
        {
            private string testString =
                "\r\n" +
                "Microsoft Visual Studio Solution File, Format Version 12.00\r\n" +
                "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"TempoIDE\", \"TempoIDE\\TempoIDE.csproj\", \"{669B4658-B1F4-4240-9DFA-6E7BD2AE9353}\r\n" +
                "EndProject\r\n" +
                "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"TempoControls\", \"TempoControls\\TempoControls.csproj\", \"{CA418233-665F-467B-9CD2-CDC7369080C3}\"\r\n" +
                "EndProject\r\n" +
                "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"TempoControlsTests\", \"TempoControlsTests\\TempoControlsTests.csproj\", \"{CE7E66D2-5CE1-4C00-AF2C-54BBEEE7B85C}\"\r\n" +
                "EndProject\r\n" +
                "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"TempoSourceGen\", \"TempoSourceGen\\TempoSourceGen.csproj\", \"{3A5C9931-BAD5-4D1E-A2C8-A847DD4F4AFA}\"\r\n" +
                "EndProject\r\n";
            private SolutionStream testStream;
            
            [SetUp]
            public void SetUp()
            {
                testStream = SolutionParser.Parse(testString);
            }
            
            [Test]
            public void TestProjectName()
            {
                Assert.True(testStream.Projects.First().Name == "TempoIDE");
            }
            
            [Test]
            public void TestProjectPath()
            {
                Assert.True(testStream.Projects.First().RelativePath == "TempoIDE\\TempoIDE.csproj");
            }
        }
    }
}