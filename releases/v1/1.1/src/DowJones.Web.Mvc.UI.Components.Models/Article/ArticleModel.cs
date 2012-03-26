// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleModel.cs" company="Dow Jones">
//    © 2010 Dow Jones, Inc. All rights reserved. 	
// </copyright>
// <summary>
//   The article model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using DowJones.Utilities.Uri;
using DowJones.Infrastructure;
using DowJones.Session;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers;
using DowJones.Utilities.DTO.Web.Request;
using DowJones.Web.Mvc.UI.Components.ArticleTranslator;
using DowJones.Web.Mvc.UI.Components.SocialButtons;
using DowJones.Web.Mvc.UI.Components.Article;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Utils.V1_0;
using MessagesArticle = Factiva.Gateway.Messages.Archive.V2_0.Article;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using DowJones.Utilities.Managers.Search;
using DowJones.Utilities.Managers.Search.Responses;
using DowJones.Ajax.Article;

namespace DowJones.Web.Mvc.UI.Components.Article
{
    /// <summary>
    /// The article model.
    /// </summary>
    public class ArticleModel : ViewComponentModel
    {
        #region ..:: Public Properties ::..
        
        #region ..:: Server-side Only Properties ::..
        
        /// <summary>
        /// Gets or sets the control data.
        /// </summary>
        /// <value>The control data.</value>
        public ControlData ControlData { get; private set; }

        /// <summary>
        /// Gets or sets the IPreferences.
        /// </summary>
        /// <value>The preferences.</value>
        public IPreferences Preferences { get; private set; }

        /// <summary>
        /// Gets or sets the interface language.
        /// </summary>
        /// <value>The interface language.</value>
        public string InterfaceLanguage { get; private set; }
        
        /// <summary>
        /// Gets or sets ImageType.
        /// </summary>
        public ImageType ImageType { get; set; }

        /// <summary>
        /// Gets or sets CanonicalSearchString.
        /// </summary>
        public string CanonicalSearchString { get; set; }

        /// <summary>
        /// Gets or sets ContentItems.
        /// </summary>
        public ContentItems ContentItems { get; set; }

        /// <summary>
        /// Gets or sets PostProcessing.
        /// </summary>
        public PostProcessing PostProcessing { get; set; }

        #endregion

        #region ..:: Client Events ::..

        [ClientEventHandler]
        public string OnEntityClick { get; set; }

        #endregion

        #region ..:: Client Tokens ::..

        [ClientTokens]
        public ArticleTokens Tokens { get; set; }

        #endregion

        #region ..:: Client Properties ::..

        [ClientProperty]
        public bool ShowSourceLinks { get; set; }

        [ClientProperty]
        public bool EnableELinks { get; set; }

        [ClientProperty]
        public bool ShowCompanyEntityReference { get; set; }

        [ClientProperty]
        public bool ShowExecutiveEntityReference { get; set; }

        [ClientProperty]
        public bool ShowHighlighting { get; set; }

        [ClientProperty]
        public bool ShowReadSpeaker { get; set; }

        [ClientProperty]
        public bool ShowSocialButtons { get; set; }

        [ClientProperty]
        public bool ShowTranslator { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// Gets or sets ArticleObject.
        /// </summary>
        public MessagesArticle ArticleObject { get; set; }

        public ArticleTranslatorModel TranslatorModel { get; set; }

        public SocialButtonsModel SocialButtonModel { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleModel"/> class.
        /// </summary>
        public ArticleModel(ControlData controlData, IPreferences preferences)
        {
            Guard.IsNotNull(controlData, "controlData");
            Guard.IsNotNull(preferences, "preferences");
            ControlData = controlData;
            Preferences = preferences;
            InterfaceLanguage = preferences.InterfaceLanguage;
            
            Tokens = new ArticleTokens();
            
        }

        public ArticleResultset ArticleDataSet { get; set; }

   }
}