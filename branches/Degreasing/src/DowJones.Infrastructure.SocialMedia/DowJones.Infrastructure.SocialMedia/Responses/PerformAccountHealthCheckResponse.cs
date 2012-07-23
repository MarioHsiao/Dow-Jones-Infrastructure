// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformAccountHealthCheckResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   Defines the PerformAccountHealthCheckResponse type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.SocialMedia.Responses
{               
    /// <summary>
    /// </summary>
    [Serializable]
    [DataContract]
    [DebuggerDisplay("{IsOk}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class PerformAccountHealthCheckResponse : AbstractSocialMediaResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether IsOk.
        /// </summary>
        [DataMember]
        public bool IsOk
        {
            get; set; 
        }
    }
}
