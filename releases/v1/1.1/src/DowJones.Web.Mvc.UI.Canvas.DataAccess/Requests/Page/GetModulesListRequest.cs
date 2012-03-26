// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetModulesListRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page
{
    public class GetModulesListRequest : IModuleListRequest
    {
        #region IModuleListRequest Members

        public bool IsValid()
        {
            return true;
        }

        #endregion
    }
}
