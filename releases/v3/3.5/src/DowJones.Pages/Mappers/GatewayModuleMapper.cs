using System;
using DowJones.DependencyInjection;
using DowJones.Pages.Common;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Module = DowJones.Pages.Modules.Module;
using ModuleProperties = DowJones.Pages.Modules.ModuleProperties;

namespace DowJones.Pages.Mappers
{
    public abstract class GatewayModuleMapper<TGatewayModule, TModule> : Mapping.TypeMapper<TGatewayModule, Module>
        where TGatewayModule : Factiva.Gateway.Messages.Assets.Pages.V1_0.Module
        where TModule : Module
    {
        protected internal static Func<TModule> CreateModuleThunk = ServiceLocator.Resolve<TModule>;

        public override Module Map(TGatewayModule source)
        {
            var module = MapInternal(source);
            
            Populate(ref module, source);

            return module;
        }

        protected virtual TModule MapInternal(TGatewayModule source)
        {
            var module = CreateModuleThunk();

            module.Description = source.Description;
            module.Id = source.Id;
            module.LastModifiedDate = source.LastModifiedDate;
            module.Position = source.Position;
            module.Title = source.Title;
            //TODO: NN - really bad work around here. Because Mapper.Map throws error when source is null. It should return null if source is null though.
            module.QueryFilterSet = source.QueryFilters == null ? null : Mapper.Map<QueryFilterSet>(source.QueryFilters);
            

            var moduleEx = source as Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleEx;
            if (moduleEx != null)
            {
                var accessQualifier = Mapper.Map<AccessQualifier>(moduleEx.ModuleQualifier);
                var publishStatusScope = Mapper.Map<PublishStatusScope>(moduleEx.ShareProperties.AccessControlScope);

                if (moduleEx.ShareProperties is SharePropertiesResponse)
                {
                    module.RootId = ((SharePropertiesResponse) moduleEx.ShareProperties).RootID;
                }

                module.ModuleQualifier = accessQualifier;
                module.ModuleProperties = new ModuleProperties
                                              {
                                                  PublishStatusScope = publishStatusScope,
                                                  ModuleMetaData = GetModuleMetadata(moduleEx.ModuleProperties.ModuleMetaData)
                                              };
            }

            return module;
        }

        private MetaData GetModuleMetadata(Metadata moduleMetaData)
        {
            if (moduleMetaData == null)
                return null;
            if (moduleMetaData.IndustryCollection != null && moduleMetaData.IndustryCollection.Count > 0)
            {
                return new MetaData
                           {
                               MetaDataCode = moduleMetaData.IndustryCollection[0].Text,
                               MetaDataType = MetaDataType.Industry
                           };
            }
            if (moduleMetaData.RegionCollection != null && moduleMetaData.RegionCollection.Count > 0)
            {
                return new MetaData
                {
                    MetaDataCode = moduleMetaData.RegionCollection[0].Text,
                    MetaDataType = MetaDataType.Geographic
                };
            }
            return null;
        }

        protected abstract void Populate(ref TModule module, TGatewayModule source);
    }
}