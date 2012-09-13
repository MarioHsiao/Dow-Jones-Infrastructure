﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleTranslatorTokens.cs" company="Dow Jones">
//    © 2010 Dow Jones, Inc. All rights reserved. 	
// </copyright>
// <summary>
//   The article tokens.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System.Text;
using DowJones.Token;

namespace DowJones.Web.Mvc.UI.Components.ArticleTranslator
{
    public class ArticleTranslatorTokens : AbstractTokenBase
    {
        /// <summary>
        /// token prefix
        /// </summary>
        private const string tokenPrefix = "lang";

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleTokens"/> class.
        /// </summary>
        public ArticleTranslatorTokens()
        {
            Italian = GetTokenByName("langIt");
            English = GetTokenByName("langEn");
            German = GetTokenByName("langDe");
            ChineseSimplified = GetTokenByName("langZhcn");
            ChineseTraditional = GetTokenByName("langZhtw");
            French = GetTokenByName("langFr");
            Spanish = GetTokenByName("langEs");
            Russian = GetTokenByName("langRu");
            Japanese = GetTokenByName("langJa");
            ArticleTranslatorTitle = GetTokenByName("articleTranslatorTitle");
        }

        #region << Accessors >>

        /// <summary>
        /// Gets or sets the italian.
        /// </summary>
        /// <value>The italian.</value>
        public string Italian { get; set; }


        /// <summary>
        /// Gets or sets the english.
        /// </summary>
        /// <value>The english.</value>
        public string English { get; set; }


        /// <summary>
        /// Gets or sets the german.
        /// </summary>
        /// <value>The german.</value>
        public string German { get; set; }

        /// <summary>
        /// Gets or sets Language.
        /// </summary>
        public string ChineseSimplified { get; set; }

        /// <summary>
        /// Gets or sets ChineseTraditional.
        /// </summary>
        public string ChineseTraditional { get; set; }

        /// <summary>
        /// Gets or sets French.
        /// </summary>
        public string French { get; set; }

        /// <summary>
        /// Gets or sets Spanish.
        /// </summary>
        public string Spanish { get; set; }

        /// <summary>
        /// Gets or sets Russian.
        /// </summary>
        public string Russian { get; set; }

        /// <summary>
        /// Gets or sets Japanese.
        /// </summary>
        public string Japanese { get; set; }

        /// <summary>
        /// Gets or sets ArticleTranslatorTitle.
        /// </summary>
        public string ArticleTranslatorTitle { get; set; }

        #endregion

        /// <summary>
        /// Gets the lang code token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public string GetLangCodeToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                char[] arrTokenChars = token.ToCharArray();
                var sb = new StringBuilder();
                sb.Append(tokenPrefix);
                sb.Append(arrTokenChars[0].ToString().ToUpper());
                for (int iCount = 1; iCount < arrTokenChars.Length; iCount++)
                {
                    sb.Append(arrTokenChars[iCount].ToString());
                }
                token = sb.ToString();
            }

            string translatedLanguage = GetTokenByName(string.Format("{0}", token));

            return translatedLanguage;
        }

        /// <summary>
        /// Sets the lang code token. This can be used to override with the user provided value.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string SetLangTextToken(string token, string value)
        {
            string translatedLanguage = (string.IsNullOrEmpty(value)
                                             ? GetTokenByName(string.Format("{0}", token))
                                             : value);
            return translatedLanguage;
        }
    }
}