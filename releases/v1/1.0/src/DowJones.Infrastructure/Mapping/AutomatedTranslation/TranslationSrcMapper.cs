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

namespace DowJones.Utilities.Mapping.AutomatedTranslation
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class TranslationSourceWhiteListMapper : IDisposable
    {
        private const string m_TranslationSourceWhiteList = "TranslationSourceWhiteList.csv";
        private const string m_TranslationSourceWhiteList_CacheName = "DowJones.Utilities.TranslationSourceWhiteList";
        private readonly object m_SyncObject = new object();

        private bool m_IsDisposed;

        private static readonly ILog m_Log = LogManager.GetLogger(typeof(TranslationSourceWhiteListMapper).FullName + ":" + MethodBase.GetCurrentMethod().Name);
        private string m_SrcCode;
        private DataTable m_TranslationSourceWhiteListData;
        private static Dictionary<string, DataTable> m_InternalTable;

        private static readonly Regex regexSpaceNormalizer = new Regex(
            @"\s+",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
            );


        public TranslationSourceWhiteListMapper()
        {
            m_IsDisposed = false;
            Init();
        }

        public string SourceCode
        {
            get { return m_SrcCode; }
            set { m_SrcCode = value; }
        }


        /// <summary>
        /// Determines whether [is source code allowed for translation] [the specified sr code].
        /// </summary>
        /// <param name="SrCode">The sr code.</param>
        /// <returns>
        /// 	<c>true</c> if [is source code allowed for translation] [the specified sr code]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSourceCodeAllowedForTranslation(string SrCode)
        {
            m_SrcCode = SrCode.ToUpper();
            return IsSourceCodeFoundInCSV();
        }



        /// <summary>
        /// Looks up source code.
        /// </summary>
        private bool IsSourceCodeFoundInCSV()
        {
            bool _bSrcCodeFound = false;
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                if (!string.IsNullOrEmpty(m_SrcCode))
                {
                    try
                    {
                        DataRow[] _rows = m_TranslationSourceWhiteListData.Select(string.Format(" SrcCode='{0}' ", m_SrcCode));

                        if (_rows != null && _rows.Length > 0)
                        {
                            _bSrcCodeFound = true;
                        }
                    }
                    catch (Exception Ex)
                    {
                        m_Log.Error("Error getting the sources for CSV File for Mapping:", Ex);
                        throw new Exception(Ex.Message);
                    }
                }
            }
            return _bSrcCodeFound;
        }

        private void Init()
        {
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                m_TranslationSourceWhiteListData = GetFromCache(m_TranslationSourceWhiteList_CacheName);

                if (m_TranslationSourceWhiteListData == null)
                {
                    lock (m_SyncObject)
                    {
                        if (m_TranslationSourceWhiteListData == null)
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

                DataRow objDataRow;





                m_TranslationSourceWhiteListData = new DataTable("TranslationSourceWhiteListData");
                m_TranslationSourceWhiteListData.Columns.Add("SrcCode", typeof(string));

                using (Stream stream = GetType().Assembly.GetManifestResourceStream(GetType(), m_TranslationSourceWhiteList))
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
                                    objDataRow = m_TranslationSourceWhiteListData.NewRow();
                                    objDataRow["SrcCode"] = csv[0]; 

                                    m_TranslationSourceWhiteListData.Rows.Add(objDataRow);
                                }
                                AddToCache(m_TranslationSourceWhiteList_CacheName, m_TranslationSourceWhiteListData);
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
                return (DataTable)HttpContext.Current.Cache.Get(name);
            }
            if (m_InternalTable == null)
            {
                m_InternalTable = new Dictionary<string, DataTable>(3);
                return null;
            }
            return m_InternalTable.ContainsKey(name) ? m_InternalTable[name] : null;
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
                    m_TranslationSourceWhiteListData = null;
                }
            }
            m_IsDisposed = true;
        }

        ~TranslationSourceWhiteListMapper()
        {
            Dispose(false);
        }
        #endregion
    }
}