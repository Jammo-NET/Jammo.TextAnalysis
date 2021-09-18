using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Jammo.ParserTools;

namespace Jammo.TextAnalysis.DotNet.MsBuild.Solutions
{
    public sealed class SolutionStream : IParserStream
    {
        private FileStream stream;
        
        public bool IsInitialized => stream == null;
        public string FilePath => stream?.Name;
        
        public FormatVersion Version;
        
        public readonly List<ProjectDefinition> Projects = new();
        public readonly List<GlobalDefinition> Globals = new();

        public GlobalSectionDefinition[] GlobalSections => Globals
            .SelectMany(s => s.Sections)
            .ToArray();
        public GlobalConfiguration[] Configurations => GlobalSections
            .SelectMany(c => c.Configurations)
            .ToArray();

        public SolutionStream(FileStream stream = null)
        {
            this.stream = stream;
        }

        [Obsolete("User Projects.Add instead")]
        public void AddProject(ProjectDefinition project)
        {
            Projects.Add(project);
        }

        [Obsolete("Use Projects.Remove instead")]
        public void RemoveProject(string guid)
        {
            Projects.Remove(Projects.First(p => p.ProjectGuid == guid));   
        }

        [Obsolete("Use Globals.Add instead")]
        public void AddGlobal(GlobalDefinition global)
        {
            Globals.Add(global);
        }

        [Obsolete("Use Globals.RemoveAt instead")]
        public void RemoveGlobal(int index)
        {
            Projects.RemoveAt(index);
        }

        [Obsolete("Use SolutionParser.Parse instead")]
        public void Parse()
        {
            if (stream == null)
                throw new IOException("Cannot parse an uninitialized file stream");
            
            using var reader = new StreamReader(stream);
            var data = SolutionParser.Parse(reader.ReadToEndAsync().Result);

            Projects.AddRange(data.Projects);
            Globals.AddRange(data.Globals);
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

            foreach (var global in Globals)
                builder.AppendLine(global.ToFormattedString());

            return builder.ToString();
        }
    }
}