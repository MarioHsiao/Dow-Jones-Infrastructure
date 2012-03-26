using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Infrastructure;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Preferences
{
    public class CachedPreferenceService : IPreferenceService
    {
        private readonly IPreferenceService _source;

        internal CachedPreferenceResponse CachedResponse { get; set; }

        internal IEnumerable<PreferenceItem> Preferences
        {
            get
            {
                if (CachedResponse == null)
                    return Enumerable.Empty<PreferenceItem>();

                return CachedResponse.Preferences;
            }
        }


        public CachedPreferenceService(IPreferenceService source)
            : this(source, null)
        {
        }

        public CachedPreferenceService(IPreferenceService source, PreferenceResponse prime)
        {
            _source = source;

            if (prime != null)
                CachedResponse = new CachedPreferenceResponse(prime);
        }


        public void AddItem(AddItemRequest request)
        {
            _source.AddItem(request);

            SaveToCache(request.Item);
        }

        public void DeleteItem(string itemId)
        {
            DeleteItems(new[] { itemId });
        }

        public void DeleteItems(IEnumerable<string> itemIds)
        {
            _source.DeleteItems(itemIds);
            RemoveFromCache(itemIds);
        }

        public PreferenceResponse GetItemByClassId(PreferenceClassID classId)
        {
            return GetItemsByClassId(new[] { classId });
        }

        public PreferenceResponse GetItemsByClassId(IEnumerable<PreferenceClassID> classIds)
        {
            var uncachedIds = GetUncachedClassIds(classIds);

            if (uncachedIds.Any())
            {
                var response = _source.GetItemsByClassId(uncachedIds);
                SaveToCache(response);
            }

            return CachedResponse;
        }

        public PreferenceResponse GetItemById(string itemId)
        {
            return GetItemsById(new[] {itemId});
        }

        public PreferenceResponse GetItemsById(IEnumerable<string> itemIds)
        {
            var uncachedIds = GetUncachedItemIds(itemIds);

            if (uncachedIds.Any())
            {
                var response = _source.GetItemsById(uncachedIds);
                SaveToCache(response);
            }

            return CachedResponse;
        }


        public void UpdateItem(UpdateItemRequest request)
        {
            _source.UpdateItem(request);

            SaveToCache(request.Item);
        }

        public PreferenceResponse GetItemsByClassIdNoCache(GetItemsByClassIDNoCacheRequest request)
        {
            var response = _source.GetItemsByClassIdNoCache(request);

            SaveToCache(response);

            return CachedResponse;
        }


        #region Caching helpers

        public void ClearCache()
        {
            CachedResponse.Clear();
        }


        protected IEnumerable<PreferenceClassID> GetUncachedClassIds(IEnumerable<PreferenceClassID> classIds)
        {
            var cachedClassIds = Preferences.Select(x => x.ClassID);
            return classIds.Except(cachedClassIds);
        }

        protected IEnumerable<string> GetUncachedItemIds(IEnumerable<string> itemIds)
        {
            var cachedItemIds = Preferences.Select(x => x.ItemID);
            return itemIds.Except(cachedItemIds, (x, y) => string.Equals(x, y, StringComparison.OrdinalIgnoreCase));
        }


        protected void RemoveFromCache(IEnumerable<string> itemIds)
        {
            Guard.IsNotNull(itemIds, "itemIds");

            if(CachedResponse == null)
                return;

            CachedResponse.Remove(x => itemIds.Contains(x.ItemID, true));
        }


        protected void SaveToCache(PreferenceItem item)
        {
            SaveToCache(new[] { item });
        }

        protected void SaveToCache(PreferenceResponse preferences)
        {
            if (preferences == null || preferences.rc != 0)
                return;

            if(CachedResponse == null)
                CachedResponse = new CachedPreferenceResponse(preferences);
            else
                CachedResponse.prefCollection.Merge(preferences.prefCollection, replaceExisting: true);
        }

        protected void SaveToCache(IEnumerable<PreferenceItem> preferences)
        {
            if (preferences == null)
                return;

            if (CachedResponse == null)
                CachedResponse = new CachedPreferenceResponse(preferences);
            else
                CachedResponse.Add(preferences);
        }

        #endregion

        internal class CachedPreferenceResponse : PreferenceResponse
        {
            protected static readonly ILog Log = LogManager.GetLogger(typeof (CachedPreferenceResponse));

            public IEnumerable<PreferenceItem> Preferences
            {
                get
                {
                    if(!isPopulated)
                        PopulatePreferenceCollection();

                    return prefCollection.Values.OfType<PreferenceItem>();
                }
            }


            public CachedPreferenceResponse(IEnumerable<PreferenceItem> preferences)
            {
                Add(preferences);
            }

            public CachedPreferenceResponse(PreferenceResponse response)
                : base(Evaluate(response))
            {
            }


            public void Add(PreferenceItem preference)
            {
                Add(new[] { preference});
            }

            public void Add(IEnumerable<PreferenceItem> preferences)
            {
                if(!isPopulated)
                    isPopulated = true;

                var itemsToRemove = preferences.Select(x => x.ItemID);
                Remove(x => itemsToRemove.Contains(x.ItemID, true));

                foreach (var preference in preferences)
                {
                    var key = preference.InstanceName;

                    if (key.IsNullOrEmpty())
                        key = preference.ItemID;

                    if(key.IsNullOrEmpty())
                        key = preference.ClassID.ToString();
                    
                    if(key.IsNullOrEmpty())
                        continue;

                    prefCollection[key] = preference;
                }
            }

            public void Clear()
            {
                prefCollection.Clear();
            }

            public void Remove(Func<PreferenceItem, bool> predicate)
            {
                Remove<PreferenceItem>(predicate);
            }

            public void Remove<T>(Func<T, bool> predicate)
            {
                var keysToDelete =
                     from item in prefCollection
                     where item.Value is T
                     where predicate((T)item.Value)
                     select item.Key;

                foreach (var key in keysToDelete.ToArray())
                    prefCollection.Remove(key);
            }


            private static ServiceResponse Evaluate(PreferenceResponse response)
            {
                if (response == null)
                    return null;

                if(response.rc != 0)
                {
                    Log.Info(string.Format("Ignoring attempt to prime preferences cache with invalid PreferenceResponse object (rc == {0})", response.rc));
                    return null;
                }

                if(!response.isPopulated)
                    response.PopulatePreferenceCollection();
                
                return response;
            }
        }
    }
}