// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonTypes.cs" company="">
//   
// </copyright>
// <summary>
//   The entity news volume variation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common
{
    /// <summary>
    /// Represents the different Module parts for the Top news Module.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [DataContract(Name = "topNewsModuleParts", Namespace = "")]
    public enum TopNewsModulePart
    {
        /// <summary>
        /// The editors choice.
        /// </summary>
        [EnumMember]
        EditorsChoice = 0, 

        /// <summary>
        /// The video.
        /// </summary>
        [EnumMember]
        VideoAndAudio = 1, 

        /// <summary>
        /// The opinion and analysis.
        /// </summary>
        [EnumMember]
        OpinionAndAnalysis = 2, 
    }
    
    /// <summary>
    /// The feed type.
    /// </summary>
    [DataContract(Name = "feedType", Namespace = "")]
    public enum FeedType
    {
        /// <summary>
        /// The rss feed.
        /// </summary>
        [EnumMember]
        RSS, 

        /// <summary>
        /// The atom feed.
        /// </summary>
        [EnumMember]
        Atom
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public enum PageDefaultBy
    {
        /// <summary>
        /// 
        /// </summary>
        Position,
        /// <summary>
        /// 
        /// </summary>
        DefaultAttribute,
        /// <summary>
        /// 
        /// </summary>
        PageId,
    }
}