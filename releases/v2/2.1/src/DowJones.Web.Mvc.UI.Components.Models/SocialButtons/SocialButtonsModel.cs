// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialButtonsExtender.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <author>
//   ..:: name ::..
// </author>
// <lastModified>
//  <entry type="original"><date>5/12/2010</date><user>dhirv</user></entry>
//  <entry type="update">
//    <date>5/17/2010</date><user>dhirv</user>
//    <reason>Added MySpace as a new social media target.</reason>
//    <date>6/09/2010</date><user>dhirv</user>
//    <reason>Added new property for Target.</reason>
//  </entry>
// </lastModified>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Web.UI;
using DowJones.Web;
using DowJones.Web.Mvc;
using DowJones.Web.UI;
using DowJones.Web.Mvc.UI.Components.SocialButtons;



namespace DowJones.Web.Mvc.UI.Components.SocialButtons
{
    /// <summary>
    /// The image size.
    /// </summary>
    public enum ImageSize
    {
        /// <summary>
        /// Small social media image
        /// </summary>
        Small,

        /// <summary>
        /// Large social media image
        /// </summary>
        Large,
    }

    /// <summary>
    /// The social networks.
    /// </summary>
    public enum SocialNetworks
    {
        /// <summary>
        /// Delicious Social Web Site.
        /// </summary>
        Delicious,

        /// <summary>
        /// Digg Social Web Site.
        /// </summary>
        Digg,

        /// <summary>
        /// Facebook Social Web Site.
        /// </summary>
        Facebook,

        /// <summary>
        /// Furl Social Web Site.
        /// </summary>
        Furl,

        /// <summary>
        /// Google Social Web Site.
        /// </summary>
        Google,

        /// <summary>
        /// Linkedin Social Web Site.
        /// </summary>
        LinkedIn,

        /// <summary>
        /// Newsvine Social Web Site.
        /// </summary>
        Newsvine,

        /// <summary>
        /// Reddit Social Web Site.
        /// </summary>
        Reddit,

        /// <summary>
        /// Stumbleupon Social Web Site.
        /// </summary>
        StumbleUpon,

        /// <summary>
        /// Technorati Social Web Site.
        /// </summary>
        Technorati,

        /// <summary>
        /// Twitter Social Web Site.
        /// </summary>
        Twitter,

        /// <summary>
        /// Yahoo Social Web Site.
        /// </summary>
        Yahoo,

        /// <summary>
        /// MySpace Social Web Site.
        /// </summary>
        MySpace,
    }

    #region List of Missing Large Icons

    /* 
    bligg,
    blogmarks,
    ekudo,
    live,
    magnolia,
    misterwong,
    netscape,    
    nujij,
    sphere,
    symbaloo,
    tailrank,
  * */
    #endregion

    /// <summary>
    /// Social Extender Control that add buttons to the accepted target control
    /// </summary>
    public class SocialButtonsModel: ViewComponentModel
    {

        /// <summary>
        /// Gets or sets the size of the image.
        /// </summary>
        /// <value>The size of the image.</value>
        [ClientProperty(Name="imageSize")]
        public ImageSize ImageSize { get; set; }

        /// <summary>
        /// Gets or sets the social network.
        /// </summary>
        /// <value>The social network.</value>
        [ClientProperty(Name = "socialNetworks")]
        public IEnumerable<SocialNetworks> SocialNetworks { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL of the posted content.</value>
        [UrlProperty]
        [ClientProperty(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [ClientProperty(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>The keywords.</value>
        [ClientProperty(Name = "keywords")]
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        [ClientProperty(Name = "target")]
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [ClientProperty(Name = "description")]
        public string Description { get; set; }

        public SocialButtonsModel()
        {
            SocialNetworks = new [] {
                    SocialButtons.SocialNetworks.Delicious,
                    SocialButtons.SocialNetworks.Digg,
                    SocialButtons.SocialNetworks.Facebook,
                    SocialButtons.SocialNetworks.Furl,
                    SocialButtons.SocialNetworks.Google,
                    SocialButtons.SocialNetworks.LinkedIn,
                    SocialButtons.SocialNetworks.Newsvine,
                    SocialButtons.SocialNetworks.Reddit,
                    SocialButtons.SocialNetworks.StumbleUpon,
                    SocialButtons.SocialNetworks.Technorati,
                    SocialButtons.SocialNetworks.Twitter,
                    SocialButtons.SocialNetworks.Yahoo
                };
        }
    }
}
