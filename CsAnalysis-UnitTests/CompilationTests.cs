using System;
using System.Linq;
using Jammo.CsAnalysis.Compilation;
using NUnit.Framework;

namespace JammaNalysis_UnitTests
{
    [TestFixture]
    public class CompilationTests
    {
        [Test]
        public void TestCompilation()
        {
            var comp = new CompilationWrapper();
            
            comp.AppendText("public class MyTestAttribute : System.Attribute { }");
            comp.GenerateCompilation();

            var type = comp.GlobalNamespace.GetTypeMembers().First();
            Assert.True(type.Name == "MyTestAttribute");
        }

        [Test]
        public void TestMultipleRaws()
        {
            var comp = new CompilationWrapper();
            
            comp.AppendText("public class MyTestClass { }");
            comp.AppendText("public class MyTestDerivedClass : MyTestClass { }");
            comp.GenerateCompilation();

            var derivedType = comp.GlobalNamespace.GetTypeMembers()[1];
            
            Assert.True(derivedType.Name == "MyTestDerivedClass" && derivedType.BaseType?.Name == "MyTestClass");
        }
    }
}