using System;
using System.Linq;
using Jammo.TextAnalysis;
using Jammo.TextAnalysis.DotNet.CSharp.Helpers;
using NUnit.Framework;

namespace JammaNalysis_UnitTests
{
    [TestFixture]
    public class CsHelperTests
    {
        [Test]
        public void TestSpecialSyntaxReader()
        {
            const string testString = "namespace A {}";

            var reader = new SpecialSyntaxReader(testString);
            
            Assert.True(reader.Matches.First().Text == "namespace");
        }
    }
}