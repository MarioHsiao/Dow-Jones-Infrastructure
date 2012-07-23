// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the EmailService type.
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
    /// The email service.
    /// </summary>
    public class EmailService : AbstractService, IEmailService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixEmailService _matrixEmailService;

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
            get { return 6; }
        }

        #endregion

        public int MaximumFoldersPerEmailSetup { get; private set; }

        public bool IsNewsDigest { get; private set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="matrixEmailService"/> class.
        /// </summary>
        /// <param name="isEmailCoreServiceOn"></param>
        /// <param name="matrixEmailService"></param>
        internal EmailService(bool isEmailCoreServiceOn, MatrixEmailService matrixEmailService)
        {
            _isOn = isEmailCoreServiceOn;
            _matrixEmailService = matrixEmailService;
            Initialize();
        }

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            string emailAC3 = _matrixEmailService.ac3.FirstOrDefault(x => x.HasValue());

            if (emailAC3 != null && emailAC3.Trim().Length > 0)
            {
                MaximumFoldersPerEmailSetup = int.Parse(emailAC3.Trim());
            }

            IsNewsDigest = _matrixEmailService.ac3.ContainsAtAnyIndex("Y");
        }

        #endregion
    }
}