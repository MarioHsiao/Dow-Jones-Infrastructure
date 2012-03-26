// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Models
{
    [CollectionDataContract(Name = "executives", Namespace = "")]
    public class ExecutiveCollection : List<Executive>
    {
        public ExecutiveCollection() : base()
        {
        }

        public ExecutiveCollection(IEnumerable<Executive> collection) : base(collection)
        {
        }
    }

    [DataContract(Name = "executive", Namespace = "")]
    public class Executive
    {
        [DataMember(Name = "consolidationCode")]
        public string ConsolidationId { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "middleNames")]
        public string MiddleNames { get; set; }

        [DataMember(Name = "suffix")]
        public string Suffix { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "completeName")]
        public string CompleteName { get; set; }
        
        [DataMember(Name = "jobTitle")]
        public string JobTitle { get; set; }

        [DataMember(Name = "isOfficer")]
        public bool IsOfficer { get; set; }

        [DataMember(Name = "isEmployee")]
        public bool IsEmployee { get; set; }
    }
}
