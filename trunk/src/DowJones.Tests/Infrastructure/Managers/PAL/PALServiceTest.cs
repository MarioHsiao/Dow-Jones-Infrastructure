using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DowJones.Managers.PAL;
using DowJones.Pages;
using DowJones.Session;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GWShareScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope;

namespace DowJones.Infrastructure.Managers.PAL
{
    /// <summary>
    /// Summary description for PALServiceTest
    /// </summary>
    [TestClass]
    public class PALServiceTest : UnitTestFixtureBase<PALServiceManager>
    {
        //private Mock<StubPALPreferenceProvider> _mockPALPreferencesSource;
        private Mock<PALPreferenceServiceProvider> _mockPALPreferencesSource;
        
        protected PALServiceManager ServiceManager
        {
            get { return UnitUnderTest; }
        }
        
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void UpdateItemScope()
        {
            List<long> savedSearchId = new List<long>();
            savedSearchId.Add(1000);
            savedSearchId.Add(2000);
            savedSearchId.Add(3000);

            ServiceManager.UpdateSavedSearchScope(savedSearchId, GWShareScope.Account);
        }

        protected override PALServiceManager CreateUnitUnderTest()
        {
            //_mockPALPreferencesSource = new Mock<StubPALPreferenceProvider>();
            _mockPALPreferencesSource = new Mock<PALPreferenceServiceProvider>();

            return new PALServiceManager(_mockPALPreferencesSource.Object);

        }
    }
}
