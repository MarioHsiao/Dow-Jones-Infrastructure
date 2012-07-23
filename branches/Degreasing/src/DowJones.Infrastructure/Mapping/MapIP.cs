using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using log4net;

namespace EMG.Utility.Mapping
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class MapIP
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MapIP).FullName + ":" + MethodInfo.GetCurrentMethod().Name);
        private string m_Ip;
        private int m_RegionId = -1;
        private CountryInfo m_CountryInfo;
        private DataTable m_IpToCCData = null;
        private DataTable m_SrcGrp2SrcsData = null;
        private DataRow[] m_RegionRows = null;
        
        // private const
        private static readonly Regex regexSpaceNormalizer = new Regex(
            @"\s+",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
            );

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
                else
                {
                    //_regionrows = SrcGrp2SrcsData.Select(string.Format("SrcGrpID,GrpRegionName"));
                    m_RegionRows = m_SrcGrp2SrcsData.Select("","GrpRegionName");
                    return m_RegionRows;
                }
            }

        }


        public MapIP()
        {
            //
            // TODO: Add constructor logic here
            //
            Init();
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
            CountryInfo _cInfo = null;
            m_Ip = IpAddress;
            LookupCountry();
            return _cInfo;
        }

        /// <summary>
        /// Will create the CountryInfo based of the RegionID and returns that.
        /// </summary>
        /// <param name="RegionID">RegionID to return the country code object</param>
        /// <returns>CountryInfo</returns>
        public CountryInfo GetCountryInfoByRegionID(int RegionID)
        {
            CountryInfo _cInfo = null;
            m_RegionId = RegionID;
            LookupCountry();
            return _cInfo;
        }



        private void Init()
        {
            if (HttpContext.Current != null)
            {
                m_IpToCCData = (DataTable)HttpContext.Current.Cache.Get("IpToCCData");
                m_SrcGrp2SrcsData = (DataTable)HttpContext.Current.Cache.Get("SrcGrp2SrcsData");
            }
            if (m_IpToCCData == null || m_SrcGrp2SrcsData == null)
            {
                LoadTables();
            }
        }

        private void LoadTables()
        {
            //string dataFilePath  = HttpContext.Current.Server.MapPath(Util.MapIPSettings("Ip2CountryFileInfo", "FilePath"));
            //string dataFilePath = ConfigurationManager.GetIpToGrpMapperFile();
            string Ip2CountryFileInfo = ""; //ConfigurationManager.GetIpToGrpMapperFile();
            string Grp2SrcMap = ""; // ConfigurationManager.GetGrpToSrcsMapperFile();

            m_IpToCCData = new DataTable("IpToCCData");
            m_SrcGrp2SrcsData = new DataTable("SrcGrp2SrcsData");
            m_IpToCCData.Columns.Add("StartRange", typeof(long));
            m_IpToCCData.Columns.Add("EndRange", typeof(long));
            m_IpToCCData.Columns.Add("ISOCode", typeof(string));
            m_IpToCCData.Columns.Add("SrcGrpID", typeof(int));

            m_SrcGrp2SrcsData.Columns.Add("SrcGrpID", typeof(int));
            m_SrcGrp2SrcsData.Columns.Add("SrcCodes", typeof(string));
            m_SrcGrp2SrcsData.Columns.Add("GrpRegionName", typeof(string));

            StreamReader reader;
            reader = new StreamReader(HttpContext.Current.Request.MapPath(Ip2CountryFileInfo));
            string data;
            string[] dataArray;
            DataRow objDataRow;
            try
            {
                //read the file and load into the tables.
                //drop the first line and then move on as this is the header//
                bool isFirstIP = true;
                while (0 < reader.Peek())
                {
                    // get a single line of text
                    data = reader.ReadLine();
                    // call routine to place delimited
                    // text into an array
                    dataArray = Csv.LineToArray(data);
                    if (!isFirstIP)
                    {
                        if (dataArray.Length >= 2)
                        {
                            objDataRow = m_IpToCCData.NewRow();
                            objDataRow["StartRange"] = dataArray[0];
                            objDataRow["EndRange"] = dataArray[1];
                            objDataRow["ISOCode"] = dataArray[2];
                            objDataRow["SrcGrpID"] = dataArray[3];

                            m_IpToCCData.Rows.Add(objDataRow);
                        }

                    }
                    else
                    {
                        isFirstIP = false;
                    }

                }
                //add the table to the cache
                HttpContext.Current.Cache.Add("IpToCCData", m_IpToCCData, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Low, null);

            }
            catch (Exception Ex)
            {
                _log.Error("Error creating table for mapping ip", Ex);
                throw;
            }

            reader = new StreamReader(HttpContext.Current.Request.MapPath(Grp2SrcMap));

            try
            {
                //drop the first line and then move on as this is the header
                bool isFirstSrcList = true;
                //read the file and load into the tables.
                while (0 < reader.Peek())
                {
                    // get a single line of text
                    data = reader.ReadLine();
                    // call routine to place delimited
                    // text into an array
                    dataArray = Csv.LineToArray(data);
                    if (!isFirstSrcList)
                    {
                        if (dataArray.Length >= 2)
                        {
                            objDataRow = m_SrcGrp2SrcsData.NewRow();
                            objDataRow["SrcGrpID"] = dataArray[0];
                            objDataRow["SrcCodes"] = dataArray[1];
                            objDataRow["GrpRegionName"] = dataArray[2];

                            m_SrcGrp2SrcsData.Rows.Add(objDataRow);
                        }
                    }
                    else
                    {
                        isFirstSrcList = false;
                    }

                }
                //add the table to the cache
                HttpContext.Current.Cache.Add("SrcGrp2SrcsData", m_SrcGrp2SrcsData, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Low, null);
            }
            catch (Exception Ex)
            {
                _log.Error("Error creating table for getting srcs", Ex);
                throw new Exception(Ex.Message);
            }
            finally
            {
                reader.Close();
            }
            
        }
        /// <summary>
        /// Get the IPAddress do the checks and the make a country info object
        /// </summary>
        private void LookupCountry()
        {
            int srcGrpID = 7; // defaulted to US Code
            m_CountryInfo = new CountryInfo();
            m_CountryInfo.CountryISOCode = "US"; // Default ISO code
            if (m_Ip != null && m_Ip.Length > 0)
            {
                try
                {
                    string[] _splittedIP = m_Ip.Split('.');
                    double _tempIPPart1;
                    double _tempIPPart2;
                    double _tempIPPart3;
                    double _tempIPPart4;
                    double _IPMapNumber;

                    _tempIPPart1 = Convert.ToDouble(_splittedIP[0]);
                    _tempIPPart2 = Convert.ToDouble(_splittedIP[1]);
                    _tempIPPart3 = Convert.ToDouble(_splittedIP[2]);
                    _tempIPPart4 = Convert.ToDouble(_splittedIP[3]);

                    _IPMapNumber = 16777216 * _tempIPPart1 + 65536 * _tempIPPart2 + 256 * _tempIPPart3 + _tempIPPart4;

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
                    _log.Error("Error getting the sources for IP:", Ex);
                    throw new Exception(Ex.Message);
                }
            }
            if (srcGrpID > 0 && srcGrpID < 15)
            {
                DataRow[] _srcrows = m_SrcGrp2SrcsData.Select(string.Format("SrcGrpID = {0}", Convert.ToInt32(srcGrpID)));
                m_CountryInfo.RegionalSources = regexSpaceNormalizer.Replace((string)_srcrows[0]["SrcCodes"], "").Replace("\"", "").Trim();
                m_CountryInfo.RegionID = srcGrpID;
            }
        }

        /// <summary>
        /// Get the IPAddress do the checks and the make a country info object
        /// </summary>
        protected void LookupCountryInfoByRegionID()
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
                        m_CountryInfo.CountryISOCode = (string)_rows[0][2];
                        srcGrpID = (int)_rows[0][3];
                    }


                }
                catch (Exception Ex)
                {
                    _log.Error("Error getting the sources for IP:", Ex);
                    throw new Exception(Ex.Message);
                }
            }
            if (srcGrpID > 0 && srcGrpID < 14)
            {
                DataRow[] _srcrows = m_SrcGrp2SrcsData.Select(string.Format("SrcGrpID = {0}", Convert.ToInt32(srcGrpID)));
                m_CountryInfo.RegionalSources = regexSpaceNormalizer.Replace((string)_srcrows[0]["SrcCodes"], "").Trim();
                m_CountryInfo.RegionName = regexSpaceNormalizer.Replace((string)_srcrows[0]["GrpRegionName"], "").Trim();

            }
        }
    }


    public class CountryInfo
    {
        private string _countryISOCode;
        private string regionSrcs;
        private string regionName;
        private int regionID;

        
        public string CountryISOCode
        {
            get { return _countryISOCode; }
            set { _countryISOCode = value; }
        }

        public string RegionalSources
        {
            get { return regionSrcs; }
            set { regionSrcs = value; }
        }
        public string RegionName
        {
            get { return regionName; }
            set { regionName = value; }
        }
        public int RegionID
        {
            get { return regionID; }
            set { regionID = value; }
        }

    }
    /// <summary>
    /// Summary description for Csv.
    /// </summary>
    public class Csv
    {
        private static readonly string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";
        private static Regex r = new Regex(pattern);

        public static string[] LineToArray(string line)
        {
            return r.Split(line);
        }
    }

}