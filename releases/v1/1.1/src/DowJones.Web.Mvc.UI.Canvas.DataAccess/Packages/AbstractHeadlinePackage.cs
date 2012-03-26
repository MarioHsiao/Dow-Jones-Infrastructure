// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractHeadlinePackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
     [DataContract(Namespace="")]
    public abstract class AbstractHeadlinePackage : IPackage, IPortalHeadlines
    {
        [EditorBrowsable(EditorBrowsableState.Never)] 
        private PortalHeadlineListDataResult headlineListDataResult;

        #region IPortalHeadlines Members

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        /// <remarks></remarks>
        [DataMember(Name = "portalHeadlineListDataResult")]
        [XmlElement(Type = typeof(PortalHeadlineListDataResult), 
            ElementName = "portalHeadlineListDataResult", 
            IsNullable = false, 
            Form = XmlSchemaForm.Qualified, 
            Namespace = "")]
        public PortalHeadlineListDataResult Result
        {
            get { return headlineListDataResult ?? (headlineListDataResult = new PortalHeadlineListDataResult()); }
            set { headlineListDataResult = value; }
        }

        #endregion
    }
}