using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Managers.SocialMedia.Config;
using DJSession = DowJones.Session;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Managers;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Utils.V1_0;


namespace DowJones.Infrastructure.Managers.SocialMedia
{
    [TestClass]
    public class SocialMediaIndustryTest : IntegrationTestFixture
    {
        [TestMethod]
        [DeploymentItem(@"Infrastructure\Managers\SocialMedia\IndustryChannel.config")]
        public void GetConfigChannelFromIndustryCode()
        {
            using (var stream = File.Open("IndustryChannel.config", FileMode.Open))
            {
                var p = new ConfigSocialMediaIndustryProvider(stream);
                Assert.AreEqual("accounting-consulting", p.GetChannelFromIndustryCode("iacc"));
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GetPAMConfigChannelFromIndustryCode()
        {
            var p = new PAMSocialMediaIndustryProvider(new DJSession.ControlData { UserID = "snap_proxy", UserPassword = "pa55w0rd", ProductID = "16" });
            Assert.AreEqual("accounting-consulting", p.GetChannelFromIndustryCode("iacc"));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GetPageModuleMetadataList()
        {
            var newsPageModuleListRequest = new GetListsDetailsListRequest();
            newsPageModuleListRequest.ListTypes.Add(ListType.SocialMediaChannelMappingList);

            ControlData controlData = ControlDataManager.GetLightWeightUserControlData("snap_proxy", "pa55w0rd", "16");

            ServiceResponse serviceResponse = ListService.GetListsDetailsList(controlData, newsPageModuleListRequest);

            Assert.AreEqual(0, serviceResponse.rc,
                            "ListService.GetListsDetailsList failed with status " + serviceResponse.rc);

            object responseObj;

            Assert.AreEqual(0, serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj));
            Assert.IsNotNull(responseObj);
            Assert.IsInstanceOfType(responseObj, typeof(GetListsDetailsListResponse));
        }

    }

}
