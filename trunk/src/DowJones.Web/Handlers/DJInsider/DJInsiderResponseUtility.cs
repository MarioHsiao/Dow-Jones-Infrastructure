/*using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Web.Handlers.DJInsider
{
    public class ResponseUtility
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static T GetObject<T>(ServiceResponse response)
        {
            T responseObject = default(T);
            if (response == null)
            {
                return responseObject;
            }
            if (response.rc == 0)
            {
                object obj;
                response.GetResponse(ServiceResponse.ResponseFormat.Object, out obj);
                responseObject = (T)obj;
                log.Debug("<<----- Received response from gateway layer ----->>");
            }
            else
            {
                throw new DowJonesInsiderException("Utility Error", response.rc);
                //ResourceText.GetInstance.GetErrorMessage(response.rc.ToString()), response.rc);
            }
            return responseObject;
        }
    }
}*/