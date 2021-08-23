using System.IO;
using System.Linq;
using JammaNalysis.CsFileAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JammaNalysis.Compilation
{
    public class MergeableCompilation
    {
        public readonly bool Success;

        public readonly CompilationNamespace GlobalNamespace;
        public readonly FileInfo Info;

        public MergeableCompilation(CompilationNamespace ns)
        {
            GlobalNamespace = ns;
            Success = true;
        }
        
        //public MergeableCompilation(FileInfo file, params DisassembledFile[] references)
        public MergeableCompilation(FileInfo file)
        {
            Info = file;
            Info.Refresh();

            try
            {
                using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var schema = FileSchema.Create(fileStream);
                
                GlobalNamespace = new CompilationNamespace(schema.GlobalNamespace);

                foreach (var ns in schema.Namespaces
                    .Where(n => n != schema.GlobalNamespace))
                {
                    GlobalNamespace.Merge(new CompilationNamespace(ns));
                }
                
                //GlobalNamespace.Merge(
                    //Merge(references.Select(r => new MergeableCompilation(r.Info)).ToArray()).GlobalNamespace);

                Success = true;
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
            // TODO: Do an actual merge instead of appending to a root
            var compNamespace = new CompilationNamespace("Root");
            compNamespace.Namespaces.Add(GlobalNamespace);
            
            foreach (var comp in others)
                compNamespace.Namespaces.Add(comp.GlobalNamespace);

            return new MergeableCompilation(compNamespace);
        }
    }
}