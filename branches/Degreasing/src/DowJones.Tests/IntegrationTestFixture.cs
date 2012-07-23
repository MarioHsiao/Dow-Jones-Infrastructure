using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones
{
    [TestClass]
    public abstract class IntegrationTestFixture : UnitTestFixture
    {
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public new static void TestFixtureInitialize(TestContext testContext)
        {
            UnitTestFixture.TestFixtureInitialize(testContext);
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public new static void TestFixtureCleanup()
        {
            UnitTestFixture.TestFixtureCleanup();
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public override void SetUp()
        {
            /* Reserved for future implementation */
            base.SetUp();
        }

        // Use TestInitialize to run code after running each test 
        [TestCleanup]
        public override void TearDown()
        {
            /* Reserved for future implementation */
            base.TearDown();
        }
    }
}