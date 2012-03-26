// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The search service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using DowJones.Web.EntitlementPrinciple.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Web.EntitlementPrinciple.Security.Services
{
    /// <summary>
    /// The search service.
    /// </summary>
    public class SearchService : AbstractService, ISearchService
    {

        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixIndexService _matrixIndexService;

        #endregion

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
            get { return 8; }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertsService"/> class.
        /// </summary>
        /// <param name="isIndexCoreServiceOn"></param>
        /// <param name="matrixIndexService"></param>
        internal SearchService(bool isIndexCoreServiceOn, MatrixIndexService matrixIndexService)
        {
            _isOn = isIndexCoreServiceOn;
            _matrixIndexService = matrixIndexService;
            Initialize();
        }

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
           
        }

        #endregion

    }
}