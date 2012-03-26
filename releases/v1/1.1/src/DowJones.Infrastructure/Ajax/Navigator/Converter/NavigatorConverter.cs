using System.Collections.Generic;
using GatewayNavigator = Factiva.Gateway.Messages.Search.V2_0.Navigator;

namespace DowJones.Ajax.Navigator.Converter
{
    class NavigatorConverter
    {
        public Navigator Process(GatewayNavigator gatewayNavigator)
        {
            var navigator = new Navigator
                                {
                                    Count = gatewayNavigator.Count,
                                    Id = gatewayNavigator.Id,
                                    BucketCollection = new List<Bucket>(),
                                    CountSpecified = gatewayNavigator.__countSpecified
                                };
            foreach (var gatewayBucket in gatewayNavigator.BucketCollection)
            {
                navigator.BucketCollection.Add(new Bucket
                                                   {
                                                       HitCount = gatewayBucket.HitCount,
                                                       Id = gatewayBucket.Id,
                                                       Value = gatewayBucket.Value,
                                                       CountSpecified = gatewayBucket.__hitCountSpecified
                                                   });
            }
            return navigator;
        }
    }
}
