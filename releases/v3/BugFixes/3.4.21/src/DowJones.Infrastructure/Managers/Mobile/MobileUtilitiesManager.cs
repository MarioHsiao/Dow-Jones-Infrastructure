using System;
using System.Collections.Specialized;
using DowJones.Exceptions;
using DowJones.Loggers;
using DowJones.Preferences;
using DowJones.Session;
using DowJones.Utilities;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using DowJones.Managers.Abstract;
using log4net;

namespace DowJones.Managers.Mobile
{
    public class MobileUtilitiesManager : AbstractAggregationManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MobileUtilitiesManager));
        private IPreferences _preferences;

        public MobileUtilitiesManager(IControlData controlData, ITransactionTimer transactionTimer, IPreferences preferences = null)
            : base(controlData, transactionTimer)
        {
            _preferences = preferences;
        }

        public string GetMobileLoginToken()
        {
            var request = new GetEncryptedCredentialsForCurrentSessionRequest();
            var response = Invoke<GetEncryptedCredentialsForCurrentSessionResponse>(request);

            if (response.rc != 0)
            {
                throw new DowJonesUtilitiesException();
            }

            var collection = new NameValueCollection
                                 {
                                     //// Agreed DateTime format display with the ipad team.
                                     { "expires", DateTime.Now.AddDays(2).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'") }, 
                                     { "encryptedCredentials", response.ObjectResponse.EncryptedCredentials },
                                     { "userid", ControlData.UserID },
                                     { "namespace", ControlData.ProductID },
                                 };
            return new AESEncryptionUtility().Encrypt(collection);
        }

        protected override ILog Log
        {
            get { return _log; }
        }
    }
}
