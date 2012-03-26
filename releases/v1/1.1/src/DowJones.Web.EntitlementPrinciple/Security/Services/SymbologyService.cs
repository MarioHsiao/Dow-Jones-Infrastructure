// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SymbologyService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The symbology service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Web.EntitlementPrinciple.Security.Interfaces;

namespace DowJones.Web.EntitlementPrinciple.Security.Services
{
    /// <summary>
    /// The symbology service.
    /// </summary>
    public class SymbologyService : AbstractService, ISymbologyService
    {
        #region Overrides of AbstractService

        /// <summary>
        /// Gets a value indicating whether IsOn.
        /// </summary>
        /// <value><c>true</c> if this instance is on; otherwise, <c>false</c>.</value>
        public override bool IsOn
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether HasOffset.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has offset; otherwise, <c>false</c>.
        /// </value>
        public override bool HasOffset
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public override int Offset
        {
            get { return 14; }
        }

        #endregion
    }
}