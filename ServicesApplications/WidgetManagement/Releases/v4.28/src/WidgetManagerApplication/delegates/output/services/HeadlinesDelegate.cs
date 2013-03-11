using System;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Argotic.Common;
using Argotic.Syndication;
using EMG.Utility.Formatters;
using EMG.Utility.Formatters.Numerical;
using EMG.Utility.Managers.Search;
using EMG.Utility.Url;
using EMG.widgets.ui.exception;
using emg.widgets.ui.headline;
using emg.widgets.ui.test.HeadlinePlugin.Utility;
using Factiva.Gateway.Messages.Search.V2_0;
using factiva.nextgen;

namespace EMG.widgets.ui.delegates.output
{
    /// <summary>
    /// 
    /// </summary>
    public struct Declarations
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SchemaVersion = "";
        /// <summary>
        /// 
        /// </summary>
        public const string SearchQueryVersion = "2.2";
    }

    /// <summary>
    /// 
    /// </summary>
    public class HeadlinesDelegate : AbstractWidgetDelegate
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly PerformContentSearchRequest _performContentSearchRequest;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly string _rssFeedUri;


        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        [XmlIgnore]
        private HeadlinePluginDataResult _result;


        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [XmlElement(Type = typeof (HeadlinePluginDataResult), ElementName = "result", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        public HeadlinePluginDataResult Result
        {
            get
            {
                if (_result == null)
                    _result = new HeadlinePluginDataResult();
                return _result;
            }
            set { _result = value; }
        }

        
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlinesDelegate"/> class.
        /// </summary>
        public HeadlinesDelegate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlinesDelegate"/> class.
        /// </summary>
        /// <param name="request">The __result.</param>
        public HeadlinesDelegate(PerformContentSearchRequest request)
        {
            _performContentSearchRequest = request;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlinesDelegate"/> class.
        /// </summary>
        /// <param name="rssFeedUri">The RSS Feed URI.</param>
        public HeadlinesDelegate(string rssFeedUri)
        {
            _rssFeedUri = rssFeedUri;
        }


        #endregion

        #region Implementation of ISessionBasedDelegate

        public override string ToRSS()
        {
            throw new NotImplementedException();
        }

        public override string ToATOM()
        {
            throw new NotImplementedException();
        }

        public override void Fill()
        {
            if (_performContentSearchRequest == null && string.IsNullOrEmpty(_rssFeedUri))
                return;

            if (_performContentSearchRequest != null)
            {
                ProcessPerformContentSearchRequest();
            }
            else
            {
                ProcessRssFeedUri();
            }
        }

        #endregion

        private void ProcessRssFeedUri()
        {
            SyndicationResourceLoadSettings settings = new SyndicationResourceLoadSettings();
            settings.Timeout = new TimeSpan(0,0,1);


            //"http://www.pwop.com/feed.aspx?show=dotnetrocks&filetype=master"
            GenericSyndicationFeed feed = GenericSyndicationFeed.Create(new EscapedUri(_rssFeedUri));

            HeadlinelineInfoConversionManager conversionManager = new HeadlinelineInfoConversionManager();
            switch (feed.Format)
            {
                case SyndicationContentFormat.Rss:
                    {
                        RssFeed rssFeed = feed.Resource as RssFeed;
                        if (rssFeed != null)
                        {
                            //  Process RSS format specific information
                            
                            Result.resultSet.first = new WholeNumber(0);
                            string language = HeadlinelineInfoConversionManager.ValidateLanguageName(rssFeed.Channel.Language.Name);
                            int i = 0;
                            foreach (RssItem item in rssFeed.Channel.Items)
                            {
                                Result.resultSet.headlines.Add(conversionManager.Convert(item, language));
                                i++;
                            }
                            Result.hitCount = new WholeNumber(i + 1);
                            Result.resultSet.count = new WholeNumber(i + 1);

                        }
                    }
                    break;
                case SyndicationContentFormat.Atom:
                    {
                        AtomFeed atomFeed = feed.Resource as AtomFeed;
                        if (atomFeed != null)
                        {
                            int i = 0;
                            foreach (AtomEntry atomEntry in atomFeed.Entries)
                            {
                                Result.resultSet.headlines.Add(conversionManager.Convert(atomEntry));
                                i++;
                            }
                            Result.hitCount = new WholeNumber(i + 1);
                            Result.resultSet.count = new WholeNumber(i + 1);
                        }
                    }
                    break;
            }
         
        }

        

        private void ProcessPerformContentSearchRequest()
        {
            SearchManager searchManager = new SearchManager(SessionData.Instance().SessionBasedControlDataEx, "en");
            PerformContentSearchResponse response = searchManager.PerformContentSearch(_performContentSearchRequest);
            NumberFormatter numberFormatter = SessionData.Instance().NumberFormatter;

            if (response == null || response.ContentSearchResult == null || response.ContentSearchResult.ContentHeadlineResultSet == null || response.ContentSearchResult.HitCount <= 0)
                return;
            // Add the HitCount to the result set
            Result.hitCount = new WholeNumber(response.ContentSearchResult.HitCount);
            // Format
            numberFormatter.Format(Result.hitCount);

            if (response.ContentSearchResult.ContentHeadlineResultSet == null)
                return;
            ContentHeadlineResultSet resultSet = response.ContentSearchResult.ContentHeadlineResultSet;
            Result.resultSet.first = new WholeNumber(resultSet.First);
            Result.resultSet.count = new WholeNumber(resultSet.Count);
            // Format
            numberFormatter.Format(Result.resultSet.first);
            numberFormatter.Format(Result.resultSet.count);

            

            if (resultSet.Count <= 0)
                return;
            HeadlinelineInfoConversionManager conversionManager = new HeadlinelineInfoConversionManager();
            foreach (ContentHeadline headline in resultSet.ContentHeadlineCollection)
            {
                Result.resultSet.headlines.Add(conversionManager.Convert(headline));

            }
        }
    }
}