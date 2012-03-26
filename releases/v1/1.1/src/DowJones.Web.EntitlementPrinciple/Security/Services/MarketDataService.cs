// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarketDataService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The market data service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using DowJones.Web.EntitlementPrinciple.Security.Interfaces;
using DowJones.Web.EntitlementPrinciple.Utility;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Web.EntitlementPrinciple.Security.Services
{
    /// <summary>
    /// The market data service.
    /// </summary>
    public class MarketDataService : AbstractService, IMarketDataService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixMdsService _matrixMdsService;

        #endregion

        #region Public Properties

        #region Overrides of AbstractService

        /// <summary>
        /// Gets a value indicating whether IsOn.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override bool IsOn
        {
            get { return _isOn; }
        }

        /// <summary>
        /// Gets a value indicating whether HasOffset.
        /// </summary>
        public override bool HasOffset
        {
            get { return true; }
        }

        /// <summary>
        /// Gets Offset.
        /// </summary>
        public override int Offset
        {
            get { return 10; }
        }

        #endregion

        public bool IsCurrentQuote { get; private set; }
        
        public bool IsQuickQuote { get; private set; }
        
        public bool IsFullQuote { get; private set; }

        public bool IsHistoricalQuote { get; private set; }

        public bool IsTreadlineQuote { get; private set; }

        public bool IsReuterInvestorQuote { get; private set; }

        public bool IsBackgroundDataFromReuters { get; private set; }

        #endregion

         #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="matrixMdsService"/> class.
        /// </summary>
        /// <param name="isMdsCoreServiceOn"></param>
        /// <param name="matrixMdsService"></param>
        internal MarketDataService(bool isMdsCoreServiceOn, MatrixMdsService matrixMdsService)
        {
            _isOn = isMdsCoreServiceOn;
            _matrixMdsService = matrixMdsService;
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            IsCurrentQuote = ListUtility.FindAnyValue(_matrixMdsService.ac1).ToUpper() != "OFF";
            IsQuickQuote = ListUtility.FindInList(_matrixMdsService.ac1, "QUICK");
            IsFullQuote = ListUtility.FindInList(_matrixMdsService.ac1, "FULL");
            IsHistoricalQuote = ListUtility.FindAnyValue(_matrixMdsService.ac2).ToUpper() != "OFF";

            if (ListUtility.FindAnyValue(_matrixMdsService.ac2).ToUpper().IndexOf("TLINE") != -1)
            {
                IsTreadlineQuote = true;
                IsReuterInvestorQuote = true;
            }
            else if (ListUtility.FindAnyValue(_matrixMdsService.ac2).ToUpper().IndexOf("RIVST") != -1)
            {
                IsReuterInvestorQuote = true;
            }
            IsBackgroundDataFromReuters = ListUtility.FindAnyValue(_matrixMdsService.ac3).ToUpper() != "OFF";
        }
        #endregion
    }
}