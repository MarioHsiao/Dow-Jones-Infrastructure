using System;
using DowJones.Json.Gateway.Converters;

namespace DowJones.Json.Gateway.Exceptions
{
    /*
     * {"Error":{"Code":589500,"Message":"Generic ServiceProxy Error Cannot find module 'sax'"}}
    */

    public class JsonGatewayError
    {
        public Error Error { get; set; }

        public static JsonGatewayError Parse(string json)
        {
            try
            {
                var serviceError = JsonDotNetDataConverterSingleton.Instance.Deserialize<JsonGatewayError>(json);


                if (serviceError != null && serviceError.Error != null)
                {
                    return serviceError;
                }

                var proxyError = JsonDotNetDataConverterSingleton.Instance.Deserialize<Error>(json);

                if (proxyError == null || proxyError.Code == -1)
                {
                    proxyError = new Error
                                 {
                                     Code = JsonGatewayException.UnableToParseError,
                                     Message = "Unable to parse error from backend proxy or service"
                                 };
                }
                return new JsonGatewayError
                       {
                           Error = proxyError
                       };
            }

            catch (Exception ex)
            {
                return new JsonGatewayError
                       {
                           Error = new Error
                                   {
                                       Code = JsonGatewayException.GenericError,
                                       Message = ex.Message
                                   }
                       };
            }
        }
    }
}