// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractProduct.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the AbstractProduct type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Dow.Jones.Utility.Security.Interfaces;
using Dow.Jones.Utility.Security.SubPrinciples;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace Dow.Jones.Utility.Security.Products
{
    /// <summary>
    /// Abstract Product class 
    /// </summary>
    public class AbstractProduct : IProduct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractProduct"/> class.
        /// </summary>
        /// <param name="ruleSet">The rule set.</param>
        /// <param name="coreServicesSubPrinciple">The core services sub principal.</param>
        /// <param name="authorizationComponent">The authorization component.</param>
        internal AbstractProduct(RuleSet ruleSet, CoreServicesSubPrinciple coreServicesSubPrinciple, AuthorizationComponent authorizationComponent)
        {
            GetRuleSet = ruleSet;
            GetCoreServicesSubPrinciple = coreServicesSubPrinciple;
            GetAuthorizationComponent = authorizationComponent;
        }

        /// <summary>
        /// Gets the rule set.
        /// </summary>
        /// <value>The rule set.</value>
        internal RuleSet GetRuleSet { get; private set; }

        /// <summary>
        /// Gets the core services subprincipal.
        /// </summary>
        /// <value>The get core services sub principal.</value>
        internal CoreServicesSubPrinciple GetCoreServicesSubPrinciple { get; private set; }

        /// <summary>
        /// Gets the authorization component.
        /// </summary>
        /// <value>The authorization component.</value>
        internal AuthorizationComponent GetAuthorizationComponent { get; private set; }
    }
}