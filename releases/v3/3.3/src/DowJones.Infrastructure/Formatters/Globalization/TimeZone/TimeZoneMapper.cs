using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using log4net;

namespace DowJones.Formatters.Globalization.TimeZone
{
    internal class TimeZoneMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TimeZoneMapper));
        private const string ResourceDataName = "TimeZoneData.xml";
        private static readonly object SyncObject = new object();

        private static readonly TimeZoneMapper m_Instance = new TimeZoneMapper();
        private static string resourceData = string.Empty;
        private readonly Dictionary<string, TimeZoneItem> _timeZoneCodeDictionary;
        private readonly Dictionary<string, string> _timeZoneStandardNameDictionary;
        private readonly List<TimeZoneItem> _timeZoneItemList;


        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZoneMapper"/> class.
        /// </summary>
        private TimeZoneMapper()
        {
            if (_timeZoneCodeDictionary != null)
                return;
            resourceData = string.Empty;
            var list = (TimeZoneList) Deserialize(GetEmbeddedXmlData(ResourceDataName), typeof(TimeZoneList));
            if (list == null || list.TimeZones == null || list.TimeZones.Length <= 0)
                return;
            _timeZoneCodeDictionary = new Dictionary<string, TimeZoneItem>(list.TimeZones.Length);
            _timeZoneStandardNameDictionary = new Dictionary<string, string>(list.TimeZones.Length);
            _timeZoneItemList = new List<TimeZoneItem>(list.TimeZones.Length);
            Array.Reverse(list.TimeZones);
            foreach (var zone in list.TimeZones)
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug(zone);
                }
                if (_timeZoneCodeDictionary.ContainsKey(zone.Code) || _timeZoneStandardNameDictionary.ContainsKey(zone.StandardName))
                    continue;

                Log.Info("Standard: " + zone.StandardName + ", Code:" + zone.Code);

                _timeZoneCodeDictionary.Add(zone.Code, zone);
                _timeZoneItemList.Add(zone);
                _timeZoneStandardNameDictionary.Add(zone.StandardName, zone.Code);
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static TimeZoneMapper Instance
        {
            get { return m_Instance; }
        }



        /// <summary>
        /// Gets the standard name by notation.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public TimeZoneItem GetTimeZoneItemByFactivaCode(string code)
        {
            return _timeZoneCodeDictionary.ContainsKey(code) ? _timeZoneCodeDictionary[code] : null;
        }


        internal List<TimeZoneItem> TimeZoneItemList
        {
            get { return _timeZoneItemList; }
        }


        /// <summary>
        /// Gets TimeZoneItem by standard name.
        /// </summary>
        /// <param name="standardName">StandardName.</param>
        /// <returns></returns>
        public TimeZoneItem GetTimeZoneItemByStandardName(string standardName)
        {
            return _timeZoneStandardNameDictionary.ContainsKey(standardName) ? _timeZoneCodeDictionary[_timeZoneStandardNameDictionary[standardName]] : null;
        }



        /// <summary>
        /// Deserializes the specified XML request.
        /// </summary>
        /// <param name="xmlRequest">The XML request.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        private static object Deserialize(string xmlRequest, Type objectType)
        {
            //System.Type objectType = null;
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRequest);
            if (xmlDoc.DocumentElement != null)
            {
                var xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);
                var xs = new XmlSerializer(objectType);
                return xs.Deserialize(xmlReader);
            }
            return null;
        }

        /// <summary>
        /// Gets the embedded support xml data file.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        private string GetEmbeddedXmlData(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceData) || string.IsNullOrEmpty(resourceData.Trim()))
            {
                lock (SyncObject)
                {
                    if (string.IsNullOrEmpty(resourceData) || string.IsNullOrEmpty(resourceData.Trim()))
                    {
                        using (var stream = GetType().Assembly.GetManifestResourceStream(GetType(), resourceName))
                        {
                            if (stream != null)
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    resourceData = reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }

            return resourceData;
        }
    }

    [Serializable]
    public class TimeZoneList
    {
        [XmlElement("TimeZone")]
        public TimeZoneItem[] TimeZones;
    }

    [Serializable]
    public class TimeZoneInfo
    {
        [XmlAttribute("st")]
        public string StandardTime { get; set; }

        [XmlAttribute("dt")]
        public string DaylightSavingsTime { get; set; }
    }

    [Serializable]
    public class TimeZoneItem
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("code")]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("StandardName")]
        public string StandardName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("AltName")]
        public string AlternateName { get; set; }

        [XmlElement("TimeZoneInfo")]
        public TimeZoneInfo TimeZoneInfo { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Code, StandardName);
        }
    }
}