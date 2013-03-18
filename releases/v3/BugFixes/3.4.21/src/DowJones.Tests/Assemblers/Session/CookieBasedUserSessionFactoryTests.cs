using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Xml.Linq;
using DowJones.Mocks;
using DowJones.Properties;
using DowJones.Session;
using DowJones.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DowJones.Assemblers.Session
{
    [TestClass]
    public class UserSessionFactoryTests : UnitTestFixtureBase<UserSessionFactory>
    {
        private Mock<HttpContextBase> _mockHttpContext;
        private Mock<HttpRequestBase> _mockRequest;
        private Mock<HttpResponseBase> _mockResponse;
        private Mock<ReferringProduct> _mockReferringProduct;

        private CookieGenerator _cookieGenerator;

        protected UserSessionFactory Factory
        {
            get { return UnitUnderTest; }
        }

        protected string ProductPrefix
        {
            get { return Settings.Default.DefaultProductPrefix; }
        }

        [TestMethod]
        public void ShouldPopulateAccountIdAndSessionIdAndProductIdAndUserIdFromGlobalSessionCookieIfExists()
        {
            const string ExpectedAccountId = "AccountId";
            const string ExpectedProductId = "ProductId";
            string ExpectedSessionID = Guid.NewGuid().ToString();
            const string ExpectedUserId = "UserId";

            var sessionCookie = _cookieGenerator.GenerateSessionCookie();
            _cookieGenerator.SetValue(sessionCookie, "_A", ExpectedAccountId);
            _cookieGenerator.SetValue(sessionCookie, "_N", ExpectedProductId);
            _cookieGenerator.SetValue(sessionCookie, "_S", ExpectedSessionID);
            _cookieGenerator.SetValue(sessionCookie, "_U", ExpectedUserId);
            _mockRequest.Setup(x => x.Cookies).Returns(new HttpCookieCollection { sessionCookie });

            var session = Factory.Create();

            Assert.AreEqual(ExpectedAccountId, session.AccountId);
            Assert.AreEqual(ExpectedProductId, session.ProductId);
            Assert.AreEqual(ExpectedSessionID, session.SessionId);
            Assert.AreEqual(ExpectedUserId, session.UserId);
        }

        [TestMethod]
        public void ShouldPopulateProductPrefixFromFactoryProperty()
        {
            var session = Factory.Create();
            Assert.AreEqual(Factory.ProductPrefix, session.ProductPrefix);
        }

        [TestMethod]
        public void ShouldGetAccountIdAndProductIdAndUserIdFromPermCookieIfSessionCookieDoesNotExist()
        {
            const string ExpectedAccountId = "AccountId";
            const string ExpectedProductId = "ProductId";
            const string ExpectedUserId = "UserId";

            var permCookie = _cookieGenerator.GeneratePermanentCookie();
            _cookieGenerator.SetValue(permCookie, "_A", ExpectedAccountId);
            _cookieGenerator.SetValue(permCookie, "_N", ExpectedProductId);
            _cookieGenerator.SetValue(permCookie, "_U", ExpectedUserId);
            _mockRequest.Setup(x => x.Cookies).Returns(new HttpCookieCollection { permCookie });

            var session = Factory.Create();

            Assert.AreEqual(ExpectedAccountId, session.AccountId);
            Assert.AreEqual(ExpectedProductId, session.ProductId);
            Assert.AreEqual(ExpectedUserId, session.UserId);
        }

        [TestMethod]
        public void ShouldPopulateProxyUsernameAndNamespaceFromXml()
        {
            const string ExpectedProxyUserID = "PROXY_USER_ID";
            const string ExpectedProxyNamespace = "PROXY_NAMESPACE";

            var session = new UserSession();

            var credentialsDoc = new XDocument(
                    new XElement("credentials",
                        new XElement("proxyUserId", ExpectedProxyUserID),
                        new XElement("proxyUserNamespace", ExpectedProxyNamespace)
                    )
                );

            Factory.PopulateProxyInfoFromCredentials(session, credentialsDoc.ToString(SaveOptions.DisableFormatting));

            Assert.AreEqual(ExpectedProxyUserID, session.ProxyUserId);
            Assert.AreEqual(ExpectedProxyNamespace, session.ProxyNamespace);
        }

        [TestMethod]
        public void ShouldPopulateProxyUsernameAndNamespaceFromJson()
        {
            const string ExpectedProxyUserID = "PROXY_USER_ID";
            const string ExpectedProxyNamespace = "PROXY_NAMESPACE";

            var session = new UserSession();

            var credentials = string.Format("{{\"proxyUserId\": \"{0}\", \"proxyUserNamespace\": \"{1}\"}}", ExpectedProxyUserID, ExpectedProxyNamespace);

            Factory.PopulateProxyInfoFromCredentials(session, credentials);

            Assert.AreEqual(ExpectedProxyUserID, session.ProxyUserId);
            Assert.AreEqual(ExpectedProxyNamespace, session.ProxyNamespace);
        }

        [TestMethod]
        public void ShouldFailToPopulateProxyUsernameAndNamespaceFromInvalidString()
        {
            var session = new UserSession();

            const string credentials = "Dummy string that is not valid JSON or XML";

            try
            {
                Factory.PopulateProxyInfoFromCredentials(session, credentials);
                Assert.Fail("Should throw an Argument Exception");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
            
        }


        [TestMethod]
        public void ShouldEnableDebuggingIfDebugLevelIsGreaterThanZeroOnLoginCookie()
        {
            var loginCookie = new HttpCookie(UserSessionFactory.LoginCookieName);
            loginCookie[UserSessionFactory.LoginCookieDebugLevelKey] = "1";
            _mockRequest.Setup(x => x.Cookies).Returns(new HttpCookieCollection { loginCookie });

            var session = Factory.Create();

            Assert.IsTrue(session.IsDebug);
        }

        [TestMethod]
        public void ShouldEnableDebuggingIfDebugLevelIsSpecifiedInHttpRequest()
        {
            var request = new MockHttpRequest(new Dictionary<string, string> {
                                { UserSessionFactory.RequestDebugLevelKey, "1" }
                            });
            _mockHttpContext.Setup(x => x.Request).Returns(request);

            var session = Factory.Create();

            Assert.IsTrue(session.IsDebug);
        }

        [TestMethod]
        public void ShouldDisableDebuggingByDefault()
        {
            var session = Factory.Create();

            Assert.IsFalse(session.IsDebug);
        }

        [TestMethod]
        public void ShouldPopulateAccessPointCodeFromReferringProduct()
        {
            const string ExpectedAccessPointCode = "1234";

            _mockReferringProduct
                .SetupGet(x => x.AccessPointCode)
                .Returns(ExpectedAccessPointCode);

            var session = Factory.Create();

            Assert.AreEqual(ExpectedAccessPointCode, session.AccessPointCode);
        }

        protected override UserSessionFactory CreateUnitUnderTest()
        {
            _mockRequest = new Mock<HttpRequestBase>();
            _mockRequest.Setup(x => x.Cookies).Returns(new HttpCookieCollection());
            _mockRequest.Setup(x => x.Headers).Returns(new NameValueCollection());
            
            _mockResponse = new Mock<HttpResponseBase>();
            _mockResponse.Setup(x => x.Cookies).Returns(new HttpCookieCollection());

            _mockHttpContext = new Mock<HttpContextBase>();
            _mockHttpContext
                .Setup(x => x.Request)
                .Returns(_mockRequest.Object);
            _mockHttpContext
                .Setup(x => x.Response)
                .Returns(_mockResponse.Object);

            _mockReferringProduct = new Mock<ReferringProduct>();
            
            _cookieGenerator = new CookieGenerator { ProductPrefix = ProductPrefix };

            var factory = 
                new UserSessionFactory(
                    _mockHttpContext.Object, 
                    new HttpCookieManager(_mockHttpContext.Object),
                    _mockReferringProduct.Object
                );

            return factory;
        }
    }
}
