// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateSyndicationFeedServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Argotic.Common;
using Argotic.Syndication;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Url;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Validation;
using Factiva.Gateway.Utils.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Validation
{
    public class ValidateSyndicationFeedServiceResult : AbstractServiceResult, IValidate<ValidateSyndicationFeedRequest>
    {
        private const int MaxResult = 100;
        private static readonly Regex HTMLTags = new Regex("<[^>]*>");
        
        public string FeedTitle { get; set; }

        public FeedType FeedType { get; set; }

        #region Implementation of IValidate<in ValidateSyndicationFeedRequest>

        public void Validate(ControlData controlData, ValidateSyndicationFeedRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                    {
                        if (!request.IsValid())
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidValidationRequest);
                        }

                        var settings = new SyndicationResourceLoadSettings
                                           {
                                               Timeout = new TimeSpan(0, 0, 10),
                                               RetrievalLimit = MaxResult,
                                           };

                        var feed = GenericSyndicationFeed.Create(new EscapedUri(request.FeedUri), settings);
                        switch (feed.Format)
                        {
                            case SyndicationContentFormat.Rss:
                                {
                                    var rssFeed = feed.Resource as RssFeed;
                                    if (rssFeed != null)
                                    {
                                        FeedTitle = StripHTML(rssFeed.Channel.Title);
                                        FeedType = FeedType.RSS;
                                    }
                                }

                                break;
                            case SyndicationContentFormat.Atom:
                                {
                                    var atomFeed = feed.Resource as AtomFeed;
                                    if (atomFeed != null)
                                    {
                                        FeedTitle = StripHTML(atomFeed.Title.ToString());
                                        FeedType = FeedType.Atom;
                                    }
                                }

                                break;
                        }
                    },
                    preferences);
        }

        #endregion

        // De-HTMLize the title
        private static string StripHTML(string feedTitle)
        {
            return string.IsNullOrWhiteSpace(feedTitle) ? string.Empty : HTMLTags.Replace(feedTitle, string.Empty);
        }
    }
}
