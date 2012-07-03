using System.Collections.Generic;
using System.Web.UI;

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
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>
        /// The name of the class.
        /// </value>
        public string ClassName { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>
        /// The name of the tag.
        /// </value>
        public string TagName { get; set; }

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
        /// Gets or sets a value indicating whether [show custom tooltip].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show custom tooltip]; otherwise, <c>false</c>.
        /// </value>
        [ClientProperty(Name = "showCustomTooltip")]
        public bool ShowCustomTooltip { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [ClientProperty(Name = "description")]
        public string Description { get; set; }

        public SocialButtonsModel()
        {
            SocialNetworks = new [] {
                    Components.SocialButtons.SocialNetworks.Delicious,
                    Components.SocialButtons.SocialNetworks.Digg,
                    Components.SocialButtons.SocialNetworks.Facebook,
                    Components.SocialButtons.SocialNetworks.Furl,
                    Components.SocialButtons.SocialNetworks.Google,
                    Components.SocialButtons.SocialNetworks.LinkedIn,
                    Components.SocialButtons.SocialNetworks.Newsvine,
                    Components.SocialButtons.SocialNetworks.Reddit,
                    Components.SocialButtons.SocialNetworks.StumbleUpon,
                    Components.SocialButtons.SocialNetworks.Technorati,
                    Components.SocialButtons.SocialNetworks.Twitter,
                    Components.SocialButtons.SocialNetworks.Yahoo
                };
        }
    }
}
