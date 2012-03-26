// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AESEncryptionUnitTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Encryption
{
    /// <summary>
    /// AESEncryption Utility UnitTest class.
    /// </summary>
    [TestClass]
    public class AESEncryptionUnitTest : AbstractUnitTest
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
        public void BaselineTest()
        {
            var encryptUtil = new AESEncryptionUtility();
            var collection = new NameValueCollection
                                 {
                                     { "userId", "dacostad" }, 
                                     { "password", "brian" },
                                     { "namespace", "16" }, 
                                     { "expires", DateTime.Now.AddDays(2).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'") }
                                 };

            var tempData = encryptUtil.Encrypt(collection, AESEncryptionUtility.DefaultKey, false);

            Console.WriteLine(encryptUtil.GetStringifiedNameValueCollection(collection));

            Console.WriteLine(tempData);

            Console.WriteLine(@"------------------------->");
            const string Token = "userId[#]parmarg[|]password[#]josh[|]namespace[#]16[|]expires[#]2011-03-19 12:44:15Z[|]";
            Console.WriteLine(@"token-->" + Token);
            var compareText = encryptUtil.Encrypt(Token, AESEncryptionUtility.DefaultKey, false);
            Console.WriteLine(@"compareText-->" + compareText);
            var outputToken = encryptUtil.Decrypt(compareText, AESEncryptionUtility.DefaultKey, false);
            Console.WriteLine(@"ouputToken -->" + outputToken);
            Console.WriteLine(@"------------------------->");


            Assert.IsTrue(Token == outputToken);

            tempData = encryptUtil.Decrypt("70zry52z7vkKMkUe2krfdTDfEk4GxPc7NU/olVgrJeuMsaVlMdAvalqt/oqGDNQI3RLibcwXplyltNyPDu1hEZ7Eq+AIQ8OIpEuFKtiO7doRfUPij/ggKUnNp6acLcaD", AESEncryptionUtility.DefaultKey);
            Console.WriteLine(tempData);
            Console.WriteLine(string.Empty);
            
            tempData = encryptUtil.Decrypt("x79e4hTs2sXCGCzv0kU69jsTbAQsG7X3vxJ2b3CN7vtKrbRgbzNUQgy9mYKda7+T1xXPU5+SiJUixI0BjGfbF+T2b127f1eiDveeKbFVELy19xdPE6aOmC4HqQ4PmR94", AESEncryptionUtility.DefaultKey, false);
            Console.WriteLine(tempData);

            var testdata = "YJEMXwbus9EsR3AC_2FRdFsTl_2FhApTedyJKYTOPKDC0mOLBobliTRSnZGG00v4mRXDGhv7Vea1BGCtN6fgt9bEM6C54cr9_2F3vsvYHXjQkHoSiUrdSL8yEnxA4xuBn1r0G5ULHE9PVYE1nuO4lf_2FU1SdQhWHzwlPjuGjvSfJbwp5bm_2FxqEZTI2bQigWuhtaLVJ8TeP7G6f2OFl0XOGn0JlduMuQg7dPLdTBhSEJyRh_2BVt6UPdRedenhpjKAOuQ6wNyq";
            tempData = encryptUtil.Decrypt(testdata, AESEncryptionUtility.DefaultKey, true);
            Console.WriteLine(tempData);
            Console.WriteLine("Test completed");


        }
    }
}
