// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChacheEnabledPageAssetsManagerFactory.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Managers.PAM;
using Factiva.Gateway.Utils.V1_0;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    public interface IChacheEnabledPageAssetsManagerFactory : IPageAssetsManagerFactory
    {
        IPageAssetsManager CreateManager(ControlData controlData, string interfaceLanguage);
    }
}
