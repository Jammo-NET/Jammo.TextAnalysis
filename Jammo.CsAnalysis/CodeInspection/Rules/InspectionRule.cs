using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.CodeInspection.Rules
{
    public abstract class InspectionRule
    {
        public abstract InspectionInfo GetInspectionInfo();

        public virtual Inspection TestUsingDirective(UsingDirectiveSyntax syntax, CompilationWrapper context) => null;
        
        public virtual Inspection TestNamespaceDeclaration(NamespaceDeclarationSyntax syntax, CompilationWrapper context) => null;
        
        public virtual Inspection TestClassDeclaration(ClassDeclarationSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestStructDeclaration(StructDeclarationSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestInterfaceDeclaration(InterfaceDeclarationSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestRecordDeclaration(RecordDeclarationSyntax syntax, CompilationWrapper context) => null;
        
        public virtual Inspection TestFieldDeclaration(FieldDeclarationSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestPropertyDeclaration(PropertyDeclarationSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestMethodDeclaration(MethodDeclarationSyntax syntax, CompilationWrapper context) => null;
        
        public virtual Inspection TestVariableDeclaration(VariableDeclaratorSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestVariableAssignment(VariableDeclarationSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestMethodInvocation(InvocationExpressionSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestMemberAccess(MemberAccessExpressionSyntax syntax, CompilationWrapper context) => null;
        
        public virtual Inspection TestValueEquals(ExpressionStatementSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestValueNotEqual(ExpressionStatementSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestNot(ExpressionStatementSyntax node, CompilationWrapper context) => null;
        public virtual Inspection TestLessThan(ExpressionStatementSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestGreaterThan(ExpressionStatementSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestLessThanOrEqual(ExpressionStatementSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestMoreThanOrEqual(ExpressionStatementSyntax syntax, CompilationWrapper context) => null;
        
        // TODO: Stuff like |, ||, &, &&...

        public virtual Inspection TestStringLiteral(LiteralExpressionSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestNumericLiteral(LiteralExpressionSyntax syntax, CompilationWrapper context) => null;
        public virtual Inspection TestSingleLineComment(SyntaxTrivia syntax, CompilationWrapper context) => null;
        public virtual Inspection TestMultiLineComment(SyntaxTrivia syntax, CompilationWrapper context) => null;
        public virtual Inspection TestDocumentationComment(SyntaxTrivia trivia, CompilationWrapper context) => null;
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