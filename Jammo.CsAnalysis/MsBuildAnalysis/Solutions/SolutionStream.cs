using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jammo.CsAnalysis.MsBuildAnalysis.Solutions
{
    public sealed class SolutionStream : IDisposable
    {
        private FileStream stream;

        private List<ProjectDefinition> projects = new();
        private List<CsProjectConfiguration> configurations = new();
        
        public bool IsInitialized => stream == null;
        
        public FormatVersion Version;
        public ProjectDefinition[] Projects => projects.ToArray();
        public CsProjectConfiguration[] Configurations => configurations.ToArray();

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
            
        }

        public void AddConfiguration(CsProjectConfiguration configuration)
        {
            
        }

        public void RemoveConfiguration(string guid)
        {
            
        }

        public void Parse()
        {
            if (stream == null)
                throw new IOException("Cannot parse an uninitialized file stream");
            
            var reader = new StreamReader(stream);
            var data = SolutionParser.Parse(reader.ReadToEndAsync().Result);
            
            
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
            
            var writer = new StreamWriter(stream);
            stream.SetLength(0);

            writer.Write(ToString());
        }
        
        public void WriteTo(string path)
        {
            var file = File.Create(path);
            var writer = new StreamWriter(file);
            
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
            
            builder.AppendLine(Version.ToFormattedString());
            
            foreach (var project in Projects)
                builder.AppendLine(project.ToFormattedString());
            
            // TODO: Create globals and stuff

            return builder.ToString();
        }
    }

    public class CsProjectConfiguration
    {
        public ConfigurationType Type;
        public ConfigurationArchitecture Architecture;
    }

    public enum ConfigurationType
    {
        Debug,
        Release
    }
    
    public enum ConfigurationArchitecture
    {
        AnyCpu
    }
}