using DowJones.Ajax.Navigator;
using DowJones.Mapping;

namespace DowJones.Assemblers.Search
{
    /// <summary>
    /// </summary>
    public class NavigatorConversionManager
        : TypeMapper<Factiva.Gateway.Messages.Search.V2_0.Navigator, Navigator>
    {
        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public override Navigator Map(Factiva.Gateway.Messages.Search.V2_0.Navigator source)
        {
            return Process(source);
        }

        /// <summary>
        /// </summary>
        /// <param name="gatewayNavigator"> </param>
        /// <returns> </returns>
        public Navigator Process(Factiva.Gateway.Messages.Search.V2_0.Navigator gatewayNavigator)
        {
            return (new NavigatorConverter()).Process(gatewayNavigator);
        }
    }
}