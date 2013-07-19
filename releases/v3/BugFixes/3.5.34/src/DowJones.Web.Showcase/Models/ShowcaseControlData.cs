using DowJones.Session;

namespace DowJones.Web.Showcase.Models
{
    public class ShowcaseControlData
    {
        public string AccessPointCode { get; set; }

        public string Password { get; set; }

        public string ProductID { get; set; }

        public string SessionID { get; set; }

        public string UserID { get; set; }

        public bool HasSessionID
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SessionID);
            }
        }

        public ShowcaseControlData()
        {
            AccessPointCode = "P";
            UserID = "dacostad";
            Password = "vader";
            ProductID = "16";
        }

        public ShowcaseControlData(IControlData source)
        {
            AccessPointCode = source.AccessPointCode;
            ProductID = source.ProductID;
            UserID = source.UserID;
            SessionID = source.SessionID;
        }


        public static explicit operator ControlData(ShowcaseControlData source)
        {
            return new ControlData
            {
                AccessPointCode = source.AccessPointCode,
                ProductID = source.ProductID,
                SessionID = source.SessionID,
                UserID = source.UserID,
            };
        }
    }
}