using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NuGet.RepositoryCleaner
{
    public class Program
    {
        public void Run(string packageFolder, string archiveFolder, bool testRun)
        {
            if (string.IsNullOrWhiteSpace(packageFolder))
                packageFolder = Environment.CurrentDirectory;

            IPackageArchiver archiver;

            if (testRun)
                archiver = new TestPackageArchiver();
            else 
                archiver = new PackageArchiver(archiveFolder);
            
            new PackageSweeper(archiver, 5).Sweep(packageFolder);
        }


        public static int Main(string[] args)
        {
            var packageFolder = args.FirstOrDefault() ?? Environment.CurrentDirectory;
            var archiveFolder = args.Skip(1).FirstOrDefault() ?? Path.Combine(packageFolder, "../archive");
            var testRun = args.Skip(2).FirstOrDefault();

            new Program().Run(packageFolder, archiveFolder, !string.IsNullOrWhiteSpace(testRun));

            return 0;
        }
    }

    public class TestPackageArchiver : IPackageArchiver
    {
        public void Archive(IEnumerable<string> filenames)
        {
            Console.WriteLine("Would have archived the following packages:");

            foreach(var filename in filenames)
                Console.WriteLine(filename);
        }
    }
}
