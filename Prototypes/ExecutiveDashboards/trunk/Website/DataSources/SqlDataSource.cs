using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DowJones.Infrastructure;
using Newtonsoft.Json;

namespace DowJones.Dash.Website.DataSources
{
    public class SqlDataSource : DataSource
    {
        public string ConnectionString { get; private set; }
        public string Query { get; private set; }

        public SqlDataSource(string connectionString, string query)
        {
            Guard.IsNotNullOrEmpty(connectionString, "connectionString");
            Guard.IsNotNullOrEmpty(query, "query");

            ConnectionString = connectionString;
            Query = query;
        }

        public override void Start()
        {
            Task.Factory.StartNew(Poll);
        }

        protected internal void Poll()
        {
            var connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(Query, connection);
            command.BeginExecuteReader(OnReaderExecuted, command);
        }

        protected internal void OnReaderExecuted(IAsyncResult result)
        {
            var command = (SqlCommand)result.AsyncState;

            using (command.Connection)
            using (var reader = command.EndExecuteReader(result))
            {
                var data = new DynamicSqlDataReader().Read(reader);
                OnDataReceived(data);
            }
        }

        protected internal void OnError(Exception ex)
        {
            Trace.TraceError("Error executing SQL Query: {0}", ex);
            Poll();
        }


        public class DynamicSqlDataReader
        {
            public IEnumerable<dynamic> Read(SqlDataReader reader)
            {
                var columnNames = GetColumnNames(reader).ToArray();

                var json = new StringBuilder("[");

                while (reader.Read())
                {
                    foreach (var col in columnNames)
                    {
                        json.AppendFormat("{{ \"{0}\": {1} }},", col, reader[col]);
                    }
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