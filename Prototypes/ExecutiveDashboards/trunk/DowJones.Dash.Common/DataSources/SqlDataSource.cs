using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DowJones.Infrastructure;
using Newtonsoft.Json;

namespace DowJones.Dash.DataSources
{
    public class SqlDataSource : PollingDataSource
    {
        private readonly IDictionary<string, object> _parameters;
        
        public CommandType CommandType { get; set; }
        
        public string ConnectionString { get; protected set; }
        
        public string Query { get; protected set; }

        protected SqlDataSource(string name, string connectionString, string query = null, IDictionary<string, object> parameters = null)
            : base(name)
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

                Log("Opening connection to {0}...", connection.Database);
                connection.Open();

                Log("Executing query: {0}", Query);
                command.BeginExecuteReader(OnReaderExecuted, command);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        protected internal void OnReaderExecuted(IAsyncResult result)
        {
            try
            {
                var command = (SqlCommand)result.AsyncState;
                using (command.Connection)
                using (var reader = command.EndExecuteReader(result))
                {
                    var data = new DynamicSqlDataReader().Read(reader);
                    OnDataReceived(data);
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        public class DynamicSqlDataReader
        {
            public IEnumerable<dynamic> Read(SqlDataReader reader)
            {
                var columnNames = GetColumnNames(reader).ToArray();

                var json = new StringBuilder("[");

                while (reader.Read())
                {
                    json.Append("{");
                    foreach (var col in columnNames)
                    {
                        json.AppendFormat("\"{0}\": \"{1}\",", col, reader[col].ToString().Replace("\"", "\\\""));
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append(" },");
                }

                json.Remove(json.Length - 1, 1);
                json.Append("]");

                return JsonConvert.DeserializeObject<IEnumerable<dynamic>>(json.ToString());
            }

            private static IEnumerable<string> GetColumnNames(IDataRecord reader)
            {
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    yield return reader.GetName(i);
                }
            }
        }
    }
}