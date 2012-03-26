using System;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Security;
using Factiva.Gateway.Error.V1_0;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Utilities.Managers.Security
{
    public class EntitlementsManager : AbstractAggregationManager
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(EntitlementsManager));

        public EntitlementsManager(string sessionID, string clientTypeCode, string accessPointCode, string interfaceLangugage)
            : base(sessionID, clientTypeCode, accessPointCode)
        {
        }

        public EntitlementsManager(ControlData controlData, string interfaceLanguage)
            : base(controlData)
        {
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        protected override ILog Log
        {
            get { return m_Log; }
        }

        /// <summary>
        /// Gets the entitlements principal. Does a cached request
        /// </summary>
        /// <returns></returns>
        public EntitlementsPrincipal GetEntitlementsPrincipal()
        {
            return GetEntitlementsPrincipal(true);
        }

        /// <summary>
        /// Gets the entitlements principal.
        /// </summary>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <returns></returns>
        public EntitlementsPrincipal GetEntitlementsPrincipal(bool useCache)
        {
            return GetEntitlementsPrincipal((useCache) ? new GetUserAuthorizationsRequest() : new GetUserAuthorizationsNoCacheRequest());
        }

        /// <summary>
        /// Gets the entitlements principal. This is a no cache request.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        public EntitlementsPrincipal GetEntitlementsPrincipal(string serviceName, string userId, string productId)
        {
            GetUserAuthorizationsNoCacheRequest request = new GetUserAuthorizationsNoCacheRequest();
            request.serviceName = serviceName;
            request.userId = userId;
            request.productId = productId;
            return GetEntitlementsPrincipal(request);
        }

        /// <summary>
        /// Gets the entitlements principal.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private EntitlementsPrincipal GetEntitlementsPrincipal(GetUserAuthorizationsRequest request)
        {
            EntitlementsPrincipal principal = new EntitlementsPrincipal();
            ServiceResponse<GetUserAuthorizationsResponse> sr = null;

            try
            {
                sr = Invoke<GetUserAuthorizationsResponse>(request);
                if (sr != null && sr.rc == 0)
                {
                    principal.Load(sr.ObjectResponse);
                    return principal;
                }
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (GatewayException gex)
            {
                // Catch the GatewayException and turn it into a EmgUtilitiesException
                throw new DowJonesUtilitiesException(gex, gex.ErrorCode);
            }
            catch (Exception ex)
            {
                throw new DowJonesUtilitiesException(ex, DowJonesUtilitiesException.BaseUtillityError);
            }
            return null;
        }
    }
}