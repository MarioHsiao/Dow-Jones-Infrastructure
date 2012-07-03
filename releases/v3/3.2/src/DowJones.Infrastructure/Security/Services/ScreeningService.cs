// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreeningService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The screening service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The screening service.
    /// </summary>
    public class ScreeningService : AbstractService, IScreeningService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixScreening _matrixScreening;

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
            //TODO: Verify offset
            get { return 130; }
        }

        #endregion

        public bool IsCompanyScreeningOn { get; private set; }

        public bool IsRadarOn { get; private set; }

        public bool IsExtendedScreeningOn { get; private set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="matrixScreening"/> class.
        /// </summary>
        /// <param name="isScreeningCoreServiceOn"></param>
        /// <param name="matrixScreening"></param>
        internal ScreeningService(bool isScreeningCoreServiceOn, MatrixScreening matrixScreening)
        {
            _isOn = isScreeningCoreServiceOn;
            _matrixScreening = matrixScreening;
            Initialize();
        }

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            IsCompanyScreeningOn = HasScreeningAccess(_matrixScreening.ac1.ToArray(), "CO");
            IsRadarOn = _matrixScreening.ac2.ToArray().HasAccess("RADAR", "ON");
            IsExtendedScreeningOn = _matrixScreening.ac3.ToArray().HasAccess("UNIVERSE", "FULL");
        }

        private static bool HasScreeningAccess(string[] acCol, string screening)
        {
            if (acCol == null || acCol.Length == 0) return true;
            var tokenStr = string.Join(";", acCol).ToUpper();
            var tokensCol = new List<string>(tokenStr.Split(';'));
            return tokensCol.HasAccess(screening, "FULL") || tokensCol.HasAccess(screening, "BASIC");
        }

        #endregion
    }
}