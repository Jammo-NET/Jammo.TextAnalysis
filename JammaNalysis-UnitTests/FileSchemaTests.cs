using System.Linq;
using System.Text;
using JammaNalysis.CsInspections;
using NUnit.Framework;

namespace JammaNalysis_UnitTests
{
    public class UsingStatementTests
    {
        [Test]
        public void TestUsingStatementNames()
        {
            const string testString = 
                "using Hello.World;" +
                "using Hello = World;" +
                "using static Hello = World.Type;";

            var schema = FileSchema.Create(testString);

            var expected = new[]
            {
                schema.UsingStatements[0].NamespaceName == "Hello.World",
                string.IsNullOrEmpty(schema.UsingStatements[0].DeclarationName),
                
                schema.UsingStatements[1].NamespaceName == "World",
                schema.UsingStatements[1].DeclarationName == "Hello",
                
                schema.UsingStatements[2].NamespaceName == "World.Type",
                schema.UsingStatements[2].DeclarationName == "Hello",
            };
            
            Assert.True(expected.All(b => b));
        }

        [Test]
        public void TestUsingStatementDeclarationFlags()
        {
            const string testString =
                "using Hello = World;" +
                "using static Hello = World.Type;";
            
            var schema = FileSchema.Create(testString);

            var expected = new[]
            {
                schema.UsingStatements[0].IsDeclaration == true,
                schema.UsingStatements[0].IsStaticDeclaration == false,
                
                schema.UsingStatements[1].IsDeclaration == true,
                schema.UsingStatements[1].IsStaticDeclaration == true,
            };
            
            Assert.True(expected.All(b => b));
        }
    }

    public class ClassDeclarationTests
    {
        [Test]
        public void TestClassAccessibilityModifiers()
        {
            const string testString = "private internal class Hello { }";
            
            var schema = FileSchema.Create(testString);
            var type = schema.Classes.First();
            
            Assert.True(type.Accessibility == (DeclarationAccessibility.Private | DeclarationAccessibility.Internal));
        }

        [Test]
        public void TestClassMethod()
        {
            const string testString = "private internal class Hello" +
                                      "{" +
                                      "    public void World() { }" +
                                      "}";
            
            var schema = FileSchema.Create(testString);
            var method = (MethodDeclaration)schema.Classes.First().Members.First();
            
            Assert.True(method.Name == "World");
        }
        
        [Test]
        public void TestClassMethodModifiers()
        {
            const string testString = "private internal class Hello" +
                                      "{" +
                                      "    public abstract void World() { }" +
                                      "}";
            
            var schema = FileSchema.Create(testString);
            var method = (MethodDeclaration)schema.Classes.First().Members.First();
            
            Assert.True(method.Accessibility == DeclarationAccessibility.Public && method.IsAbstract);
        }
    }
}