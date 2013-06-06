// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CIBsService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the CIBService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Extensions;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The cib service.
    /// </summary>
    public class CIBsService : AbstractService, ICIBsService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixCibsService _matrixCibsService;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="CIBsService"/> class.
        /// </summary>
        /// <param name="IsCibsCoreServiceOn">The rule set.</param>
        /// <param name="matrixCibsService">The authorization matrix.</param>
        internal CIBsService(bool IsCibsCoreServiceOn, MatrixCibsService matrixCibsService)
        {
            _isOn = IsCibsCoreServiceOn;
            _matrixCibsService = matrixCibsService;
            Initialize();
        }

        #endregion

        #region Properties 

        #region Overrides of AbstractService

        /// <summary>
        /// Gets a value indicating whether IsOn.
        /// </summary>
        /// <value><c>true</c> if this instance is on; otherwise, <c>false</c>.</value>
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
            get { return 2; }
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether this instance has access to usage reports.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has access to usage reports; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccessToUsageReports { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has access to other users usage reports.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has access to other users usage reports; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccessToOtherUsersUsageReports { get; private set; }

        public bool IsReaderXUser { get; private set; }
        
        #endregion

        internal override sealed void Initialize()
        {
            if (_matrixCibsService != null)
            {
                if (_matrixCibsService.ac1 != null &&
                    _matrixCibsService.ac1.Count > 0)
                {
                    HasAccessToUsageReports = !_matrixCibsService.ac1.ContainsAtAnyIndex("N");
                }

                if (_matrixCibsService.ac2 != null &&
                    _matrixCibsService.ac2.Count > 0)
                {
                    HasAccessToOtherUsersUsageReports = _matrixCibsService.ac2.ContainsAtAnyIndex("Y");
                }

                if (_matrixCibsService.ac3 != null && _matrixCibsService.ac3.Count > 0)
                {
                    IsReaderXUser = _matrixCibsService.ac3.ContainsAtAnyIndex("RM")
                        || _matrixCibsService.ac3.ContainsAtAnyIndex("XM");
                }
            }
        }
    }
}