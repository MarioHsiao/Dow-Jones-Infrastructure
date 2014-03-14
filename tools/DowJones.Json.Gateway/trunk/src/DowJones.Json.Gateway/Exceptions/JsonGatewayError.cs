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
                return DataConverterDecoratorSingleton.Instance.Deserialize<JsonGatewayError>(json);
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
