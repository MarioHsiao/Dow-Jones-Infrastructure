using System.Collections;
using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    public interface ICacheDataList
    {
        void PopulateCache<TListType>();

        //IEnumerable<TListItems> GetListItemsCollection<TListType>();
    }

}