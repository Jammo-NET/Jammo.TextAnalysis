using System;
using System.Linq;
using Jammo.TextAnalysis;
using Jammo.TextAnalysis.DotNet.CSharp;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection;
using Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules;
using Microsoft.CodeAnalysis.CSharp;
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
            
                comp.AppendText(new MultiLineString() +
                                "public class MyInspectionTest" +
                                "{" +
                                "   private int myVar;" +
                                "}");
                
                var inspector = new CSharpInspector();
                inspector.AddRules(new UnusedFieldInspection());
                
                comp.GenerateCompilation();
                inspector.Inspect(comp);
                
                Assert.True(inspector.Diagnostics.First().Span.Size == 5);
            }

            [Test]
            public void TestUnusedFieldInspection()
            {
                var comp = new CSharpAnalysisCompilation();
            
                comp.AppendText(new MultiLineString() +
                                "public class MyInspectionTest" +
                                "{" +
                                "   private int myVar, myVar2, myVar3;" +
                                "}");
                
                var inspector = new CSharpInspector();
                inspector.AddRules(new UnusedFieldInspection());
                
                comp.GenerateCompilation();
                inspector.Inspect(comp);
                
                Assert.True(inspector.Diagnostics.Count() == 3);
            }

            [Test]
            public void TestIncorrectFlagInspection()
            {
                var comp = new CSharpAnalysisCompilation();
            
                comp.AppendText(new MultiLineString() +
                                "[Flags]" +
                                "public enum MyEnum" +
                                "{" +
                                "   Foo = 1;" +
                                "   OtherFoo = 1<<0" +
                                "}");

                var inspector = new CSharpInspector();
                inspector.AddRules(new IncorrectFlagInspection());

                comp.GenerateCompilation();
                inspector.Inspect(comp);
                
                Assert.True(inspector.Diagnostics.Count() == 1);
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