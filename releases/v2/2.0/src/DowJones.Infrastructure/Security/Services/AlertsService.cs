// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlertsService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the AlertsService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using DowJones.Extensions;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The alerts service.
    /// </summary>
    public class AlertsService : AbstractService, IAlertsService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixTrackService _matrixTrackService;
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertsService"/> class.
        /// </summary>
        /// <param name="isTrackCoreServiceOn"></param>
        /// <param name="matrixTrackService"></param>
        internal AlertsService(bool isTrackCoreServiceOn, MatrixTrackService matrixTrackService)
        {
            _isOn = isTrackCoreServiceOn;
            _matrixTrackService = matrixTrackService;
            Initialize();
        }

        #endregion

        #region Public Properties

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
            get { return 4; }
        }

        #endregion

        public bool IsTrackAdministrator { get; private set; }

        public bool IsDMMUser { get; private set; }

        public bool IsSelectFullUser { get; private set; }

        public bool IsSelectHeadlinesUser { get; private set; }

        public bool IsAlertsUser { get; private set; }

        /// <summary>
        /// Gets MaxFoldersForGlobal.
        /// </summary>
        public int MaxFoldersForGlobal { get; private set; }

        /// <summary>
        /// Gets MaxFoldersForSelectHeadline.
        /// </summary>
        public int MaxFoldersForSelectHeadline { get; private set; }

        /// <summary>
        /// Gets the max folders for select full text.
        /// </summary>
        /// <value>The max folders for select full text.</value>
        public int MaxFoldersForSelectFullText { get; private set; }

        /// <summary>
        /// Gets the max folders for fast alert.
        /// </summary>
        /// <value>The max folders for fast alert.</value>
        public int MaxFoldersForFastAlert { get; private set; }

        /// <summary>
        /// Gets the max folders for companies and executives.
        /// </summary>
        /// <value>The max folders for companies and executives.</value>
        public int MaxFoldersForCompaniesAndExecutives { get; private set; }

        /// <summary>
        /// Gets MaxFolderForDmm.
        /// </summary>
        [Obsolete]
        public int MaxFolderForDmm { get; private set; }

        /// <summary>
        /// Gets MaxFolderForTrigger.
        /// </summary>
        [Obsolete]
        public int MaxFolderForTrigger { get; private set; }

        public int MaximumPersonalFoldersEmailDelivery { get; private set; }

        #endregion
        
        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            IsTrackAdministrator = _matrixTrackService.ac1.ContainsAtAnyIndex("-1");
            //TODO: Decide which user-type takes precedence for multiple cross values
            IsDMMUser = _matrixTrackService.ac8.ContainsAtAnyIndex("m");
            IsSelectFullUser = _matrixTrackService.ac8.ContainsAtAnyIndex("t");
            IsSelectHeadlinesUser = _matrixTrackService.ac8.ContainsAtAnyIndex("h");
            IsAlertsUser = _matrixTrackService.ac8.ContainsAtAnyIndex("f");

            MaxFoldersForGlobal = GetNumberOfAlertFolders("G");
            MaxFoldersForSelectHeadline = GetNumberOfAlertFolders("H");
            MaxFoldersForSelectFullText = GetNumberOfAlertFolders("T");
            MaxFoldersForFastAlert = GetNumberOfAlertFolders("F");
            MaxFoldersForCompaniesAndExecutives = GetNumberOfAlertFolders("P");
            MaxFolderForDmm = GetNumberOfAlertFolders("M");
            MaxFolderForTrigger = GetNumberOfAlertFolders("Y");

            var trackAc3 = _matrixTrackService.ac3.FirstOrDefault(x => x.HasValue());
            MaximumPersonalFoldersEmailDelivery = string.IsNullOrEmpty(trackAc3) ? 50 : int.Parse(trackAc3.Trim());
            
        }

        /// <summary>
        /// Gets the number of alert folders.
        /// </summary>
        /// <param name="productCode">
        /// The product code.
        /// </param>
        /// <returns>
        /// A Instance of the <see cref="int"/> class
        /// </returns>
        internal int GetNumberOfAlertFolders(string productCode)
        {
            if (!IsOn)
            {
                return 0;
            }

            string ac4 = null;
            if (_matrixTrackService != null &&
                _matrixTrackService.ac4 != null &&
                _matrixTrackService.ac4.Count == 1)
            {
                ac4 = _matrixTrackService.ac4.FirstOrDefault(x => x.HasValue());
            }

            if (ac4 == null)
            {
                ac4 = String.Empty;
            }

            if (ac4.Trim() == string.Empty)
            {
                return 25;
            }

            if (ac4.IsNumeric())
            {
                return int.Parse(ac4, CultureInfo.InvariantCulture);
            }

            // Parse ac4
            string number = null;
            var parts = ac4.Split(new[] { ';' });
            foreach (var part in parts)
            {
                if (part.IndexOf(productCode + ":") != -1)
                {
                    number = part.Split(new[] { ':' })[1];
                }
            }

            if (number == null)
            {
                number = parts[0];
            }

            return StringExtensions.IsNumeric(number) ? int.Parse(number, CultureInfo.InvariantCulture) : 0;
        }

        #endregion
    }
}