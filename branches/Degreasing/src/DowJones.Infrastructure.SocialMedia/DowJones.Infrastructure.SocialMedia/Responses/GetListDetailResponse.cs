// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
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
    [DebuggerDisplay("{Name}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class GetListDetailResponse : TwitterList, ISocialMediaResponse
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GetListDetailResponse"/> class.
        /// </summary>
        public GetListDetailResponse()
        {
            Audit = new Audit();
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Message.
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
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
