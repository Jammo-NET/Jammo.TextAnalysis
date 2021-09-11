using System.Linq;
using Jammo.CsAnalysis.MsBuildAnalysis.Projects;
using NUnit.Framework;

namespace JammaNalysis_UnitTests
{
    [TestFixture]
    public class ProjectStreamTests
    {
        [Test]
        public void TestSdk()
        {
            var stream = new ProjectStream();

            stream.Sdk = "MySdk";

            var document = stream.ToDocument();
            Assert.True(document.Root.Attribute("Sdk").Value == "MySdk");
        }

        [Test]
        public void TestRemovedFile()
        {
            var stream = new ProjectStream();
            var group = new ItemGroup { new CompileItem { Remove = "This "} };
            
            stream.ItemGroups.Add(group);
            
            Assert.False(stream.CompiledFiles.Any());
        }

        [Test]
        public void TestProperties()
        {
            var stream = new ProjectStream();
            stream.Properties.Add("MyProperty", "SomeValue");

            var document = stream.ToDocument();
            Assert.True(document.Root.Element("PropertyGroup").Element("MyProperty").Value == "SomeValue");
        }
    }
}