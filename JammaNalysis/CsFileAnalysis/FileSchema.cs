using System.Collections.Generic;
using System.IO;
using System.Linq;
using JammaNalysis.Compilation;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis.CsFileAnalysis
{
    public class FileSchema
    {
        public UsingStatement[] UsingStatements;
        public NamespaceDeclaration[] Namespaces;
        public NamespaceDeclaration GlobalNamespace => Namespaces.FirstOrDefault();

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

            schema.UsingStatements = root.Usings
                .Select(statement => new UsingStatement(statement.Span, statement))
                .ToArray();

            var namespaces = new List<NamespaceDeclaration>();
            var types = new List<ClassDeclaration>();
            
            namespaces.Add(new NamespaceDeclaration(new IndexSpan(0, 0)));

            foreach (var member in root.Members)
            {
                switch (member)
                {
                    case NamespaceDeclarationSyntax ns:
                        namespaces.Add(
                            new NamespaceDeclaration(ns.Span, ns));
                        break;
                    case ClassDeclarationSyntax classType:
                        types.Add(
                            new ClassDeclaration(classType.Span, classType));
                        break;
                }
            }

            schema.Namespaces = namespaces.ToArray();
            schema.GlobalNamespace.Members = types.Cast<TypeDeclaration>().ToArray();

            return schema;
        }

        private FileSchema()
        {
            
        }
    }
}