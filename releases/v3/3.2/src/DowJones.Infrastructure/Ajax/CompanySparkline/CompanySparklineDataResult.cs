// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanySparklineDataResult.cs" company="Dow Jones & Company">
//   
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.Formatters;
using Newtonsoft.Json;

namespace DowJones.Ajax.CompanySparkline
{
    public enum SparklineStatus
    {
        /// <summary>
        /// Not specified
        /// </summary>
        Unspecified,
        Increase,
        Decrease,   
        Same,
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract( Name = "companySparklineDataResult", Namespace = "" )]
    [JsonObject( MemberSerialization.OptIn, Id = "companySparklineDataResult" )]
    public class CompanySparklineDataResult
    {
        /// <summary>
        ///   Gets or sets code.
        /// </summary>
        [DataMember(Name = "code")]
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        ///   Gets or sets Name.
        /// </summary>
        [DataMember(Name = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets ClosePrices.
        /// </summary>
        [DataMember(Name = "closePrices")]
        [JsonProperty("closePrices")]
        public ClosePriceCollection ClosePrices { get; set; }

        /// <summary>
        ///   Gets Latest Close Price.
        /// </summary>
        [DataMember(Name = "latestClosePrice")]
        [JsonProperty("latestClosePrice")]
        public DoubleNumberStock LatestClosePrice
        {
            get
            { 
                if (this.ClosePrices != null &&
                    this.ClosePrices.Count > 0)
                {
                    return this.ClosePrices.Last();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the max value with in the CompanySparlineDataResult.
        /// </summary>
        /// <value>
        /// The max.
        /// </value>
        [DataMember(Name = "max")]
        [JsonProperty("max")]
        public DoubleNumberStock Max
        {
            get
            {
                var max = new DoubleNumberStock(decimal.MinValue);

                if (this.ClosePrices != null &&
                    this.ClosePrices.Count > 0)
                {
                    foreach (var closePrice in this.ClosePrices.Where(closePrice => max.Value < closePrice.Value))
                    {
                        max = closePrice;
                    }
                }

                return max;
            }
        }

        /// <summary>
        /// Gets the min.
        /// </summary>
        /// <value>
        /// The min.
        /// </value>
        [DataMember(Name = "min")]
        [JsonProperty("min")]
        public DoubleNumberStock Min
        {
            get
            {
                var min = new DoubleNumberStock(decimal.MaxValue);

                if (this.ClosePrices != null && 
                    this.ClosePrices.Count > 0)
                {
                    foreach (var closePrice in this.ClosePrices.Where(closePrice => min.Value > closePrice.Value))
                    {
                        min = closePrice;
                    }
                }

                return min;
            }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        [DataMember(Name = "status")]
        [JsonProperty("status")]
        public SparklineStatus Status
        {
            get
            {     
                if (this.ClosePrices != null &&
                    this.ClosePrices.Count >= 2)
                {
                    var secondToLast = this.ClosePrices[this.ClosePrices.Count - 2];
                    var last = this.ClosePrices[this.ClosePrices.Count - 1];

                    if (secondToLast.Value == last.Value)
                    {
                        return SparklineStatus.Same;
                    }

                    return secondToLast.Value > last.Value ? SparklineStatus.Decrease : SparklineStatus.Increase;
                }

                return SparklineStatus.Unspecified;
            }
        }   
    }

    /// <summary>
    /// The close price collection.
    /// </summary>
    [CollectionDataContract(ItemName = "closePrice", Namespace = "")]
    public class ClosePriceCollection : List<DoubleNumberStock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClosePriceCollection"/> class.
        /// </summary>
        public ClosePriceCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClosePriceCollection"/> class.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        public ClosePriceCollection(IEnumerable<DoubleNumberStock> items)
            : base(items)
        {
        }
    }
}