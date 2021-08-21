using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis.CsInspections
{
    public class FileSchema
    {
        public UsingStatement[] UsingStatements;
        public NamespaceDeclaration[] Namespaces;
        public ClassDeclaration[] Classes;

        public static FileSchema Create(FileStream file)
        {
            var reader = new StreamReader(file);
            
            return Create(reader.ReadToEndAsync().Result);
        }

        public static FileSchema Create(string text)
        {
            var schema = new FileSchema();
            var result = CSharpSyntaxTree.ParseText(text);
            
            var root = result.GetCompilationUnitRoot();

            schema.UsingStatements = root.Usings.Select(statement =>
                new UsingStatement(
                    IndexSpan.FromTextSpan(statement.Span),
                    statement)).ToArray();

            var namespaces = new List<NamespaceDeclaration>();
            var types = new List<ClassDeclaration>();

            foreach (var member in root.Members)
            {
                switch (member)
                {
                    case NamespaceDeclarationSyntax ns:
                        namespaces.Add(
                            new NamespaceDeclaration(IndexSpan.FromTextSpan(ns.Span), ns));
                        break;
                    case ClassDeclarationSyntax classType:
                        types.Add(
                            new ClassDeclaration(IndexSpan.FromTextSpan(classType.Span), classType));
                        break;
                }
            }

            schema.Namespaces = namespaces.ToArray();
            schema.Classes = types.ToArray();

            return schema;
        }

        private FileSchema()
        {
            
        }
    }
}