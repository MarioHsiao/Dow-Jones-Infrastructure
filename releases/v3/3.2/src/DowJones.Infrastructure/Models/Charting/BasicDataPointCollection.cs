using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Models.Charting
{
    [DataContract]
    public class BasicDataPointCollection : List<BasicDataPoint>
    {
    }
}