using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Exceptions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Infrastructure;
using DowJones.Session;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Preferences
{
    public class PreferenceService : IPreferenceService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PreferenceService));
        
        private readonly IControlData _controlData;
        
        public PreferenceService(IControlData controlData)
        {
            _controlData = controlData;
        }

        public void AddItem(AddItemRequest request)
        {
            Guard.IsNotNull(request, "request");
            Guard.IsNotNull(request.Item, "request.Item");

            ServiceResponse response = null;

            try
            {
                response = Factiva.Gateway.Services.V1_0.PreferenceService.AddItem(ControlDataManager.Convert(_controlData), request);
            }
            catch (Exception ex)
            {
                Logger.Warn("PreferenceService::AddItem: Error when adding item", ex);     
            }

            EnsureValidResponse(response);
        }

        public static IPreferences Convert(PreferenceResponse response, string interfaceLanguage = null)
        {
            var preferences = new Preferences();

            // Clock type
            if (response.TimeFormat != null && response.TimeFormat.TimeFormat == PreferenceTimeFormat.HOURS12)
                preferences.ClockType = ClockType.TwelveHours;

            // Time Zone
            if (response.TimeZone != null)
                preferences.TimeZone = response.TimeZone.ToString();

            // Content Languages
            if (response.SearchLanguage != null && !string.IsNullOrWhiteSpace(response.SearchLanguage.SearchLanguage))
            {
                foreach(var language in response.SearchLanguage.SearchLanguage.Split(','))
                {
                    var lang = language.Trim().ToLower();
                    if (lang == "all")
                    {
                        preferences.ContentLanguages.Clear();
                        break;
                    }
                    preferences.ContentLanguages.Add(lang);
                }
            }

            // Interface language
            preferences.InterfaceLanguage = interfaceLanguage ?? "en";

            return preferences;
        }

        public void DeleteItem(string itemId)
        {
            Guard.IsNotNullOrEmpty(itemId, "itemId");

            DeleteItems(new[] {itemId});
        }

        public void DeleteItems(IEnumerable<string> itemIds)
        {
            Guard.IsNotNullOrEmpty(itemIds, "itemIds");

            var request = new DeleteItemRequest {ItemIDList = itemIds.ToArray()};
            
            ServiceResponse response = null;
            try
            {
                response = Factiva.Gateway.Services.V1_0.PreferenceService.DeleteItem(ControlDataManager.Convert(_controlData), request);
            }
            catch (Exception ex)
            {
                Logger.Warn("PreferenceService::DeleteItems: Error when deleting item", ex);   
            }

            EnsureValidResponse(response);
        }

        public PreferenceResponse GetItemByClassId(PreferenceClassID classId)
        {
            return GetItemsByClassId(new[] {classId});
        }

        public PreferenceResponse GetItemsByClassId(IEnumerable<PreferenceClassID> classIds)
        {
            Guard.IsNotNullOrEmpty(classIds, "classIds");

            var request = new GetItemsByClassIDRequest {ClassID = classIds.ToArray()};
            
            return GetItemsByClassId(request);
        }

        public PreferenceResponse GetItemsByClassId(GetItemsByClassIDRequest request)
        {
            PreferenceResponse response = null;
            try
            {
                response = Factiva.Gateway.Services.V1_0.PreferenceService.GetItemsByClassID(ControlDataManager.Convert(_controlData), request);
            }
            catch (Exception ex)
            {
                Logger.Warn("PreferenceService::GetItemsByClassId: Error retrieving items by  ClassID", ex);
            }

            return response;
        }

        public PreferenceResponse GetItemsByClassIdNoCache(GetItemsByClassIDNoCacheRequest request)
        {
            PreferenceResponse response = null;
            try
            {
                response = Factiva.Gateway.Services.V1_0.PreferenceService.GetItemsByClassIDNoCache(ControlDataManager.Convert(_controlData), request);
            }
            catch (Exception ex)
            {
                Logger.Warn("PreferenceService::GetItemsByClassId: Error retrieving items by  ClassID", ex);
            }

            return response;
        }

        public PreferenceResponse GetItemsById(IEnumerable<string> itemIds)
        {
            Guard.IsNotNullOrEmpty(itemIds, "itemIds");

            var request = new GetItemsByIDRequest { ItemID = itemIds.ToArray() };

            return GetItemsById(request);
        }

        public PreferenceResponse GetItemsById(GetItemsByIDRequest request)
        {
            PreferenceResponse response = null;
            try
            {
                response = Factiva.Gateway.Services.V1_0.PreferenceService.GetItemsByID(ControlDataManager.Convert(_controlData), request);
            }
            catch (Exception ex)
            {
                Logger.Warn("PreferenceService::GetItemsById: Error retrieving items by ID", ex);                
            }

            return response;
        }

        public PreferenceResponse GetItemById(string itemId)
        {
            Guard.IsNotNullOrEmpty(itemId, "itemId");

            return GetItemsById(new[] {itemId});
        }

        public void UpdateItem(UpdateItemRequest request)
        {
            Guard.IsNotNull(request, "request");
            Guard.IsNotNull(request.Item, "request.Item");

            ServiceResponse response = null;
            try
            {
                response = Factiva.Gateway.Services.V1_0.PreferenceService.UpdateItem(ControlDataManager.Convert(_controlData), request);
            }
            catch (Exception ex)
            {
                Logger.Warn("PreferenceService::UpdateItem: Error when updating item", ex);     
            }

            EnsureValidResponse(response);
        }

        private static void EnsureValidResponse(ServiceResponse response)
        {
            if (response == null || response.rc != 0)
            {
                // TODO:  Add message
                throw new DowJonesUtilitiesException();
            }
        }
    }
}