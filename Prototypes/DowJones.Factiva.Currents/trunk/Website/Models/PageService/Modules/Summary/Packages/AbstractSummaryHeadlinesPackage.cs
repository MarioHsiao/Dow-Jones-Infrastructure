using System.ComponentModel;
using System.Runtime.Serialization;
using DowJones.Ajax.PortalHeadlineList;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Summary.Packages
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(SummaryVideosPackage))]
    [KnownType(typeof(SummaryRecentArticlesPackage))]
    public abstract class AbstractSummaryHeadlinesPackage : AbstractSummaryPackage, IPortalHeadlines
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
        public PortalHeadlineListDataResult Result
        {
            get { return headlineListDataResult ?? (headlineListDataResult = new PortalHeadlineListDataResult()); }

            set { headlineListDataResult = value; }
        }

        #endregion
    }
}