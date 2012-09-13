﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericDataService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The numeric data service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using DowJones.Security.Interfaces;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The numeric data service.
    /// </summary>
    public class NumericDataService : AbstractService, INumericDataService
    {
        #region Overrides of AbstractService

        /// <summary>
        /// Gets a value indicating whether IsOn.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override bool IsOn
        {
            get { return false; }
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
            get { return 114; }
        }

        /// <summary>
        /// Gets a value indicating whether IsActive.
        /// </summary>
        public override sealed bool IsActive
        {
            get { return false; }
        }

        #endregion
    }
}