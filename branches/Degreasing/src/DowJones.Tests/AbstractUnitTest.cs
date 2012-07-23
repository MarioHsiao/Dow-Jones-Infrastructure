// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractUnitTest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DowJones.Globalization;
using DowJones.Preferences;
using DowJones.Session;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace DowJones
{
    public abstract class AbstractUnitTest
    {
        private const string BaseCongigurationFile = "log4net.Config";
        private static readonly ILog Log = LogManager.GetLogger(typeof(AbstractUnitTest));
        private readonly Level level = Level.All;

        protected IControlData ControlData = ControlDataManager.GetLightWeightUserControlData("dacostad", "brian", "16");
        protected IPreferences Preferences = new Preferences.Preferences
                                                 {
                                                     InterfaceLanguage = "en",
                                                     ContentLanguages = new ContentLanguageCollection()
                                                 };

        protected enum GatewayUserType
        {
            Default, // Default usually is Regular user.
            AccountAdmin,
            CustomerService,
            CVDEnabled,
            Premium,
            Regular,
            TrackFolder
        }
        
        public Level LoggerLevel
        {
            get { return level; }
        }

        private static string GetLogFileLocation()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Added for unit testing in a class library.
            const string bin = @"\bin";
            var index = baseDirectory.IndexOf(bin);
            if (index > 0)
            {
                int diff = baseDirectory.Length - index;
                baseDirectory = baseDirectory.Substring(0, baseDirectory.Length - diff);
            }
            return Path.Combine(baseDirectory, BaseCongigurationFile);
        }

        private void InstantiateLog4Net()
        {
            var fileInfo = new FileInfo(GetLogFileLocation());
            XmlConfigurator.Configure(fileInfo);
            var h = ((Hierarchy)LogManager.GetRepository());
            h.Root.Level = level;
            if (Log.IsDebugEnabled)
            {
                Log.Debug("Finished Instantiating Log4Net");
            }
        }


        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        protected static string Serialize(object obj)
        {
            // Serialization
            var serializer = new XmlSerializer(obj.GetType());
            using (var swBlob = new StringWriter())
            {
            
                //create an xmlwriter and then write nothing to this to fake and remove xml decl
                var xw = new XmlTextWriter(swBlob) {Formatting = Formatting.Indented};

                // serialize the request data.
                serializer.Serialize(xw, obj);
                return swBlob.ToString();
            }
        }

        /// <summary>
        /// Serializes the with no XML declaration.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        protected static string SerializeWithNoXmlDeclaration(object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            using (var swBlob = new StringWriter())
            {
                // create an xmlwriter and then write nothing to this to fake and remove xml decl
                var xw = new XmlTextWriter(swBlob) {Formatting = Formatting.None};
                xw.WriteRaw("");

                // serialize the request data.
                serializer.Serialize(xw, obj);
                return swBlob.ToString();
            }
        }
         
        /// <summary>
        /// De-serializes the specified XML request.
        /// </summary>
        /// <param name="xmlRequest">The XML request.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        protected static object Deserialize(string xmlRequest, Type objectType)
        {
            //System.Type objectType = null;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequest);
            if (xmlDoc.DocumentElement != null)
            {
                var xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);
            
                var xs = new XmlSerializer(objectType);
                var obj = xs.Deserialize(xmlReader);
                return obj;
            }
            return null;
        }

        protected static object DeserializeXmlFile(string fileName, Type objectType)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {

                XmlReader reader = new XmlTextReader(fs);

                var xs = new XmlSerializer(objectType);
                return xs.Deserialize(reader);
            }
        }


        /// <summary>
        /// De-serializes the specified XML request.
        /// </summary>
        /// <param name="xmlRequest">The XML request.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xmlRequest)
        {
            //System.Type objectType = null;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequest);
            if (xmlDoc.DocumentElement == null)
            {
            }
            else
            {
                var xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);
                var xs = new XmlSerializer(typeof (T));
                return (T) xs.Deserialize(xmlReader);
            }
            return default(T);
        }

        /// <summary>
        /// De-serializes the specified XML request.
        /// </summary>
        /// <returns></returns>
        public static T Deserialize<T>(XmlReader reader)
        {
            var xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(reader);
        }

        public static FileStream GetFileStream(string path, string file)
        {
            Directory.CreateDirectory(path);
            var stream = File.Create (string.Concat(path, "\\", file));            
            return stream;
        }

        protected AbstractUnitTest(Level level)
        {
            this.level = level;
            InstantiateLog4Net();
        }

        protected AbstractUnitTest(): this(Level.All)
        {
            
        }

        public ArrayList GetArrayListFromDelimitedString(string strToSplit, char[] strDelimiter)
        {

            if (string.IsNullOrEmpty(strToSplit))
            {
                throw new Exception("strToSplit Is Null Or Empty.");
            }

            var tokens = new ArrayList();

            foreach (string token in strToSplit.Split(strDelimiter[0]))
            {
                tokens.Add(token.Trim());
            }

            return tokens;

        }

    }
}
