using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Jammo.CsAnalysis.CsFileAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jammo.CsAnalysis.Compilation
{
    public class MergeableCompilation
    {
        public readonly bool Success;

        public readonly CompilationNamespace GlobalNamespace;
        public FileInfo[] Files { get; set; } = Array.Empty<FileInfo>();

        public MergeableCompilation(CompilationNamespace ns)
        {
            GlobalNamespace = ns;
            Success = true;
        }
        
        public MergeableCompilation(FileInfo file)
        {
            try
            {
                using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var schema = FileSchema.Create(fileStream);
                
                GlobalNamespace = new CompilationNamespace(schema.GlobalNamespace.Name).Merge(
                    schema.GlobalNamespace.Members
                        .OfType<NamespaceDeclaration>()
                        .Select(ns => new CompilationNamespace(ns))
                        .ToArray());
                    
                Success = true;
                Files = Files.Concat(new[] { file }).ToArray();
            }
            catch (IOException)
            {
                Success = false;
            }
        }
        
        public MergeableCompilation Merge(params MergeableCompilation[] others)
        {
            if (others.Length == 0)
                return this;
            
            var compNamespace = new CompilationNamespace("Root");
            compNamespace.Namespaces.Add(GlobalNamespace);

            foreach (var comp in others)
                compNamespace.Namespaces.Add(comp.GlobalNamespace);

            var compilation = new MergeableCompilation(compNamespace);
            compilation.Files = compilation.Files
                .Concat(others.Select(o => o.Files).SelectMany(i => i))
                .Concat(Files)
                .ToArray();
                
            return new MergeableCompilation(compNamespace);
        }
    }
}