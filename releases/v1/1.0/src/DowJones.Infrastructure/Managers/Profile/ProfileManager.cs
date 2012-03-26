using System;
using DowJones.Utilities.Exceptions;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Messages.Profile.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using log4net;
using GetUserProfileResponse = Factiva.Gateway.Messages.Profile.V1_0.GetUserProfileResponse;



namespace DowJones.Utilities.Managers.Profile
{
    /// <summary>
    /// Summary description for ProfileManager.
    /// </summary>
    public class ProfileManager : AbstractAggregationManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProfileManager));
        private GetUserProfileResponse _response;
        private PreferenceResponse _preferenceResponse;

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected override ILog Log
        {
            get { return _log; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public ProfileManager(ControlData controlData, string interfaceLanguage)
            : base(controlData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileManager"/> class.
        /// </summary>
        /// <param name="sessionID">The session ID.</param>
        /// <param name="clientTypeCode">The client type code.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        public ProfileManager(string sessionID, string clientTypeCode, string accessPointCode, string interfaceLanguage)
            : base(sessionID, clientTypeCode, accessPointCode)
        {
        }

        /// <summary>
        /// retrieves user's stored email address
        /// </summary>
        /// <returns></returns>
        public string GetEmailAddress()
        {
            try
            {
                if (_response != null)
                {
                    return _response.email;
                }
                var request = new GetUserProfileRequest();
                var response = Process<GetUserProfileResponse>(request);
                _response = response;
                return response == null ? null : response.email;
            }
            catch
            {
                return null;
            }
        }

       

        /// <summary>
        /// Gets the rule set.
        /// </summary>
        /// <returns></returns>
        public RuleSet GetRuleSet()
        {
            try
            {
                if (_response != null)
                {
                    return new RuleSet(_response.ruleSet);
                }
                var request = new GetUserProfileRequest();
                var response = Process<GetUserProfileResponse>(request);
                _response = response;
                return response == null ? null : new RuleSet(response.ruleSet);
                
            }
            catch
            {
                return null;
            }
        }

        public GetUserProfileResponse GetUserProfileResponseStub()
        {
            
                if (_response != null)
                {
                    return _response;
                }
                var request = new GetUserProfileRequest();
                var response = Process<GetUserProfileResponse>(request);
                _response = response;
                return _response;

        }

        /// <summary>
        /// Checks if the user is Academic Uer type
        /// </summary>
        /// <returns>True if academic else false..</returns>
        public bool IsAcademicUserType()
        {
            if (_response != null)
            {
                return _response.userType.ToUpper().Equals("A");
            }
            var request = new GetUserProfileRequest();
            var response = Process<GetUserProfileResponse>(request);
            _response = response;
            return _response.userType.ToUpper().Equals("A"); 
        }

        /// <summary>
        /// Gets the news views permissions.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="namespace">The @namespace.</param>
        /// <returns></returns>
        public NewsViewsPermissions GetNewsViewsPermissions(string userId, string @namespace)
        {
            GetUserAuthorizationsNoCacheResponse authResp = GetUserAuthorizationsNoCacheResponse(userId, @namespace);
            return new NewsViewsPermissions(authResp);
        }

        /// <summary>
        /// Gets the news views permissions.
        /// </summary>
        /// <param name="authResp">The auth resp.</param>
        /// <returns></returns>
        public NewsViewsPermissions GetNewsViewsPermissions(GetUserAuthorizationsNoCacheResponse authResp)
        {
            return new NewsViewsPermissions(authResp);
        }

        /// <summary>
        /// Gets the get user authorizations no cache response.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="namespace">The @namespace.</param>
        /// <returns></returns>
        public GetUserAuthorizationsNoCacheResponse GetUserAuthorizationsNoCacheResponse(string userId, string @namespace)
        {
            var auth = new GetUserAuthorizationsNoCacheRequest {userId = userId};
            return Process<GetUserAuthorizationsNoCacheResponse>(auth);
        }

        /// <summary>
        /// Gets the get user authorizations no cache response.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="namespace">The @namespace.</param>
        /// <param name="overrideControlData">The overriden control data.</param>
        /// <returns></returns>
        public GetUserAuthorizationsNoCacheResponse GetUserAuthorizationsNoCacheResponse(string userId, string @namespace, ControlData overrideControlData)
        {
            var auth = new GetUserAuthorizationsNoCacheRequest {userId = userId, productId = @namespace};

            ServiceResponse resp = Invoke<GetUserAuthorizationsNoCacheRequest>(auth, overrideControlData);

            Object objResponse;
            resp.GetResponse(ServiceResponse.ResponseFormat.Object, out objResponse);

            return (GetUserAuthorizationsNoCacheResponse)objResponse;
        }

        public bool IsUserAllowedToChangePassword()
        {
            PreferenceResponse preferenceResponse = GetPreferences();
            if (preferenceResponse != null && _preferenceResponse.rc == 0)
            {
                return preferenceResponse.AdminEditPassword.CanChangeLoginInformation;
            }
            return false;
        }

        public bool IsUserAllowedToSavePersistentCookie()
        {
            var preferenceResponse = GetPreferences();
            if (preferenceResponse != null && _preferenceResponse.rc == 0)
            {
                return preferenceResponse.AdminAutoLogin.CanAutomaticallyLogin;
            }
            return false;
        }

        private PreferenceResponse GetPreferences()
        {
            if (_preferenceResponse != null)
            {
                return _preferenceResponse;
            }
            var request = new GetItemsByClassIDRequest
                              {
                                  ClassID = new[]
                                                {
                                                    PreferenceClassID.AdminEditPassword,
                                                    PreferenceClassID.AdminAutoLogin,
                                                }
                              };

            var preferenceResponse = PreferenceService.GetItemsByClassID(ControlDataManager.Clone(ControlData), request);
           
            if (preferenceResponse.rc != 0)
            {
                // Throw the new exception
                throw new DowJonesUtilitiesException(preferenceResponse.rc);
            }
            return _preferenceResponse = preferenceResponse;
        }
    }
}