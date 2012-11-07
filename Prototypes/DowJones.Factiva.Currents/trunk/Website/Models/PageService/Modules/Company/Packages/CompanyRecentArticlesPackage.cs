// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyRecentArticlesPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Ajax.PortalHeadlineList;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Company.Packages
{
    
    [DataContract(Name = "companyRecentArticlesPackage", Namespace = "")]
    public class CompanyRecentArticlesPackage : AbstractCompanyPackage, IPortalHeadlines
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
        [XmlElement(Type = typeof(PortalHeadlineListDataResult), ElementName = "portalHeadlineListDataResult", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = "")]
        public PortalHeadlineListDataResult Result
        {
            get { return headlineListDataResult ?? (headlineListDataResult = new PortalHeadlineListDataResult()); }

            set { headlineListDataResult = value; }
        }

        #endregion

        [DataMember(Name = "viewAllSearchContext")]
        public string ViewAllSearchContextRef { get; set; }
    }
}
