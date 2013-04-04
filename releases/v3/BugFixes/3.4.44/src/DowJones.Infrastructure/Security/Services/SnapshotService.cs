// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SnapshotService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The snapshot service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The snapshot service.
    /// </summary>
    public class SnapshotService : AbstractService, ISnapshotService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixSnapshot _matrixSnapshot;

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
            get { return 120; }
        }

        #endregion

        public bool IsCompanySnapshotOn { get; private set; }

        public bool IsIndustrySnapshotOn { get; private set; }

        public bool IsExecutiveSnapshotOn { get; private set; }

        public bool IsGovernmetSnapshotOn { get; private set; }

        public bool IsCorporateFamilyOn { get; private set; }

        public bool IsPremiumSnapshotOn { get; private set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="matrixSnapshot"/> class.
        /// </summary>
        /// <param name="isSnapshotCoreServiceOn"></param>
        /// <param name="matrixSnapshot"></param>
        internal SnapshotService(bool isSnapshotCoreServiceOn, MatrixSnapshot matrixSnapshot)
        {
            _isOn = isSnapshotCoreServiceOn;
            _matrixSnapshot = matrixSnapshot;
            Initialize();
        }

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            IsCompanySnapshotOn = HasSnapshotAccess(_matrixSnapshot.ac1.ToArray(), "CO");
            IsIndustrySnapshotOn = HasSnapshotAccess(_matrixSnapshot.ac1.ToArray(), "IN");
            IsExecutiveSnapshotOn = HasSnapshotAccess(_matrixSnapshot.ac1.ToArray(), "EXEC");
            IsGovernmetSnapshotOn = HasSnapshotAccess(_matrixSnapshot.ac1.ToArray(), "GOVT");
            IsCorporateFamilyOn = IsPremiumSnapshotOn = _matrixSnapshot.ac2.ToArray().HasAccess("COFTFULL", "ON");
        }

        private static bool HasSnapshotAccess(string[] acCol, string snapshot)
        {
            if (acCol == null || acCol.Length == 0) return true;
            var tokenStr = string.Join(";", acCol).ToUpper();
            var tokensCol = new List<string>(tokenStr.Split(';'));
            if (tokensCol.Contains("ALL=ON"))
                return true;
            if (tokensCol.Contains("ALL=OFF"))
                return false;

            return tokensCol.HasAccess(snapshot, "ON");
        }

        #endregion
    }
}