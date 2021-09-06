using System;
using System.Linq;
using Jammo.CsAnalysis.CodeInspection;
using Jammo.CsAnalysis.CodeInspection.Rules;
using Jammo.CsAnalysis.Compilation;
using NUnit.Framework;

namespace JammaNalysis_UnitTests
{
    [TestFixture]
    public class CompilationTests
    {
        [TestFixture]
        public class InspectionTests
        {
            [Test]
            public void TestUnusedFieldInspection()
            {
                var comp = new CompilationWrapper();
            
                comp.AppendText("public class MyInspectionTest" +
                                "{" +
                                "   private int myVar;" +
                                "}");
                
                var inspector = new CodeInspector();
                inspector.WithRules(new[] { new UnusedFieldInspection() });
                    
                comp.WithInspector(inspector);
                comp.GenerateCompilation();
                
                Assert.True(comp.Inspections.Any());
            }
            
            [Test]
            public void TestUsedField()
            {
                var comp = new CompilationWrapper();
            
                comp.AppendText("public class MyInspectionTest" +
                                "{" +
                                "   private int myVar;" +
                                "" +
                                "   public MyInspectionTest()" +
                                "   {" +
                                "       myVar = 0;" +
                                "   }" +
                                "}");
                
                var inspector = new CodeInspector();
                inspector.WithRules(new[] { new UnusedFieldInspection() });
                    
                comp.WithInspector(inspector);
                comp.GenerateCompilation();
                
                Assert.True(!comp.Inspections.Any());
            }
        }

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