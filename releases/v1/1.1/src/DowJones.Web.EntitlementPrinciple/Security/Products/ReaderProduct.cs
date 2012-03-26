// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReaderProduct.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the ReaderProduct type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Dow.Jones.Utility.Security.SubPrinciples;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace Dow.Jones.Utility.Security.Products
{
    public class ReaderProduct : AbstractProduct
    {
        internal ReaderProduct(RuleSet ruleSet, CoreServicesSubPrinciple coreServicesSubPrincipal, AuthorizationComponent authorizationComponent) : base(ruleSet, coreServicesSubPrincipal, authorizationComponent)
        {
        }
    }
}