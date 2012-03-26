using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DowJones.Session;
using DowJones.Web.Showcase.Models;

namespace DowJones.Web.Showcase
{
    public static class ControlDataFactory
    {
        private const string UrlFormat =
            "http://fdevweb3/api/1.0/Session/login/xml?userid={0}&password={1}&namespace={2}";

        public static IControlData GetControlData(HttpContext context)
        {
            ShowcaseControlData controlData;

            // If the session was passed into the querystring, always use that
            if (!string.IsNullOrWhiteSpace(context.Request["sessionid"]))
            {
                controlData = new ShowcaseControlData { SessionID = context.Request["sessionid"] };
                return (ControlData)controlData;
            }

            controlData = context.Session["ControlData"] as ShowcaseControlData;

            controlData = controlData ?? new ShowcaseControlData();

            string userId = ValueOrDefault(context.Request["UserID"], controlData.UserID);
            if (userId != controlData.UserID)
            {
                controlData.UserID = userId;
                controlData.SessionID = null;
            }

            controlData.Password = ValueOrDefault(context.Request["Password"], controlData.Password);
            controlData.ProductID = ValueOrDefault(context.Request["ProductID"], controlData.ProductID);

            if (!controlData.HasSessionID)
                controlData.SessionID = CreateSession(controlData);

            context.Session["ControlData"] = controlData;

            return (ControlData)controlData;
        }

        private static string CreateSession(ShowcaseControlData showcaseControl)
        {
            string url = string.Format(UrlFormat, showcaseControl.UserID, showcaseControl.Password, showcaseControl.ProductID);

            var response = XDocument.Load(url);

            if(response.Root.Name.LocalName.Contains("ErrorResponse"))
                throw new ApplicationException(response.Document.ToString());

            var sessionId = response.Descendants("SessionId").SingleOrDefault();

            return sessionId.Value;
        }

        private static string ValueOrDefault(string value, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }
    }
}