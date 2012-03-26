﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalHeadlineListDataResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Utilities.Formatters;
using Newtonsoft.Json;

namespace DowJones.Tools.Ajax.PortalHeadlineList
{
    [DataContract(Name = "portalHeadlineListDataResult", Namespace = "")]
    public class PortalHeadlineListDataResult : IListDataResult
    {

        private PortalHeadlineListResultSet resultSet;
        
        [DataMember(Name = "hitCount")]
        [JsonProperty("hitCount")]
        public WholeNumber HitCount { get; set; }

        [DataMember(Name = "resultSet")]
        [JsonProperty("resultSet")]
        public PortalHeadlineListResultSet ResultSet
        {
            get
            {
                return resultSet ?? (resultSet = new PortalHeadlineListResultSet());
            }

            set
            {
                resultSet = value;
            }
        }
    }
}
