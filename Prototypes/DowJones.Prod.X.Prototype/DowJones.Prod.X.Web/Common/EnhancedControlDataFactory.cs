using System.Web;
using DowJones.Assemblers.Session;
using DowJones.Prod.X.Web.Properties;
using DowJones.Session;

namespace DowJones.Prod.X.Web.Common
{
    public class EnhancedControlDataFactory : ControlDataFactory
    {
        public EnhancedControlDataFactory(IUserSession session, HttpRequestBase request = null)
            : base(session, request)
        {
        }

        public override IControlData Create()
        {
            var controlData = base.Create();

            controlData.SessionID = null;
            controlData.EncryptedToken = Settings.Default.EncryptedToken;

            return controlData;
        }
    }
}