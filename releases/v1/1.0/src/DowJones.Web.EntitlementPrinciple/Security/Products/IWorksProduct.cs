// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWorksProduct.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the IWorksProduct type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Dow.Jones.Utility.Security.Interfaces;
using Dow.Jones.Utility.Security.SubPrinciples;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace Dow.Jones.Utility.Security.Products
{
    public class IWorksProduct : AbstractProduct, IHasAlerts
    {
        #region ..:: Constructor :: ..

        /// <summary>
        /// Initializes a new instance of the <see cref="IWorksProduct"/> class.
        /// </summary>
        /// <param name="ruleSet">The rule set.</param>
        /// <param name="coreServicesSubPrinciple">The core services sub principal.</param>
        /// <param name="authorizationComponent">The authorization component.</param>
        internal IWorksProduct(RuleSet ruleSet, CoreServicesSubPrinciple coreServicesSubPrinciple, AuthorizationComponent authorizationComponent) : base(ruleSet, coreServicesSubPrinciple, authorizationComponent)
        {
            //GetMaxNumberOfFolders = coreServicesSubPrinciple.AlertsService.GetNumberOfAlertFolders("A");
        }

        #endregion

        #region IHasAlerts Members

        /// <summary>
        /// Gets the max number of folders.
        /// </summary>
        /// <value>The max number of folders.</value>
        public int GetMaxNumberOfFolders { get; private set; }

        #endregion
    }
}