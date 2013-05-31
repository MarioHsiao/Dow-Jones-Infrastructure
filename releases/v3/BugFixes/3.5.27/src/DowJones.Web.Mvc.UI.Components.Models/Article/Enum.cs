// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enum.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Enum type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Attributes;


namespace DowJones.Web.Mvc.UI.Components.Article
{
    /// <summary>
    /// The codes.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// The an.
        /// </summary>
        AN,

        /// <summary>
        /// The clm.
        /// </summary>
        CLM,

        /// <summary>
        /// The se.
        /// </summary>
        SE,

        /// <summary>
        /// The by.
        /// </summary>
        BY,

        /// <summary>
        /// The cr.
        /// </summary>
        CR,

        /// <summary>
        /// The wc.
        /// </summary>
        WC,

        /// <summary>
        /// The pd.
        /// </summary>
        PD,

        /// <summary>
        /// The et.
        /// </summary>
        ET,

        /// <summary>
        /// The sn.
        /// </summary>
        SN,

        /// <summary>
        /// The sc.
        /// </summary>
        SC,

        /// <summary>
        /// The ngc.
        /// </summary>
        NGC,

        /// <summary>
        /// The gc.
        /// </summary>
        GC,

        /// <summary>
        /// The ed.
        /// </summary>
        ED,

        /// <summary>
        /// The pg.
        /// </summary>
        PG,

        /// <summary>
        /// The vol.
        /// </summary>
        VOL,

        /// <summary>
        /// The la.
        /// </summary>
        LA,

        /// <summary>
        /// The cy.
        /// </summary>
        CY,

        /// <summary>
        /// The ct.
        /// </summary>
        CT,

        /// <summary>
        /// The rf.
        /// </summary>
        RF,

        /// <summary>
        /// The art.
        /// </summary>
        ART,

        /// <summary>
        /// The co.
        /// </summary>
        co,

        /// <summary>
        /// The pe.
        /// </summary>
        pe
    }

    /// <summary>
    /// The elink type.
    /// </summary>
    public enum ElinkType
    {
        /// <summary>
        /// The pro.
        /// </summary>
        pro,

        /// <summary>
        /// The webpage.
        /// </summary>
        webpage,

        /// <summary>
        /// The company.
        /// </summary>
        company,

        /// <summary>
        /// The doc.
        /// </summary>
        doc,

        /// <summary>
        /// The executive.
        /// </summary>
        executive
    }

    /// <summary>
    /// The content type.
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// The article.
        /// </summary>
        article,

        /// <summary>
        /// The picture.
        /// </summary>
        picture,

        /// <summary>
        /// The articlewithgraphics.
        /// </summary>
        articlewithgraphics,

        /// <summary>
        /// The webpage.
        /// </summary>
        webpage,

        /// <summary>
        /// The analyst.
        /// </summary>
        analyst
    }

    /// <summary>
    /// The category.
    /// </summary>
    public enum Category
    {
        /// <summary>
        /// The company.
        /// </summary>
        company,

        /// <summary>
        /// The executive.
        /// </summary>
        executive,

        /// <summary>
        /// The source.
        /// </summary>
        source,

        /// <summary>
        /// The companynews.
        /// </summary>
        companynews
    }

    /// <summary>
    /// The Display options for the article
    /// </summary>
    public enum DisplayOptions
    {
        [AssignedToken("fullArticle")] 
        Full,

        [AssignedToken("fullArticleIdx")] 
        Indexing,

        [AssignedToken("keywordsInContext")] 
        Keywords,

        [AssignedToken("headlineLeadParaIdx")] 
        Headline
    }
   

    
}
