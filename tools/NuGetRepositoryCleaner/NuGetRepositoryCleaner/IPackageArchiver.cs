using System.Collections.Generic;

namespace NuGet.RepositoryCleaner
{
    public interface IPackageArchiver
    {
        void Archive(IEnumerable<string> filenames);
    }
}