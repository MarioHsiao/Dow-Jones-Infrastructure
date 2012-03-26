// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using DowJones.Formatters;
using DowJones.Pages.Company;

namespace DowJones.Pages
{
    [DataContract(Name = "newsDataPoint", Namespace = "")]
    public class NewsDataPoint : IDataPoint
    {
        [DataMember(Name = "date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "dateDisplay")]
        public string DateDisplay { get; set; }

        [DataMember(Name = "dataPoint")]
        public Number DataPoint { get; set; }
    }
}
