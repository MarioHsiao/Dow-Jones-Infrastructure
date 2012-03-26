// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsPagesListServicePartResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServicePartResult
{
    [DataContract(Name = "newsPagesListServicePartResult", Namespace = "")]
    public class NewsPagesListServicePartResult<TPackage> : AbstractServicePartResult<TPackage> 
        where TPackage : NewsPagesListPackage
    {
    }
}