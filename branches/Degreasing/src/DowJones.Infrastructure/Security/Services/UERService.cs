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
using System.Linq;
using DowJones.Extensions;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The UER service.
    /// </summary>
    public class UERService : AbstractService, IUERService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixUerService _matrixUerService;

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
            get { return 20; }
        }

        #endregion

        public bool IsDNB { get; private set; }

        public bool IsDNBSnapshotOnly { get; private set; }

        public bool IsDNBReportsWithFactivaBill { get; private set; }

        public bool IsDNBReportsWithCreditcard { get; private set; }

        public bool IsSECFilings { get; private set; }

        public bool IsBvd { get; private set; }
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="matrixSnapshot"/> class.
        /// </summary>
        /// <param name="isSnapshotCoreServiceOn"></param>
        /// <param name="matrixSnapshot"></param>
        internal UERService(bool IsUerCoreServiceOn, MatrixUerService matrixUerService)
        {
            _isOn = IsUerCoreServiceOn ;
            _matrixUerService = matrixUerService;
            Initialize();
        }

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            IsDNB = _matrixUerService.ac1.FirstOrDefault(x => x.HasValue()) != "0";
            IsDNBSnapshotOnly = _matrixUerService.ac1.FirstOrDefault(x => x.HasValue()) == "1";
            IsDNBReportsWithFactivaBill = _matrixUerService.ac1.FirstOrDefault(x => x.HasValue()) == "3";
            IsDNBReportsWithCreditcard = (_matrixUerService.ac1.FirstOrDefault(x => x.HasValue()) == "" || _matrixUerService.ac1.FirstOrDefault(x => x.HasValue()) == "2");
            if(_matrixUerService.ac2.FirstOrDefault(x => x.HasValue())!=null){
                IsSECFilings = _matrixUerService.ac2.FirstOrDefault(x => x.HasValue()).ToUpper().IndexOf("SEC=1") != -1;
                IsBvd = _matrixUerService.ac2.FirstOrDefault(x => x.HasValue()).ToUpper().IndexOf("BVD=1") != -1;
            }
        }

        #endregion
    }
}
