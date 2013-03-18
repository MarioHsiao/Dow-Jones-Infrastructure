// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AffiliationService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Affiliation Service
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using DowJones.Security.Interfaces;

namespace DowJones.Security.Services
{
    /// <summary>
    /// Affiliation Service 
    /// </summary>
    public class AffiliationService : AbstractService, IAffiliationService
    {
        #region Overrides of AbstractService

        /// <summary>
        /// Gets a value indicating whether this instance is on.
        /// </summary>
        /// <value><c>true</c> if this instance is on; otherwise, <c>false</c>.</value>
        public override bool IsOn
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has offset.
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
            get { return 124; }
        }

        #endregion
    }
}