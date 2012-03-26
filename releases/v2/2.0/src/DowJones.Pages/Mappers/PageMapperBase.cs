using System.Collections.Generic;
using System.Linq;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Pages.DataAccess.Managers;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Module = DowJones.Pages.Modules.Module;
using DowJones.Pages.Common;

namespace DowJones.Pages.Mappers
{
    public abstract class PageMapperBase
    {
        protected virtual TPage CreatePageModel<TPage>(Factiva.Gateway.Messages.Assets.Pages.V1_0.Page source, PageProperties pageProperties, Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier pageQualifier)
            where TPage : Page, new()
        {
            var newsPage = CreatePageModel<TPage>(
                pageProperties,
                source.ShareProperties as SharePropertiesResponse,
                pageQualifier,
                source.Id.ToString()
            );

            //TODO: NN - really bad work around here. Because Mapper.Map throws error when source is null. It should return null if source is null though.
            newsPage.QueryFilterSet = source.QueryFilters == null ? null : Mapper.Map<QueryFilterSet>(source.QueryFilters);
            newsPage.ModuleCollection = source.ModuleCollection.Cast<ModuleEx>().Select(Mapper.Map<Module>).ToList();

            return newsPage;
        }

        protected virtual TPage CreatePageModel<TPage>(PageProperties pageProperties, SharePropertiesResponse pageSharePropertiesResponse, Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier pageQualifier, string id)
            where TPage : Page, new()
        {
            var hasShareProperties = pageSharePropertiesResponse != null;

            var newsPage = new TPage
            {
                AccessQualifier = Mapper.Map<AccessQualifier>(pageQualifier),
                // TODO: Map AccessScope
                AccessScope = hasShareProperties ? Mapper.Map<AccessScope>(pageSharePropertiesResponse.ShareType) : AccessScope.OwnedByUser,
                CreatedDate = DateTimeFormatter.ConvertToUtc(pageProperties.CreatedDate),
                Description = pageProperties.Description,
                ID = id,
                IsActive = hasShareProperties && pageSharePropertiesResponse.ShareStatus == ShareStatus.Active,
                LastModifiedDate = DateTimeFormatter.ConvertToUtc(pageProperties.LastModifiedDate),
                ModuleCollection = new List<Module>(),
                OwnerNamespace = pageProperties.CreatedByNamespace,
                OwnerUserId = pageProperties.CreatedBy,
                ParentID = hasShareProperties ? pageSharePropertiesResponse.RootID.ToString() : 0.ToString(),
                Position = pageProperties.Position,
                PublishStatusScope = pageSharePropertiesResponse != null ? Mapper.Map<PublishStatusScope>(pageSharePropertiesResponse.AccessControlScope) : PublishStatusScope.Personal,
                Title = pageProperties.Title,
            };

            return newsPage;
        }

        protected virtual List<MetaData> GetMetadataList(Metadata gatewayPageMetadata, string interfaceLanguage)
        {
            var metaDataList = new List<MetaData>();

            List<MetaData> symbologyCodes;
            foreach(var metadataField in gatewayPageMetadata.IndustryCollection)
            {
                metaDataList.Add(PageMetadataUtility.GetMetaDataMetaData(metadataField.Text, interfaceLanguage, out symbologyCodes));
                if (symbologyCodes != null)
                    metaDataList.AddRange(symbologyCodes);
            }

            foreach (var metadataField in gatewayPageMetadata.RegionCollection)
            {
                metaDataList.Add(PageMetadataUtility.GetMetaDataMetaData(metadataField.Text, interfaceLanguage, out symbologyCodes));
                if (symbologyCodes != null)
                    metaDataList.AddRange(symbologyCodes);
            }

            //metaDataList.AddRange(gatewayPageMetadata.QueryFilterCollection.Select(code => new MetaData
            //{
            //    MetaDataCode = code.Text,
            //    MetaDataType = MetaDataType.Topic,
            //}));

            //metaDataList.AddRange(gatewayPageMetadata.FreeTextCollection.Select(code => new MetaData
            //{
            //    MetaDataCode = code.Text,
            //    MetaDataType = MetaDataType.Custom,
            //}));

            return metaDataList;
        }

    }
}
