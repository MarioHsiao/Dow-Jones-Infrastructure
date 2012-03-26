// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentationSubService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the ThirdPartyServices type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Dow.Jones.Utility.Security;
using Dow.Jones.Utility.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace EMG.Utility.Security
{
    public enum ThirdPartyServices
    {
        /// <summary>
        /// tributes.com data 
        /// </summary>
        TributesDotCom,

        /// <summary>
        /// Noza Charitable Data
        /// </summary>
        NozaCharitableData,
        
        /// <summary>
        /// Market Data
        /// </summary>
        MarketData,
    }

    public class SegmentationSubService : ISubService
    {
        private readonly List<ThirdPartyServices> thirdPartyServices = new List<ThirdPartyServices>();

        #region Constructor(s)

        internal SegmentationSubService()
        {
        }

        internal SegmentationSubService(MatrixFSService service)
        {
            Initialize(service);
        }

        #endregion

        public bool HasAccessToAllProducts { get; private set; }
        
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

        public bool IsWealthMngrWthAllOptns { get; private set; }

        public bool IsInvestmentBanker { get; private set; }

        public bool IsSegmentationEditor { get; private set; }

        public bool IsDeveloper { get; private set; }

        public List<ThirdPartyServices> ThirdPartyServices
        {
            get { return thirdPartyServices; }
        }

        #region ISubPrincipal Members

        public void Initialize(AuthorizationComponent component)
        {
            Initialize((MatrixFSService)component);
        }
        
        #endregion

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

            if (service.ac3 != null && service.ac3.Count > 0)
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
            
            // Check for product type
            switch (service.ac1[0].ToUpper())
            {
                case "ALL":
                    IsNewsPlusAllDow = true;
                    IsWealthMngrWthAllOptns = true;
                    IsInvestmentBanker = true;
                    HasAccessToAllProducts = true;
                    break;
                case "NPGE":
                    IsNewsPlusGlobalEquities = true;
                    break;
                case "NPGM":
                    IsNewsPlusGlobalMarkets = true;
                    break;
                case "NPCMR":
                    IsNewsPlusCapitalMarketsReports = true;
                    break;
                case "NPNRG":
                    IsNewsPlusEnergy = true;
                    break;
                case "NPALL":
                    IsNewsPlusAllDow = true;
                    break;
                case "NPNAE":
                    IsNewsPlusNorthAmericanEquities = true;
                    break;
                case "IB":
                    IsInvestmentBanker = true;
                    break;
                case "ED":
                    IsSegmentationEditor = true;
                    IsWealthMngrWthAllOptns = true;
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
                    IsWealthMngrWthAllOptns = true;
                    break;
            }
        }

        private void InitializeAC2(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                return;
            }

            var vals = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in vals)
            {
                switch (s.ToUpper())
                {
                    case "TRIB":
                        thirdPartyServices.Add(Security.ThirdPartyServices.TributesDotCom);
                        break;
                    case "NOZA":
                        thirdPartyServices.Add(Security.ThirdPartyServices.NozaCharitableData);
                        break;
                    case "MARK":
                        thirdPartyServices.Add(Security.ThirdPartyServices.MarketData);
                        break;
                }
            }
        }
    }
}
