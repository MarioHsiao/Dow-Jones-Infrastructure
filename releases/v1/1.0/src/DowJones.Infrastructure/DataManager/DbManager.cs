using System;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using log4net;

namespace DowJones.Utilities.DataManager
{
    public class DbManager : IDisposable
    {
        #region Protected members

        protected IDbCommand _command;
        protected int _commandTimeout = -1;
        protected bool _connected;

        protected IDbConnection _connection;
        protected int _connectionRetryCount = -1;
        protected TimeSpan _connectionRetryTimeout = TimeSpan.MinValue;
        protected string _connectionString;
        protected string _connectionStringSettingsName;

        protected bool _disposed;
        protected DbProviderType _provider = DbProviderType.SqlClient;

        protected string _providerAssembly;
        protected string _providerCommandBuilderClass;
        protected string _providerConnectorClass;
        protected IDbTransaction _transaction;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="DbManager"/> class.
        /// </summary>
        public DbManager()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DbManager"/> class with a connection string setting name parameter.
        /// </summary>
        /// <param name="connectionStringSettingsName">Connection string settings name.</param>
        public DbManager(string connectionStringSettingsName)
        {
            ConnectionStringSettingsName = connectionStringSettingsName;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DbManager"/> class with a database provider type and 
        /// a connection string setting name parameter.
        /// </summary>
        /// <param name="provider">Database provider type.</param>
        /// <param name="connectionStringSettingsName">Connection string settings name.</param>
        public DbManager(DbProviderType provider, string connectionStringSettingsName)
        {
            _provider = provider;
            ConnectionStringSettingsName = connectionStringSettingsName;
        }

        #endregion

        #region Connection management methods

        /// <summary>
        /// Gets <c>true</c> if the database connection is opened or <c>false</c> if the database 
        /// connection is disconnected.  
        /// </summary>
        public bool IsConnected
        {
            get { return _connected; }
        }

        /// <summary>
        /// Ensures that a connection is established before proceeding.
        /// </summary>
        /// <returns></returns>
        protected bool EnsureConnection()
        {
            return _connected || Connect();
        }

        /// <summary>
        /// Opens a connection to the database specified by <see cref="ConnectionStringSettingsName"/> property or, otherwise, by the <see cref="ConnectionString"/> property.
        /// </summary>
        /// <returns><c>true</c> if connection is established, otherwise <c>false</c>.</returns>
        /// <see cref="InvalidOperationException">Thrown when a non-existent <see cref="ConnectionStringSettingsName"/> or an empty <see cref="ConnectionString"/> is specified.</see>
        public bool Connect()
        {
            // check for valid connection string
            if (string.IsNullOrEmpty(ConnectionString))
            {
                if (!string.IsNullOrEmpty(ConnectionStringSettingsName))
                {
                    if (ConfigurationManager.ConnectionStrings[ConnectionStringSettingsName] == null)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                "Unknown connection string settings name '{0}' is specified.  Please update your application configuration file.",
                                ConnectionStringSettingsName));
                    }

                    ConnectionString =
                        ConfigurationManager.ConnectionStrings[ConnectionStringSettingsName].ConnectionString;
                }

                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new InvalidOperationException("Invalid database connection string.");
                }
            }

            // disconnect if already connected
            Disconnect();

            // get connection object
            _connection = GetConnection();
            _connection.ConnectionString = ConnectionString;

            // Implement connection retries
            for (var i = 0; i <= ConnectionRetryCount; i++)
            {
                try
                {
                    _connection.Open();

                    if (_connection.State == ConnectionState.Open)
                    {
                        _connected = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (i == ConnectionRetryCount)
                        throw new DbManangerException(DbManagerErrors.SQL_CONNECTION_ERROR, ex);

                    // Wait and try again
                    Thread.Sleep(ConnectionRetryTimeout);
                }
            }

            // Get command object
            _command = _connection.CreateCommand();
            _command.CommandTimeout = CommandTimeout;

            return _connected;
        }

        /// <summary>
        /// Closes the database connection, if opened, and rolls back a transaction from a pending state, if a transaction was started.
        /// </summary>
        public void Disconnect()
        {
            // Disconnect can be called from Dispose and should guarantee no errors
            if (!_connected)
            {
                return;
            }

            if (_transaction != null)
            {
                RollbackTransaction(false);
            }

            if (_command != null)
            {
                _command.Dispose();
                _command = null;
            }

            if (_connection != null)
            {
                try
                {
                    _connection.Close();
                }
                catch
                {
                }

                _connection.Dispose();
                _connection = null;
            }

            _connected = false;
        }

        #endregion

        #region Transaction management methods

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        public void BeginTransaction()
        {
            EnsureConnection();

            _transaction = _connection.BeginTransaction();
            _command.Transaction = _transaction;

            return;
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="BeginTransaction"/> was not called previously to start the transaction.</exception>
        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw (new InvalidOperationException(
                    "BeginTransaction must be called before commit or rollback. No open transactions found"));
            }

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        /// <remarks>
        /// <para>This method will throw an exception if rollback is unsuccessful for any reason.  </para>
        /// <para>To prevent the exception from being thrown during rollback, use <see cref="RollbackTransaction(bool)"/></para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="BeginTransaction"/> was not called previously to start the transaction.</exception>
        public void RollbackTransaction()
        {
            RollbackTransaction(true);
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        /// <param name="throwError">Indicates whether any exceptions should be thrown from this method if rollback operation is unsuccessful.</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="BeginTransaction"/> was not called previously to start the transaction.</exception>
        public void RollbackTransaction(bool throwError)
        {
            if (_transaction == null)
            {
                if (throwError)
                {
                    throw (new InvalidOperationException(
                        "BeginTransaction must be called before commit or rollback. No open transactions found"));
                }
            }

            try
            {
                if (_transaction != null) _transaction.Rollback();
            }
            catch
            {
                if (throwError) throw;
            }
            finally
            {
                if (_transaction != null) _transaction.Dispose();
                _transaction = null;
            }
        }

        #endregion

        #region ADO.NET Command and DataAdapter wrapper methods

        #region ExecuteReader

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.IDbCommand.Connection"/>
        /// and builds an <see cref="System.Data.IDataReader"/>.  
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <returns>An <see cref="System.Data.IDataReader"/> object.</returns>
        /// <remarks>
        /// <para>Executed <paramref name="commandText"/> is always interpreted as a text command.  
        /// To execute a stored procedure or a table direct command, 
        /// use <see cref="ExecuteReader(string,CommandType)"/></para>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public IDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, CommandType.Text);
        }

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.IDbCommand.Connection"/>
        /// and builds an <see cref="System.Data.IDataReader"/>.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <returns>An <see cref="System.Data.IDataReader"/> object.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public IDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return ExecuteReader(commandText, commandType, null);
        }

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.IDbCommand.Connection"/>
        /// and builds an <see cref="System.Data.IDataReader"/>.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <param name="dataParameters">Array of database parameters to use with command.</param>
        /// <returns>An <see cref="System.Data.IDataReader"/> object.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public IDataReader ExecuteReader(string commandText, CommandType commandType, IDataParameter[] dataParameters)
        {
            EnsureConnection();

            _command.CommandText = commandText;
            _command.CommandType = commandType;

            AddParameters(dataParameters);

            return _command.ExecuteReader();
        }

        #endregion

        #region ExecuteXmlReader

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.SqlClient.SqlCommand.Connection"/>
        /// and builds an <see cref="System.Xml.XmlReader"/> object.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <returns>An <see cref="System.Xml.XmlReader"/> object.</returns>
        /// <remarks>
        /// <para>Executed <paramref name="commandText"/> is always interpreted as a text command.  
        /// To execute a stored procedure or a table direct command, 
        /// use <see cref="ExecuteXmlReader(string,CommandType)"/></para>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public XmlReader ExecuteXmlReader(string commandText)
        {
            return ExecuteXmlReader(commandText, CommandType.Text);
        }

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.SqlClient.SqlCommand.Connection"/>
        /// and builds an <see cref="System.Xml.XmlReader"/> object.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <returns>An <see cref="System.Xml.XmlReader"/> object.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public XmlReader ExecuteXmlReader(string commandText, CommandType commandType)
        {
            return ExecuteXmlReader(commandText, commandType, null);
        }

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.SqlClient.SqlCommand.Connection"/>
        /// and builds an <see cref="System.Xml.XmlReader"/> object.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <param name="dataParameters">Array of database parameters to use with command.</param>
        /// <returns>An <see cref="System.Xml.XmlReader"/> object.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public XmlReader ExecuteXmlReader(string commandText, CommandType commandType, IDataParameter[] dataParameters)
        {
            EnsureConnection();

            _command.CommandText = commandText;
            _command.CommandType = commandType;

            AddParameters(dataParameters);

            if (Provider == DbProviderType.SqlClient)
            {
                return ((SqlCommand) _command).ExecuteXmlReader();
            }
            var xml = (string) _command.ExecuteScalar();
            return new XmlTextReader(new StringReader(xml));
        }

        #endregion

        #region GetDataSet

        /// <summary>
        /// Adds rows in the <see cref="System.Data.DataSet"/> to match those in the data
        /// source using the <paramref name="commandText"/> selection query, and creates 
        /// a <see cref="System.Data.DataTable"/> named "Table".
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <returns>A filled <see cref="System.Data.DataSet"/> object.</returns>
        /// <remarks>
        /// <para>Executed <paramref name="commandText"/> is always interpreted as a text command.  
        /// To execute a stored procedure or a table direct command, 
        /// use <see cref="GetDataSet(string,CommandType)"/></para>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public DataSet GetDataSet(string commandText)
        {
            var ds = new DataSet();
            return GetDataSet(commandText, CommandType.Text, null, ds);
        }

        /// <summary>
        /// Adds rows in the <see cref="System.Data.DataSet"/> to match those in the data
        /// source using the <paramref name="commandText"/> selection query, and creates 
        /// a <see cref="System.Data.DataTable"/> named "Table".
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <returns>A filled <see cref="System.Data.DataSet"/> object.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public DataSet GetDataSet(string commandText, CommandType commandType)
        {
            var ds = new DataSet();
            return GetDataSet(commandText, commandType, null, ds);
        }

        /// <summary>
        /// Adds rows in the <see cref="System.Data.DataSet"/> to match those in the data
        /// source using the <paramref name="commandText"/> selection query, and creates
        /// a <see cref="System.Data.DataTable"/> named "Table".
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <param name="dataParameters">Array of database parameters to use with command.</param>
        /// <returns>
        /// A filled <see cref="System.Data.DataSet"/> object.
        /// </returns>
        /// <remarks>
        /// Will automatically connect, if connection was not already established.
        /// </remarks>
        public DataSet GetDataSet(string commandText, CommandType commandType, IDataParameter[] dataParameters)
        {
            var ds = new DataSet();
            return GetDataSet(commandText, commandType, dataParameters, ds);
        }

        /// <summary>
        /// Adds rows in the <see cref="System.Data.DataSet"/> to match those in the data
        /// source using the <paramref name="commandText"/> selection query, and creates 
        /// a <see cref="System.Data.DataTable"/> named "Table".
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <param name="dataParameters">Array of database parameters to use with command.</param>
        /// <param name="ds"><see cref="System.Data.DataSet"/> object to be filled.</param>
        /// <returns>A filled <see cref="System.Data.DataSet"/> object.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public DataSet GetDataSet(string commandText, CommandType commandType, IDataParameter[] dataParameters,
                                  DataSet ds)
        {
            EnsureConnection();

            _command.CommandType = commandType;
            _command.CommandText = commandText;

            AddParameters(dataParameters);

            var adapter = GetDataAdapter(commandText);
            adapter.Fill(ds);

            return ds;
        }

        public DataSet GetDataSet(string commandText, CommandType commandType, IDataParameter[] dataParameters,
                                  DataSet ds, string[] tableNames)
        {
            if (tableNames == null || tableNames.Length == 0)
            {
                throw new ArgumentNullException("tableNames");
            }

            for (var i = 0; i < tableNames.Length; i++)
            {
                if (string.IsNullOrEmpty(tableNames[i]))
                {
                    throw new ArgumentNullException(string.Concat("tableNames[", i, "]"));
                }
            }

            EnsureConnection();

            _command.CommandType = commandType;
            _command.CommandText = commandText;

            AddParameters(dataParameters);

            var adapter = GetDataAdapter(commandText);

            const string defaultTableNamePrefix = "Table";
            for (var i = 0; i < tableNames.Length; i++)
            {
                var defaultTableName =
                    (i == 0) ? defaultTableNamePrefix : defaultTableNamePrefix + i;

                adapter.TableMappings.Add(defaultTableName, tableNames[i]);
            }

            adapter.Fill(ds);

            return ds;
        }

        public DataSet GetDataSet(string commandText, CommandType commandType, IDataParameter[] dataParameters,
                                  DataSet ds, string tableName)
        {
            return GetDataSet(commandText, commandType, dataParameters, ds, new[] {tableName});
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.SqlClient.SqlCommand.Connection"/>
        /// and returns the first column of the first row in the 
        /// resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <returns>The first column of the first row in the resultset.</returns>
        /// <remarks>
        /// <para>Executed <paramref name="commandText"/> is always interpreted as a text command.  
        /// To execute a stored procedure or a table direct command, 
        /// use <see cref="ExecuteScalar(string,CommandType)"/></para>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, CommandType.Text);
        }

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.SqlClient.SqlCommand.Connection"/>
        /// and returns the first column of the first row in the 
        /// resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <returns>The first column of the first row in the resultset.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return ExecuteScalar(commandText, commandType, null);
        }

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.SqlClient.SqlCommand.Connection"/>
        /// and returns the first column of the first row in the 
        /// resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <param name="dataParameters">Array of database parameters to use with command.</param>
        /// <returns>The first column of the first row in the resultset.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public object ExecuteScalar(string commandText, CommandType commandType, IDataParameter[] dataParameters)
        {
            EnsureConnection();

            _command.CommandText = commandText;
            _command.CommandType = commandType;

            AddParameters(dataParameters);

            return _command.ExecuteScalar();
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.SqlClient.SqlCommand.Connection"/>
        /// and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <returns>The first column of the first row in the resultset.</returns>
        /// <remarks>
        /// <para>Executed <paramref name="commandText"/> is always interpreted as a text command.  
        /// To execute a stored procedure or a table direct command, 
        /// use <see cref="ExecuteNonQuery(string,CommandType)"/></para>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, CommandType.Text);
        }

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.SqlClient.SqlCommand.Connection"/>
        /// and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <returns>The first column of the first row in the resultset.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, commandType, null);
        }

        /// <summary>
        /// Executes the <paramref name="commandText"/> against the <see cref="System.Data.SqlClient.SqlCommand.Connection"/>
        /// and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">Text command to run against the data source.</param>
        /// <param name="commandType">Indicates or specifies how the <paramref name="commandText"/> parameter
        /// is interpreted.</param>
        /// <param name="dataParameters">Array of database parameters to use with command.</param>
        /// <returns>The first column of the first row in the resultset.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public int ExecuteNonQuery(string commandText, CommandType commandType, IDataParameter[] dataParameters)
        {
            EnsureConnection();

            _command.CommandText = commandText;
            _command.CommandType = commandType;

            AddParameters(dataParameters);

            return _command.ExecuteNonQuery();
        }

        #endregion

        #region Parameter methods

        /// <summary>
        /// Gets the current command parameters collection.
        /// </summary>
        /// <returns>An <see cref="System.Data.IDataParameterCollection"/> object.</returns>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public IDataParameterCollection GetParameters()
        {
            EnsureConnection();

            return _command.Parameters;
        }

        /// <summary>
        /// Clears the current command parameters.  Same as <see cref="ClearParameters"/>.
        /// </summary>
        public void ResetParameters()
        {
            ClearParameters();
        }

        /// <summary>
        /// Adds an <see cref="System.Data.IDataParameter"/> item to the current command parameter list.
        /// </summary>
        /// <param name="p">New command parameter object.</param>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public void AddParameter(IDataParameter p)
        {
            if (p == null)
            {
                throw new ArgumentNullException("p");
            }

            EnsureConnection();

            // change the input value here
            if (p.Value == null)
            {
                p.Value = DBNull.Value;
            }

            _command.Parameters.Add(p);
        }

        /// <summary>
        /// Adds an array of <see cref="System.Data.IDataParameter"/> item to the current command parameter list.
        /// </summary>
        /// <param name="parameters">Array of new command parameter objects.</param>
        /// <remarks>
        /// <para>Will automatically connect, if connection was not already established.</para>
        /// </remarks>
        public void AddParameters(IDataParameter[] parameters)
        {
            EnsureConnection();

            if (parameters != null && parameters.Length > 0)
            {
                foreach (IDataParameter p in parameters)
                {
                    // change the input value here
                    if (p.Value == null)
                    {
                        p.Value = DBNull.Value;
                    }

                    _command.Parameters.Add(p);
                }
            }
        }

        /// <summary>
        /// Clears the current command parameters.  Same as <see cref="ResetParameters"/>.
        /// </summary>
        public void ClearParameters()
        {
            if (_command != null) _command.Parameters.Clear();
        }

        #endregion

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Dispose in thie block, only managed resources
                if (disposing)
                {
                }

                // Free only un-managed resources here
            }

            _disposed = true;
        }

        #endregion

        #region properties (get/set methods)

        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// Gets or sets the connection string settings name from the app.config that is used.
        /// </summary>
        public string ConnectionStringSettingsName
        {
            get { return _connectionStringSettingsName; }
            set { _connectionStringSettingsName = value; }
        }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command
        /// and generating an error.
        /// </summary>
        /// <remarks>
        /// The time (in seconds) to wait for the command to execute. The default value
        /// is 30 seconds.
        /// </remarks>
        public int CommandTimeout
        {
            get
            {
                if (_commandTimeout < 0)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DbManager.CommandTimeout"]))
                    {
                        if (
                            !int.TryParse(ConfigurationManager.AppSettings["DbManager.CommandTimeout"],
                                          out _commandTimeout))
                        {
                            _commandTimeout = 30;
                        }
                    }
                    else
                    {
                        _commandTimeout = 30;
                    }
                }

                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;

                if (_commandTimeout <= 0)
                    _commandTimeout = 0;

                if (_command != null)
                {
                    _command.CommandTimeout = _commandTimeout;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum count of connection retry attempts in case the connection fails.
        /// </summary>
        /// <remarks>
        /// The default value is 3 retries.
        /// </remarks>
        public int ConnectionRetryCount
        {
            get
            {
                if (_connectionRetryCount < 0)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DbManager.ConnectionRetryCount"]))
                    {
                        if (
                            !int.TryParse(ConfigurationManager.AppSettings["DbManager.ConnectionRetryCount"],
                                          out _connectionRetryCount))
                        {
                            _connectionRetryCount = 3;
                        }
                    }
                    else
                    {
                        _connectionRetryCount = 3;
                    }
                }

                return _connectionRetryCount;
            }
            set
            {
                _connectionRetryCount = value;

                if (_connectionRetryCount <= 0)
                    _connectionRetryCount = 0;
            }
        }

        /// <summary>
        /// Gets or sets the connection retry attempts in case the connection fails.
        /// </summary>
        /// <remarks>
        /// The default is 1 second.
        /// </remarks>
        public TimeSpan ConnectionRetryTimeout
        {
            get
            {
                if (_connectionRetryTimeout == TimeSpan.MinValue)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DbManager.ConnectionRetryTimeout"]))
                    {
                        if (
                            !TimeSpan.TryParse(ConfigurationManager.AppSettings["DbManager.ConnectionRetryTimeout"],
                                               out _connectionRetryTimeout))
                        {
                            _connectionRetryTimeout = new TimeSpan(0, 0, 1);
                        }
                    }
                    else
                    {
                        _connectionRetryTimeout = new TimeSpan(0, 0, 1);
                    }
                }

                return _connectionRetryTimeout;
            }
            set { _connectionRetryTimeout = value; }
        }

        public string ProviderAssemblyName
        {
            get { return _providerAssembly; }
            set { _providerAssembly = value; }
        }

        public string ProviderConnectionClassName
        {
            get { return _providerConnectorClass; }
            set { _providerConnectorClass = value; }
        }

        public string ProviderCommandBuilderClassName
        {
            get { return _providerCommandBuilderClass; }
            set { _providerCommandBuilderClass = value; }
        }

        public DbProviderType Provider
        {
            get { return _provider; }
        }

        #endregion

        #region Utility functions

        protected IDbConnection GetConnection()
        {
            IDbConnection rv;

            switch (Provider)
            {
                case DbProviderType.SqlClient:
                    rv = new SqlConnection();
                    break;

                case DbProviderType.OleDb:
                    rv = new OleDbConnection();
                    break;

                case DbProviderType.Odbc:
                    rv = new OdbcConnection();
                    break;

                case DbProviderType.Oracle:
#pragma warning disable 612,618
                    rv = new OracleConnection();
#pragma warning restore 612,618
                    break;

                case DbProviderType.Other:
                    rv =
                        (IDbConnection)
                        GetAdoNetProviderObject(ProviderAssemblyName, ProviderCommandBuilderClassName, null);
                    break;

                default:
                    throw (new InvalidOperationException("Invalid provider type"));
            }

            if (rv == null)
            {
                throw (new InvalidOperationException("Failed to get ADO.NET Connection object [IDbConnection]"));
            }

            return rv;
        }

        protected IDataAdapter GetDataAdapter(string commandText)
        {
            IDataAdapter rv;

            switch (Provider)
            {
                case DbProviderType.SqlClient:
                    rv = new SqlDataAdapter(commandText, (SqlConnection) _connection);
                    ((SqlDataAdapter) rv).SelectCommand = (SqlCommand) _command;
                    break;

                case DbProviderType.OleDb:
                    rv = new OleDbDataAdapter(commandText, (OleDbConnection) _connection);
                    ((OleDbDataAdapter) rv).SelectCommand = (OleDbCommand) _command;
                    break;

                case DbProviderType.Odbc:
                    rv = new OdbcDataAdapter(commandText, (OdbcConnection) _connection);
                    ((OdbcDataAdapter) rv).SelectCommand = (OdbcCommand) _command;
                    break;

                case DbProviderType.Oracle:
#pragma warning disable 612,618
                    rv = new OracleDataAdapter(commandText, (OracleConnection)_connection);
                    ((OracleDataAdapter) rv).SelectCommand = (OracleCommand) _command;
#pragma warning restore 612,618
                    break;

                case DbProviderType.Other:
                    {
                        var args = new Object[2];
                        args[0] = commandText;
                        args[1] = _connection;

                        rv =
                            (IDbDataAdapter)
                            GetAdoNetProviderObject(ProviderAssemblyName, ProviderCommandBuilderClassName, args);
                        break;
                    }

                default:
                    throw (new InvalidOperationException("Invalid provider type"));
            }

            if (rv == null)
                throw (new InvalidOperationException("Failed to get ADO.NET Data Adapter object [IDataAdapter]"));

            return rv;
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the
        /// <see cref="System.Data.IDbCommand"/> and populates the <see cref="System.Data.IDbCommand.Parameters"/> 
        /// collection of the specified <see cref="System.Data.IDbCommand"/> object.
        /// </summary>
        /// <param name="storedProc">Name of the stored procedure from
        //  which the parameter information is to be derived.</param>
        /// <returns>A <see cref="System.Data.IDataParameterCollection"/> object.  The derived parameters
        //  are added to the <see cref="System.Data.IDbCommand.Parameters"/> collection of the <see cref="System.Data.IDbCommand"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when DeriveParameters method is not suppored by the selected provider.</exception>
        public IDataParameterCollection DeriveParameters(string storedProc)
        {
            return DeriveParameters(storedProc, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the
        /// <see cref="System.Data.IDbCommand"/> and populates the <see cref="System.Data.IDbCommand.Parameters"/> 
        /// collection of the specified <see cref="System.Data.IDbCommand"/> object.
        /// </summary>
        /// <param name="commandText">Name of the stored procedure from
        //  which the parameter information is to be derived.</param>
        /// <returns>A <see cref="System.Data.IDataParameterCollection"/> object.  The derived parameters
        //  are added to the <see cref="System.Data.IDbCommand.Parameters"/> collection of the <see cref="System.Data.IDbCommand"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when DeriveParameters method is not suppored by the selected provider.</exception>
        public IDataParameterCollection DeriveParameters(string commandText, CommandType commandType)
        {
            EnsureConnection();

            ClearParameters();

            _command.CommandText = commandText;
            _command.CommandType = commandType;

            switch (Provider)
            {
                case DbProviderType.SqlClient:
                    SqlCommandBuilder.DeriveParameters((SqlCommand) _command);
                    break;

                case DbProviderType.OleDb:
                    OleDbCommandBuilder.DeriveParameters((OleDbCommand) _command);
                    break;

                case DbProviderType.Odbc:
                    OdbcCommandBuilder.DeriveParameters((OdbcCommand) _command);
                    break;

                case DbProviderType.Oracle:
#pragma warning disable 612,618
                    OracleCommandBuilder.DeriveParameters((OracleCommand) _command);
#pragma warning restore 612,618
                    break;

                case DbProviderType.Other:
                    {
                        Type commandBuilderType = Type.GetType(ProviderCommandBuilderClassName);

                        MethodInfo method = commandBuilderType.GetMethod("DeriveParameters");

                        if (method == null)
                            throw (new InvalidOperationException(
                                "DeriveParameters method is not suppored by the selected provider"));

                        var parameters = new Object[1];
                        parameters[0] = _command;

                        // DeriveParameters is static method
                        method.Invoke(null, parameters);

                        break;
                    }

                default:
                    throw (new Exception("Invalid provider type"));
            }

            return _command.Parameters;
        }

        protected object GetAdoNetProviderObject(string providerAssembly, string providerClass, object[] args)
        {
            if (providerAssembly == null || providerAssembly.Trim().Length == 0)
                throw (new InvalidOperationException("Invalid provider providerAssembly name"));

            if (providerClass == null || providerClass.Trim().Length == 0)
                throw (new InvalidOperationException("Invalid provider connection class name"));

            var logger = LogManager.GetLogger(typeof(DbManager));
            if (logger.IsDebugEnabled)
                logger.DebugFormat("Attempting to load assembly: {0}", providerAssembly);
            var sourceAssembly = Assembly.Load(providerAssembly);
            if (args == null)
            {
                return sourceAssembly.CreateInstance(providerClass, true);
            }
            var commandType = sourceAssembly.GetType(providerClass, true, true);
            var types = new Type[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                types[i] = args[0].GetType();
            }
            var c = commandType.GetConstructor(types);

            return c.Invoke(args);
        }

        #endregion
    }
}