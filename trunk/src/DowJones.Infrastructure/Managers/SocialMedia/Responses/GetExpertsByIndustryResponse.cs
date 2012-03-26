using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DowJones.Managers.SocialMedia.Responses
{
    using Infrastructure.Models.SocialMedia;

    [Serializable]
    [DataContract]
    [DebuggerDisplay("{TotalCount}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class GetExpertsByIndustryResponse : List<TwitterUser>, ISocialMediaResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetExpertsByIndustryResponse"/> class.
        /// </summary>
        public GetExpertsByIndustryResponse()
        {
            Audit = new Audit();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        public Status Status { get; set; }

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


