// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyOverviewNewsPageServicePartResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.CompanyOverview;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult
{
    [DataContract(Name = "companyOverviewNewsPageModuleServicePart", Namespace = "")]
    public class CompanyOverviewNewsPageServicePartResult<TPackage> : AbstractServicePartResult<TPackage>
        where TPackage : AbstractCompanyPackage
    {
    }
}
