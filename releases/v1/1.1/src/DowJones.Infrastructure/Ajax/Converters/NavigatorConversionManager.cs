using GatewayNavigator = Factiva.Gateway.Messages.Search.V2_0.Navigator;

namespace DowJones.Ajax.Converters
{
    public class NavigatorConversionManager
    {
        public Navigator.Navigator Process(GatewayNavigator gatewayNavigator)
        {
            return (new Navigator.Converter.NavigatorConverter()).Process(gatewayNavigator);
        }
    }
}
