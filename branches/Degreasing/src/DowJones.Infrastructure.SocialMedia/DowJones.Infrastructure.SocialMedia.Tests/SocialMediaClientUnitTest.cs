// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClientUnitTest.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   This is used for TDD development of the SocialMediaClientUnit
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Infrastructure.Models.SocialMedia;

namespace DowJones.Infrastructure.SocialMedia.Tests
{      
    /// <summary>
    /// This is used for TDD development of the SocialMediaClientUnit
    /// </summary>
    [TestClass]
    public class SocialMediaClientUnitTest
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Properties

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion

        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        #region Public Methods

        /// <summary>
        /// The account health check.
        /// </summary>
        [TestMethod]
        public void AccountHealthCheck()
        {
            var client = new SocialMediaClient();
            var temp = client.PerformAccountHealthCheck();
            Assert.IsTrue(temp.Status == Status.Success);
            Assert.IsTrue(temp.IsOk);
        }

        /// <summary>
        /// The get tweets.
        /// </summary>
        [TestMethod]
        public void GetTweetsByCategories()
        {
            var client = new SocialMediaClient();
            var temp = client.GetTweetsByCategory("all/sports/football");

            Assert.IsTrue(temp.Status == Status.Success);
            Assert.IsTrue(temp.Count > 0);
        }

        /// <summary>
        /// Performs the categories search.
        /// </summary>
        [TestMethod]
        public void PerformCategoriesSearch()
        {
            var client = new SocialMediaClient();
            var temp = client.PerformCategoriesSearch("cupcakes");

            Assert.IsTrue(temp.Status == Status.Success);
            Assert.IsTrue(temp.TotalCount > 0);
        }

        /// <summary>
        /// Performs the lists search.
        /// </summary>
        [TestMethod]
        public void PerformListsSearch()
        {
            var client = new SocialMediaClient();
            var temp = client.PerformListsSearch("cupcakes");

            Assert.IsTrue(temp.Status == Status.Success);
            Assert.IsTrue(temp.TotalCount > 0);
        }

        /// <summary>
        /// Gets the recent lists.
        /// </summary>
        [TestMethod]
        public void GetRecentLists()
        {
            var client = new SocialMediaClient();
            var temp = client.GetRecentLists(50, 2);

            Assert.IsTrue(temp.Status == Status.Success);
            Assert.IsTrue(temp.TotalCount > 0);
        }

        /// <summary>
        /// Gets the list detail.
        /// </summary>
        [TestMethod]
        public void GetListDetail()
        {
            var client = new SocialMediaClient();
            var temp = client.GetListDetail("kenreisman", "machine-learning");

            Assert.IsTrue(temp.Status == Status.Success);
            Assert.IsTrue(temp.User != null);
        }

        /// <summary>
        /// Gets the list members.
        /// </summary>
        [TestMethod]
        public void GetListMembers()
        {
            var client = new SocialMediaClient();
            var temp = client.GetListMembers("kenreisman", "machine-learning", 10, 2);

            Assert.IsTrue(temp.Status == Status.Success);
            Assert.IsTrue(temp.Users.Count > 0);
        }

        /// <summary>
        /// Gets the category super list.
        /// </summary>
        [TestMethod]
        public void GetCategorySuperList()
        {
            var client = new SocialMediaClient();
            var temp = client.GetCategorySuperList("all/sports/baseball", true);

            Assert.IsTrue(temp.Status == Status.Success);
            Assert.IsTrue(temp.Id > 0);
        }

        /// <summary>
        /// Gets the category users.
        /// </summary>
        [TestMethod]
        public void GetCategoryUsers()
        {
            var client = new SocialMediaClient();
            var temp = client.GetCategoryUsers("all/sports/baseball");

            Assert.IsTrue(temp.Status == Status.Success);
            Assert.IsTrue(temp.TotalCount > 0); 
        }

        /// <summary>
        /// Gets the category lists.
        /// </summary>
        [TestMethod]
        public void GetCategoryLists()
        {
            var client = new SocialMediaClient();
            var temp = client.GetCategoryLists("all/sports/baseball", 1000);

            Assert.IsTrue(temp.Status == Status.Success);
            Assert.IsTrue(temp.TotalCount > 0);
        }

        [TestMethod]
        public void TestDateConversion()
        {
            DateTime date;
            if( DateTime.TryParseExact( "Tue Jul 5 20:01:43 2011", "ddd MMM d HH':'mm':'ss yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date ) )
            {
                Console.WriteLine(date.ToString());
                return;
            }
            Console.WriteLine( "unable to parse string" );
            
        }

        public void TestTweetEntity()
        {
            var rawTweet = new[] {
                               "RT @Jamar51Chaney: Enough w/ all that y'all talkin bout on Twitter. Let's get this #Lockout over & get this season on the role. #NFL I agree",
                               "Rob Rubick on WZAM: Lions quarterback Matthew Stafford is 'unlucky' not 'injury-prone' bit.ly/lRo3lr"
                               };

            //rawTweet.
        }

        #endregion
    }
}