using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.CodeInspection.Rules
{
    public abstract class InspectionRule
    {
        public abstract InspectionInfo GetInspectionInfo();

        public virtual void TestUsingDirective(UsingDirectiveSyntax syntax, CompilationWrapper context) { }
                       
        public virtual void TestNamespaceDeclaration(NamespaceDeclarationSyntax syntax, CompilationWrapper context) { }
                       
        public virtual void TestClassDeclaration(ClassDeclarationSyntax syntax, CompilationWrapper context) { }
        public virtual void TestStructDeclaration(StructDeclarationSyntax syntax, CompilationWrapper context) { }
        public virtual void TestInterfaceDeclaration(InterfaceDeclarationSyntax syntax, CompilationWrapper context) { }
        public virtual void TestRecordDeclaration(RecordDeclarationSyntax syntax, CompilationWrapper context) { }
                       
        public virtual void TestFieldDeclaration(FieldDeclarationSyntax syntax, CompilationWrapper context) { }
        public virtual void TestPropertyDeclaration(PropertyDeclarationSyntax syntax, CompilationWrapper context) { }
        public virtual void TestMethodDeclaration(MethodDeclarationSyntax syntax, CompilationWrapper context) { }
                       
        public virtual void TestVariableDeclaration(VariableDeclarationSyntax syntax, CompilationWrapper context) { }
        public virtual void TestVariableAssignment(VariableDeclaratorSyntax syntax, CompilationWrapper context) { }
        public virtual void TestMethodInvocation(InvocationExpressionSyntax syntax, CompilationWrapper context) { }
        public virtual void TestMemberAccess(MemberAccessExpressionSyntax syntax, CompilationWrapper context) { }
                       
        public virtual void TestValueEquals(ExpressionStatementSyntax syntax, CompilationWrapper context) { }
        public virtual void TestValueNotEqual(ExpressionStatementSyntax syntax, CompilationWrapper context) { }
        public virtual void TestNot(ExpressionStatementSyntax node, CompilationWrapper context) { }
        public virtual void TestLessThan(ExpressionStatementSyntax syntax, CompilationWrapper context) { }
        public virtual void TestGreaterThan(ExpressionStatementSyntax syntax, CompilationWrapper context) { }
        public virtual void TestLessThanOrEqual(ExpressionStatementSyntax syntax, CompilationWrapper context) { }
        public virtual void TestMoreThanOrEqual(ExpressionStatementSyntax syntax, CompilationWrapper context) { }

        public virtual void TestStringLiteral(LiteralExpressionSyntax syntax, CompilationWrapper context) { }
        public virtual void TestNumericLiteral(LiteralExpressionSyntax syntax, CompilationWrapper context) { }
        public virtual void TestSingleLineComment(SyntaxTrivia syntax, CompilationWrapper context) { }
        public virtual void TestMultiLineComment(SyntaxTrivia syntax, CompilationWrapper context) { }
        public virtual void TestDocumentationComment(SyntaxTrivia trivia, CompilationWrapper context) { }
    }

    public readonly struct InspectionInfo
    {
        public readonly string InspectionCode;
        public readonly string InspectionName;
        public readonly string InspectionMessage;

        public InspectionInfo(string code, string name, string message)
        {
            InspectionCode = code;
            InspectionName = name;
            InspectionMessage = message;
        }
    }
}