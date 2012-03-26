// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DJCEProduct.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the DJCEProduct type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Dow.Jones.Utility.Security.SubPrinciples;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace Dow.Jones.Utility.Security.Products
{
    public class DJCEProduct : AbstractProduct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DJCEProduct"/> class.
        /// </summary>
        /// <param name="ruleSet">The rule set.</param>
        /// <param name="coreServicesSubPrinciple">The core services sub principal.</param>
        /// <param name="authorizationComponent">The authorization component.</param>
        internal DJCEProduct(RuleSet ruleSet, CoreServicesSubPrinciple coreServicesSubPrinciple, AuthorizationComponent authorizationComponent) 
            : base(ruleSet, coreServicesSubPrinciple, authorizationComponent)
        {
            Initialize(authorizationComponent);
        }

        public bool IsLists { get; private set; }

        public bool IsRadar { get; private set; }
        
        public bool IsResearch { get; private set; }
        
        public bool IsResearchPlus { get; private set; }
        
        public bool IsSales { get; private set; }

        /// <summary>
        /// Initializes the DJCEProduct object.
        /// </summary>
        /// <param name="service">The service.</param>
        internal void Initialize(MatrixInterfaceService service)
        {
            if (service == null || service.ac3.Count  != 1)
            {
                return;
            }

            switch (service.ac3[0].ToUpper())
            {
                case "3":
                case "4":
                    IsResearch = true;
                    break;
                case "5":
                    IsRadar = true;
                    break;                
                case "6":
                    IsLists = true;
                    break;
                case "7":
                    IsSales = true;
                    break;
                case "8":
                    IsResearchPlus = true;
                    break;
            }
        }

        /// <summary>
        /// Initializes the DJCEProduct object.
        /// </summary>
        /// <param name="component">The component.</param>
        internal void Initialize(AuthorizationComponent component)
        {
            Initialize((MatrixInterfaceService)component);
        }
    }
}
