using System;
using System.Reflection;
using DowJones.DependencyInjection;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DowJones
{
    [TestClass]
    public abstract class UnitTestFixture
    {
        private IServiceLocator _originalServiceLocator;

        protected Mock<IServiceLocator> MockServiceLocator
        {
            get { return _mockServiceLocator; }
            set
            {
                if (_originalServiceLocator == null)
                    _originalServiceLocator = ServiceLocator.Current;

                _mockServiceLocator = value;

                if(_mockServiceLocator == null)
                    ServiceLocator.Current = null;
                else
                    ServiceLocator.Current = value.Object;
            }
        }
        private Mock<IServiceLocator> _mockServiceLocator;

        public virtual TestContext TestContext { get; set; }

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void TestFixtureInitialize(TestContext testContext) { }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void TestFixtureCleanup() { }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public virtual void SetUp()
        {
            MockServiceLocator = new Mock<IServiceLocator>();
        }

        // Use TestInitialize to run code after running each test 
        [TestCleanup]
        public virtual void TearDown()
        {
            MockServiceLocator = null;
            ServiceLocator.Current = _originalServiceLocator;
        }

        protected static string GetWebResourceUrl(Assembly assembly, string resourceName)
        {
            return string.Format("/WebResource.axd?d={0}.{1}", assembly.GetName().Name, resourceName);
        }
    }


    [TestClass]
    public abstract class UnitTestFixtureBase<TUnitUnderTest> : UnitTestFixture 
    {
        protected TUnitUnderTest UnitUnderTest { get; set; }

        protected ILog MockLogger
        {
            get { return new Mock<ILog>().Object; }
        }

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();

            UnitUnderTest = CreateUnitUnderTest();
        }

        [TestCleanup]
        public override void TearDown()
        {
            base.TearDown();

            if (UnitUnderTest is IDisposable)
                ((IDisposable)UnitUnderTest).Dispose();

            UnitUnderTest = default(TUnitUnderTest);
        }

        protected abstract TUnitUnderTest CreateUnitUnderTest();
    }
}
