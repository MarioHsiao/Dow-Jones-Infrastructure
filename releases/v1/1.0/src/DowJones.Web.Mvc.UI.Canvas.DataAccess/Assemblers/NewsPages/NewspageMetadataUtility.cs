using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;
using Factiva.Gateway.V1_0;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.NewsPages
{
    public static class NewspageMetadataUtility
    {
        #region <<< Constructor and variables >>>
        private static readonly Dictionary<string, Dictionary<string, MetaData>> NewsPageModuleListNames = new Dictionary<string, Dictionary<string, MetaData>>();
        private static readonly Dictionary<string, Dictionary<string, MetaData>> PageModuleMetadataListNames = new Dictionary<string, Dictionary<string, MetaData>>();
        private static readonly Dictionary<string, Dictionary<string, MetaData>> PageCategoryListNames = new Dictionary<string, Dictionary<string, MetaData>>();

        private static DateTime updateTimeNewsPageModuleListNames = DateTime.Now.AddDays(-1);
        private static DateTime updateTimePageModuleMetadataListNames = DateTime.Now.AddDays(-1);
        private static DateTime updateTimePageCategoryListNames = DateTime.Now.AddDays(-1);
        #endregion

        #region <<< Public methods >>>
        public static string GetPageCategoryName(string code, string language)
        {
            var metaData = GetPageCategoryNameMetaData(code, language);
            return metaData == null ? code : metaData.MetaDataDescriptor;
        }

        public static MetaData GetPageCategoryNameMetaData(string code, string language)
        {
            return GetMetaData(PageCategoryListNames, code, language);
        }

        public static string GetMetaDataName(string code, string language)
        {
            var metaData = GetMetaDataMetaData(code, language);
            return metaData == null ? code : metaData.MetaDataDescriptor;
        }

        public static MetaData GetMetaDataMetaData(string code, string language)
        {
            return GetMetaData(PageModuleMetadataListNames, code, language);
        }

        public static string GetModuleName(string code, string language)
        {
            var metaData = GetModuleMetaData(code, language);
            return metaData == null ? code : metaData.MetaDataDescriptor;
        }

        public static MetaData GetModuleMetaData(string code, string language)
        {
            return GetMetaData(NewsPageModuleListNames, code, language);
        }

        public static List<MetaData> GetModulesMetaData(IEnumerable<string> codes, string language)
        {
            var metaDataList = new List<MetaData>();
            if (codes == null)
            {
                // return all
                metaDataList.AddRange(from module in NewsPageModuleListNames where module.Value.ContainsKey(language) select module.Value[language]);
            }
            else
            {
                metaDataList.AddRange(from code in codes where NewsPageModuleListNames.ContainsKey(code) && NewsPageModuleListNames[code].ContainsKey(language) select NewsPageModuleListNames[code][language]);
            }
            return metaDataList;
        }
        #endregion

        #region <<< Private methods >>>
        private static MetaData GetMetaData(Dictionary<string, Dictionary<string, MetaData>> dictionary, string code, string language)
        {
            // try to update
            UpdateNames(dictionary);
            return dictionary.ContainsKey(code) && dictionary[code].ContainsKey(language) ? dictionary[code][language] : null;
        }

        public static void UpdateNamesAll()
        {
            updateTimeNewsPageModuleListNames = DateTime.Now.AddTicks(-1);
            updateTimePageCategoryListNames = DateTime.Now.AddTicks(-1);
            updateTimePageModuleMetadataListNames = DateTime.Now.AddTicks(-1);
            UpdateNames(NewsPageModuleListNames);
            UpdateNames(PageModuleMetadataListNames);
            UpdateNames(PageCategoryListNames);
        }

        private static void UpdateNames(Dictionary<string, Dictionary<string, MetaData>> dictionary)
        {
            if (dictionary == NewsPageModuleListNames)
                UpdateNames(NewsPageModuleListNames, ref updateTimeNewsPageModuleListNames, ListType.NewsPageModuleList);
            else if (dictionary == PageModuleMetadataListNames)
                UpdateNames(PageModuleMetadataListNames, ref updateTimePageModuleMetadataListNames, ListType.PageModuleMetadataList);
            else if (dictionary == PageCategoryListNames)
                UpdateNames(PageCategoryListNames, ref updateTimePageCategoryListNames, ListType.PageCategoryList);
        }

        private static void UpdateNames(Dictionary<string, Dictionary<string, MetaData>> dictionary, ref DateTime updateTime, ListType listType)
        {
            if (DateTime.Now <= updateTime)
                return;

            lock (dictionary)
            {
                dictionary.Clear();
                GetListsDetailsListResponse newsPageModuleListResponse;
                try
                {
                    var newsPageModuleListRequest = new GetListsDetailsListRequest();
                    newsPageModuleListRequest.ListTypes.Add(listType);
                    newsPageModuleListResponse = Process<GetListsDetailsListResponse>(newsPageModuleListRequest);
                }
                catch (Exception)
                {
                    // if error, something is wrong with PAM
                    // set update time to be 10 mins from now
                    updateTime = DateTime.Now.AddMinutes(10);
                    return;
                }

                if (newsPageModuleListResponse != null)
                {
                    foreach (var listDetailsItem in newsPageModuleListResponse.ListDetailsItems) //var listItem in newsPageModuleListResponse.ListDetailsItems.SelectMany(listDetailsItem => listDetailsItem.List.Items))
                    {
                        var listItem = listDetailsItem.List.Items;
                        var metaDataType = MapCustomCodeToMetadaType(listDetailsItem.List.CustomCode);
                        foreach (var item in listItem)
                        {
                            AddItem(dictionary, item, metaDataType);
                        }
                    }
                }

                updateTime = DateTime.Now.AddDays(1);
            }
        }

        private static MetaDataType MapCustomCodeToMetadaType(string customCode)
        {
            switch (customCode.ToLower())
            {
                case "top":
                    return MetaDataType.Topic;
                case "reg":
                    return MetaDataType.Geographic;
                case "ind":
                    return MetaDataType.Industry;
                case "mod":
                    return MetaDataType.Custom;
            }
            return MetaDataType.Industry;
        }

        private static void AddItem(Dictionary<string, Dictionary<string, MetaData>> dictionary, ListItem listItem, MetaDataType metaDataType)
        {
            //dictionary.Add(DateTime.Now.Ticks.ToString(), new Dictionary<string, MetaData>());
            if (listItem is PageCategoryListItem)
            {
                var convertedListItem = (PageCategoryListItem) listItem;
                AddItem(dictionary, convertedListItem.ItemCode, convertedListItem.ItemsCollection, metaDataType);
            }
            else if (listItem is NewsPageModuleListItem)
            {
                var convertedListItem = (NewsPageModuleListItem)listItem;
                AddItem(dictionary, convertedListItem.ItemCode, convertedListItem.ItemsCollection, metaDataType);
            }
            else if (listItem is PageModuleMetadataListItem)
            {
                var convertedListItem = (PageModuleMetadataListItem)listItem;
                AddItem(dictionary, convertedListItem.ItemCode, convertedListItem.ItemsCollection, metaDataType);
            }
        }

        static void AddItem(Dictionary<string, Dictionary<string, MetaData>> dictionary, string code, IEnumerable<LanguageBasedItem> languageBasedItems, MetaDataType metaDataType)
        {
            if (!dictionary.ContainsKey(code))
                dictionary.Add(code, new Dictionary<string, MetaData>());

            var languagesDictionary = dictionary[code];

            foreach(var languageBasedItem in languageBasedItems)
            {
                var languageCode = languageBasedItem.LanguageCode.ToString();
                var metaData = new MetaData
                                   {
                                       MetaDataCode = code,
                                       MetaDataDescription = string.IsNullOrEmpty(languageBasedItem.Description) ? null : languageBasedItem.Description.Trim(),
                                       MetaDataDescriptor = string.IsNullOrEmpty(languageBasedItem.Title) ? null : languageBasedItem.Title.Trim(),
                                       MetaDataType = metaDataType
                                   };
                if (!languagesDictionary.ContainsKey(languageCode))
                    languagesDictionary.Add(languageCode, metaData);
                languagesDictionary[languageCode] = metaData;
            }
        }

        private static T Process<T>(object request)
        {
            try
            {
                var controlData = ControlDataManager.GetLightWeightUserControlData(Settings.Default.DataAccessProxyUser);
                var sr = FactivaServices.Invoke<T>(ControlDataManager.Clone(controlData, true), request);

                if (sr == null)
                {
                    throw new DowJonesUtilitiesException(new NullReferenceException("ServiceResponse is null"), -1);
                }

                if (sr.rc != 0)
                {
                    throw new DowJonesUtilitiesException("Error in Metadata utility", sr.rc);
                }

                return sr.ObjectResponse;
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
            }
        }
        #endregion
    }
}
