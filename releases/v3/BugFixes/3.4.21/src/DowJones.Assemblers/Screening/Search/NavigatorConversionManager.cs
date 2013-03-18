using DowJones.Ajax.Navigator;
using DowJones.Mapping;

namespace DowJones.Assemblers.Search
{
    public class NavigatorConversionManager
        : TypeMapper<Factiva.Gateway.Messages.Search.V2_0.Navigator, Navigator>
    {
        public override Navigator Map(Factiva.Gateway.Messages.Search.V2_0.Navigator source)
        {
            return Process(source);
        }

        public Navigator Process(Factiva.Gateway.Messages.Search.V2_0.Navigator gatewayNavigator)
        {
            return (new NavigatorConverter()).Process(gatewayNavigator);
        }
    }
}
