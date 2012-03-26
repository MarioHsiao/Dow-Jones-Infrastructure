using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NuGet.RepositoryCleaner
{
    public class PackageArchiver : IPackageArchiver
    {
        public string ArchiveFolder { get; private set; }

        
        public PackageArchiver(string archiveFolder)
        {
            if (string.IsNullOrWhiteSpace(archiveFolder))
                throw new ArgumentOutOfRangeException("archiveFolder");

            ArchiveFolder = archiveFolder;
        }


        public void Archive(IEnumerable<string> filenames)
        {
            if (filenames == null || !filenames.Any())
            {
                Debug.WriteLine("No packages to archive!");
                return;
            }

            VerifyArchiveFolder();

            Debug.WriteLine(string.Format("Archiving files: {0}", string.Join(", ", filenames)));

            Debug.WriteLine(string.Format("Moving packages to archive folder {0}...", ArchiveFolder));

            foreach (var filename in filenames)
            {
                var archivedFilename = Path.Combine(ArchiveFolder, Path.GetFileName(filename));

                Debug.WriteLine(string.Format("Moving {0} to {1}...", filename, archivedFilename));
                File.Copy(filename, archivedFilename, true);
                Debug.WriteLine(string.Format("Deleting {0}...", filename));
                File.Delete(filename);
                Console.WriteLine("Archived {0}", filename);
            }
        }

        private void VerifyArchiveFolder()
        {
            if (Directory.Exists(ArchiveFolder))
                return;

            Debug.WriteLine(string.Format("Archive folder {0} does not exist - creating...", ArchiveFolder));
            Directory.CreateDirectory(ArchiveFolder);
        }
    }
}