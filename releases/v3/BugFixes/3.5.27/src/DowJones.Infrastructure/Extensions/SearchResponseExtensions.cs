using DowJones.Exceptions;
using Factiva.Gateway.V1_0;

namespace DowJones.Extensions
{
    public static class SearchResponseExtensions
    {
        public static T GetObject<T>(this ServiceResponse response)
        {
            T responseObject = default(T);

            if (response != null && response.rc == 0)
            {
                object obj = null;
                response.GetResponse(ServiceResponse.ResponseFormat.Object, out obj);
                responseObject = (T) obj;
            }
            else if (response != null)
            {
                throw new DowJonesUtilitiesException(response.rc);
            }
            return responseObject;
        }
    }
}