// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentationProduct.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the WealthManagementProduct type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Dow.Jones.Utility.Security.SubPrinciples;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using EnumThirdPartyServices = Dow.Jones.Utility.Security.Products.ThirdPartyServices;

namespace Dow.Jones.Utility.Security.Products
{
    /// <summary>
    /// Wealth Management Product
    /// </summary>
    public class SegmentationProduct : AbstractProduct
    {
        private readonly List<ThirdPartyServices> thirdPartyServices = new List<ThirdPartyServices>();
        
        internal SegmentationProduct(RuleSet ruleSet, CoreServicesSubPrinciple coreServicesSubPrinciple, AuthorizationComponent authorizationComponent) : base(ruleSet, coreServicesSubPrinciple, authorizationComponent)
        {
        }

        /// <summary>
        /// Gets value indicating whether this instance has access to all products.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has access to all products; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccessToAllProducts { get; private set; }

        public bool HasNewsPlus { get; private set; }

        public bool HasWeathManager { get; private set; }

        public bool HasGlobalEquities { get; private set; }

        public bool HasGlobalMarkets { get; private set; }

        public bool HasNorthAmericanEquities { get; private set; }

        public bool HasEnergy { get; private set; }
        
        public bool IsNewsPlusGlobalEquities { get; private set; }

        public bool IsNewsPlusGlobalMarkets { get; private set; }

        public bool IsNewsPlusCapitalMarketsReports { get; private set; }

        public bool IsNewsPlusEnergy { get; private set; }

        public bool IsNewsPlusAllDow { get; private set; }

        public bool IsNewsPlusNorthAmericanEquities { get; private set; }

        public bool IsWealthMngrMrktsAndInvst { get; private set; }

        public bool IsWealthMngrAdvInst { get; private set; }

        public bool IsWealthMngrCmpsAndExecs { get; private set; }

        public bool IsWealthMngrMrktsAndInvstWthAdvInst { get; private set; }

        public bool IsWealthMngrMrktsAndInvstWthCmpsAndExecs { get; private set; }

        public bool IsWealthMngrAdvInstWthCmpsAndExecs { get; private set; }

        public bool IsWealthMngrMrktsAndInvstWthAdvInstAndCompaniesExecutives { get; private set; }

        public bool IsWealthMngrMrktsAndInvstWthFactCont { get; private set; }

        public bool IsWealthMngrAdvInstWthFactCont { get; private set; }

        public bool IsWealthMngrCmpsAndExecsWthFactCont { get; private set; }

        public bool IsWealthMngrMrktsAndInvstWthAdvInstAndFactContent { get; private set; }

        public bool IsWealthMngrMrktsAndInvstWthCmpsAndExecsAndFactContent { get; private set; }

        public bool IsWealthMngrAdvInstWthCmpsAndExecsAndFactContent { get; private set; }

        public bool HasWealthMngrWthAllOptns { get; private set; }

        public bool IsInvestmentBanker { get; private set; }

        public bool IsEditor { get; private set; }

        public bool IsDeveloper { get; private set; }

        public List<ThirdPartyServices> ThirdPartyServices
        {
            get { return thirdPartyServices; }
        }

        /// <summary>
        /// Initializes the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        internal void Initialize(MatrixFSService service)
        {
            if (service == null || service.ac1 == null || service.ac1.Count != 1)
            {
                return;
            }

            if (service.ac2 != null && service.ac2.Count == 1)
            {
                InitializeAC2(service.ac2[0]);
            }

            if (service.ac3 != null && service.ac3.Count == 1)
            {
                // Check to see if developer
                switch (service.ac3[0].ToUpper())
                {
                    case "DEV":
                        IsDeveloper = true;
                        break;
                }
            }

            if (string.IsNullOrEmpty(service.ac1[0]))
            {
                return;
            }

            //// Check for product type
            switch (service.ac1[0].ToUpper())
            {
                case "ALL":
                    IsNewsPlusAllDow = true;
                    HasWealthMngrWthAllOptns = true;
                    IsInvestmentBanker = true;
                    HasAccessToAllProducts = true;
                    break;
                case "NPGE":
                    IsNewsPlusGlobalEquities = true;
                    HasNewsPlus = true;
                    break;
                case "NPGM":
                    IsNewsPlusGlobalMarkets = true;
                    HasNewsPlus = true;
                    break;
                case "NPCMR":
                    IsNewsPlusCapitalMarketsReports = true;
                    HasNewsPlus = true;
                    break;
                case "NPNRG":
                    HasNewsPlus = true;
                    IsNewsPlusEnergy = true;
                    break;
                case "NPALL":
                    HasNewsPlus = true;
                    IsNewsPlusAllDow = true;
                    break;
                case "NPNAE":
                    HasNewsPlus = true;
                    IsNewsPlusNorthAmericanEquities = true;
                    break;
                case "IB":
                    IsInvestmentBanker = true;
                    break;
                case "ED":
                    IsEditor = true;
                    HasWealthMngrWthAllOptns = true;
                    IsInvestmentBanker = true;
                    break;
                case "WMM":
                    IsWealthMngrMrktsAndInvst = true;
                    break;
                case "WMA":
                    IsWealthMngrAdvInst = true;
                    break;
                case "WMC":
                    IsWealthMngrCmpsAndExecs = true;
                    break;
                case "WMMA":
                    IsWealthMngrMrktsAndInvstWthAdvInst = true;
                    break;
                case "WMMC":
                    IsWealthMngrMrktsAndInvstWthCmpsAndExecs = true;
                    break;
                case "WMAC":
                    IsWealthMngrAdvInstWthCmpsAndExecs = true;
                    break;
                case "WMMAC":
                    IsWealthMngrMrktsAndInvstWthAdvInstAndCompaniesExecutives = true;
                    break;
                case "WMMF":
                    IsWealthMngrMrktsAndInvstWthFactCont = true;
                    break;
                case "WMAF":
                    IsWealthMngrAdvInstWthFactCont = true;
                    break;
                case "WMCF":
                    IsWealthMngrCmpsAndExecsWthFactCont = true;
                    break;
                case "WMMAF":
                    IsWealthMngrMrktsAndInvstWthAdvInstAndFactContent = true;
                    break;
                case "WMMCF":
                    IsWealthMngrMrktsAndInvstWthCmpsAndExecsAndFactContent = true;
                    break;
                case "WMACF":
                    IsWealthMngrAdvInstWthCmpsAndExecsAndFactContent = true;
                    break;
                case "WMMACF":
                    HasWealthMngrWthAllOptns = true;
                    break;
            }
        }

        /// <summary>
        /// Initializes the specified component.
        /// </summary>
        /// <param name="component">The component.</param>
        internal void Initialize(AuthorizationComponent component)
        {
            Initialize((MatrixFSService)component);
        }

        private void InitializeAC2(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                return;
            }

            var vals = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var s in vals)
            {
                switch (s.ToUpper())
                {
                    case "TRIB":
                        thirdPartyServices.Add(EnumThirdPartyServices.TributesDotCom);
                        break;
                    case "NOZA":
                        thirdPartyServices.Add(EnumThirdPartyServices.NozaCharitableData);
                        break;
                    case "MARK":
                        thirdPartyServices.Add(EnumThirdPartyServices.MarketData);
                        break;
                }
            }
        }
    }
}