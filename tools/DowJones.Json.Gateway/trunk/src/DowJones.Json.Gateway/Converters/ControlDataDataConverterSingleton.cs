namespace DowJones.Json.Gateway.Converters
{
    internal class ControlDataDataConverterSingleton
    {
        private static ISerialize _instance;

        private ControlDataDataConverterSingleton() { }

        internal static ISerialize Instance
        {
            get { return _instance ?? (_instance = new ControlDataJsonConverter()); }
        }
    }
}