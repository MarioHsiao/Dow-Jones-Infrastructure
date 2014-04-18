namespace DowJones.Json.Gateway.Converters
{
    internal class DataContractConverterSingleton
    {
        private static DataContractJsonConverter _instance;

        private DataContractConverterSingleton() { }

        internal static DataContractJsonConverter Instance
        {
            get { return _instance ?? (_instance = new DataContractJsonConverter()); }
        }
    }
}