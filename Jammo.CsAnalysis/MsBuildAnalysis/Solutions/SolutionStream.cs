using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Jammo.CsAnalysis.MsBuildAnalysis.Projects;

namespace Jammo.CsAnalysis.MsBuildAnalysis.Solutions
{
    public sealed class SolutionStream : IParserStream, IDisposable
    {
        private FileStream stream;
        
        private List<ProjectDefinition> projects = new();
        private List<GlobalDefinition> globals = new();
        
        public bool IsInitialized => stream == null;
        public string FilePath => stream?.Name;
        
        public FormatVersion Version;
        
        public ProjectDefinition[] Projects => projects.ToArray();
        public GlobalDefinition[] Globals => globals.ToArray();

        public GlobalSectionDefinition[] GlobalSections => globals
            .SelectMany(s => s.Sections)
            .ToArray();
        public GlobalConfiguration[] Configurations => GlobalSections
            .SelectMany(c => c.Configurations)
            .ToArray();

        public SolutionStream(FileStream stream = null)
        {
            this.stream = stream;
        }

        public void AddProject(ProjectDefinition project)
        {
            projects.Add(project);
        }

        public void RemoveProject(string guid)
        {
            projects.Remove(projects.First(p => p.ProjectGuid == guid));   
        }

        public void AddGlobal(GlobalDefinition global)
        {
            globals.Add(global);
        }

        public void RemoveGlobal(int index)
        {
            globals.RemoveAt(index);
        }

        public void Parse()
        {
            if (stream == null)
                throw new IOException("Cannot parse an uninitialized file stream");
            
            var reader = new StreamReader(stream);
            var data = SolutionParser.Parse(reader.ReadToEndAsync().Result);

            projects = data.projects;
            globals = data.globals;
            Version = data.Version;
        }

        public void Write()
        {
            if (stream == null)
            {
                var working = Directory.GetCurrentDirectory();

                Console.WriteLine("The current stream is null, a new file will be created in the working directory." +
                                  $"Current working directory: {working}");
                
                stream = File.Create(Path.Join(Directory.GetCurrentDirectory(), "Jammo_SolutionStream.sln"));
            }
            
            using var writer = new StreamWriter(stream);
            stream.SetLength(0);

            writer.Write(ToString());
        }
        
        public void WriteTo(string path)
        {
            using var file = File.Create(path);
            using var writer = new StreamWriter(file);
            
            file.SetLength(0);
            writer.Write(ToString());
        }

        public void Dispose()
        {
            stream?.Dispose();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (Version != null)
                builder.AppendLine(Version.ToFormattedString());
            
            foreach (var project in Projects)
                builder.AppendLine(project.ToFormattedString());

            foreach (var global in globals)
                builder.AppendLine(global.ToFormattedString());

            return builder.ToString();
        }
    }
}