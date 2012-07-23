// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCategorySuperListResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   Defines the CategorySuperListResponse type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using DowJones.Infrastructure.Models.SocialMedia;
using Newtonsoft.Json;

namespace DowJones.SocialMedia.Responses
{  
    /// <summary>
    /// </summary>
    [Serializable]
    [DataContract]
    [DebuggerDisplay("{TotalCount}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class GetCategorySuperListResponse : CategorySuperList, ISocialMediaResponse
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCategorySuperListResponse"/> class.
        /// </summary>
        public GetCategorySuperListResponse()
        {
            Audit = new Audit();
        }
        
        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets Message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Audit.
        /// </summary>
        /// <value>
        /// The audit.
        /// </value>
        public Audit Audit { get; private set; }

        #endregion
    }
}
