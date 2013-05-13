using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Preferences;
using DowJones.Session;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
//using Factiva.ServiceModel;

namespace DowJones.Managers.Authorization
{
    public class AuthorizationManager
    {
        private IControlData _controlData;


        public AuthorizationManager(IControlData controlData)
        {
            _controlData = controlData;
        }

        private GetUserAuthorizationsResponse GetUserAuthorization()
        {
            //no control data
            //if (_controlData == null || _controlData.SessionID == null || _controlData.EncryptedToken == null)
            //{
            //    throw new DowJonesUtilitiesException(DowJonesUtilitiesException.SessionDataAuthorizationError);
            //}

            try
            {
                GetUserAuthorizationsRequest getUserAuthorizationsRequest = new GetUserAuthorizationsRequest();

                ServiceResponse serviceResponse = MembershipService.GetUserAuthorizations(ControlDataManager.Convert(_controlData), getUserAuthorizationsRequest);
                GetUserAuthorizationsResponse response = serviceResponse.GetObject<GetUserAuthorizationsResponse>();
                //response.AuthorizationMatrix.FsInterfaceService.ac6
                return response;
            }
            catch( DowJonesUtilitiesException )
            {
                throw;
            }
            catch( Exception ex )
            {
                throw DowJonesUtilitiesException.ParseExceptionMessage( ex );
            }
        }

        public Boolean IsSocialMediaBlocked()
        {
            string SocialMediaBlockingString = Properties.SocialMedia.Default.SocialMediaBlockingString;
            GetUserAuthorizationsResponse getUserAuthorizationsResponse = GetUserAuthorization();
            try
            {
                if (getUserAuthorizationsResponse == null 
                    || getUserAuthorizationsResponse.AuthorizationMatrix == null 
                    || getUserAuthorizationsResponse.AuthorizationMatrix.FinacialServiceInterface == null
                    || getUserAuthorizationsResponse.AuthorizationMatrix.FinacialServiceInterface.ac6 == null
                    || getUserAuthorizationsResponse.AuthorizationMatrix.FinacialServiceInterface.ac6.Count <= 0)
                {
                    return false;
                }

                foreach (string ac6 in getUserAuthorizationsResponse.AuthorizationMatrix.FinacialServiceInterface.ac6)
                {
                    if(ac6.Trim()==SocialMediaBlockingString)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
            }
        }
    }
}
