using System;
using Jammo.CsAnalysis.MsBuildAnalysis;
using NUnit.Framework;

namespace JammaNalysis_UnitTests
{
    [TestFixture]
    public class SlnParserTests
    {
        [Test]
        public void TestVersion()
        {
            var result = SlnParser.Parse("Microsoft Visual Studio Solution File, Format Version 12.00");
            
            Assert.True(result.FormattedVersion.Version == "12.00");
        }
    }
}