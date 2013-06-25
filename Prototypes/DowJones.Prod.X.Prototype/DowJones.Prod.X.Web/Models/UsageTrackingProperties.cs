using System;
using System.Web;
using DowJones.Prod.X.Common;
using DowJones.Prod.X.Models.Site;
using DowJones.Security.Interfaces;
using DowJones.Session;
using Newtonsoft.Json;

namespace DowJones.Prod.X.Web.Models
{
    public class UsageTrackingProperties : IUsageTrackingProperties
    {
        private static readonly Lazy<string> UsageTrackingAccountName = new Lazy<string>(() => Properties.Settings.Default.UsageTrackingAccount);
        private readonly IPrinciple _principle;

        public UsageTrackingProperties(IUserSession userSession, IPrinciple principle)
        {
            _principle = principle;
            LogUsageTracking = UsageTrackingUtility.LogUsageTrackingMetrics(Properties.Settings.Default.UsageTrackingOn,
                                               Properties.Settings.Default.SkipUsageTrackingAccounts, userSession.AccountId);
            SessionId = userSession.SessionId;
            UserIdNs = String.Format("{0}_{1}", userSession.UserId, userSession.ProductId);
            AccountId = userSession.AccountId;
            FullUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            InterfaceLanguage = userSession.InterfaceLanguage.ToString();
            Events = "event12,event19";
            PageChannel = "Research and Tools";
            Source = "Factiva";
            Section = "Newsletters";
        }

        [JsonProperty("LogUsageTracking")]
        public bool LogUsageTracking { get; private set; }

        [JsonProperty("UsageTrackingAccount")]
        public string UsageTrackingAccount { get { return UsageTrackingAccountName.Value; } }

        [JsonProperty("SessionId")]
        public string SessionId { get; private set; }

        [JsonProperty("UserIdNs")]
        public string UserIdNs { get; private set; }

        [JsonProperty("AccountId")]
        public string AccountId { get; private set; }

        [JsonProperty("AccessCode")]
        public string AccessCode { get { return _principle.UserServices.PlanId; } }

        [JsonProperty("FullUrl")]
        public string FullUrl { get; private set; }

        [JsonProperty("InterfaceLanguage")]
        public string InterfaceLanguage { get; private set; }

        [JsonProperty("PageName")]
        public string PageName { get; set; }

        [JsonProperty("Events")]
        public string Events { get; private set; }

        [JsonProperty("PageChannel")]
        public string PageChannel { get; private set; }

        [JsonProperty("Source")]
        public string Source { get; private set; }

        [JsonProperty("Section")]
        public string Section { get; private set; }
    }
}