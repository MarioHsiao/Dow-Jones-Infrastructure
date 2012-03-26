using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.DependencyInjection;
using DowJones.Mapping;
using FactivaModule=Factiva.Gateway.Messages.Assets.Pages.V1_0.Module;

namespace DowJones.Web.Mvc.UI.Canvas.Mapping
{
    public class FactivaModuleModelMapper
    {
        private readonly GenericTypeMapper _mapper;

        public FactivaModuleModelMapper(IEnumerable<FactivaModuleModelMapping> mappings)
        {
            _mapper = new GenericTypeMapper(mappings.Select(x => x.GenericTypeMapping));
        }


        public IModule MapFactivaModule(FactivaModule factivaModule)
        {
            Guard.IsNotNull(factivaModule, "factivaModule");

            var factivaModuleType = factivaModule.GetType();
            var moduleType = ModuleModelFor(factivaModuleType);

            if (moduleType == null)
                throw new FactivaModuleModelMappingNotFoundException(factivaModuleType);

            var module = CreateModuleFor(moduleType);
            module.Asset = factivaModule;

            return module;
        }

        public IEnumerable<IModule> MapFactivaModules(IEnumerable<FactivaModule> moduleCollection)
        {
            Guard.IsNotNull(moduleCollection, "moduleCollection");

            return moduleCollection
                    .Where(x => x != null)
                    .Select(MapFactivaModule);
        }

        public Type ModuleModelFor(Type factivaModuleType)
        {
            Guard.IsNotNull(factivaModuleType, "factivaModuleType");

            return _mapper.GetDeclaringTypeByGenericType(factivaModuleType);
        }

        public Type FactivaModuleFor(Type moduleType)
        {
            Guard.IsNotNull(moduleType, "moduleType");

            return _mapper.GetGenericTypeByDeclaringType(moduleType);
        }


        // Wrapped in its own virtual method to allow overriding
        // to avoid ServiceLocator call if desired
        protected internal virtual Module CreateModuleFor(Type moduleType)
        {
            return ServiceLocator.Resolve<Module>(moduleType);
        }
    }
}
