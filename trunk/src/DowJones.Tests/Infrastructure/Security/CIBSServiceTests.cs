using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Moq;
using DowJones.Security;
using DowJones.Preferences;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Utils.V1_0;

namespace DowJones.Infrastructure.Security
{
    [TestClass]
    public class CIBSServiceTests : UnitTestFixture
    {

        private ControlData _controlData = new ControlData { UserID = "JimS", UserPassword = "password", ProductID = "16" };
        private IPreferences _preferences = new DowJones.Preferences.Preferences("en");
        
        [TestMethod]
        public void ShouldCheckAC3andAC4valuesAreReturnedFromCIBS()
        {            

            var request = new GetUserAuthorizationsRequest();
            ServiceResponse response = null;
            GetUserAuthorizationsResponse getUserAuthorizationsResponse = null;
            object obj = null;

            try
            {
                response = Factiva.Gateway.Services.V1_0.MembershipService.GetUserAuthorizations(_controlData, request);
            }
            catch {}

            Assert.IsNotNull(response);
            Assert.IsTrue(response.rc == 0);
            
            response.GetResponse(ServiceResponse.ResponseFormat.Object, out obj);

            Assert.IsNotNull(obj);
            getUserAuthorizationsResponse = obj as GetUserAuthorizationsResponse;
                        
            Assert.IsNotNull(getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3);
            Assert.IsNotNull(getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac4);

            var ac3 = getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac3;
            Assert.IsTrue(ac3.Count > 0);
            ac3.ForEach(f => Assert.IsNotNull(f));

            var ac4 = getUserAuthorizationsResponse.AuthorizationMatrix.Cibs.ac4;
            Assert.IsTrue(ac4.Count > 0);
            ac4.ForEach(f => Assert.IsNotNull(f));
        }
    }
}
