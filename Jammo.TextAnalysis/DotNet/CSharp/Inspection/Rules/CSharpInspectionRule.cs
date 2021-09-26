using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.TextAnalysis.DotNet.CSharp.Inspection.Rules
{
    public abstract class CSharpInspectionRule : InspectionRule
    {
        public virtual IEnumerable<CSharpDiagnostic> TestUsingDirective(UsingDirectiveSyntax syntax, CSharpAnalysisCompilation context) => null;
        
        public virtual IEnumerable<CSharpDiagnostic> TestNamespaceDeclaration(NamespaceDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        
        public virtual IEnumerable<CSharpDiagnostic> TestClassDeclaration(ClassDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestStructDeclaration(StructDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestInterfaceDeclaration(InterfaceDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestRecordDeclaration(RecordDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestEnumDeclaration(EnumDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        
        public virtual IEnumerable<CSharpDiagnostic> TestEnumMemberDeclaration(EnumMemberDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        
        public virtual IEnumerable<CSharpDiagnostic> TestFieldDeclaration(FieldDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestPropertyDeclaration(PropertyDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestMethodDeclaration(MethodDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        
        public virtual IEnumerable<CSharpDiagnostic> TestVariableDeclaration(VariableDeclarationSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestVariableAssignment(VariableDeclaratorSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestMethodInvocation(InvocationExpressionSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestMemberAccess(MemberAccessExpressionSyntax syntax, CSharpAnalysisCompilation context) => null;
        
        public virtual IEnumerable<CSharpDiagnostic> TestValueEquals(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestValueNotEqual(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestNot(ExpressionStatementSyntax node, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestLessThan(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestGreaterThan(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestLessThanOrEqual(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestMoreThanOrEqual(ExpressionStatementSyntax syntax, CSharpAnalysisCompilation context) => null;
        
        public virtual IEnumerable<CSharpDiagnostic> TestStringLiteral(LiteralExpressionSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestNumericLiteral(LiteralExpressionSyntax syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestSingleLineComment(SyntaxTrivia syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestMultiLineComment(SyntaxTrivia syntax, CSharpAnalysisCompilation context) => null;
        public virtual IEnumerable<CSharpDiagnostic> TestDocumentationComment(SyntaxTrivia trivia, CSharpAnalysisCompilation context) => null;
    }
}