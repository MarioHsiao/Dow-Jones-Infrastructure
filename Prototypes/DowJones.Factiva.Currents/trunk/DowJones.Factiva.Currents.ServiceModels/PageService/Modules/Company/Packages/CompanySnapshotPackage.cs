// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanySnapshotPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Models.Company;
using DowJones.Pages.Executive;

//using DowJones.Pages.Company;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Company.Packages
{
    [DataContract(Name = "companySnapshotPackage", Namespace = "")]
    public class CompanySnapshotPackage : AbstractCompanyPackage
    {
        [DataMember(Name = "quote")]
        public Quote Quote { get; protected internal set; }

        [DataMember(Name = "executives")]
        public ExecutiveCollection Executives { get; protected internal set; }

        [DataMember(Name = "dataMonitorReports")]
        public PortalHeadlineListDataResult DataMonitorReports { get; protected internal set; }

        [DataMember(Name = "investextReports")]
        public PortalHeadlineListDataResult InvestextReports { get; protected internal set; }
    }
}
