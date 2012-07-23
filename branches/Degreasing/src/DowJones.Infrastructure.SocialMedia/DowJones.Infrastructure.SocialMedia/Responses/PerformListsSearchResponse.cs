// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformListsSearchResponse.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   Defines the PerformListsSearchResponse type.
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
    [DebuggerDisplay("{TotalCount}")]
    [JsonObject(MemberSerialization.OptIn)]
    public class PerformListsSearchResponse : BaseListsResponse
    {
    }
}
