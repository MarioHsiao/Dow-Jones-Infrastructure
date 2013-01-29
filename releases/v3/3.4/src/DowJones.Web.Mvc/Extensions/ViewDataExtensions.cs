using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.Extensions
{
    public static class ViewDataExtensions
    {

        public static void SetTitle(this ViewDataDictionary viewData, string title)
        {
            viewData["Title"] = title;
        }

        public static T Get<T>(this ViewDataDictionary viewData, string key)
        {
            object item;

            viewData.TryGetValue(key, out item);

            return (item is T) ? (T) item : default(T);
        }

        public static IEnumerable<T> Get<T>(this ViewDataDictionary viewData)
        {
            var itemsOfT = viewData.Values.OfType<T>();
            return itemsOfT;
        }

        public static T SingleOrDefault<T>(this ViewDataDictionary viewData)
        {
            var itemsOfT = Get<T>(viewData);
            
            return itemsOfT.SingleOrDefault();
        }
    }
}
