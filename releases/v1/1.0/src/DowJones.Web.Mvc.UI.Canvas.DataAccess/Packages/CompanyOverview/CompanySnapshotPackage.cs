// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanySnapshotPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Models.Company;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.CompanyOverview
{
    [DataContract(Name = "companySnapshotPackage", Namespace = "")]
    public class CompanySnapshotPackage : AbstractCompanyPackage
    {
        [DataMember(Name = "quote")]
        public Quote Quote { get; protected internal set; }

        [DataMember(Name = "executives")]
        public Models.ExecutiveCollection Executives { get; protected internal set; }

        [DataMember(Name = "zacksReports")]
        public PortalHeadlineListDataResult ZacksReports { get; protected internal set; }

        [DataMember(Name = "dataMonitorReports")]
        public PortalHeadlineListDataResult DataMonitorReports { get; protected internal set; }

        [DataMember(Name = "investextReports")]
        public PortalHeadlineListDataResult InvestextReports { get; protected internal set; }
    }
}
