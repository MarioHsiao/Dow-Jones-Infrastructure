using System;
using System.Linq;
using DowJones.Infrastructure.Models.SocialMedia;
using DowJones.Managers.SocialMedia;
using DowJones.Managers.SocialMedia.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Managers.SocialMedia.TweetRiver;
using System.Collections.Generic;
using DJSession = DowJones.Session;

namespace DowJones.Infrastructure.SocialMedia.Tests
{


    /// <summary>
    ///This is a test class for TweetRiverServiceTest and is intended
    ///to contain all TweetRiverServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class TweetRiverServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        /// <summary>
        ///A test for GetTweetsByChannel
        ///</summary>
        [TestMethod]
		[TestCategory("Integration")]
		public void GetTweetsByChannelTest()
		{
			DJSession.IControlData _controlData = new DJSession.ControlData { UserID = "snap_proxy", UserPassword = "pa55w0rd", ProductID = "16" };
            var target = new SocialMediaService(new TweetRiverProvider(), new PAMSocialMediaIndustryProvider(_controlData), _controlData);
            const string channel = "accounting-consulting";
            var actual = target.GetTweetsByChannel(channel, new RequestOptions {Limit = 20});
            Assert.AreEqual(Status.Success, actual.Status);
            Assert.IsTrue(actual.Capacity > 0);
        }

        /// <summary>
        ///A test for GetTweetsByIndustry
        ///</summary>
        [TestMethod]
		[TestCategory("Integration")]
		public void GetTweetsByIndustryTest()
        {
			DJSession.IControlData _controlData = new DJSession.ControlData { UserID = "snap_proxy", UserPassword = "pa55w0rd", ProductID = "16" };
            var target = new SocialMediaService(new TweetRiverProvider(), new PAMSocialMediaIndustryProvider(_controlData), _controlData);
            const string industryCode = "iacc";
            var actual = target.GetTweetsByIndustry(industryCode, new RequestOptions { Limit = 20 }, true);
            Assert.AreEqual(Status.Success, actual.Status);
            Assert.IsTrue(actual.Capacity > 0);
        }


        /// <summary>
        ///A test for GetExpertsByIndustry
        ///</summary>
        [TestMethod]
		[TestCategory("Integration")]
		public void GetExpertsByIndustry()
        {
			DJSession.IControlData _controlData = new DJSession.ControlData { UserID = "snap_proxy", UserPassword = "pa55w0rd", ProductID = "16" };
            var target = new SocialMediaService(new TweetRiverProvider(), new PAMSocialMediaIndustryProvider(_controlData), _controlData);
            const string industryCode = "iacc";
            var actual = target.GetExpertsByIndustry(industryCode);
            Assert.AreEqual(Status.Success, actual.Status);
            Assert.IsTrue(actual.Capacity > 0);
        }

        /// <summary>
        ///A negative test for GetTweetsByIndustry by passing invalid code
        ///</summary>
        [TestMethod]
		[TestCategory("Integration")]
		public void ShouldFailOnGetTweetsByIndustry()
        {
			DJSession.IControlData _controlData = new DJSession.ControlData { UserID = "snap_proxy", UserPassword = "pa55w0rd", ProductID = "16" };
            var target = new SocialMediaService(new TweetRiverProvider(), new PAMSocialMediaIndustryProvider(_controlData), _controlData);
            const string industryCode = "junk";
            try
            {
                target.GetTweetsByIndustry(industryCode, new RequestOptions() { Limit = 20 });
                Assert.Fail("Should have thrown exception");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                
                Assert.IsTrue(ex != null);
            }
           
        }


        /// <summary>
        ///A test for GetTweetsByIndustry
        ///</summary>
        [TestMethod]
		[TestCategory("Integration")]
		public void GetTweetsByIndustryMapperTest()
        {
            List<Ajax.SocialMedia.Tweet> ajaxTweets = new List<Ajax.SocialMedia.Tweet>();
			DJSession.IControlData _controlData = new DJSession.ControlData { UserID = "snap_proxy", UserPassword = "pa55w0rd", ProductID = "16" };
            var target = new SocialMediaService(new TweetRiverProvider(), new PAMSocialMediaIndustryProvider(_controlData), _controlData);
            const string industryCode = "iacc";
            var actual = target.GetTweetsByIndustry(industryCode, new RequestOptions { Limit = 20 });
            Assert.AreEqual(Status.Success, actual.Status);
            Assert.IsTrue(actual.Capacity > 0);

            List<Tweet> tweets = new List<Tweet>(actual);
            Ajax.SocialMedia.Mapper mapper = new Ajax.SocialMedia.Mapper();
            foreach (Tweet tweet in tweets)
            {
                ajaxTweets.Add(mapper.Map(tweet));
            }
            Assert.IsTrue(ajaxTweets.Capacity > 0);
        }


        /// <summary>
        ///A test for GetExpertsByIndustry
        ///</summary>
        [TestMethod]
		[TestCategory("Integration")]
		public void GetExpertsByIndustryMapperTest()
        {
            List<Ajax.SocialMedia.User> ajaxUsers = new List<Ajax.SocialMedia.User>();

			DJSession.IControlData _controlData = new DJSession.ControlData { UserID = "snap_proxy", UserPassword = "pa55w0rd", ProductID = "16" };
            var target = new SocialMediaService(new TweetRiverProvider(), new PAMSocialMediaIndustryProvider(_controlData), _controlData);
            const string industryCode = "iacc";
            var actual = target.GetExpertsByIndustry(industryCode);
            Assert.AreEqual(Status.Success, actual.Status);
            Assert.IsTrue(actual.Capacity > 0);

            List<TwitterUser> users = new List<TwitterUser>(actual);
            Ajax.SocialMedia.Mapper mapper = new Ajax.SocialMedia.Mapper();
            foreach (TwitterUser user in users)
            {
                ajaxUsers.Add(mapper.Map(user));
            }
            Assert.IsTrue(ajaxUsers.Capacity > 0);
        }
    }
}

