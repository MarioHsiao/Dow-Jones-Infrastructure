using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Models.Charting
{
    [CollectionDataContract]
    public class BasicDataPointCollection : List<BasicDataPoint>
    {
    }
}