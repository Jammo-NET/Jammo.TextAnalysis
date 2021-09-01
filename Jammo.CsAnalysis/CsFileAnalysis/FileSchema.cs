using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.CsFileAnalysis
{
    public class FileSchema
    {
        public UsingStatement[] UsingStatements;
        public NamespaceDeclaration GlobalNamespace;

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

            var members = new List<IMember>();

            foreach (var member in root.Members)
            {
                switch (member)
                {
                    case NamespaceDeclarationSyntax ns:
                        members.Add(
                            new NamespaceDeclaration(ns.Span, ns));
                        break;
                    case ClassDeclarationSyntax classType:
                        members.Add(
                            new ClassDeclaration(classType.Span, classType));
                        break;
                }
            }

            schema.GlobalNamespace = new NamespaceDeclaration(new IndexSpan(0, 0))
            {
                Members = members.ToArray()
            };

            return schema;
        }

        private FileSchema()
        {
            
        }
    }
}