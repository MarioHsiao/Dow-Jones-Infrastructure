using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Url;
using DowJones.Web.Mvc.UI.Components.Attributes;
using System.Web;
using System.Web.UI;
using DowJones.Web.Mvc.UI.Components.InlineMp3Player;
using DowJones.Web;

[assembly: WebResource(InlineMp3PlayerModel.SPEAKER_IMAGE_RESOURCE_LOCATION, KnownMimeTypes.GifImage)]
[assembly: WebResource(InlineMp3PlayerModel.DOW_JONES_FLASH_PLAYER_RESOURCE_LOCATION, KnownMimeTypes.ShockwaveFlash)]   
[assembly: WebResource(InlineMp3PlayerModel.MINI_FLASH_PLAYER_RESOURCE_LOCATION, KnownMimeTypes.ShockwaveFlash)]        
[assembly: WebResource(InlineMp3PlayerModel.MULTIPLE_FLASH_PLAYER_RESOURCE_LOCATION, KnownMimeTypes.ShockwaveFlash)]    
[assembly: WebResource(InlineMp3PlayerModel.NORMAL_FLASH_PLAYER_RESOURCE_LOCATION, KnownMimeTypes.ShockwaveFlash)]      
[assembly: WebResource(InlineMp3PlayerModel.READ_SPEAKER_FLASH_PLAYER_RESOURCE_LOCATION, KnownMimeTypes.ShockwaveFlash)]
[assembly: WebResource(InlineMp3PlayerModel.SPEAKER_IMAGE_RESOURCE_LOCATION, KnownMimeTypes.ShockwaveFlash)]            
[assembly: WebResource(InlineMp3PlayerModel.VOLUME_FLASH_PLAYER_RESOURCE_LOCATION, KnownMimeTypes.ShockwaveFlash)]  


namespace DowJones.Web.Mvc.UI.Components.InlineMp3Player
{
    public class InlineMp3PlayerModel : ViewComponentModel
    {

        public InlineMp3PlayerModel()
        {
            _mp3PlayerType = DEFAULT_MP3_PLAYER_TYPE;
            _volume = 100;
            _enableTransparency = true;
            _backgroundColor = "FFFFFF";

        }


        #region ..:: Private Variables ::..

        private static readonly object SyncObject = new object();
        private static string dowJonesFlashPlayerUrl;
        private static string _miniFlashPlayerUrl;
        private static string _multipleFlashPlayerUrl;
        private static string _normalFlashPlayerUrl;
        private static string _readSpeakerFlashPlayerUrl;
        private static string _speakerImageUrl;
        private static string _volumeFlashPlayerUrl;
        private FlashPlayerMetaData _flashPlayerMetaData;


        #endregion


        #region ..:: Internal Constants ::..

        internal const MP3PlayerType DEFAULT_MP3_PLAYER_TYPE = MP3PlayerType.DowJones;
        internal const string BASE_RESOURCE_DIRECTORY = "DowJones.Web.Mvc.UI.Components.InlineMp3Player";
        internal const string DOW_JONES_FLASH_PLAYER_RESOURCE_LOCATION = BASE_RESOURCE_DIRECTORY + ".players.dewplayer-dowjones.swf";
        internal const string MINI_FLASH_PLAYER_RESOURCE_LOCATION = BASE_RESOURCE_DIRECTORY + ".players.dewplayer-mini.swf";
        internal const string MULTIPLE_FLASH_PLAYER_RESOURCE_LOCATION = BASE_RESOURCE_DIRECTORY + ".players.dewplayer-multi.swf";
        internal const string NORMAL_FLASH_PLAYER_RESOURCE_LOCATION = BASE_RESOURCE_DIRECTORY + ".players.dewplayer.swf";
        internal const string READ_SPEAKER_FLASH_PLAYER_RESOURCE_LOCATION = BASE_RESOURCE_DIRECTORY + ".players.rplayerpro.swf";
        internal const string SPEAKER_IMAGE_RESOURCE_LOCATION = BASE_RESOURCE_DIRECTORY + ".speaker.gif";
        internal const string VOLUME_FLASH_PLAYER_RESOURCE_LOCATION = BASE_RESOURCE_DIRECTORY + ".players.dewplayer-vol.swf";


        #endregion


        #region ..:: Public Properties (options, tokens etc) ::..

        #region ..:: Client Options ::..


        /// <summary>
        /// Gets or sets a value indicating whether [show download link].
        /// </summary>
        /// <value><c>true</c> if [show download link]; otherwise, <c>false</c>.</value>
        [ClientProperty(Name = "showDownloadLink")]
        public bool ShowDownloadLink { get; set; }


        private MP3PlayerType _mp3PlayerType;
        /// <summary>
        /// Gets or sets the MP3 player type
        /// </summary>
        /// <value>The type of the MP3 player.</value>
        [ClientProperty(Name = "mp3PlayerType")]
        public MP3PlayerType Mp3PlayerType
        {
            get { return _mp3PlayerType; }
            set { _mp3PlayerType = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [auto start].
        /// </summary>
        /// <value><c>true</c> if [auto start]; otherwise, <c>false</c>.</value>
        [ClientProperty(Name = "autoStart")]
        public bool AutoStart { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [auto replay].
        /// </summary>
        /// <value><c>true</c> if [auto replay]; otherwise, <c>false</c>.</value>
        [ClientProperty(Name = "autoReplay")]
        public bool AutoReplay { get; set; }


        private int _volume;

        /// <summary>
        /// Gets or sets the volume. Values are between 1-100;
        /// </summary>
        /// <value>The volume.</value>
        [ClientProperty(Name = "volume")]
        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (value >= 1 && value <= 100)
                {
                    _volume = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [show time]. Display(mm:ss)
        /// </summary>
        /// <value><c>true</c> if [show time]; otherwise, <c>false</c>.</value>
        [ClientProperty(Name = "showTime")]
        public bool ShowTime { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [random play].
        /// </summary>
        /// <value><c>true</c> if [random play]; otherwise, <c>false</c>.</value>
        [ClientProperty(Name = "randomPlay")]
        public bool RandomPlay { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [no cursor/pointer].
        /// </summary>
        /// <value><c>true</c> if [no cursor/pointer]; otherwise, <c>false</c>.</value>
        [ClientProperty(Name = "noPointer")]
        public bool NoPointer { get; set; }

        private bool _enableTransparency;

        /// <summary>
        /// Gets or sets a value indicating whether [enable transparency]. if transparency is true then background color will not be added
        /// </summary>
        /// <value><c>true</c> if [enable transparency]; otherwise, <c>false</c>.</value>
        [ClientProperty(Name = "enableTransparency")]
        public bool EnableTransparency
        {
            get { return _enableTransparency; }
            set { _enableTransparency = value; }
        }

        private string _backgroundColor;

        /// <summary>
        /// Gets or sets the color of the background. (white = FFFFFF, without #)
        /// </summary>
        /// <value>The color of the background.</value>
        [ClientProperty(Name = "backgroundColor")]
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }


        /// <summary>
        /// Gets the MP3 player URL.
        /// </summary>
        /// <value>The MP3 player URL.</value>
        [ClientProperty(Name = "mp3PlayerUrl")]
        public string Mp3PlayerUrl
        {
            get { return GetMp3PlayerUrl(); }
        }


        /// <summary>
        /// Gets the width of the flash player.
        /// </summary>
        /// <value>The width of the flash player.</value>
        [ClientProperty(Name = "flashPlayerWidth")]
        public int FlashPlayerWidth
        {
            get
            {
                UpdateMetaData(Mp3PlayerType);
                return _flashPlayerMetaData.Width;
            }
        }


        /// <summary>
        /// Gets the height of the flash player.
        /// </summary>
        /// <value>The height of the flash player.</value>
        [ClientProperty(Name = "flashPlayerHeight")]
        public int FlashPlayerHeight
        {
            get
            {
                UpdateMetaData(Mp3PlayerType);
                return _flashPlayerMetaData.Height;
            }
        }


        /// <summary>
        /// Gets the flash player version.
        /// </summary>
        /// <value>The flash player version.</value>
        [ClientProperty(Name = "flashPlayerVersion")]
        public string FlashPlayerVersion
        {
            get
            {
                UpdateMetaData(Mp3PlayerType);
                return _flashPlayerMetaData.TargetFlashPlayerVersion;
            }
        }


        /// <summary>
        /// Gets the dewplayer version.
        /// </summary>
        /// <value>The dewplayer version.</value>
        [ClientProperty(Name = "dewplayerVersion")]
        public string DewplayerVersion
        {
            get
            {
                UpdateMetaData(Mp3PlayerType);
                return _flashPlayerMetaData.DewplayerVersion;
            }
        }


        private string _mp3FilesUrls;

        /// <summary>
        /// Gets or sets the URL of the MP3 file.
        /// Separate URLs (absolute or relative path) with [sep] used to separate multiple Urls.
        /// Example : /media/sons/test.mp3|/media/sons/test2.mp3
        /// </summary>
        /// <value>The URL of the MP3 file.</value>
        [ClientProperty(Name = "mp3FilesUrls")]
        public string Mp3FilesUrls
        {
            get
            {
                if (!string.IsNullOrEmpty(_mp3FilesUrls))
                    return GetUrlsForProcessing(_mp3FilesUrls);
                else
                    return string.Empty;
            }
            set { _mp3FilesUrls = value; }
        }


        private string _downloadUrl;

        /// <summary>
        /// Gets or sets the download URL.
        /// </summary>
        /// <value>The download URL.</value>
        [ClientProperty(Name = "downloadUrl")]
        public string DownloadUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_downloadUrl))
                    return GetDownloadUrl(Mp3FilesUrls);
                else
                    return GetDownloadUrl(_downloadUrl);
            }

            set
            {
                _downloadUrl = value;
            }
        }


        /// <summary>
        /// Gets the speaker Image Url
        /// </summary>
        [ClientProperty(Name = "speakerImageUrl")]
        public string SpeakerImageUrl
        {
            get { return GetSpeakerImageResourceLocation(); }
        }

        #endregion

        #region ..:: Client Tokens ::..

        [ClientToken(Name = "listenToArticleToken")]
        public string ListenToArticleToken { get; set; }

        [ClientToken(Name = "downloadToken")]
        public string DownloadToken { get; set; }

        [ClientToken(Name = "attributionToken")]
        public string AttributionToken { get; set; }

        #endregion

        #endregion


        private static string GetUrlsForProcessing(string mUrls)
        {
            if (!string.IsNullOrEmpty(mUrls))
            {
                var urls = mUrls.Split(new[] { "[sep]" }, StringSplitOptions.RemoveEmptyEntries);
                var encodedUrls = new List<string>(urls.Length);
                encodedUrls.AddRange(urls.Select(s => new UrlBuilder(s) { OutputType = UrlBuilder.UrlOutputType.Absolute }).Select(ub => HttpUtility.UrlEncode(ub.ToString())));
                if (encodedUrls.Count > 0)
                {
                    return string.Join("|", encodedUrls.ToArray());
                }
            }

            return string.Empty;
        }

        private static string GetDownloadUrl(string mUrls)
        {
            if (!string.IsNullOrEmpty(mUrls))
            {
                var urls = mUrls.Split(new[] { "[sep]" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var ub in urls.Select(s => new UrlBuilder(s) { OutputType = UrlBuilder.UrlOutputType.Absolute }))
                {
                    return HttpUtility.UrlEncode(ub.ToString());
                }
            }

            return string.Empty;
        }

        private void UpdateMetaData(MP3PlayerType mp3PlayerType)
        {
            var fieldInfo = typeof(MP3PlayerType).GetField(mp3PlayerType.ToString());
            if (fieldInfo != null)
            {
                _flashPlayerMetaData = (FlashPlayerMetaData)Attribute.GetCustomAttribute(fieldInfo, typeof(FlashPlayerMetaData));
            }
        }

        private string GetSpeakerImageResourceLocation()
        {
            if (string.IsNullOrWhiteSpace(_speakerImageUrl))
            {
                lock (SyncObject)
                {
                    if (string.IsNullOrWhiteSpace(_speakerImageUrl))
                    {
                        _speakerImageUrl = GetType().Assembly.GetWebResourceUrl(SPEAKER_IMAGE_RESOURCE_LOCATION);
                    }
                }
            }

            return GetFullUrl(_speakerImageUrl);
        }

        private string GetFullUrl(string resourceUrl)
        {
            Uri currentUri = HttpContext.Current.Request.Url;
            return string.Concat(
                                 currentUri.Scheme,
                                 "://",
                                 currentUri.Host,
                                 currentUri.IsDefaultPort ? string.Empty : ":{0}".FormatWith(currentUri.Port),
                                 resourceUrl);
        }

        private string GetMp3PlayerUrl()
        {
            switch (Mp3PlayerType)
            {
                case MP3PlayerType.Mini:
                    return GetMiniFlashPlayerUrl();
                case MP3PlayerType.Volume:
                    return GetVolumeFlashPlayerUrl();
                case MP3PlayerType.Multiple:
                    return GetMultipleFlashPlayerUrl();
                case MP3PlayerType.DowJones:
                    return GetDowJonesFlashPlayerUrl();
                case MP3PlayerType.ReadSpeaker:
                    return GetReadSpeakerFlashPlayerUrl();
                default:
                    return GetNormalFlashPlayerUrl();
            }
        }

        private string GetDowJonesFlashPlayerUrl()
        {
            if (string.IsNullOrWhiteSpace(dowJonesFlashPlayerUrl))
            {
                lock (SyncObject)
                {
                    if (string.IsNullOrWhiteSpace(dowJonesFlashPlayerUrl))
                    {
                        dowJonesFlashPlayerUrl = GetType().Assembly.GetWebResourceUrl(DOW_JONES_FLASH_PLAYER_RESOURCE_LOCATION);
                    }
                }
            }

            return GetFullUrl(dowJonesFlashPlayerUrl);
        }

        private string GetReadSpeakerFlashPlayerUrl()
        {
            if (string.IsNullOrWhiteSpace(_readSpeakerFlashPlayerUrl))
            {
                lock (SyncObject)
                {
                    if (string.IsNullOrWhiteSpace(_readSpeakerFlashPlayerUrl))
                    {
                        _readSpeakerFlashPlayerUrl = GetType().Assembly.GetWebResourceUrl(READ_SPEAKER_FLASH_PLAYER_RESOURCE_LOCATION);
                    }
                }
            }

            return GetFullUrl(dowJonesFlashPlayerUrl);
        }

        private string GetMiniFlashPlayerUrl()
        {
            if (string.IsNullOrWhiteSpace(_miniFlashPlayerUrl))
            {
                lock (SyncObject)
                {
                    if (string.IsNullOrWhiteSpace(_miniFlashPlayerUrl))
                    {
                        _miniFlashPlayerUrl = GetType().Assembly.GetWebResourceUrl(MINI_FLASH_PLAYER_RESOURCE_LOCATION);
                    }
                }
            }

            return GetFullUrl(_miniFlashPlayerUrl);
        }

        private string GetNormalFlashPlayerUrl()
        {
            if (string.IsNullOrWhiteSpace(_normalFlashPlayerUrl))
            {
                lock (SyncObject)
                {
                    if (string.IsNullOrWhiteSpace(_normalFlashPlayerUrl))
                    {
                        _normalFlashPlayerUrl = GetType().Assembly.GetWebResourceUrl(NORMAL_FLASH_PLAYER_RESOURCE_LOCATION);
                    }
                }
            }

            return GetFullUrl(_normalFlashPlayerUrl);
        }

        private string GetVolumeFlashPlayerUrl()
        {
            if (string.IsNullOrWhiteSpace(_volumeFlashPlayerUrl))
            {
                lock (SyncObject)
                {
                    if (string.IsNullOrWhiteSpace(_volumeFlashPlayerUrl))
                    {
                        _volumeFlashPlayerUrl = GetType().Assembly.GetWebResourceUrl(VOLUME_FLASH_PLAYER_RESOURCE_LOCATION);
                    }
                }
            }

            return GetFullUrl(_volumeFlashPlayerUrl);
        }

        private string GetMultipleFlashPlayerUrl()
        {
            if (string.IsNullOrWhiteSpace(_multipleFlashPlayerUrl))
            {
                lock (SyncObject)
                {
                    if (string.IsNullOrWhiteSpace(_multipleFlashPlayerUrl))
                    {
                        _multipleFlashPlayerUrl = GetType().Assembly.GetWebResourceUrl(MULTIPLE_FLASH_PLAYER_RESOURCE_LOCATION);
                    }
                }
            }

            return GetFullUrl(_multipleFlashPlayerUrl);
        }
    }
}
