//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DowJones.Session;
//using Factiva.Gateway.Messages.Symbology.Taxonomy.V1_0;
//using Factiva.Gateway.Messages.Symbology.V1_0;

//namespace DowJones.Pages.Pages
//{
//    public class QueryFilterCodesUtility
//    {
//        #region <<< Constructor and variables >>>
//        private static readonly Dictionary<string, Dictionary<string, string>> Industries = new Dictionary<string, Dictionary<string, string>>();
//        private static readonly Dictionary<string, Dictionary<string, string>> Regions = new Dictionary<string, Dictionary<string, string>>();

//        private static DateTime updateTimeIndustries = DateTime.Now.AddDays(-1);
//        private static DateTime updateTimeRegions = DateTime.Now.AddDays(-1);

//        private static IControlData controlData = ControlDataManager.GetLightWeightUserControlData(Settings.Default.PageMetadataProxyUser);
//        #endregion

//        #region <<< Public methods >>>
//        public static string GetIndustryDescriptor(string code, string language = "en")
//        {
//            var validatedLanguage = ValidateLanguage(language);
//            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(validatedLanguage))
//                return null;

//            // try to update
//            UpdateNames(dictionary);
//            return dictionary.ContainsKey(code) && dictionary[code].ContainsKey(language) ? dictionary[code][language] : null;


//            var metaData = GetPageCategoryNameMetaData(code, language);
//            return metaData == null ? code : metaData.MetaDataDescriptor;
//        }



//        #endregion

//        #region <<< Private methods >>>
//        private static string ValidateLanguage(string language)
//        {
//            return language == null ? null : language.Replace("-", "");
//        }

//        private static string GetDescriptor(Dictionary<string, Dictionary<string, string>> dictionary, string code, string language)
//        {
//            var validatedLanguage = ValidateLanguage(language);
//            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(validatedLanguage))
//                return null;

//            // try to update
//            UpdateValues(dictionary);

//            return dictionary.ContainsKey(code) && dictionary[code].ContainsKey(language) ? dictionary[code][language] : null;
//        }

//        public static void UpdateNamesAll()
//        {
//            updateTimeIndustries = DateTime.Now.AddTicks(-1);
//            updateTimeRegions = DateTime.Now.AddTicks(-1);
//            UpdateValues(Industries);
//            UpdateValues(Regions);
//        }

//        private static void UpdateValues(Dictionary<string, Dictionary<string, string>> dictionary)
//        {
//            if (dictionary == Industries)
//                UpdateValues(Industries, ref updateTimeIndustries, TaxonomyScheme.FactivaIndustry);
//            else if (dictionary == Regions)
//                UpdateValues(Regions, ref updateTimeRegions, TaxonomyScheme.FactivaRegion);
//        }

//        private static void UpdateValues(Dictionary<string, Dictionary<string, string>> dictionary, ref DateTime updateTime, TaxonomyScheme taxonomyScheme)
//        {
//            if (DateTime.Now <= updateTime)
//                return;

//            lock (dictionary)
//            {
//                dictionary.Clear();
//                var getTaxonomyListRequest = new GetTaxonomyListRequest
//                {
//                    ElementsToReturnCollection = new TaxonomyElementsToReturnCollection { TaxonomyElements.Descriptor },
//                    Language = "zhtw",
//                    Scheme = TaxonomyScheme.FactivaIndustry
//                };
//                GetListsDetailsListResponse newsPageModuleListResponse;
//                try
//                {
//                    var newsPageModuleListRequest = new GetListsDetailsListRequest();
//                    newsPageModuleListRequest.ListTypes.Add(listType);
//                    newsPageModuleListResponse = Process<GetListsDetailsListResponse>(newsPageModuleListRequest);
//                }
//                catch (Exception)
//                {
//                    // if error, something is wrong with PAM
//                    // set update time to be 10 mins from now
//                    updateTime = DateTime.Now.AddMinutes(10);
//                    return;
//                }

//                if (newsPageModuleListResponse != null)
//                {
//                    foreach (var listDetailsItem in newsPageModuleListResponse.ListDetailsItems) //var listItem in newsPageModuleListResponse.ListDetailsItems.SelectMany(listDetailsItem => listDetailsItem.List.Items))
//                    {
//                        var listItem = listDetailsItem.List.Items;
//                        var metaDataType = MapCustomCodeToMetadaType(listDetailsItem.List.CustomCode);
//                        foreach (var item in listItem)
//                        {
//                            AddItem(dictionary, item, metaDataType);
//                        }
//                    }
//                }

//                updateTime = DateTime.Now.AddDays(10);
//            }
//        }

//        private static MetaDataType MapCustomCodeToMetadaType(string customCode)
//        {
//            switch (customCode.ToLower())
//            {
//                case "top":
//                    return MetaDataType.Topic;
//                case "reg":
//                    return MetaDataType.Geographic;
//                case "ind":
//                    return MetaDataType.Industry;
//                case "mod":
//                    return MetaDataType.Custom;
//            }
//            return MetaDataType.Industry;
//        }

//        private static void AddItem(Dictionary<string, Dictionary<string, MetaData>> dictionary, ListItem listItem, MetaDataType metaDataType)
//        {
//            if (listItem is PageCategoryListItem)
//            {
//                var convertedListItem = (PageCategoryListItem)listItem;
//                AddItem(dictionary, convertedListItem.ItemCode, convertedListItem.ItemsCollection, metaDataType);
//            }
//            else if (listItem is NewsPageModuleListItem)
//            {
//                var convertedListItem = (NewsPageModuleListItem)listItem;
//                AddItem(dictionary, convertedListItem.ItemCode, convertedListItem.ItemsCollection, metaDataType);
//            }
//            else if (listItem is PageModuleMetadataListItem)
//            {
//                var convertedListItem = (PageModuleMetadataListItem)listItem;

//                if (!string.IsNullOrWhiteSpace(convertedListItem.SymbologyCode) &&
//                    !SymbologyCodes.ContainsKey(convertedListItem.ItemCode))
//                {
//                    SymbologyCodes.Add(
//                        convertedListItem.ItemCode,
//                        convertedListItem.SymbologyCode.Split(',').Select(symbologyCode => new MetaData
//                        {
//                            MetaDataCode = symbologyCode,
//                            MetaDataType = MetaDataType.SymbologyCode
//                        }).ToList());
//                }

//                AddItem(dictionary, convertedListItem.ItemCode, convertedListItem.ItemsCollection, metaDataType);
//            }
//        }

//        static void AddItem(Dictionary<string, Dictionary<string, MetaData>> dictionary, string code, IEnumerable<LanguageBasedItem> languageBasedItems, MetaDataType metaDataType)
//        {
//            if (!dictionary.ContainsKey(code))
//                dictionary.Add(code, new Dictionary<string, MetaData>());

//            var languagesDictionary = dictionary[code];

//            foreach (var languageBasedItem in languageBasedItems)
//            {
//                var languageCode = languageBasedItem.LanguageCode.ToString();
//                var metaData = new MetaData
//                {
//                    MetaDataCode = code,
//                    MetaDataDescription = string.IsNullOrEmpty(languageBasedItem.Description) ? null : languageBasedItem.Description.Trim(),
//                    MetaDataDescriptor = string.IsNullOrEmpty(languageBasedItem.Title) ? null : languageBasedItem.Title.Trim(),
//                    MetaDataType = metaDataType
//                };
//                if (!languagesDictionary.ContainsKey(languageCode))
//                    languagesDictionary.Add(languageCode, metaData);
//                languagesDictionary[languageCode] = metaData;
//            }
//        }

//        private static T Process<T>(object request)
//        {
//            try
//            {
//                var sr = FactivaServices.Invoke<T>(ControlDataManager.Convert(controlData, true), request);

//                if (sr == null)
//                {
//                    throw new DowJonesUtilitiesException(new NullReferenceException("ServiceResponse is null"), -1);
//                }

//                if (sr.rc != 0)
//                {
//                    throw new DowJonesUtilitiesException("Error in Metadata utility", sr.rc);
//                }

//                return sr.ObjectResponse;
//            }
//            catch (DowJonesUtilitiesException)
//            {
//                throw;
//            }
//            catch (Exception ex)
//            {
//                throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
//            }
//        }
//        #endregion
//    }
//}
