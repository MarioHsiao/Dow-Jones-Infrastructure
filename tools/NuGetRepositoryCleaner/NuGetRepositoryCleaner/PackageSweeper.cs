using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NuGet.RepositoryCleaner
{
    public class PackageSweeper
    {
        private readonly IPackageArchiver _archiver;

        public int VersionsToKeep { get; set; }


        public PackageSweeper(IPackageArchiver archiver, int versionsToKeep)
        {
            VersionsToKeep = versionsToKeep;
            _archiver = archiver;
        }

        public void Sweep(string folder)
        {
			if (!Debugger.IsAttached)
				Debugger.Launch();

            var rawPackages =
                from fullPath in Directory.GetFiles(folder, "*.nupkg")
                let filename = Path.GetFileName(fullPath)
                let parts = Regex.Match(filename, @"(?<Name>(?:[a-zA-Z]+[0-9]*\.?)+)\.(?<Version>[0-9.]*).nupkg").Groups
                let name = parts["Name"].Value.ToLower()
                let version = new Version(parts["Version"].Value)		// do a proper check so that 3.3.9 doesn't come ahead of 3.3.25 (e.g.)
                select new { filename = fullPath, version, name };

            var packages =
                rawPackages
                    .GroupBy(x => x.name)
                    .Select(x => new {Name = x.Key, Versions = x.ToArray()});

            var filesToArchive = new List<string>();

            foreach(var package in packages)
            {
                var versionsToKeep = package.Versions.OrderByDescending(x => x.version).Take(VersionsToKeep);
                var versionsToArchive = package.Versions.Except(versionsToKeep);
                filesToArchive.AddRange(versionsToArchive.Select(x => x.filename));
            }

            _archiver.Archive(filesToArchive);
        }
    }
}
