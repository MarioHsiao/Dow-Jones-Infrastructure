// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsPagesListPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Models.NewsPages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "newsPagesListPackage", Namespace = "")]
    public class NewsPagesListPackage : IPackage
    {

        [DataMember(Name = "newsPages")]
        public List<NewsPage> NewsPages { get; set; }

        [DataMember(Name = "requestedNewsPage")]
        public NewsPage RequestedNewsPage { get; set; }
    }
}
