// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArchiveService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the ArchiveService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Linq;
using DowJones.Extensions;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The archive service.
    /// </summary>
    public class ArchiveService : AbstractService, IArchiveService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixArchiveService _matrixArchiveService;

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
            get { return 0; }
        }

        #endregion

        public string ArchiveAC2 { get; private set; }

        public string ArchiveAC3 { get; private set; }

        public string ArchiveAC9 { get; private set; }

        public bool IsRICForHistoricalOn { get; private set; }
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="matrixArchiveService"/> class.
        /// </summary>
        /// <param name="isArchiveCoreServiceOn"></param>
        /// <param name="matrixArchiveService"></param>
        internal ArchiveService(bool isArchiveCoreServiceOn, MatrixArchiveService matrixArchiveService)
        {
            _isOn = isArchiveCoreServiceOn;
            _matrixArchiveService = matrixArchiveService;
            Initialize();
        }

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            ArchiveAC2 = _matrixArchiveService.ac2.FirstOrDefault(x => x.HasValue());
            ArchiveAC3 = _matrixArchiveService.ac3.FirstOrDefault(x => x.HasValue());
            ArchiveAC9 = _matrixArchiveService.ac9.FirstOrDefault(x => x.HasValue());

            if (ArchiveAC9 != null && ArchiveAC9.ToUpper() == "RIC=ON")
            {
                IsRICForHistoricalOn = true;
            }
        }

        #endregion
    }
}