using System;
using DowJones.Preferences;
using DowJones.Security;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Services.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure.Security
{
    [TestClass]
    public class ReaderXTest
    {
        private ControlData m_ControlData = new ControlData { UserID = "JimS", UserPassword = "password", ProductID = "16" };
        // block0441 & User0026: do not pass
        private IPreferences m_preferences = new DowJones.Preferences.Preferences("en");
        
        [TestMethod]
        public void ReaderX_WhenCibsAc3ContainsRMOrXM_IsReaderXUserIsTrue()
        {
            var request = new GetUserAuthorizationsRequest();
            ServiceResponse response = null;
            GetUserAuthorizationsResponse getUserAuthorizationsResponse = null;
            try
            {
                response = MembershipService.GetUserAuthorizations(m_ControlData, request);
            }
            catch
            {}

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.rc);

            object obj = null;
            response.GetResponse(ServiceResponse.ResponseFormat.Object, out obj);
            Assert.IsNotNull(obj);

            getUserAuthorizationsResponse = obj as GetUserAuthorizationsResponse;
            Assert.IsNotNull(getUserAuthorizationsResponse);
            Assert.IsNotNull(getUserAuthorizationsResponse.AuthorizationMatrix);
            Assert.IsNotNull(getUserAuthorizationsResponse.AuthorizationMatrix.Cibs);
            Assert.IsNotNull(getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3);

            if (!getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3.Contains("RM")
                || !getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3.Contains("XM"))
            {
                getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3.Add("RM");
            }

            EntitlementsPrinciple entitlementsPrinciple = new EntitlementsPrinciple(getUserAuthorizationsResponse);
            Assert.IsTrue(entitlementsPrinciple.CoreServices.CIBsService.IsReaderXUser, "IsReaderXUser should be true");
        }

        [TestMethod]
        public void ReaderX_WhenCibsAc3DoesNotContainRMOrXM_IsReaderXUserIsFalse()
        {
            var request = new GetUserAuthorizationsRequest();
            ServiceResponse response = null;
            GetUserAuthorizationsResponse getUserAuthorizationsResponse = null;
            try
            {
                response = MembershipService.GetUserAuthorizations(m_ControlData, request);
            }
            catch
            { }

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.rc);

            object obj = null;
            response.GetResponse(ServiceResponse.ResponseFormat.Object, out obj);
            Assert.IsNotNull(obj);

            getUserAuthorizationsResponse = obj as GetUserAuthorizationsResponse;
            Assert.IsNotNull(getUserAuthorizationsResponse);
            Assert.IsNotNull(getUserAuthorizationsResponse.AuthorizationMatrix);
            Assert.IsNotNull(getUserAuthorizationsResponse.AuthorizationMatrix.Cibs);
            Assert.IsNotNull(getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3);

            if (getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3.Contains("RM")
                || getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3.Contains("XM"))
            {
                getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3.RemoveAll(x => x.Equals("RM"));
                getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3.RemoveAll(x => x.Equals("XM"));
            }

            EntitlementsPrinciple entitlementsPrinciple = new EntitlementsPrinciple(getUserAuthorizationsResponse);
            Assert.IsFalse(entitlementsPrinciple.CoreServices.CIBsService.IsReaderXUser, "IsReaderXUser should be false");
        }
    }
}
