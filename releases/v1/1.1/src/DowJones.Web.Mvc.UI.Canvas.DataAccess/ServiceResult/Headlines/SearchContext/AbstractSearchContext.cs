using DowJones.Utilities.Exceptions;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Headlines.SearchContext
{
    public abstract class AbstractSearchContext<T>
    {
        public static T FromString(string searchRef)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(searchRef);
            }
            catch
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidSearchContextString);
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
