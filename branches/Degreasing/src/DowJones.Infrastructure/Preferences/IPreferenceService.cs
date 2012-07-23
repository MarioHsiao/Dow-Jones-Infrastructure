using System.Collections.Generic;
using Factiva.Gateway.Messages.Preferences.V1_0;

namespace DowJones.Preferences
{
    public interface IPreferenceService
    {
        void AddItem(AddItemRequest request);
        void DeleteItem(string itemId);
        void DeleteItems(IEnumerable<string> itemIds);
        PreferenceResponse GetItemByClassId(PreferenceClassID classId);
        PreferenceResponse GetItemsByClassId(IEnumerable<PreferenceClassID> classIds);
        PreferenceResponse GetItemsById(IEnumerable<string> itemIds);
        PreferenceResponse GetItemById(string itemId);
        void UpdateItem(UpdateItemRequest request);
        PreferenceResponse GetItemsByClassIdNoCache(GetItemsByClassIDNoCacheRequest request);
    }
}