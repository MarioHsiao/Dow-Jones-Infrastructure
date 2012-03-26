// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using DowJones.Formatters;

namespace DowJones.Pages.Company
{
    [DataContract(Name = "marketIndexIntradayResult", Namespace = "")]
    public class MarketIndexIntradayResult
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "low")]
        public DoubleNumberStock Low { get; set; }

        [DataMember(Name = "high")]
        public DoubleNumberStock High { get; set; }

        [DataMember(Name = "previousClose")]
        public DoubleNumberStock PreviousClose { get; set; }

        [DataMember(Name = "dataPoints")]
        public BasicDataPointCollection DataPoints { get; set; }

        [DataMember(Name = "date")]
        public DateTime Start { get; set; }

        [DataMember(Name = "dateDescripter")]
        public string StartDescripter { get; set; }

        [DataMember(Name = "provider")]
        public Provider Provider { get; set; }
    }
}

