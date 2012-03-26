using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using DowJones.Utilities.Loggers;
using DowJones.Utilities.Readers.CSV;
using log4net;

namespace DowJones.Utilities.Mapping.IpAddress
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class IpAddressMapper : IDisposable
    {
        private const string m_IpToCCData_FileName = "Ip-To-GrpIDMap.csv";
        private const string m_SrcGrp2SrcsData_FileName = "GrpID-SrcMap.csv";
        private const string m_IpToCCData_CacheName = "DowJones.Utilities.IpToCCData";
        private const string m_SrcGrp2SrcsData_CacheName = "DowJones.Utilities.SrcGrp2SrcsData";
        private readonly object m_SyncObject = new object();

        private bool m_IsDisposed;

        private static readonly ILog m_Log = LogManager.GetLogger(typeof (IpAddressMapper).FullName + ":" + MethodBase.GetCurrentMethod().Name);
        private CountryInfo m_CountryInfo;
        private string m_Ip;
        private DataTable m_IpToCCData;
        private int m_RegionId = -1;
        private DataRow[] m_RegionRows;
        private DataTable m_SrcGrp2SrcsData;
        private static Dictionary<string, DataTable> m_InternalTable;

        private static readonly Regex regexSpaceNormalizer = new Regex(
            @"\s+",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
            );


        public IpAddressMapper()
        {
            m_IsDisposed = false;
            Init();
        }

        public string IPAddress
        {
            get { return m_Ip; }
            set { m_Ip = value; }
        }

        public DataRow[] RegionList
        {
            get
            {
                if (m_RegionRows != null && m_RegionRows.Length > 0)
                    return m_RegionRows;
                m_RegionRows = m_SrcGrp2SrcsData.Select("", "GrpRegionName");
                return m_RegionRows;
            }
        }


        /// <summary>
        /// get the iso country code..IPaddress must be set to retrieve the country code
        /// </summary>
        /// <returns>ISO countrycode</returns>
        public string GetCountryCode()
        {
            string _isocc = string.Empty;
            LookupCountry();
            if (m_CountryInfo != null)
            {
                _isocc = m_CountryInfo.CountryISOCode;
            }
            return _isocc;
        }

        /// <summary>
        /// get the iso country code.
        /// </summary>
        /// <param name="IpAddress">IPAddress to return the countrycode</param>
        /// <returns>ISO countrycode</returns>
        public string GetCountryCode(string IpAddress)
        {
            string _isocc = string.Empty;
            m_Ip = IpAddress;
            LookupCountry();
            if (m_CountryInfo != null)
            {
                _isocc = m_CountryInfo.CountryISOCode;
            }
            return _isocc;
        }


        /// <summary>
        /// returns the countryinfo object 
        /// </summary>
        /// <returns></returns>
        public CountryInfo GetCountryInfo()
        {
            LookupCountry();
            return m_CountryInfo;
        }

        /// <summary>
        /// Will create the CountryInfo based of the IPAddress and returns that.
        /// </summary>
        /// <param name="IpAddress">IPaddress to return the country code object</param>
        /// <returns>CountryInfo</returns>
        public CountryInfo GetCountryInfo(string IpAddress)
        {
            m_Ip = IpAddress;
            LookupCountry();
            return m_CountryInfo;
        }

        /// <summary>
        /// Will create the CountryInfo based of the RegionID and returns that.
        /// </summary>
        /// <param name="RegionID">RegionID to return the country code object</param>
        /// <returns>CountryInfo</returns>
        public CountryInfo GetCountryInfoByRegionID(int RegionID)
        {
            m_RegionId = RegionID;
            LookupCountry();
            return m_CountryInfo;
        }


        private void Init()
        {
            using(new TransactionLogger(m_Log,MethodBase.GetCurrentMethod()))
            {
                m_IpToCCData = GetFromCache(m_IpToCCData_CacheName);
                m_SrcGrp2SrcsData = GetFromCache(m_SrcGrp2SrcsData_CacheName);

                if (m_IpToCCData == null || m_SrcGrp2SrcsData == null)
                {
                    lock (m_SyncObject)
                    {
                        if (m_IpToCCData == null || m_SrcGrp2SrcsData == null)
                        {
                            LoadTables();
                        }
                    }
                }
            }
        }


        private void LoadTables()
        {
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_IpToCCData = new DataTable("IpToCCData");
                
                m_IpToCCData.Columns.Add("StartRange", typeof (long));
                m_IpToCCData.Columns.Add("EndRange", typeof (long));
                m_IpToCCData.Columns.Add("ISOCode", typeof (string));
                m_IpToCCData.Columns.Add("SrcGrpID", typeof (int));

                

                DataRow objDataRow;


                using (Stream stream = GetType().Assembly.GetManifestResourceStream(GetType(), m_IpToCCData_FileName))
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            try
                            {
                                CsvReader csv = new CsvReader(reader, true);
                                while (csv.ReadNextRecord())
                                {
                                    objDataRow = m_IpToCCData.NewRow();
                                    objDataRow["StartRange"] = csv[0];
                                    objDataRow["EndRange"] = csv[1];
                                    objDataRow["ISOCode"] = csv[2];
                                    objDataRow["SrcGrpID"] = csv[3];

                                    m_IpToCCData.Rows.Add(objDataRow);
                                }
                                AddToCache(m_IpToCCData_CacheName, m_IpToCCData);
                            }
                            catch (Exception Ex)
                            {
                                m_Log.Error("Error creating table for mapping ip", Ex);
                                throw;
                            }
                        }
                    }
                }


                m_SrcGrp2SrcsData = new DataTable("SrcGrp2SrcsData");
                m_SrcGrp2SrcsData.Columns.Add("SrcGrpID", typeof(int));
                m_SrcGrp2SrcsData.Columns.Add("SrcCodes", typeof(string));
                m_SrcGrp2SrcsData.Columns.Add("GrpRegionName", typeof(string));

                using (Stream stream = GetType().Assembly.GetManifestResourceStream(GetType(), m_SrcGrp2SrcsData_FileName))
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            try
                            {
                                CsvReader csv = new CsvReader(reader, true);
                                while (csv.ReadNextRecord())
                                {
                                    objDataRow = m_SrcGrp2SrcsData.NewRow();
                                    objDataRow["SrcGrpID"] = csv[0];
                                    objDataRow["SrcCodes"] = csv[1];
                                    objDataRow["GrpRegionName"] = csv[2];

                                    m_SrcGrp2SrcsData.Rows.Add(objDataRow);
                                }
                                AddToCache(m_SrcGrp2SrcsData_CacheName, m_SrcGrp2SrcsData);
                            }
                            catch (Exception Ex)
                            {
                                m_Log.Error("Error creating table for getting srcs", Ex);
                                throw new Exception(Ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private static void AddToCache(string name, DataTable data)
        {
            //add the table to the cache
            if (HttpContext.Current != null && m_InternalTable == null)
            {
                HttpContext.Current.Cache.Add(name, data, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, null);   
            }
            else
            {
                if (m_InternalTable == null)
                {
                    m_InternalTable = new Dictionary<string, DataTable>(3);
                }
                if (!m_InternalTable.ContainsKey(name))
                {
                    m_InternalTable[name] = data;
                }
            }
        }

        private static DataTable GetFromCache(string name)
        {
            if (HttpContext.Current != null && m_InternalTable == null)
            {
                return (DataTable) HttpContext.Current.Cache.Get(name);
            }
            if (m_InternalTable == null)
            {
                m_InternalTable = new Dictionary<string, DataTable>(3);
                return null;
            }
            return m_InternalTable.ContainsKey(name) ? m_InternalTable[name] : null;
        }

        /// <summary>
        /// Get the IPAddress do the checks and the make a country info object
        /// </summary>
        private void LookupCountry()
        {
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                int srcGrpID = 7; // defaulted to US Code
                m_CountryInfo = new CountryInfo();
                m_CountryInfo.CountryISOCode = "US"; // Default ISO code
                if (!string.IsNullOrEmpty(m_Ip))
                {
                    try
                    {
                        string[] _splittedIP = m_Ip.Split('.');

                        double _tempIPPart1 = Convert.ToDouble(_splittedIP[0]);
                        double _tempIPPart2 = Convert.ToDouble(_splittedIP[1]);
                        double _tempIPPart3 = Convert.ToDouble(_splittedIP[2]);
                        double _tempIPPart4 = Convert.ToDouble(_splittedIP[3]);

                        double _IPMapNumber = 16777216 * _tempIPPart1 + 65536 * _tempIPPart2 + 256 * _tempIPPart3 + _tempIPPart4;

                        DataRow[] _rows = m_IpToCCData.Select(string.Format("{0} > StartRange and {0} < EndRange", _IPMapNumber));

                        if (_rows != null && _rows.Length > 0)
                        {
                            //get the first match.. irrespective...
                            m_CountryInfo.CountryISOCode = (string)_rows[0][2];
                            srcGrpID = (int)_rows[0][3];
                        }
                    }
                    catch (Exception Ex)
                    {
                        m_Log.Error("Error getting the sources for IP:", Ex);
                        throw new Exception(Ex.Message);
                    }
                }
                if (srcGrpID <= 0 || srcGrpID >= 15)
                    return;
                DataRow[] _srcrows = m_SrcGrp2SrcsData.Select(string.Format("SrcGrpID = {0}", Convert.ToInt32(srcGrpID)));
                m_CountryInfo.RegionalSources = regexSpaceNormalizer.Replace((string)_srcrows[0]["SrcCodes"], "").Replace("\"", "").Trim();
                m_CountryInfo.RegionId = srcGrpID;
            }
            
        }

        /// <summary>
        /// Get the IPAddress do the checks and the make a country info object
        /// </summary>
        protected void LookupCountryInfoByRegionID()
        {
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                int srcGrpID = 7; // defaulted to US Code
                m_CountryInfo = new CountryInfo();
                m_CountryInfo.CountryISOCode = "US"; // Default ISO code
                if (m_RegionId != -1)
                {
                    try
                    {
                        DataRow[] _rows = m_IpToCCData.Select(string.Format(" SrcGrpID = {0}", m_RegionId));

                        if (_rows != null && _rows.Length > 0)
                        {
                            //get the first match.. irrespective...
                            m_CountryInfo.CountryISOCode = (string) _rows[0][2];
                            srcGrpID = (int) _rows[0][3];
                        }
                    }
                    catch (Exception Ex)
                    {
                        m_Log.Error("Error getting the sources for IP:", Ex);
                        throw new Exception(Ex.Message);
                    }
                }
                if (srcGrpID > 0 && srcGrpID < 14)
                {
                    DataRow[] _srcrows = m_SrcGrp2SrcsData.Select(string.Format("SrcGrpID = {0}", Convert.ToInt32(srcGrpID)));
                    m_CountryInfo.RegionalSources = regexSpaceNormalizer.Replace((string) _srcrows[0]["SrcCodes"], "").Trim();
                    m_CountryInfo.RegionName = regexSpaceNormalizer.Replace((string) _srcrows[0]["GrpRegionName"], "").Trim();
                }
            }
        }

        #region IDisposable Members

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_IsDisposed)
            {
                if (disposing)
                {
                    m_IpToCCData = null;
                    m_SrcGrp2SrcsData = null;
                }
            }
            m_IsDisposed = true;
        }

        ~IpAddressMapper()
        {
            Dispose(false);
        }
        #endregion
    }
}