using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.Inspection.Rules
{
    public abstract class InspectionRule
    {
        public abstract string InspectionCode { get; }
        public abstract string InspectionName { get; }
        public abstract string InspectionMessage { get; }

        public virtual void TestUsingDirective(UsingDirectiveSyntax syntax, MergeableCompilation context) { }

        public virtual void TestNamespaceDeclaration() { }
        
        public virtual void TestTypeDeclaration() { }
        public virtual void TestClassDeclaration() { }
        public virtual void TestStructDeclaration() { }
        public virtual void TestInterfaceDeclaration() { }
        public virtual void TestRecordDeclaration() { }
        
        public virtual void TestFieldDeclaration() { }
        public virtual void TestPropertyDeclaration() { }
        public virtual void TestMethodDeclaration() { }
        
        public virtual void TestVariableDeclaration() { }
        public virtual void TestVariableAssignment() { }
        public virtual void TestMethodInvocation() { }
        public virtual void TestMemberAccess() { }
        public virtual void TestValueComparison() { }
        
        public virtual void TestString() { }
        public virtual void TestNumber() { }
        
        public virtual void TestPreProcessorDirective() { }
        public virtual void TestSingleLineComment() { }
        public virtual void TestMultiLineComment() { }
    }
}