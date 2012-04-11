﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.TweetLines.TweetLines.js", "text/javascript")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.TweetLines.ClientTemplates.tweetlines.htm", "text/html")]
[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.TweetLines.ClientTemplates.noData.htm", "text/html")]

namespace DowJones.Web.Mvc.UI.Components.TweetLines
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using DowJones.Extensions;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 04/03/2012 02:04 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.TweetLines.TweetLines.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.TweetLines.TweetLines))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.TweetLines.ClientTemplates.tweetlines.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="tweetlines", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.TweetLines.TweetLines))]
    [DowJones.Web.ClientTemplateResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.TweetLines.ClientTemplates.noData.htm", ResourceKind=DowJones.Web.ClientResourceKind.ClientTemplate, TemplateId="noData", DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.TweetLines.TweetLines))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "1.0.0.22175")]
    public class TweetLines : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.SocialMedia.TweetLinesModel>
    {
#line hidden

        public TweetLines()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_TweetLines";
            }
        }
        public override void ExecuteTemplate()
        {







   CssClass += " dj_TweetLines ";
   Html.DJ().ScriptRegistry().WithJQueryTimeAgo();
   Html.DJ().ScriptRegistry().WithODSManager();


WriteLiteral("<div class=\"dj_more-tweets dj_new-tweets hide\" style=\"z-index: 15;\">\r\n\t<span></sp" +
"an>\r\n</div>\r\n<ul class=\"dj_twitter-recent-tweets\">\r\n");


 	foreach (var tweet in Model.Tweets)
 {

WriteLiteral("\t\t<li data-tweet-id=\"");


                Write(tweet.Id);

WriteLiteral("\" data-user-id=\"");


                                         Write(tweet.User.Id);

WriteLiteral("\" data-full-name=\"");


                                                                         Write(tweet.User.FullName);

WriteLiteral("\" data-screen-name=\"");


                                                                                                                 Write(tweet.User.ScreenName);

WriteLiteral("\" class=\"dj_tweet-item\">\r\n\t\t\t<img data-user-id=\"");


                 Write(tweet.User.Id);

WriteLiteral("\" alt=\"");


                                      Write(tweet.User.FullName);

WriteLiteral("\" src=\"");


                                                                 Write(tweet.User.ProfileImageUrl);

WriteLiteral("\">\r\n\t\t\t<div class=\"dj_tweet-content\">\r\n\t\t\t\t<div class=\"dj_tweet-meta\">\r\n\t\t\t\t\t<a t" +
"itle=\"");


          Write(tweet.User.FullName);

WriteLiteral("\" href=\"");


                                      Write(tweet.User.ProfileUrl);

WriteLiteral("\" data-user-id=\"");


                                                                            Write(tweet.User.Id);

WriteLiteral("\" class=\"dj_full-name\" target=\"_blank\">\r\n\t\t\t\t\t\t");


 Write(tweet.User.FullName);

WriteLiteral("\r\n\t\t\t\t\t</a><a href=\"");


             Write(tweet.User.ProfileHashUrl);

WriteLiteral("\" title=\"");


                                                Write(tweet.User.FullName);

WriteLiteral("\" class=\"dj_screen-name\" target=\"_blank\">\r\n\t\t\t\t\t\t");


WriteLiteral("@");


   Write(tweet.User.ScreenName);

WriteLiteral("\r\n\t\t\t\t\t</a><span class=\"dj_time-stamp\" data-time=\"");


                                           Write(tweet.IsoTimeStamp);

WriteLiteral("\"><span title=\"");


                                                                             Write(tweet.IsoTimeStamp);

WriteLiteral("\">\r\n\t\t\t\t\t</span>\r\n\t\t\t\t\t\t");


  Write(string.IsNullOrEmpty(tweet.SourceText) ? "" : string.Format(" {0} {1}", @Html.DJ().Token("via"), tweet.SourceText));

WriteLiteral("</span>\r\n\t\t\t\t\t<ul class=\"dj_post-processing\">\r\n\t\t\t\t\t\t<li class=\"dj_follow\" data-a" +
"ction=\"follow\">");


                                            Write(Html.DJ().Token("follow"));

WriteLiteral(" *</li>\r\n\t\t\t\t\t\t<li class=\"dj_reply\" data-action=\"reply\">");


                                          Write(Html.DJ().Token("replyTweet"));

WriteLiteral(" *</li>\r\n\t\t\t\t\t\t<li class=\"dj_retweet\" data-action=\"retweet\">");


                                              Write(Html.DJ().Token("reTweet"));

WriteLiteral(" *</li>\r\n\t\t\t\t\t\t<li class=\"dj_favorite\" data-action=\"favorite\">");


                                                Write(Html.DJ().Token("favorite"));

WriteLiteral(" *</li>\r\n\t\t\t\t\t\t<li class=\"dj_details\" data-action=\"details\">");


                                              Write(Html.DJ().Token("details"));

WriteLiteral("</li>\r\n\t\t\t\t\t</ul>\r\n\t\t\t\t</div>\r\n\t\t\t\t<p class=\"dj_tweet\">\r\n\t\t\t\t\t");


Write(tweet.Html);

WriteLiteral("\r\n\t\t\t\t</p>\r\n\t\t\t</div>\r\n\t\t</li>\r\n");


 }

WriteLiteral("</ul>\r\n<div class=\"dj_more-tweets dj_old-tweets \" style=\"z-index: 15;\">\r\n\t<span>");


  Write(Html.DJ().Token("loadMoreTweets"));

WriteLiteral("</span></div>\r\n<div class=\"dj_to-top-wrap hide\">\r\n\t<span class=\"dj_bird-label\"></" +
"span><span class=\"dj_to-top\">");


                                                       Write(Html.DJ().Token("backToTop"));

WriteLiteral("\r\n\t\t↑ </span>\r\n</div>\r\n");


        }
    }
}
