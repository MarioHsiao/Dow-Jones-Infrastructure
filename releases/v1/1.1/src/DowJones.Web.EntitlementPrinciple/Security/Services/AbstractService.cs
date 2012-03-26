// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Abstract class for the IService interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using DowJones.Web.EntitlementPrinciple.Security.Interfaces;

namespace DowJones.Web.EntitlementPrinciple.Security.Services
{
    /// <summary>
    /// Abstract class for the IService interface
    /// </summary>
    public abstract class AbstractService : IService
    {
        /// <summary>
        /// Gets a value indicating whether this instance has offset.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has offset; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasOffset { get; }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public abstract int Offset { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public virtual bool IsActive
        {
            get { return true; }
        }

        #region IService Members

        /// <summary>
        /// Gets a value indicating whether this instance is on.
        /// </summary>
        /// <value><c>true</c> if this instance is on; otherwise, <c>false</c>.</value>
        public abstract bool IsOn { get; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal virtual void Initialize()
        {
        }

        #endregion
    }
}