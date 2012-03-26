// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractCompanyPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.CompanyOverview
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(CompanyChartPackage))]
    [KnownType(typeof(CompanyRecentArticlesPackage))]
    [KnownType(typeof(CompanySnapshotPackage))]
    [KnownType(typeof(CompanyTrendingPackage))]
    public class AbstractCompanyPackage : IPackage
    {
    }
}
