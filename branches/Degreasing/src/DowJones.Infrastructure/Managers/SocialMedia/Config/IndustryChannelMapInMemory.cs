using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Managers.SocialMedia.Config
{
    public partial class IndustryChannelMap
    {
        public IDictionary<string, string> LoadInMemoryMap()
        {
            return new Dictionary<string, string>
                          {
                              { "iacc", "accounting-consulting" },
                              { "iadv", "advertising-public-relations"}, 
                              { "iaer", "aerospace-defense"}, 
                              { "i0", "agriculture-forestry"}, 
                              { "i75", "airlines"}, 
                              { "iaut", "automobiles"}, 
                              { "ibnk", "banking-credit"}, 
                              { "i2569", "biotechnology"}, 
                              { "ibcs", "business-consumer-services"}, 
                              { "i25", "chemicals"}, 
                              { "iclt", "clothing-textiles"}, 
                              { "i3302 ", "computers-electronics"}, 
                              { "icre", "construction-real-estate"}, 
                              { "icnp", "consumer-products"}, 
                              { "i1", "energy"}, 
                              { "iewm", "environment-waste-management"}, 
                              { "i41", "food-beverages-tobacco"}, 
                              { "i851", "health-care"}, 
                              { "i66", "hotels-restaurants-casinos"}, 
                              { "i82", "insurance"}, 
                              { "iint", "internet-online-services"}, 
                              { "iinv", "investing-securities"}, 
                              { "ilea", "leisure-arts"}, 
                              { "i32", "machinery-industrial-goods"}, 
                              { "imed", "media"}, 
                              { "imet", "metals-mining"}, 
                              { "ipap", "paper-packaging"}, 
                              { "i257", "pharmaceuticals"}, 
                              { "i64", "retail-wholesale"}, 
                              { "i7902", "telecommunications"}, 
                              { "itsp", "transportation-shipping"} 
                          };

        }
    }
}
