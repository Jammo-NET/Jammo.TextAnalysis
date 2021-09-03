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
            var parser = new SlnParser("Microsoft Visual Studio Solution File, Format Version 12.00");
            
            Assert.True(parser.Result.FormattedVersion.ToString() == "12.00");
        }
    }
}