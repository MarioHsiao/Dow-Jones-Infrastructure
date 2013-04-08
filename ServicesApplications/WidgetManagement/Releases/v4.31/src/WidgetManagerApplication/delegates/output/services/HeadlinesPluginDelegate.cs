using System;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using EMG.Tools.Ajax.Converters;
using EMG.Tools.Ajax.HeadlineList;
using EMG.Utility.Managers.Search;
using EMG.Utility.Uri;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Utils.V1_0;
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
        public const string SCHEMA_VERSION = "";
        /// <summary>
        /// 
        /// </summary>
        public const string SEARCH_QUERY_VERSION = "2.2";
    }

    /// <summary>
    /// 
    /// </summary>
    public class HeadlinesPluginDelegate : AbstractWidgetDelegate
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly PerformContentSearchRequest _performContentSearchRequest;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly string _rssFeedUri;

        //<lightWeightUser name="PremiumContentLightWeightUser"> <userId>search2prm</userId><userPassword>search2pr</userPassword><productId>22</productId></lightWeightUser>
        private readonly ControlData _lightweightUser;

        private bool _updateUrls;
        private bool _updateThumbnails;

        /// <summary>
        /// Gets or sets a value indicating whether [update urls].
        /// </summary>
        /// <value><c>true</c> if [update urls]; otherwise, <c>false</c>.</value>
        [ScriptIgnore]
        [XmlIgnore]
        public bool UpdateUrls
        {
            get { return _updateUrls; }
            set { _updateUrls = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [update thumbnails].
        /// </summary>
        /// <value><c>true</c> if [update thumbnails]; otherwise, <c>false</c>.</value>
        [ScriptIgnore]
        [XmlIgnore]
        public bool UpdateThumbnails
        {
            get { return _updateThumbnails; }
            set { _updateThumbnails = value; }
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        [XmlIgnore]
        private HeadlineListDataResult _result;


        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        [XmlElement(Type = typeof(HeadlineListDataResult), ElementName = "result", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SCHEMA_VERSION)]
        public HeadlineListDataResult Result
        {
            get
            {
                if (_result == null)
                    _result = new HeadlineListDataResult();
                return _result;
            }
            set { _result = value; }
        }

        
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlinesPluginDelegate"/> class.
        /// </summary>
        public HeadlinesPluginDelegate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlinesPluginDelegate"/> class.
        /// </summary>
        public HeadlinesPluginDelegate(ControlData cData)
        {
            _lightweightUser = cData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlinesPluginDelegate"/> class.
        /// </summary>
        /// <param name="request">The __result.</param>
        public HeadlinesPluginDelegate(PerformContentSearchRequest request)
        {
            _performContentSearchRequest = request;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlinesPluginDelegate"/> class.
        /// </summary>
        /// <param name="request">The __result.</param>
        /// <param name="cData"></param>
        public HeadlinesPluginDelegate(PerformContentSearchRequest request, ControlData cData)
        {
            _performContentSearchRequest = request;
            _lightweightUser = cData;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="HeadlinesPluginDelegate"/> class.
        /// </summary>
        /// <param name="rssFeedUri">The RSS Feed URI.</param>
        public HeadlinesPluginDelegate(string rssFeedUri)
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
            HeadlineListConversionManager conversionManager = new HeadlineListConversionManager(SessionData.Instance().InterfaceLanguage);
            if (_performContentSearchRequest != null)
            {
                SearchManager searchManager = new SearchManager(_lightweightUser ?? SessionData.Instance().SessionBasedControlDataEx, "en");
                PerformContentSearchResponse response = searchManager.PerformContentSearch(_performContentSearchRequest);
                Result = conversionManager.Process(response, GenerateUrls, UpdateImage);
            }
            else
            {
                Result = conversionManager.Process(_rssFeedUri);
            }
        }

        #endregion


        #region Delegate Implementation
        private static string GenerateUrls(ContentHeadline headline, bool isDup)
        {
            return "javascript:alert(\"dave\");"; 
        }

        private static void UpdateImage(ThumbnailImage image,ContentHeadline headline)
        {
            //C:\FactivaEncription\EMGWidgets\WidgetManagerApplication_2009_Bucket2\test\img\placeholder.jpg
            UrlBuilder urlBuilder = new UrlBuilder("~image/imageHandler.ashx");
            urlBuilder.Append("reference", image.guid);
            image.src = urlBuilder.ToString();
        }
        #endregion
    }
}