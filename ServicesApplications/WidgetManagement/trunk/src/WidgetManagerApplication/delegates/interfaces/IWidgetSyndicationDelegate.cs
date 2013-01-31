// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWidgetSyndicationDelegate.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Interface that all output Delegates must adhere too
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using EMG.widgets.ui.dto;

namespace EMG.widgets.ui.delegates.interfaces
{
    /// <summary>
    /// Interface that all output Delegates must adhere too
    /// </summary>
    public interface IWidgetSyndicationDelegate
    {
        /// <summary>
        /// Deserializes to RSS.
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1616:ElementReturnValueDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here.")]
        string ToRSS();

        /// <summary>
        /// Deserializes to ATOM.
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1616:ElementReturnValueDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here.")]
        string ToATOM();

        /// <summary>
        /// Deserializes to JSON.
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1616:ElementReturnValueDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here.")]
        string ToJSON();

        /// <summary>
        /// Deserializes to XML.
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1616:ElementReturnValueDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here.")]
        string ToXML();

        /// <summary>
        /// Deserializes to JSONP.
        /// </summary>
        /// <param name="callback">The callback string.</param>
        /// <param name="args">The arguments params.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1616:ElementReturnValueDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here.")]
        string ToJSONP(string callback, params string[] args);

        /// <summary>
        /// Fills this instance. This is a contract for Command pattern.
        /// </summary>
        /// <param name="strToken">The widget token.</param>
        /// <param name="integrationTarget">The integration target.</param>
        void Fill(string strToken, IntegrationTarget integrationTarget);
    }
}