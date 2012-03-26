using System.Collections.Generic;
using System.Web.UI;
using Factiva.Gateway.Messages.EmailDeliverySettings.V1_0;
using Factiva.Gateway.Utils.V1_0;

namespace DowJones.Utilities.Handlers.DJInsider
{
    public class DJIRequestDTO
    {
        public string EmailAddress { get; set; }
        public DowJonesInsiderState Status { get; set; }
        public string CountryCode { get; set; } //<Profile Country code> this is generally in cookie or some state that is preserved in the UI.
        public string AccountId { get; set; }
        public string DeliveryTime { get; set; }
        public EmailFormat EmailFormat { get; set; } //Always PLAIN
        public EmailLayout EmailLayout { get; set; } //Always Headline
        public bool IsWirelessFriendly { get; set; }
      //  public string Language { get; set; }
        public string Subject { get; set; } //ResourceText.GetInstance.GetString("djInsiderSubject"); //<DowJones Insider>  -  Token
        public string TimeZone { get; set; } //<user preference or default to GMT, in case of no preference>
        public string SetupCode { get; set; }
       }

    public class DJIResponseDTO
    {
        public string ErrorCode = "0";
        public string ErrorDescription = "DJI.success";
        public string SetupCode { get; set; }
        public string EmailAddress { get; set; }
      
    }

    public enum DowJonesInsiderState
    {
        Undefined = -1,
        Unsubscribed = 0,
        Subscribed = 1
    }
}
