using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis.CsInspections
{
    public class FileSchema
    {
        public UsingStatement[] UsingStatements;
        
        public FileSchema(FileStream file)
        {
            var reader = new StreamReader(file);
            var result = CSharpSyntaxTree.ParseText(reader.ReadToEndAsync().Result);
            var root = result.GetCompilationUnitRoot();

            UsingStatements = root.Usings.Select(statement => 
                new UsingStatement(
                    statement.Name.ToString(),
                    statement.Alias?.Name.ToString(),
                    statement.Alias != null,
                    statement.StaticKeyword.Span.Length > 0)).ToArray();
        }
    }

    public abstract class Statement
    {
        public IndexSpan Span;
    }

    public abstract class BlockStatement : Statement
    {
        public Block Block;
    }
    
    public class Block
    {
        public IndexSpan Span;
        
        public Statement[] Statements;
        public int ScopeLevel;
    }

    public class UsingStatement : Statement
    {
        public string NamespaceName;
        public string DeclarationName;
        
        public bool IsDeclaration;
        public bool IsStaticDeclaration;

        public UsingStatement(string namespaceName, string declarationName, bool isDeclaration, bool isStaticDeclaration)
        {
            NamespaceName = namespaceName;
            DeclarationName = declarationName;
            IsDeclaration = isDeclaration;
            IsStaticDeclaration = isStaticDeclaration;
        }
    }

    public class ForLoop : BlockStatement
    {
        
    }

    public class ForeachLoop : BlockStatement
    {
        
    }

    public class WhileLoop : BlockStatement
    {
        
    }

    public class DoWhileLoop : BlockStatement
    {
        
    }
}