using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure.Preferences
{
    /// <summary>
    /// Summary description for PreferenceServiceTest
    /// </summary>
    [TestClass]
    public class PreferenceServiceTest : UnitTestFixture
    {
        private ControlData m_ControlData = new ControlData { UserID = "joshimoi", UserPassword = "passwd", ProductID = "16" };

        public PreferenceServiceTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestMethod]
        public void GatewayGetItemsByClassIDNoCacheTest()
        {
            var request = new GetItemsByClassIDNoCacheRequest
            {
                ClassID = new[] { PreferenceClassID.GroupFolder },
                ReturnUsersCategorizedItems = true,
                ReturnBlob = true
            };
            ServiceResponse response = null;
            GetItemsByClassIDNoCacheResponse getItemsByClassIdNoCacheResponse = null;
            object obj = null;
            try
            {
                response = Factiva.Gateway.Services.V1_0.PreferenceService.GetItemsByClassIDNoCache(m_ControlData, request);
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
                getItemsByClassIdNoCacheResponse = obj as GetItemsByClassIDNoCacheResponse;
                Assert.IsNotNull(getItemsByClassIdNoCacheResponse);
            }
            else
            {
                //Always guarantee the object is created!
                var message = "Error code=";
                if (response != null)
                {
                    message += response.rc;
                }
                
                Console.WriteLine(message);
                //_logger.Warn(message);

                //return new UninitializedPrinciple();
            }
        }
    }
}
