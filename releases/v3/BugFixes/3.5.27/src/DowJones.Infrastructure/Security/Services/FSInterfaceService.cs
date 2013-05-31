// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FISPrivateMarketsService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the FISPrivateMarketsService type.
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
    /// The FSInterface service.
    /// </summary>
    public class FSInterfaceService : AbstractService, IFSInterfaceService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        private readonly bool _isOffset100On;
        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixFSService _matrixFSService;

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
            get { return 108; }
        }

        #endregion

        public bool IsDowJonesConsultantUser { get; private set; }

        public bool IsWMDJAPCO { get; private set; } //Compliance Officer

        public bool IsWMDJAPAE { get; private set; } //Account Editor

        public bool IsWMDJAPFA { get; private set; } //Financial Adviser
        
        public bool IsDJIB { get; private set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="matrixFSService"/> class.
        /// </summary>
        /// <param name="isFSInterfaceCoreServiceOn"></param>
        /// <param name="isOffset100On"></param>
        /// <param name="matrixFSService"></param>
        internal FSInterfaceService(bool isFSInterfaceCoreServiceOn, bool isOffset100On, MatrixFSService matrixFSService)
        {
            _isOn = isFSInterfaceCoreServiceOn;
            _isOffset100On = isOffset100On;
            _matrixFSService = matrixFSService;
            Initialize();
        }

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            if (_isOffset100On &&
                        _matrixFSService != null &&
                        _matrixFSService.ac1.FirstOrDefault(x => x.HasValue()) != null
                        )
            {
                string fsAc1 = _matrixFSService.ac1.FirstOrDefault(x => x.HasValue()).ToUpper();

                //check if the user is Dow Jones Consultant user
                if (fsAc1 == "DJCON" || fsAc1 == "ALL")
                {
                    IsDowJonesConsultantUser = true;
                }

                //Check the access codes for the DJAP roles
                if (fsAc1 == "WMCOMPLO" || fsAc1 == "ALL")
                {
                    IsWMDJAPCO = true;
                }
                if (fsAc1 == "WMACCTED")
                {
                    IsWMDJAPAE = true;
                }
                if (fsAc1 == "WMFINAD")
                {
                    IsWMDJAPFA = true;
                }
                if (fsAc1 == "IB" || fsAc1 == "ED")
                {
                    IsDJIB = true;
                }
            }
        }

        #endregion
    }
}
