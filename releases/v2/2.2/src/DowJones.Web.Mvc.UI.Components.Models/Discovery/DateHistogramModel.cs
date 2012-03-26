// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateHistogramModel.cs" company="Dow Jones & Company">
//   © 2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The distribution.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using DowJones.Models.Search;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Models.Discovery
{       
    /// <summary>
    /// The date histogram model.
    /// </summary>
    [DataContract(Name = "dateHistogramModel", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn, Id = "dateHistogramModel")]
    public class DateHistogramModel : ViewComponentModel
    {
        /// <summary>
        ///   Gets or sets BarColor.
        /// </summary>
        [ClientProperty("barColor")]
        public string BarColor { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [DefaultValue(60)]
        [ClientProperty("height")]
        public int Height { get; set; }

        /// <summary>
        ///   Gets or sets Histogram.
        /// </summary>
        [ClientData]
        public Histogram Histogram { get; set; }

        public DateHistogramModel()
        {
        }

        public DateHistogramModel(Histogram histogram)
        {
            Histogram = histogram;
        }
    }
}