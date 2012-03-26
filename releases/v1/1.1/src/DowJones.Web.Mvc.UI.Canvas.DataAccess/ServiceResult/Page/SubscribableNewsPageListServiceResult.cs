// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscribableNewsPageListServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using AccessQualifier = DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page.AccessQualifier;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;


namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page
{
    [DataContract(Name = "subscribableNewsPagesListServiceResult", Namespace = "")]
    public class SubscribableNewsPagesListServiceResult : AbstractServiceResult, IPopulate<SubscribableNewsPagesListRequest>
    {
        [DataMember(Name = "package")]
        public SubscribableNewsPagesListPackage Package { get; set; }

        public void Populate(ControlData controlData, SubscribableNewsPagesListRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                {
                    if (!request.IsValid())
                    {
                        throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                    }
                    
                    RecordTransaction(
                        "PageListManager.GetSubscribableNewsPagesList",
                        null,
                        manager =>
                        {
                            Package = new SubscribableNewsPagesListPackage
                            {
                                NewsPages = manager.GetSubscribableNewsPagesList(MapAccessQualifiers(request.AccessQualifiers), CreateMetadataFilter(request))
                            };
                        },
                        new PageListManager(controlData, preferences));
                },
                preferences);
        }

        private static MetadataFilter CreateMetadataFilter(SubscribableNewsPagesListRequest request)
        {
            var metadataFilter = new MetadataFilter();

            if (request.FilterType != FilterType.Unspecified)
            {
                metadataFilter.SearchFilterTypeCollection = GetSearchFilterTypeCollection(request.FilterType);
                if (!string.IsNullOrEmpty(request.FilterValue) && request.FilterValue.ToLower() != "all")
                {
                    var metadataFieldCollection = new MetadataFieldCollection
                                                      {
                                                          new MetadataField
                                                              {
                                                                  Text = request.FilterValue
                                                              }
                                                      };
                    switch (request.FilterType)
                    {
                        case FilterType.Author:
                            metadataFilter.Filter.AuthorCollection = metadataFieldCollection;
                            break;
                        case FilterType.Category:
                            metadataFilter.Filter.CategoryCollection = metadataFieldCollection;
                            break;
                        case FilterType.Company:
                            metadataFilter.Filter.CompanyCollection = metadataFieldCollection;
                            break;
                        case FilterType.Executive:
                            metadataFilter.Filter.ExecutiveCollection = metadataFieldCollection;
                            break;
                        case FilterType.Industry:
                            metadataFilter.Filter.IndustryCollection = metadataFieldCollection;
                            break;
                        case FilterType.NewsSubject:
                            metadataFilter.Filter.NewsSubjectCollection = metadataFieldCollection;
                            break;
                        case FilterType.Region:
                            metadataFilter.Filter.RegionCollection = metadataFieldCollection;
                            break;
                        case FilterType.Source:
                            metadataFilter.Filter.SourceCollection = metadataFieldCollection;
                            break;
                        case FilterType.Topic:
                            metadataFilter.Filter.TopicCollection = metadataFieldCollection;
                            break;
                    }
                }
            }

            return metadataFilter;
        }

        private static SearchFilterTypeCollection GetSearchFilterTypeCollection(FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Author:
                    return new SearchFilterTypeCollection { MetadataFilterType.AllAuthor };
                case FilterType.Category:
                    return new SearchFilterTypeCollection { MetadataFilterType.AllCategory };
                case FilterType.Company:
                    return new SearchFilterTypeCollection { MetadataFilterType.AllCompany };
                case FilterType.Executive:
                    return new SearchFilterTypeCollection { MetadataFilterType.AllExecutive };
                case FilterType.Industry:
                    return new SearchFilterTypeCollection { MetadataFilterType.AllIndustry };
                case FilterType.NewsSubject:
                    return new SearchFilterTypeCollection { MetadataFilterType.AllNewsSubject };
                case FilterType.Region:
                    return new SearchFilterTypeCollection { MetadataFilterType.AllRegion };
                case FilterType.Source:
                    return new SearchFilterTypeCollection { MetadataFilterType.AllSource };
                case FilterType.Topic:
                    return new SearchFilterTypeCollection { MetadataFilterType.AllTopic };
                default:
                    return null;
            }
        }

        private static List<Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier> MapAccessQualifiers(IEnumerable<AccessQualifier> accessQualifiers)
        {
            var gatewayAccessQualifiers = new List<Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier>();
            foreach (var accessQualifier in accessQualifiers)
            {
                switch (accessQualifier)
                {
                    case AccessQualifier.Account:
                        gatewayAccessQualifiers.Add(Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Account);
                        break;
                    case AccessQualifier.User:
                        gatewayAccessQualifiers.Add(Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.User);
                        break;
                    case AccessQualifier.Factiva:
                        gatewayAccessQualifiers.Add(Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Factiva);
                        break;
                }
            }

            return gatewayAccessQualifiers;
        }
    }
}
