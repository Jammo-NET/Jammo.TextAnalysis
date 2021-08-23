using System.Linq;
using System.Text;
using JammaNalysis.CsFileAnalysis;
using NUnit.Framework;

namespace JammaNalysis_UnitTests
{
    public class UsingStatementTests
    {
        [Test]
        public void TestBaseUsingStatement()
        {
            const string testString = "using Foo;";

            var schema = FileSchema.Create(testString);
            var declaration = schema.UsingStatements.First();

            Assert.True(declaration.NamespaceName == "Foo" && !declaration.IsDeclaration);
        }
        
        [Test]
        public void TestUsingDeclaration()
        {
            const string testString = "using Foo = Bar;";

            var schema = FileSchema.Create(testString);
            var declaration = schema.UsingStatements.First();

            Assert.True(
                declaration.NamespaceName == "Bar" &&
                declaration.DeclarationName == "Foo" &&
                declaration.IsDeclaration);
        }
        
        [Test]
        public void TestStaticUsingDeclaration()
        {
            const string testString = "using static Foo = Bar.Tap;";

            var schema = FileSchema.Create(testString);
            var declaration = schema.UsingStatements.First();

            Assert.True(
                declaration.NamespaceName == "Bar.Tap" &&
                declaration.DeclarationName == "Foo" &&
                declaration.IsDeclaration &&
                declaration.IsStaticDeclaration);
        }
    }
    
    public class NamespaceDeclarationTests
    {
        [Test]
        public void TestNamespaceDeclaration()
        {
            const string testString = "namespace Hello { }";

            var schema = FileSchema.Create(testString);
            
            Assert.True(schema.Namespaces[1].NamespaceName == "Hello");
        }
    }

    public class ClassDeclarationTests
    {
        [Test]
        public void TestClassAccessibilityModifiers()
        {
            const string testString = "private internal class Hello { }";
            
            var schema = FileSchema.Create(testString);
            var type = schema.GlobalNamespace.Types.First();
            
            Assert.True(type.Accessibility == (DeclarationAccessibility.Private | DeclarationAccessibility.Internal));
        }
    }

    public class MethodDeclarationTests
    {
        [Test]
        public void TestClassMethod()
        {
            const string testString = 
                "private internal class Hello" +
                "{" +
                "    public void World() { }" +
                "}";
            
            var schema = FileSchema.Create(testString);
            var method = (MethodDeclaration)schema.GlobalNamespace.Types.First().Members.First();
            
            Assert.True(method.Name == "World");
        }
        
        [Test]
        public void TestClassMethodModifiers()
        {
            const string testString = 
                "private internal class Hello" +
                "{" +
                "    public abstract void World() { }" +
                "}";
            
            var schema = FileSchema.Create(testString);
            var method = (MethodDeclaration)schema.GlobalNamespace.Types.First().Members.First();
            
            Assert.True(method.Accessibility == DeclarationAccessibility.Public && method.IsAbstract);
        }
    }
}