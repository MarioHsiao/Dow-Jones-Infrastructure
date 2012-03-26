// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntitlementsPrincipal.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the EntitlementsPrinciple type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Web.EntitlementPrinciple.Security.Interfaces;
using DowJones.Web.EntitlementPrinciple.Security.SubPrinciples;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Web.EntitlementPrinciple.Security
{
    /// <summary>
    /// The entitlements principle.
    /// </summary>
    public class EntitlementsPrinciple : IPrinciple
    {
        #region Private Variables
        /// <summary>
        /// The user authorization response.
        /// </summary>
        private readonly GetUserAuthorizationsResponse _getUserAuthorizationsResponse;

        /// <summary>
        /// 
        /// </summary>
        private CoreServicesSubPrinciple _coreServices;

        /// <summary>
        /// 
        /// </summary>
        private UserSubPrinciple _userServices;

        private RuleSet _ruleSet;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitlementsPrinciple"/> class.
        /// </summary>
        /// <param name="getUserAuthorizationsResponse"></param>
        public EntitlementsPrinciple(GetUserAuthorizationsResponse getUserAuthorizationsResponse)
        {
            _getUserAuthorizationsResponse = getUserAuthorizationsResponse;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets UserServices.
        /// </summary>
        public IUserSubPrinciple UserServices
        {
            get
            {
                InitUserService();
                return _userServices;
            }
        }

        /// <summary>
        /// Gets CoreServices.
        /// </summary>
        public ICoreServicesSubPrinciple CoreServices
        {
            get
            {
                InitCoreServices();
                return _coreServices;
            }
        }

        /// <summary>
        /// Gets RuleSet.
        /// </summary>
        public IRuleSet RuleSet
        {
            get
            {
                InitRuleSet();
                return _ruleSet;
            }
        }

        #endregion

        #region Service Initialization Methods

        private void InitUserService()
        {
            if (_userServices != null)
            {
                return;
            }
            _userServices = new UserSubPrinciple(_getUserAuthorizationsResponse);
        }

        private void InitCoreServices()
        {
            if (_coreServices != null)
            {
                return;
            }
            _coreServices = new CoreServicesSubPrinciple(RuleSet, _getUserAuthorizationsResponse.AuthorizationMatrix);
        }

        private void InitRuleSet()
        {
            if (_ruleSet != null)
            {
                return;
            }
            _ruleSet = new RuleSet(_getUserAuthorizationsResponse.RuleSet);
        }

        #endregion
    }
}