using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DowJones.Web.Mvc.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.UI.ClientState
{
    [TestClass]
    public class ClientStateMapperTests : UnitTestFixtureBase<ClientStateMapper>
    {
        public const string NamedClientEventHandlerEventName = "onClick";

        protected ClientStateMapper Mapper
        {
            get { return UnitUnderTest; }
        }

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();

            MockServiceLocator
                .Setup(x => x.Resolve(typeof(CustomTypeConverter)))
                .Returns(new CustomTypeConverter());
        }

        [TestCleanup]
        public override void TearDown()
        {
            base.TearDown();
        }


        [TestMethod]
        public void ShouldMapClientData()
        {
            var adornedClass = new AdornedSourceClass();

            var clientState = Mapper.Map(adornedClass);

            Assert.AreEqual(clientState.Data, adornedClass.ClientDataProperty);
        }

        [TestMethod]
        public void ShouldMapClientEventHandlers()
        {
            var adornedClass = new AdornedSourceClass();

            var clientState = Mapper.Map(adornedClass);

            Assert.AreEqual(clientState.EventHandlers["ClientEventHandlerProperty"], adornedClass.ClientEventHandlerProperty);
        }

        [TestMethod]
        public void ShouldMapNamedClientEventHandlers()
        {
            var adornedClass = new AdornedSourceClass();

            var clientState = Mapper.Map(adornedClass);

            Assert.AreEqual(clientState.EventHandlers[NamedClientEventHandlerEventName],
                            adornedClass.NamedClientEventHandlerProperty);
        }

        [TestMethod]
        public void ShouldMapClientProperties()
        {
            var adornedClass = new AdornedSourceClass();

            var clientState = Mapper.Map(adornedClass);

            Assert.AreEqual(clientState.Options["ClientOptionProperty"], adornedClass.ClientOptionProperty);
        }

        [TestMethod]
        public void ShouldApplyTypeConverterToClientProperties()
        {
            var adornedClass = new AdornedSourceClass();

            var clientState = Mapper.Map(adornedClass);

            Assert.AreEqual(CustomTypeConverter.ConvertedValue, clientState.Options["ConvertedClientOptionProperty"]);
        }

        [TestMethod]
        public void ShouldMergeUnNestedClientPropertiesDictionary()
        {
            var adornedClass = new AdornedSourceClass();
            var expectedOption = adornedClass.MergableClientOptionProperty.First();

            var clientState = Mapper.Map(adornedClass);

            Assert.AreEqual(clientState.Options[expectedOption.Key], expectedOption.Value);
        }

        [TestMethod]
        public void ShouldNotMergeNestedClientPropertiesDictionary()
        {
            var adornedClass = new AdornedSourceClass();

            var clientState = Mapper.Map(adornedClass);

            Assert.AreEqual(clientState.Options["NestedClientOptionProperty"], adornedClass.NestedClientOptionProperty);
        }

        [TestMethod]
        public void ShouldMapClientPropertiesFromSuperClasses()
        {
            var adornedSourceSubClass = new AdornedSourceSubClass();

            var clientState = Mapper.Map(adornedSourceSubClass);

            Assert.AreEqual(clientState.Options["SubClassClientOptionProperty"], adornedSourceSubClass.SubClassClientOptionProperty);
        }

        [TestMethod]
        public void ShouldMapClientTokens()
        {
            var adornedClass = new AdornedSourceClass();

            var clientState = Mapper.Map(adornedClass);

            Assert.AreEqual(clientState.Tokens["ClientTokenProperty"], adornedClass.ClientTokenProperty);
        }

        [TestMethod]
        public void ShouldMapClientTokensFromDictionary()
        {
            var adornedClass = new AdornedSourceClass();
            var expectedToken = adornedClass.ClientTokenDictionaryProperty.First();

            var clientState = Mapper.Map(adornedClass);

            Assert.AreEqual(clientState.Tokens[expectedToken.Key], expectedToken.Value);
        }




        protected override ClientStateMapper CreateUnitUnderTest()
        {
            ClientStateMapper unitUnderTest = new ClientStateMapper();
            return unitUnderTest;
        }


        internal class AdornedSourceClass
        {
            [ClientData]
            public object ClientDataProperty { get; set; }

            [ClientEventHandler]
            public string ClientEventHandlerProperty { get; set; }

            [ClientEventHandler(Name = NamedClientEventHandlerEventName)]
            public string NamedClientEventHandlerProperty { get; set; }

            [ClientProperty]
            public int ClientOptionProperty { get; set; }

            [ClientToken]
            public string ClientTokenProperty { get; set; }

            [TypeConverter(typeof(CustomTypeConverter))]
            [ClientProperty]
            public string ConvertedClientOptionProperty { get; set; }

            [ClientToken]
            public IDictionary<string, string> ClientTokenDictionaryProperty { get; set; }

            [ClientProperty(Nested = false)]
            public IDictionary<string, object> MergableClientOptionProperty { get; set; }

            [ClientProperty(Nested = true)]
            public IDictionary<string, object> NestedClientOptionProperty { get; set; }


            public AdornedSourceClass()
            {
                ClientDataProperty = new { Name = "Test object" };
                ClientEventHandlerProperty = "ClientEvent";
                NamedClientEventHandlerProperty = "NamedClientEvent";
                ClientOptionProperty = 1234;
                ClientTokenProperty = "ClientToken";
                ClientTokenDictionaryProperty = new Dictionary<string, string> { { "SomeToken", "Token Value" } };
                MergableClientOptionProperty = new Dictionary<string, object> { { "MyValue", "Merged Value" } };
                NestedClientOptionProperty = new Dictionary<string, object> { { "MyValue", "Nested Value" } };
            }
        }

        internal class AdornedSourceSubClass : AdornedSourceClass
        {
            [ClientProperty]
            public double SubClassClientOptionProperty { get; set; }

            public AdornedSourceSubClass()
            {
                SubClassClientOptionProperty = 123345;
            }
        }


        internal class CustomTypeConverter : TypeConverter
        {
            internal const string ConvertedValue = "CONVERTED VALUE";

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
            {
                return ConvertedValue;
            }
        }

    }
}