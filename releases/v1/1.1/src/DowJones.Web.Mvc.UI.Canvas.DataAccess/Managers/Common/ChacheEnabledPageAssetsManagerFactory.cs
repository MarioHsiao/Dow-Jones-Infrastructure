// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChacheEnabledPageAssetsManagerFactory.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Managers.PAM;
using DowJones.Session;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common
{
    public class ChacheEnabledPageAssetsManagerFactory : Interfaces.IChacheEnabledPageAssetsManagerFactory
    {
        public virtual IPageAssetsManager CreateManager()
        {
            throw new NotImplementedException();
        }

        public IPageAssetsManager CreateManager(ControlData controlData, string interfaceLanguage)
        {
            return new PageListManager(controlData, new BasePreferences(interfaceLanguage));
        }
    }
}
