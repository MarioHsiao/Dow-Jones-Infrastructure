// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyTrendingPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Web.Mvc.Models.News;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.CompanyOverview
{
    [DataContract(Name = "companyTrendingPackage", Namespace = "")]
    public class CompanyTrendingPackage : AbstractCompanyPackage
    {
        [DataMember(Name = "result")]
        public TagCollection Result { get; protected internal set; }
    }
}
