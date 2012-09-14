using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DowJones.Web.ClientResources
{
    public class LocalFileClientResourceRepository : IClientResourceRepository
    {
        private readonly ClientResourceConfiguration _config;
        private readonly Lazy<IEnumerable<ClientResource>> _clientResources;

        protected internal string RootDirectory
        {
            get
            {
                return _rootDirectory
                       ?? HttpContext.Current.Server.MapPath("~/");
            }
            set { _rootDirectory = value; }
        }
        private string _rootDirectory;

        public LocalFileClientResourceRepository(ClientResourceConfiguration config)
        {
            _config = config;
            _clientResources = new Lazy<IEnumerable<ClientResource>>(DiscoverClientResources);
        }

        public IEnumerable<ClientResource> GetClientResources()
        {
            return _clientResources.Value;
        }

        private IEnumerable<ClientResource> DiscoverClientResources()
        {
            IEnumerable<ClientResource> specificMappedResources = GetSpecificMappedResources();
            IEnumerable<ClientResource> directoryMappedResources = GetDirectoryMappedResources();

            return specificMappedResources.Union(directoryMappedResources);
        }

        private IEnumerable<ClientResource> GetSpecificMappedResources()
        {
            return _config.Resources().Select(Mapper.Map<ClientResource>);
        }

        private IEnumerable<ClientResource> GetDirectoryMappedResources()
        {
            var directoryMappings = _config.Directories;

            var resources =
                from mapping in directoryMappings
                let absolutePath =
                        (mapping.Path.StartsWith("~/"))
                            ? Path.Combine(RootDirectory, mapping.Path.Substring(2))
                            : mapping.Path
                from filename in Directory.GetFiles(mapping.Path)
                select new ClientResource(filename)
                           {
                               DependencyLevel = mapping.Level
                           };

            return resources;
        }

    }
}