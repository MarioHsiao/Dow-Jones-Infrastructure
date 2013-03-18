// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Histogram.cs" company="DowJones &amp; Company">
//     Enterprise Market Group
// </copyright>
// <summary>
//   The distribution.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
     
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.Formatters;
using Newtonsoft.Json;

namespace DowJones.Models.Search
{
    /// <summary>
    /// The distribution.
    /// </summary>
    public enum Distribution
    {
        /// <summary>
        ///   The daily.
        /// </summary>
        Daily, 

        /// <summary>
        ///   The weekly.
        /// </summary>
        Weekly, 

        /// <summary>
        ///   The monthy.
        /// </summary>
        Monthy, 

        /// <summary>
        ///   The yearly.
        /// </summary>
        Yearly, 
    }                   

    /// <summary>
    /// The historgram.
    /// </summary>
    [DataContract(Name = "histogram", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn, Id = "histogram")]
    public class Histogram
    {
        /// <summary>
        ///   Gets or sets the distribution.
        /// </summary>
        /// <value>
        ///   The distribution.
        /// </value>
        [JsonProperty("distribution")]
        [DataMember(Name = "distribution")]
        public Distribution Distribution { get; set; }

        /// <summary>
        ///   Gets or sets DistributionText.
        /// </summary>
        [JsonProperty("distributionText")]
        [DataMember(Name = "distributionText")]
        public string DistributionText { get; set; }

        /// <summary>
        ///   Gets or sets the start date.
        /// </summary>
        /// <value>
        ///   The end date.
        /// </value>
        [JsonProperty("startDate")]
        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        ///   Gets or sets the end date.
        /// </summary>
        /// <value>
        ///   The start date.
        /// </value>
        [JsonProperty("endDate")]
        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        ///   Gets or sets the start dateText.
        /// </summary>
        /// <value>
        ///   The end date.
        /// </value>
        [JsonProperty("startDateText")]
        [DataMember(Name = "startDateText")]
        public string StartDateText { get; set; }

        /// <summary>
        ///   Gets or sets the end date.
        /// </summary>
        /// <value>
        ///   The start date.
        /// </value>
        [JsonProperty("endDateText")]
        [DataMember(Name = "endDateText")]
        public string EndDateText { get; set; }

        /// <summary>
        /// Gets the max.
        /// </summary>
        [JsonProperty("max")]
        [DataMember(Name = "max")]
        public WholeNumber Max
        {
            get
            {
                var max = new WholeNumber(0);

                if (this.Items != null &&
                    this.Items.Count > 0)
                {
                    foreach (var item in this.Items.Where(item => max.Value < item.HitCount.Value))
                    {
                        max = item.HitCount;
                    }
                }

                return max;
            }
        }

        /// <summary>
        ///   Gets or sets DateItems.
        /// </summary>
        [JsonProperty("items")]
        [DataMember(Name = "items")]
        public IList<HistogramItem> Items { get; set; }
    }

    /// <summary>
    /// The date item.
    /// </summary>
    [DataContract(Name = "histogramItem", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn, Id = "histogramItem")]
    public class HistogramItem
    {
        /// <summary>
        ///   Gets or sets Start Date.
        /// </summary>
        [JsonProperty("startDate")]
        [DataMember(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///   Gets or sets End Date.
        /// </summary>
        [JsonProperty("endDate")]
        [DataMember(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///   Gets or sets the start dateText.
        /// </summary>
        /// <value>
        ///   The end date.
        /// </value>
        [JsonProperty("startDateText")]
        [DataMember(Name = "startDateText")]
        public string StartDateText { get; set; }

        /// <summary>
        ///   Gets or sets the iso start date.
        /// </summary>
        /// <value>
        ///   The end date.
        /// </value>
        [JsonProperty("isoStartDate")]
        [DataMember(Name = "isoStartDate")]
        public string IsoStartDate { get; set; }

        /// <summary>
        ///   Gets or sets the end date.
        /// </summary>
        /// <value>
        ///   The start date.
        /// </value>
        [JsonProperty("endDateText")]
        [DataMember(Name = "endDateText")]
        public string EndDateText { get; set; }

        /// <summary>
        ///   Gets or sets the iso start date.
        /// </summary>
        /// <value>
        ///   The end date.
        /// </value>
        [JsonProperty("isoEndDate")]
        [DataMember(Name = "isoEndDate")]
        public string IsoEndDate { get; set; }

        /// <summary>
        ///   Gets or sets CurrentDate.
        /// </summary>
        [JsonProperty("currentDate")]
        [DataMember(Name = "currentDate")]
        public DateTime? CurrentDate { get; set; }

        /// <summary>
        ///   Gets or sets the current date.
        /// </summary>
        /// <value>
        ///   The start date.
        /// </value>
        [JsonProperty("currentDateText")]
        [DataMember(Name = "currentDateText")]
        public string CurrentDateText { get; set; }

        /// <summary>
        ///   Gets or sets HitCounts.
        /// </summary>
        [JsonProperty("hitCount")]
        [DataMember(Name = "hitCount")]
        public WholeNumber HitCount { get; set; }
    }
}