// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyTrendingPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Pages;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Company.Packages
{
    [DataContract(Name = "companyTrendingPackage", Namespace = "")]
    public class CompanyTrendingPackage : AbstractCompanyPackage
    {
        [DataMember(Name = "result")]
        public TagCollection Result { get; protected internal set; }
    }
}
