// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCategoryUsersResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.SocialMedia.Responses
{
    using DowJones.Infrastructure.Models.SocialMedia;

    /// <summary>
    /// </summary>
    [Serializable]
    [DataContract]
    [DebuggerDisplay("{TotalCount}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class GetCategoryUsersResponse : BaseUsersResponse
    {
    }
}
