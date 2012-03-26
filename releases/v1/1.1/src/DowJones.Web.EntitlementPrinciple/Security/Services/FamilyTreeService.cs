﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FamilyTreeService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Family Tree Service
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using DowJones.Web.EntitlementPrinciple.Security.Interfaces;

namespace DowJones.Web.EntitlementPrinciple.Security.Services
{
    /// <summary>
    /// Family Tree Service
    /// </summary>
    public class FamilyTreeService : AbstractService, IFamilyTreeService
    {
        #region Properties

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
            get { return 128; }
        }

        #endregion

        #endregion
    }
}