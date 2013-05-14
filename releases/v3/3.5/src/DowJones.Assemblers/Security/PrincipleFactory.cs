using System;
using DowJones.Exceptions;
using DowJones.Extensions;
using log4net;
using DowJones.Infrastructure;
using DowJones.Security;
using DowJones.Security.Interfaces;
using DowJones.Session;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Assemblers.Security
{
    public class PrincipleFactory : Factory<IPrinciple>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PrincipleFactory));

        private readonly IControlData _controlData;

        public PrincipleFactory(IControlData controlData)
        {
            _controlData = controlData;
        }

        public override IPrinciple Create()
        {
            ServiceResponse response = null;
            GetUserAuthorizationsResponse getUserAuthorizationsResponse;
            object obj = null;
            try
            {
                if (_controlData.ProxyUserId.IsNotEmpty() && _controlData.ProxyProductId.IsNotEmpty())
                {
                    var tempControlData = ControlDataManager.Convert(ControlDataManager.Clone(_controlData));
                    tempControlData.ProxyUserID = null;
                    tempControlData.ProxyUserNamespace = null;
                    var request = new GetUserAuthorizationsNoCacheRequest()
                                      {
                                        userId = _controlData.ProxyUserId, 
                                        productId = _controlData.ProxyProductId,
                                      };

                    response = Factiva.Gateway.Services.V1_0.MembershipService.GetUserAuthorizationsNoCache(tempControlData, request);
                }
                else
                {
                    var request = new GetUserAuthorizationsRequest();
                    response = Factiva.Gateway.Services.V1_0.MembershipService.GetUserAuthorizations(ControlDataManager.Convert(ControlDataManager.Clone(_controlData)), request);
                }
            }
            catch (Exception ex)
            {
                Logger.Warn("Entitlements :: GetUserAuthorizations : Error in retrieving user authorization", ex);
            }

            if (response != null && response.rc == 0)
            {
                response.GetResponse(ServiceResponse.ResponseFormat.Object, out obj);
            }

            if (obj != null)
            {
                getUserAuthorizationsResponse = obj as GetUserAuthorizationsResponse;
            }
            else
            {
                //Always guarantee the object is created!
                var message = "Entitlements :: GetUserAuthorizations : Error in retrieving user authorization, Error code=";
                if (response != null)
                {
                    message += response.rc;
                }
                Logger.Warn(message);
                
                return new UninitializedPrinciple();
            }

            var entitlementsPrinciple = new EntitlementsPrinciple(getUserAuthorizationsResponse);
            return entitlementsPrinciple;
        }
    }
}
