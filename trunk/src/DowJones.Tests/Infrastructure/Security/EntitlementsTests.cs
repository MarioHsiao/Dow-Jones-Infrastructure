using System;
using System.Collections.Generic;
using DowJones.Preferences;
using DowJones.Security;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure.Security
{
    [TestClass]
    public class EntitlementsTests : UnitTestFixture
    {
        private ControlData m_ControlData = new ControlData { UserID = "JimS", UserPassword = "password", ProductID = "16" };
        private IPreferences m_preferences = new DowJones.Preferences.Preferences("en");

        [TestMethod]
        public void EntiltlementTest()
        {
            var request = new GetUserAuthorizationsRequest();
            ServiceResponse response = null;
            GetUserAuthorizationsResponse getUserAuthorizationsResponse = null;
            object obj = null;
            try
            {
                response = Factiva.Gateway.Services.V1_0.MembershipService.GetUserAuthorizations(m_ControlData, request);
            }
            catch (Exception)
            {
                //_logger.Warn("Entitlements :: GetUserAuthorizations : Error in retrieving user authorization", ex);
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
                //_logger.Warn(message);

                //return new UninitializedPrinciple();
            }

            if (getUserAuthorizationsResponse == null || getUserAuthorizationsResponse.AuthorizationMatrix == null) return;
            var entitlementsPrinciple = new EntitlementsPrinciple(getUserAuthorizationsResponse);

            Console.WriteLine(@"IsSelectFullUser: " + entitlementsPrinciple.CoreServices.AlertsService.IsSelectFullUser);
            Console.WriteLine(@"IsSelectHeadlinesUser: " + entitlementsPrinciple.CoreServices.AlertsService.IsSelectHeadlinesUser);
            Console.WriteLine(@"IsDULinkEnabled: " + entitlementsPrinciple.CoreServices.InterfaceService.IsDULinkEnabled);
            Console.WriteLine(@"IsTranslateArticleAllowed: " + entitlementsPrinciple.CoreServices.InterfaceService.IsTranslateArticleAllowed);
            //return entitlementsPrinciple;
        }

        [TestMethod]
        public void Entiltlement_FTODE_Test()
        {
            var request = new GetUserAuthorizationsRequest();
            ServiceResponse response = null;
            GetUserAuthorizationsResponse getUserAuthorizationsResponse = null;
            object obj = null;
            try
            {
                m_ControlData.UserID = "E000013340";
                m_ControlData.UserPassword = "test123-";
                m_ControlData.ProductID = "16";

                response = Factiva.Gateway.Services.V1_0.MembershipService.GetUserAuthorizations(m_ControlData, request);
            }
            catch (Exception)
            {
                //_logger.Warn("Entitlements :: GetUserAuthorizations : Error in retrieving user authorization", ex);
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
                //_logger.Warn(message);

                //return new UninitializedPrinciple();
            }

            if (getUserAuthorizationsResponse == null || getUserAuthorizationsResponse.AuthorizationMatrix == null) return;
            var entitlementsPrinciple = new EntitlementsPrinciple(getUserAuthorizationsResponse);

            //Assert.IsNotNull(entitlementsPrinciple);
            //Assert.IsNotNull(entitlementsPrinciple.CoreServices);
            //Assert.IsNotNull(entitlementsPrinciple.CoreServices.EmailService);
            //Assert.IsTrue(entitlementsPrinciple.CoreServices.EmailService.IsFullTextODEAllowed);
            Console.WriteLine(@"IsFullTextODEAllowed: " + entitlementsPrinciple.CoreServices.EmailService.IsFullTextODEAllowed);
        }


        [TestMethod]
        public void IsFullTextODEAllowed_Should_Be_True_When_Email_FTODE_Flag_Is_1()
        {

            GetUserAuthorizationsResponse getUserAuthorizationsResponse = new GetUserAuthorizationsResponse
                                                                              {
                                                                                  AuthorizationMatrix = new AuthorizationMatrix
                                                                                                            {
                                                                                                                Email = new MatrixEmailService {
                                                                                                                    ac3 = new List<string> {"15"},
                                                                                                                    FTODE = "1"
                                                                                                                }
                                                                                                            },
                                                                                  RuleSet = "8999CPBE99999999RN9999999999999999999999990099000099990000000099999900000000009999990000999999999999000000000099MM9998999899000000999999000000000000000000000000"
                                                                              };

            var entitlementsPrinciple = new EntitlementsPrinciple(getUserAuthorizationsResponse);

            Assert.IsNotNull(entitlementsPrinciple);
            Assert.IsNotNull(entitlementsPrinciple.CoreServices);
            Assert.IsNotNull(entitlementsPrinciple.CoreServices.EmailService);
            Assert.IsTrue(entitlementsPrinciple.CoreServices.EmailService.IsFullTextODEAllowed);
            //Console.WriteLine(@"IsFullTextODEAllowed: " + entitlementsPrinciple.CoreServices.EmailService.IsFullTextODEAllowed);
        }

        [TestMethod]
        public void IsFullTextODEAllowed_Should_Be_False_When_Email_FTODE_Flag_Is_Not_1()
        {

            GetUserAuthorizationsResponse getUserAuthorizationsResponse = new GetUserAuthorizationsResponse
            {
                AuthorizationMatrix = new AuthorizationMatrix
                {
                    Email = new MatrixEmailService
                    {
                        ac3 = new List<string> { "15" },
                        FTODE = "0"
                    }
                },
                RuleSet = "8999CPBE99999999RN9999999999999999999999990099000099990000000099999900000000009999990000999999999999000000000099MM9998999899000000999999000000000000000000000000"
            };

            var entitlementsPrinciple = new EntitlementsPrinciple(getUserAuthorizationsResponse);

            Assert.IsNotNull(entitlementsPrinciple);
            Assert.IsNotNull(entitlementsPrinciple.CoreServices);
            Assert.IsNotNull(entitlementsPrinciple.CoreServices.EmailService);
            Assert.IsFalse(entitlementsPrinciple.CoreServices.EmailService.IsFullTextODEAllowed);
            //Console.WriteLine(@"IsFullTextODEAllowed: " + entitlementsPrinciple.CoreServices.EmailService.IsFullTextODEAllowed);
        }

        [TestMethod]
        public void IsFullTextODEAllowed_Should_Be_False_When_Email_FTODE_Flag_Is_Not_Exist()
        {

            GetUserAuthorizationsResponse getUserAuthorizationsResponse = new GetUserAuthorizationsResponse
            {
                AuthorizationMatrix = new AuthorizationMatrix
                {
                    Email = new MatrixEmailService
                    {
                        ac3 = new List<string> { "15" }
                    }
                },
                RuleSet = "8999CPBE99999999RN9999999999999999999999990099000099990000000099999900000000009999990000999999999999000000000099MM9998999899000000999999000000000000000000000000"
            };

            var entitlementsPrinciple = new EntitlementsPrinciple(getUserAuthorizationsResponse);

            Assert.IsNotNull(entitlementsPrinciple);
            Assert.IsNotNull(entitlementsPrinciple.CoreServices);
            Assert.IsNotNull(entitlementsPrinciple.CoreServices.EmailService);
            Assert.IsFalse(entitlementsPrinciple.CoreServices.EmailService.IsFullTextODEAllowed);
            //Console.WriteLine(@"IsFullTextODEAllowed: " + entitlementsPrinciple.CoreServices.EmailService.IsFullTextODEAllowed);
        }
    }
}
