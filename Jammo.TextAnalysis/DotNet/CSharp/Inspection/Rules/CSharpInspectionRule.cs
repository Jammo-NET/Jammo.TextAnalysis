using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules
{
    public abstract class CSharpInspectionRule : InspectionRule
    {
        public virtual void TestUsingDirective(UsingDirectiveSyntax syntax, CSharpAnalysisCompilation context) { }
                       
        public virtual void TestNamespaceDeclaration(NamespaceDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
                       
        public virtual void TestClassDeclaration(ClassDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestStructDeclaration(StructDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestInterfaceDeclaration(InterfaceDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestRecordDeclaration(RecordDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestEnumDeclaration(EnumDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
        
        public virtual void TestEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
                       
        public virtual void TestFieldDeclaration(FieldDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestPropertyDeclaration(PropertyDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestMethodDeclaration(MethodDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
                       
        public virtual void TestVariableDeclaration(VariableDeclarationSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestVariableAssignment(VariableDeclaratorSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestMethodInvocation(InvocationExpressionSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestMemberAccess(MemberAccessExpressionSyntax syntax, CSharpAnalysisCompilation context) { }
                       
        public virtual void TestValueEquals(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestValueNotEqual(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestNot(ExpressionStatementSyntax node, CSharpAnalysisCompilation context) { }
        public virtual void TestLessThan(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestGreaterThan(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestLessThanOrEqual(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestMoreThanOrEqual(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) { }

        public virtual void TestStringLiteral(LiteralExpressionSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestNumericLiteral(LiteralExpressionSyntax syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestSingleLineComment(SyntaxTrivia syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestMultiLineComment(SyntaxTrivia syntax, CSharpAnalysisCompilation context) { }
        public virtual void TestDocumentationComment(SyntaxTrivia trivia, CSharpAnalysisCompilation context) { }
    }
}