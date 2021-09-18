using System.Linq;
using Jammo.TextAnalysis.DotNet.CSharp;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
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
            public void TestInspectionSpan()
            {
                var comp = new CSharpAnalysisCompilation();
            
                comp.AppendText("public class MyInspectionTest" +
                                "{" +
                                "   private int myVar;" +
                                "}");
                
                var inspector = new CSharpInspector();
                inspector.AddRules(new[] { new UnusedFieldInspection() });
                    
                comp.SetInspector(inspector);
                comp.GenerateInspections();
                
                Assert.True(comp.Inspections.First().Span.Size == 5);
            }

            [Test]
            public void TestUnusedFieldInspection()
            {
                var comp = new CSharpAnalysisCompilation();
            
                comp.AppendText("public class MyInspectionTest\n" +
                                "{\n" +
                                "   private int myVar, myVar2, myVar3;\n" +
                                "}");
                
                var inspector = new CSharpInspector();
                inspector.AddRules(new[] { new UnusedFieldInspection() });
                    
                comp.SetInspector(inspector);
                comp.GenerateInspections();
                
                Assert.True(comp.Inspections.Count() == 3);
            }

            [Test]
            public void TestIncorrectFlagInspection()
            {
                var comp = new CSharpAnalysisCompilation();
            
                comp.AppendText("[Flags]\n" +
                                "public enum MyEnum\n" +
                                "{\n" +
                                "   Foo = 1;\n" +
                                "   OtherFoo = 1<<0\n" +
                                "}");
                
                var inspector = new CSharpInspector();
                inspector.AddRules(new[] { new IncorrectFlagInspection() });
                
                comp.SetInspector(inspector);
                comp.GenerateInspections();
                
                Assert.True(comp.Inspections.Count() == 1);
            }
        }

        [Test]
        public void TestCompilation()
        {
            var comp = new CSharpAnalysisCompilation();
            
            comp.AppendText("public class MyTestAttribute : System.Attribute { }");
            comp.GenerateCompilation();

            var type = comp.GlobalNamespace.GetTypeMembers().First();
            Assert.True(type.Name == "MyTestAttribute");
        }

        [Test]
        public void TestMultipleRaws()
        {
            var comp = new CSharpAnalysisCompilation();
            
            comp.AppendText("public class MyTestClass { }");
            comp.AppendText("public class MyTestDerivedClass : MyTestClass { }");
            comp.GenerateCompilation();

            var derivedType = comp.GlobalNamespace.GetTypeMembers()[1];
            
            Assert.True(derivedType.Name == "MyTestDerivedClass" && derivedType.BaseType?.Name == "MyTestClass");
        }
    }
}