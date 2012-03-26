// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Utilities.Formatters;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Models.Common;
using DowJones.Web.Mvc.UI.Models.NewsPages;

namespace DowJones.Web.Mvc.UI.Models.Common
{
    /// <summary>
    /// The entity news volume variation.
    /// </summary>
    /// <remarks></remarks>
    [DataContract(Name = "entityNewsVolumeVariation", Namespace = "")]
    public class NewsEntityNewsVolumeVariation : NewsEntity
    {
        /// <summary>
        /// Gets or sets the volume for previous time frame.
        /// </summary>
        /// <value>The volume for previous time frame.</value>
        /// <remarks></remarks>
        [DataMember(Name = "previousNewsVolume")]
        public WholeNumber PreviousTimeFrameNewsVolume { get; set; }

        /// <summary>
        /// Gets or sets the percent volume change.
        /// </summary>
        /// <value>The percent volume change.</value>
        /// <remarks></remarks>
        [DataMember(Name = "percentVolumeChange")]
        public Percent PercentVolumeChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [new entrant].
        /// </summary>
        /// <value><c>true</c> if [new entrant] it will be true; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        [DataMember(Name = "newEntrant")]
        public bool NewEntrant { get; set; }
    }

    [CollectionDataContract(Name = "newsEntities", ItemName = "newsEntity", Namespace = "")]
    public class NewsEntities : List<NewsEntity>
    {
    }

    [CollectionDataContract(Name = "parentNewsEntities", ItemName = "parentNewsEntity", Namespace = "")]
    public class ParentNewsEntities : List<ParentNewsEntity>
    {
    }

    [DataContract(Name = "parentNewsEntity", Namespace = "")]
    public class ParentNewsEntity : NewsEntity
    {
        [DataMember(Name = "newsEntities", EmitDefaultValue = false)]
        public NewsEntities NewsEntities { get; set; }
    }

    /// <summary>
    /// The trending entity. Contains the code, volume the descriptor in the requested interface language
    /// </summary>
    /// <remarks></remarks>
    [DataContract(Name = "newsEntity", Namespace = "")]
    public class NewsEntity
    {
        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        /// What we probably need is Bucket
        /// <remarks></remarks>
        [DataMember(Name = "type")]
        public EntityType Type { get; set; }

        /// <summary>
        /// Gets or sets the news volume.
        /// </summary>
        /// <value>The news volume.</value>
        /// <remarks></remarks>
        [DataMember(Name = "currentTimeFrameNewsVolume")]
        public WholeNumber CurrentTimeFrameNewsVolume { get; set; }

        /// <summary>
        /// Gets or sets the entity factiva code.
        /// </summary>
        /// <value>The entity factiva code.</value>
        /// <remarks></remarks>
        [DataMember(Name = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the entity descriptor.
        /// </summary>
        /// <value>The entity descriptor.</value>
        /// <remarks></remarks>
        [DataMember(Name = "descriptor")]
        public string Descriptor { get; set; }

        [DataMember(Name = "searchContextRef")]
        public string SearchContextRef { get; set; }
    }

    public class ModuleIdByMetadata
    {
        [DataMember(Name = "moduleId")]
        public int ModuleId;

        [DataMember(Name = "metaData")]
        public MetaData MetaData;

        [DataMember(Name = "interfaceLanguage")]
        public string InterfaceLanguage;
    }
}
