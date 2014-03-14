namespace DowJones.Json.Gateway.Converters
{
    internal class DataConverterDecoratorSingleton
    {
        private static DataContractDataConverterDecorator _instance;

        private DataConverterDecoratorSingleton() { }

        internal static DataContractDataConverterDecorator Instance
        {
            get { return _instance ?? (_instance = new DataContractDataConverterDecorator(DataContractConverterSingleton.Instance)); }
        }
    }
}