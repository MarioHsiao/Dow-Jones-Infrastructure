// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractHeadlinePackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Ajax.PortalHeadlineList;

namespace DowJones.Factiva.Currents.Website.Models.PageService
{
     [DataContract(Namespace="")]
    public abstract class AbstractHeadlinePackage : IPackage, IPortalHeadlines
    {
        #region IPortalHeadlines Members

	     /// <summary>
	     /// Gets or sets the result.
	     /// </summary>
	     /// <value>The result.</value>
	     /// <remarks></remarks>
	     [DataMember(Name = "portalHeadlineListDataResult")]
	     public PortalHeadlineListDataResult Result{ get; set; }

        #endregion
    }
}