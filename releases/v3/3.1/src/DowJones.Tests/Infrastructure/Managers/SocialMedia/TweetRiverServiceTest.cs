using System;
using DowJones.Infrastructure.Models.SocialMedia;
using DowJones.Managers.SocialMedia;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Managers.SocialMedia.TweetRiver;

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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetTweetsByChannel
        ///</summary>
        [TestMethod]
        public void GetTweetsByChannelTest()
        {
            SocialMediaService target = new SocialMediaService(new TweetRiverProvider());
            string channel = "green-energy"; 
            GetTweetsByChannelResponse actual;
            actual = target.GetTweetsByChannel(channel, new RequestOptions() {Limit = 20});
            Assert.AreEqual(Status.Success, actual.Status);
            Assert.IsTrue(actual.Capacity > 0);
        }

        /// <summary>
        ///A test for GetTweetsByIndustry
        ///</summary>
        [TestMethod]
        public void GetTweetsByIndustryTest()
        {
            SocialMediaService target = new SocialMediaService(new TweetRiverProvider());
            string industryCode = "iacc";
            GetTweetsByChannelResponse actual;
            actual = target.GetTweetsByIndustry(industryCode, new RequestOptions() { Limit = 20 });
            Assert.AreEqual(Status.Success, actual.Status);
            Assert.IsTrue(actual.Capacity > 0);
        }

        /// <summary>
        ///A negative test for GetTweetsByIndustry by passing invalid code
        ///</summary>
        [TestMethod]
        public void ShouldFailOnGetTweetsByIndustry()
        {
            SocialMediaService target = new SocialMediaService(new TweetRiverProvider());
            string industryCode = "junk";
            GetTweetsByChannelResponse actual;
            try
            {
                actual = target.GetTweetsByIndustry(industryCode, new RequestOptions() { Limit = 20 });
                Assert.Fail("Should have thrown exception");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                
                Assert.IsTrue(ex != null);
            }
           
        }
    }
}
