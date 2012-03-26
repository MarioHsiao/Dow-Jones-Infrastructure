using System;
using DowJones.Exceptions;
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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PrincipleFactory));
        
        private readonly ControlData _controlData;

        public PrincipleFactory(IControlData controlData)
        {
            _controlData = ControlDataManager.Convert(controlData);
        }

        public override IPrinciple Create()
        {
            var request = new GetUserAuthorizationsRequest();
            ServiceResponse response = null;
            GetUserAuthorizationsResponse getUserAuthorizationsResponse = null;
            object obj = null;
            try
            {
                response = Factiva.Gateway.Services.V1_0.MembershipService.GetUserAuthorizations(_controlData, request);
            }
            catch (Exception ex)
            {
                _logger.Warn("Entitlements :: GetUserAuthorizations : Error in retrieving user authorization", ex);
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
                _logger.Warn(message);
                
                return new UninitializedPrinciple();
            }

            var entitlementsPrinciple = new EntitlementsPrinciple(getUserAuthorizationsResponse);
            return entitlementsPrinciple;
        }
    }
}
