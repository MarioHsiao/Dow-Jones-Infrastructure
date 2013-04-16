// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlatformAssetManagementService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The platform asset management service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using DowJones.Extensions;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The platform asset management service.
    /// </summary>
    public class PlatformAssetManagementService : AbstractService, IPlatformAssetManagementService
    {
        #region Private Variables

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixPAMService _matrixPAMService;

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        #endregion

        #region Constructor(s)

        //TODO: replace with MatrixPAMService
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixPAMService"/> class.
        /// </summary>
        /// <param name="isPAMServiceOn"></param>
        /// <param name="matrixPAMService"></param>
        internal PlatformAssetManagementService(bool isPAMServiceOn, MatrixPAMService matrixPAMService)
        {
            _isOn = isPAMServiceOn;
            _matrixPAMService = matrixPAMService;
            Initialize();
        }

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
            get { return 116; }
        }

        #endregion

        public bool HasNewspages { get; private set; }

        public bool IsNewspagesSubscribeOnly { get; private set; }

        public int NumberOfPersonalNewspages { get; private set; }

        public bool IsMctUser { get; private set; }

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            //TODO: Verify these logic
            if (_matrixPAMService == null)
            {
                return;
            }

            //AC's
            NumberOfPersonalNewspages = _matrixPAMService.ac1.MatchPattern(@"newspage:([0-9\-]+)", 1);
            if(!(NumberOfPersonalNewspages < 0) && NumberOfPersonalNewspages < 1)
            {
                IsNewspagesSubscribeOnly = _matrixPAMService.ac1.ContainsAtAnyIndex("newspage:0");
            }

            if (IsNewspagesSubscribeOnly || NumberOfPersonalNewspages > 0)
            {
                HasNewspages = true;
            }
            
            //DA's
        }

        #endregion
    }
}

