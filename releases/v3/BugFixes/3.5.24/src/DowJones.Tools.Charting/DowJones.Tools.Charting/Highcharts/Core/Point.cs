using System;
using DowJones.Tools.Charting.Highcharts.Core.Data.Chart;
using DowJones.Tools.Charting.Highcharts.Core.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    public class Point
    {
        public Point()
        {
        }

        public Point(object oY)
        {
            Y = oY;
        }

        public Point(object oY, string symbol)
        {
            Y = oY;
            Marker = new Marker(symbol);
        }

        public Point(object oX, object oY)
        {
            X = oX;
            Y = oY;
        }

        public Point(object oX, object oY, object id)
        {
            X = oX;
            Y = oY;
            Id = id.ToString();
        }

        public string Color { get; set; }
        public Serie Drilldown { get; set; }
        public PointEvents Events { get; set; }
        public string Id { get; set; }
        public Marker Marker { get; set; }
        public string Name { get; set; }
        public bool? Sliced { get; set; }
        public object X { get; set; }
        public object Y { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
                                                                          {
                                                                              NullValueHandling = NullValueHandling.Ignore,
                                                                              ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                          });
        }
    }
}