using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.UI
{
    [TestClass]
    public class ClientResourceAppSettingProcessorTests : UnitTestFixtureBase<ClientResourceAppSettingProcessor>
    {
        private ClientResource _clientResource;
        private NameValueCollection _appSettings;


        protected override ClientResourceAppSettingProcessor CreateUnitUnderTest()
        {
            _clientResource = new ClientResource(string.Empty) { PerformSubstitution = true };
            _appSettings = new NameValueCollection();
            
            var processor = new ClientResourceAppSettingProcessor { AppSettings = _appSettings };

            return processor;
        }


        [TestMethod]
        public void ShouldReplaceAppSettingInContent()
        {
            const string appSettingName = "APP_SETTING_NAME";
            const string appSettingValue = "APP_SETTING_VALUE";
            string content = string.Format("This is an app setting replacement example: <div>100% <%= AppSetting(\"{0}\") %></div>", appSettingName);

            _appSettings.Add(appSettingName, appSettingValue);

            var resource = new ProcessedClientResource(_clientResource, content);
            UnitUnderTest.Process(resource);

            Assert.AreEqual(
                    Regex.Replace(content, @"\<%=.*%>", appSettingValue),
                    resource.Content
                );
        }

        [TestMethod]
        public void ShouldReplaceAppSettingInContentWithDefaultWhenNoAppSettingIsFound()
        {
            const string appSettingName = "APP_SETTING_NAME";
            const string defaultValue = "DEFAULT_VALUE";
            string content = string.Format("This is an app setting replacement example: <div>100% <%= AppSetting(\"{0}\", \"{1}\") %></div>", 
                                           appSettingName, defaultValue);

            // No AppSetting specified
            _appSettings.Clear();

            var resource = new ProcessedClientResource(_clientResource, content);
            UnitUnderTest.Process(resource);

            Assert.AreEqual(
                    Regex.Replace(content, @"\<%=.*%>", defaultValue),
                    resource.Content
                );
        }
    }
}
