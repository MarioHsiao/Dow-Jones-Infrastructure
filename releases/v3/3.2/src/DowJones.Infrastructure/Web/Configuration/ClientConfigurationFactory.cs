using System.Web;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Infrastructure.Common;
using DowJones.Preferences;
using DowJones.Session;

namespace DowJones.Web.Configuration
{
    public class ClientConfigurationFactory : Factory<ClientConfiguration>
    {
        private readonly HttpContextBase _context;
        private readonly IControlData _controlData;
        private readonly IUserSession _session;
        private readonly IPreferences _preferences;
        private readonly Product _product;

        public bool? Debug { get; set; }


        public ClientConfigurationFactory(IControlData controlData, IUserSession session, IPreferences preferences, Product product, HttpContextBase context)
        {
            _controlData = controlData;
            _session = session;
            _preferences = preferences;
            _product = product;
            _context = context;
        }


        public override ClientConfiguration Create()
        {
            var config = _context.Items["ClientConfiguration"] as ClientConfiguration;

            if (config != null)
                return config;


            var credentials = new ClientCredentials
            {
                AccessPointCode = _controlData.AccessPointCode,
                AccessPointCodeUsage = _controlData.AccessPointCodeUsage,
                ClientCode = _controlData.ClientCode,
                ClientType = _controlData.ClientType,
                ProxyUserId = _controlData.ProxyUserId,
                ProxyUserNamespace = _controlData.ProxyProductId,
                RemoteAddress = _controlData.IpAddress,
                SeamlessAccessFrom = _session.ProductPrefix,
                SessionId = _controlData.SessionID,
                Token = _controlData.EncryptedToken,
            };

            config = new ClientConfiguration
                         {
                             Credentials = credentials, 
                             Debug = Debug,
                             Preferences = new ClientPreferences(_preferences),
                             ProductId = _product.Id,
                         };

            _context.Items["ClientConfiguration"] = config;

            return config;
        }


    }
}