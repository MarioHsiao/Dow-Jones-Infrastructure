// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsPages.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.NewsPages;
using DowJones.Web.Mvc.UI.Models.NewsPages.Modules;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using AlertsNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.AlertsNewspageModule;
using CompanyOverviewNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.CompanyOverviewNewspageModule;
using CustomTopicsNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.CustomTopicsNewspageModule;
using NewsstandNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsstandNewspageModule;
using RadarNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.RadarNewspageModule;
using RegionalMapNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.RegionalMapNewspageModule;
using SourcesNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.SourcesNewspageModule;
using SummaryNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.SummaryNewspageModule;
using SyndicationNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.SyndicationNewspageModule;
using TopNewsNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.TopNewsNewspageModule;
using TrendingNewsPageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.TrendingNewsPageModule;

namespace DowJones.Web.Mvc.UI.Models.NewsPages
{
    public class IRTCodeAttribute : Attribute
    {
        public readonly string Code;

        public IRTCodeAttribute(string code)
        {
            Code = code;
        }
    }

    public enum ModuleType
    {
        [EnumMember]
        [IRTCode("malt")]
        AlertsNewspageModule, 

        [EnumMember]
        [IRTCode("mrss")]
        SyndicationNewspageModule, 

        [EnumMember]
        [IRTCode("mnews")]
        NewsstandNewspageModule, 

        [EnumMember]
        [IRTCode("mcus")]
        CustomTopicsNewspageModule, 

        [EnumMember]
        [IRTCode("msrc")]
        SourcesNewspageModule, 

        [EnumMember]
        [IRTCode("mrad")]
        RadarNewspageModule, 

        [EnumMember]
        [IRTCode("mreg")]
        RegionalMapNewspageModule, 

        [EnumMember]
        [IRTCode("mtop")]
        TopNewsNewspageModule, 

        [EnumMember]
        [IRTCode("mtre")]
        TrendingNewsPageModule, 

        [EnumMember]
        [IRTCode("msum")]
        SummaryNewspageModule, 

        [EnumMember]
        [IRTCode("mcom")]
        CompanyOverviewNewspageModule, 
    }

    public static class ModuleTypeHelper
    {
        public static ModuleType GetModuleType(Factiva.Gateway.Messages.Assets.Pages.V1_0.Module module)
        {
            if (module is AlertsNewspageModule)
                return ModuleType.AlertsNewspageModule;

            if (module is CompanyOverviewNewspageModule)
                return ModuleType.CompanyOverviewNewspageModule;

            if (module is CustomTopicsNewspageModule)
                return ModuleType.CustomTopicsNewspageModule;

            if (module is NewsstandNewspageModule)
                return ModuleType.NewsstandNewspageModule;

            if (module is RadarNewspageModule)
                return ModuleType.RadarNewspageModule;

            if (module is RegionalMapNewspageModule)
                return ModuleType.RegionalMapNewspageModule;

            if (module is SourcesNewspageModule)
                return ModuleType.SourcesNewspageModule;

            if (module is SummaryNewspageModule)
                return ModuleType.SummaryNewspageModule;

            if (module is SyndicationNewspageModule)
                return ModuleType.SyndicationNewspageModule;

            if (module is TopNewsNewspageModule)
                return ModuleType.TopNewsNewspageModule;

            if (module is TrendingNewsPageModule)
                return ModuleType.TrendingNewsPageModule;

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidModuleType);
        }

        public static string GetIRTCode(this ModuleType moduleType)
        {
            // using attribute
            try
            {
                var memberInfo = typeof(ModuleType).GetMember(moduleType.ToString())[0];
                var attribute = (IRTCodeAttribute)memberInfo.GetCustomAttributes(typeof(IRTCodeAttribute), false)[0];
                return attribute.Code;
            }
            catch (Exception)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidModuleType);
            }

            // No attribute, no reflection

            //switch (moduleType)
            //{
            //    case ModuleType.AlertsNewspageModule:
            //        return "malt";
            //    case ModuleType.SyndicationNewspageModule:
            //        return "mrss";
            //    case ModuleType.NewsstandNewspageModule:
            //        return "mnews";
            //    case ModuleType.CustomTopicsNewspageModule:
            //        return "mcus";
            //    case ModuleType.SourcesNewspageModule:
            //        return "msrc";
            //    case ModuleType.RadarNewspageModule:
            //        return "mrad";
            //    case ModuleType.RegionalMapNewspageModule:
            //        return "mreg";
            //    case ModuleType.TopNewsNewspageModule:
            //        return "mtop";
            //    case ModuleType.TrendingNewsPageModule:
            //        return "mtre";
            //    case ModuleType.SummaryNewspageModule:
            //        return "msum";
            //    case ModuleType.CompanyOverviewNewspageModule:
            //        return "mcom";
            //    default:
            //        throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidModuleType);
            //}
        }
    }


    /// <summary>
    /// The meta data type.
    /// </summary>
    public enum MetaDataType
    {
        [EnumMember] Industry,
        [EnumMember] Geographic,
        [EnumMember] Topic,
        [EnumMember] Custom,
    }

    /// <summary>
    /// The meta data.
    /// </summary>
    [DataContract(Name = "metaData", Namespace = "")]
    public class MetaData
    {
        [DataMember(Name = "metaDataType")]
        public MetaDataType MetaDataType { get; set; }

        [DataMember(Name = "metaDataCode")]
        public string MetaDataCode { get; set; }

        [DataMember(Name = "metaDataDescriptor")]
        public string MetaDataDescriptor { get; set; }

        [DataMember(Name = "metaDataDescription")]
        public string MetaDataDescription { get; set; }
    }

    /// <summary>
    /// The admin publish status.
    /// </summary>
    public enum AdminPublishStatus
    {
        [EnumMember] Published,
        [EnumMember] UnPublished,
    }

    public enum PublishStatusScope
    {
        [EnumMember] Personal,
        [EnumMember] Account,
        [EnumMember] Global,
    }

    public enum AccessQualifier
    {
        [EnumMember] User,
        [EnumMember] Account,
        [EnumMember] Global, 
    }

    public enum AccessScope
    {
        [EnumMember] OwnedByUser,
        [EnumMember] AssignedToUser,
        [EnumMember] SubscribedByUser,
        [EnumMember] UnSpecified,
    }

    [DataContract(Name = "pageCategoryInfo", Namespace = "")]
    public class CategoryInfo
    {
        [DataMember(Name = "categoryDescriptor")]
        public string CategoryDescriptor { get; set; }

        [DataMember(Name = "categoryCode")]
        public string CategoryCode { get; set; }
    }

    [DataContract(Name = "newsPage", Namespace = "")]
    public class AdminNewsPage : NewsPage
    {
        [DataMember(Name = "adminPublishStatus")]
        public AdminPublishStatus AdminPublishStatus { get; set; }
    }

    [DataContract(Name = "newsPage", Namespace = "")]
    public class NewsPage
    {
        public NewsPage()
        {
           // ModuleCollection = new List<NewsPageModule>();
        }

        [DataMember(Name = "parentId")]
        public string ParentID { get; set; }

        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "modules", EmitDefaultValue = false)]
        public List<NewsPageModule> ModuleCollection { get; set; }

        [DataMember(Name = "isActive")]
        public bool IsActive { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "metaDataInfo")]
        public MetaData MetaData { get; set; }

        [DataMember(Name = "pageCategoryInfo")]
        public CategoryInfo CategoryInfo { get; set; }

        [DataMember(Name = "accessQualifier")]
        public AccessQualifier AccessQualifier { get; set; }

        [DataMember(Name = "accessScope")]
        public AccessScope AccessScope { get; set; }

        [DataMember(Name = "publishStatusScope")]
        public PublishStatusScope PublishStatusScope { get; set; }

        [DataMember(Name = "lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [DataMember(Name = "ownerUserId")]
        public string OwnerUserId { get; set; }

        [DataMember(Name = "ownerNamespace")]
        public string OwnerNamespace { get; set; }
    }

    public class NewsPageModuleFactory
    {
        /// <summary>
        /// The get module by factiva type.
        /// </summary>
        /// <param name="moduleEx">The module ex.</param>
        /// <param name="controlData"></param>
        /// <param name="preferences">The preferences.</param>
        /// <returns></returns>
        public static NewsPageModule GetModuleByFactivaType(ModuleEx moduleEx, Factiva.Gateway.Utils.V1_0.ControlData controlData, IPreferences preferences)
        {
            var factivaTypeDict = new Dictionary<Type, int>
                                      {
                                          { typeof(AlertsNewspageModule), 0 }, 
                                          { typeof(CustomTopicsNewspageModule), 1 }, 
                                          { typeof(NewsstandNewspageModule), 2 }, 
                                          { typeof(RadarNewspageModule), 3 }, 
                                          { typeof(SourcesNewspageModule), 4 }, 
                                          { typeof(SyndicationNewspageModule), 5 }, 
                                          { typeof(TopNewsNewspageModule), 6 }, 
                                          { typeof(TrendingNewsPageModule), 7 }, 
                                          { typeof(CompanyOverviewNewspageModule), 8 }, 
                                          { typeof(RegionalMapNewspageModule), 9 },
                                          { typeof(SummaryNewspageModule), 10 },
                                      };

            // Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleEx 
            var assembler = new NewsPageModuleAssembler(controlData, preferences);

            switch (factivaTypeDict[moduleEx.GetType()])
            {
                case 0:
                    return assembler.Convert((AlertsNewspageModule)moduleEx);
                case 1:
                    return assembler.Convert((CustomTopicsNewspageModule)moduleEx);
                case 2:
                    return assembler.Convert((NewsstandNewspageModule)moduleEx);
                case 3:
                    return assembler.Convert((RadarNewspageModule)moduleEx);
                case 4:
                    return assembler.Convert((SourcesNewspageModule)moduleEx);
                case 5:
                    return assembler.Convert((SyndicationNewspageModule)moduleEx);
                case 6:
                    return assembler.Convert((TopNewsNewspageModule)moduleEx);
                case 7:
                    return assembler.Convert((TrendingNewsPageModule)moduleEx);
                case 8:
                    return assembler.Convert((CompanyOverviewNewspageModule)moduleEx);
                case 9:
                    return assembler.Convert((RegionalMapNewspageModule)moduleEx);
                case 10:
                    return assembler.Convert((SummaryNewspageModule)moduleEx);
                default:
                    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleTypeNotFoundInDictionary);
            }
        }

        /// <summary>
        /// The get news page.
        /// </summary>
        /// <param name="newsPage">The news page.</param>
        /// <returns></returns>
        public static NewsPage GetNewsPage(Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsPage newsPage)
        {
            var assembler = new NewsPageAssembler();
            return assembler.Convert(newsPage);
        }

        // public static CategoryInfo GetCategoryInfo(Factiva.Gateway.Messages.Assets.Pages.V1_0.ca)
        // {
        // Guard.IsNotNull(newsPage, "newsPage");
        // }
    }

    [DataContract(Name = "newsPageListWithNewsPage", Namespace = "")]
    public class NewsPageListWithNewsPage
    {
        [DataMember(Name = "newsPages")]
        public List<NewsPage> NewsPages { get; set; }

        [DataMember(Name = "requestedNewsPage")]
        public NewsPage RequestedNewsPage { get; set; }
    }

    [DataContract(Name = "module", Namespace = "")]
    public class Module
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "moduleType")]
        public ModuleType ModuleType { get; set; }

        [DataMember(Name = "isActive")]
        public bool IsActive { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }

        [DataMember(Name = "metaData ")]
        public MetaDataType MetaData { get; set; }

        [DataMember(Name = "accessQualifier")]
        public AccessQualifier AccessQualifier { get; set; }
    }

    [DataContract(Name = "pagePosition", Namespace = "")]
    public class PagePosition
    {
        [DataMember(Name = "pageId")]
        public string PageId { get; set; }

        [DataMember(Name = "position")]
        public int Position { get; set; }
    }
}
