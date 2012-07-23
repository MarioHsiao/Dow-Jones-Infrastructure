using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using DowJones.Mapping;
using DowJones.Mocks;
using DowJones.Token;
using DowJones.Web;
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

        protected string CurrentDirectory
        {
            get { return Path.GetDirectoryName(GetType().Assembly.Location); }
        }

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
            MockServiceLocator.Setup(x => x.Resolve<ITokenRegistry>()).Returns(new MockTokenRegistry());

            var locator = new TypeMappingLocator(new AssemblyRegistry(typeof(ClientResourceDefinitionToClientResourceMapper).Assembly, GetType().Assembly));
            var mapperDefinitions = locator.Locate(mapperType => (ITypeMapper)Activator.CreateInstance(mapperType));
            Mapper.Instance = new Mapper(mapperDefinitions);
        }

        // Use TestCleanup to run code after running each test 
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

        protected static void ExpectException<TException>(Action action, string message = null) where TException : Exception
        {
            action.ExpectException<TException>(message);
        }

        protected static string Serialize(object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            using (var stringWriter = new StringWriter())
            {
                var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
                serializer.Serialize(xmlTextWriter, obj);
                return stringWriter.ToString();
            }
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
