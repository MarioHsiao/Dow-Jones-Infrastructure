// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISubPrincipal.cs" company="Dow Jones Inc.">
//   Copyright © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the ISubPrincipal type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security
{
    /// <summary>
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1606:ElementDocumentationMustHaveSummaryText", Justification = "Reviewed. Suppression is OK here.")]
    internal interface ISubPrincipal
    {
        /// <summary>
        /// Initializes the specified component.
        /// </summary>
        /// <param name="component">The component.</param>
        void Initialize(AuthorizationComponent component);
    }
}