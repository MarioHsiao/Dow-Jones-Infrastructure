using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using DowJones.Infrastructure;
using log4net;

namespace DowJones.Dash.Common.DataSources
{
    public class SqlDataSource : PollingDataSource
    {
        private readonly IDictionary<string, object> _parameters;
        
        public CommandType CommandType { get; set; }
        
        public string ConnectionString { get; protected set; }
        
        public string Query { get; protected set; }

        public bool Scalar { get; set; }

        protected override ILog Log
        {
            get { return _log; }
        }
        private static readonly ILog _log = LogManager.GetLogger(typeof(SqlDataSource));

        protected SqlDataSource(string name, string dataName, string connectionString, string query = null, IDictionary<string, object> parameters = null, Func<int> pollDelayFactory = null, Func<int> errorDelayFactory = null)
            : base(name, dataName, pollDelayFactory, errorDelayFactory)
        {
            _parameters = parameters;
            Guard.IsNotNullOrEmpty(connectionString, "connectionString");

            CommandType = CommandType.StoredProcedure;
            ConnectionString = connectionString;
            Query = query;
        }

        protected override void Poll()
        {
            if(string.IsNullOrWhiteSpace(Query))
                throw new ApplicationException("Missing SQL query");

            try
            {
                var connection = new SqlConnection(ConnectionString);
                var command = new SqlCommand(Query, connection) { CommandType = CommandType };

                foreach (var parameter in _parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                Log.DebugFormat("Opening connection to {0}...", connection.Database);
                connection.Open();

                Log.DebugFormat("Executing query: {0}", Query);
                command.BeginExecuteReader(OnReaderExecuted, command);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        protected internal void OnReaderExecuted(IAsyncResult result)
        {
            Log.DebugFormat("Query executed: {0}", Query);

            try
            {
                var command = (SqlCommand)result.AsyncState;
                using (command.Connection)
                using (var reader = command.EndExecuteReader(result))
                {
                    var data = new DynamicSqlDataReader().Read(reader).ToArray();

                    OnDataReceived(Scalar ? data.FirstOrDefault() : data);
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }
        
		public class DynamicSqlDataReader
		{
			private static dynamic ToExpando(IDataRecord record)
			{
				var expandoObject = new ExpandoObject() as IDictionary<string, object>;

				for (var i = 0; i < record.FieldCount; i++)
					expandoObject.Add(record.GetName(i), record[i]);

				return expandoObject;
			}

			public IEnumerable<dynamic> Read(SqlDataReader reader)
			{
				while (reader.Read())
				{
					yield return ToExpando(reader);
				}
			}
		}
    }
}