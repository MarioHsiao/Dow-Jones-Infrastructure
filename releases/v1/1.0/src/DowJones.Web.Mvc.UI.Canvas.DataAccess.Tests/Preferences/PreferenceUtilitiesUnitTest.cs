// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreferenceUtilitiesUnitTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using DJ.Core.Logging;
using DowJones.Session;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Preferences
{
    /// <summary>
    /// Description for BasePreferenceUnitTest
    /// </summary>
    [TestClass]
    public class PreferenceUtilityUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
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
        #endregion


        [TestMethod]
        public void TestDeserialization()
        {
            var preferences = GetUserPreferences();

            Console.WriteLine(preferences.ContentLanguages.Count);
        }

        protected static BasePreferences GetUserPreferences()
        {
            var preference = new BasePreferences();
            preference.ContentLanguages = new ContentLanguageCollection{""};
            const string PreferencesInput = @"<preferences>
                                        <interfaceLanguage>en</interfaceLanguage>
                                        <timeZone></timeZone>
                                        <clockType></clockType>
                                        <contentLanguages>
                                         <contentLanguage></contentLanguage>
                                        </contentLanguages>
                                        </preferences>";

            if (!string.IsNullOrWhiteSpace(PreferencesInput))
            {
                preference = (BasePreferences)SerializationUtility.Deserialize(PreferencesInput, preference.GetType());
            }
            else
            {
                preference.InterfaceLanguage = "en";
            }

            return preference;
        }

        protected class SerializationUtility
        {
            public static object Deserialize(string xmlRequest, Type objectType)
            {
                var ser = new DataContractSerializer(objectType);

                var enc = new UTF8Encoding();
                var b = enc.GetBytes(xmlRequest);
                var reader = XmlDictionaryReader.CreateTextReader(b, new XmlDictionaryReaderQuotas());
                var obj = ser.ReadObject(reader);
                return obj;
            }
        }
    }
}
