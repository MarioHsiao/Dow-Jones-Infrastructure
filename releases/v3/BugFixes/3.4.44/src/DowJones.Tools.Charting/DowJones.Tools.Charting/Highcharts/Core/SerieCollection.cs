//
//  Copyright (c) 2010, 100loop
//  All rights reserved.
//
//  Authors: 
//           
//           * André Paulovich (paulovich@100loop.com)
//           Blog: http://www.100loop.com/          
//           Talk: andre.paulovich@gmail.com 
//

using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Tools.Charting.Highcharts.Core.Data.Chart;
using DowJones.Tools.Charting.Highcharts.Core.PlotOptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DowJones.Tools.Charting.Highcharts.Core
{
    [Serializable]
    public class SerieCollection : List<Serie>
    {
        public SerieCollection()
        {
        }

        public SerieCollection(IEnumerable<Serie> series) : base(series){ }

        public DataLabels DataLabels { get; set; }
        
        public override string ToString()
        {
            var serialize = string.Format("series: [{0}[@DataLabels]]", string.Join(",", this.Select(serie => JsonConvert.SerializeObject(serie, Formatting.None, new JsonSerializerSettings
                                                                                                                                                                         {
                                                                                                                                                                             NullValueHandling = NullValueHandling.Ignore,
                                                                                                                                                                             ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                                                                                                                                         })).ToArray()));

            serialize = DataLabels != null ? serialize.Replace("[@DataLabels]", ", " + DataLabels) : serialize.Replace("[@DataLabels]", string.Empty);

            return serialize;
        }
    }
}