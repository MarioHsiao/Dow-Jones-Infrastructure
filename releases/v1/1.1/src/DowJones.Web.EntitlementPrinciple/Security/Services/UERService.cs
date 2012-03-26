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
using DowJones.Web.EntitlementPrinciple.Security.Interfaces;
using DowJones.Web.EntitlementPrinciple.Utility;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Web.EntitlementPrinciple.Security.Services
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
            IsDNB = ListUtility.FindAnyValue(_matrixUerService.ac1) != "0";
            IsDNBSnapshotOnly = ListUtility.FindAnyValue(_matrixUerService.ac1) == "1";
            IsDNBReportsWithFactivaBill = ListUtility.FindAnyValue(_matrixUerService.ac1) == "3";
            IsDNBReportsWithCreditcard = (ListUtility.FindAnyValue(_matrixUerService.ac1) == "" || ListUtility.FindAnyValue(_matrixUerService.ac1) == "2");
            if(ListUtility.FindAnyValue(_matrixUerService.ac2)!=null){
                IsSECFilings = ListUtility.FindAnyValue(_matrixUerService.ac2).ToUpper().IndexOf("SEC=1") != -1;
                IsBvd = ListUtility.FindAnyValue(_matrixUerService.ac2).ToUpper().IndexOf("BVD=1") != -1;
            }
        }

        #endregion
    }
}
