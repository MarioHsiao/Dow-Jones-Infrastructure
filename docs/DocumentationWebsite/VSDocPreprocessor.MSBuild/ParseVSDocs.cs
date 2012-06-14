using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace VSDocPreprocessor.MSBuild
{
    public class ParseVSDocs : Task
    {
        private readonly TypeWriter _typeWriter;
        private readonly Processor _processor;

        [Required]
        public ITaskItem[] DocFiles { get; set; }

        public ITaskItem OutDir
        {
            get { return new TaskItem(_typeWriter.OutputDirectory); }
            set { _typeWriter.OutputDirectory = value.ItemSpec; }
        }


        public ParseVSDocs()
        {
            _typeWriter = new TypeWriter();
            _processor = new Processor(typeWriter: _typeWriter);
        }


        public override bool Execute()
        {
            try
            {
                var vsDocFiles = GetVSDocFilenames();

                if(vsDocFiles.Any())
                {
                    Log.LogMessage(MessageImportance.High, "Transforming VSDoc files: {0}", string.Join(", ", vsDocFiles));
                    InitializeOutputDirectory();
                    _processor.TransformVSDocFiles(vsDocFiles);
                }
                else
                {
                    Log.LogWarning("No VSDoc files found - skipping...");
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex);
                return false;
            }
        }

        private void InitializeOutputDirectory()
        {
            var outputDirectory = OutDir.ItemSpec;

            if (Directory.Exists(outputDirectory)) 
                return;

            Log.LogMessage("Output directory {0} does not exist - creating...", outputDirectory);
            Directory.CreateDirectory(outputDirectory);
        }

        private IEnumerable<string> GetVSDocFilenames()
        {
            var vsDocFiles = DocFiles.Select(x => x.ItemSpec).ToArray();

            if (!vsDocFiles.Any())
            {
                Log.LogMessage("No VSDoc files specified - discovering from current directory");
                var currentDirectory = Directory.GetCurrentDirectory();
                vsDocFiles = Directory.GetFiles(currentDirectory, "*.xml");
            }

            var existingVSDocFiles = vsDocFiles.Where(File.Exists).ToArray();
            var nonExistingVSDocFiles = vsDocFiles.Except(existingVSDocFiles).ToArray();

            if(nonExistingVSDocFiles.Any())
                Log.LogWarning("Skipping the following non-existent files: {0}", string.Join(", ", nonExistingVSDocFiles));

            return existingVSDocFiles;
        }
    }
}
