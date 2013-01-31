using System;
using System.Xml;
using Data;
using System.Web;

namespace FactivaRssManager
{
	/// <summary>
	/// Summary description for RssRequest.
	/// </summary>
	public class RssRequest
	{
		private string _channelTitle = "";
		public string channelTitle
		{
			get{return _channelTitle;}
		}

		private string _channelDesc = "";
		public string channelDesc
		{
			get{return _channelDesc;}
		}

		private string _factivaChannelLink = "";
		public string factivaChannelLink
		{
			get{return _factivaChannelLink;}
		}

		private string _customerServiceLink = "";
		public string customerServiceLink
		{
			get{return _customerServiceLink;}
		}

		private string _factivaCopyright = "";
		public string factivaCopyright
		{
			get{return _factivaCopyright;}
		}

		private string _rssChannelDocs = "";
		public string rssChannelDocs
		{
			get{return _rssChannelDocs;}
		}

		private string _rssChannelLanguage = "";
		public string rssChannelLanguage
		{
			get{return _rssChannelLanguage;}
		}

		private string _channelLastBuildDate = "";
		public string channelLastBuildDate
		{
			get{return _channelLastBuildDate;}
		}

		private readonly string _channelManagingEditor = "";
		public string channelManagingEditor
		{
			get{return _channelManagingEditor;}
		}

        private string _rssChannelImageHeight = "";
        public string rssChannelImageHeight
		{
            get { return _rssChannelImageHeight; }
		}

        private string _rssChannelImageWidth = "";
        public string rssChannelImageWidth
        {
            get { return _rssChannelImageWidth; }
        }

        private string _rssChannelImage = "";
        public string rssChannelImage
        {
            get { return _rssChannelImage; }
        }

        
        private string _channelttl = "";
        public string channelttl
		{
            get { return _channelttl; }
		}

        private string _channelAuthor = "";
        public string channelAuthor
        {
            get { return _channelAuthor; }
        }
        

		private string _articleURL = "";
		public string articleURL
		{
			get{return _articleURL;}
		}

		public virtual void buildRequest(InputData inputData,ConfigData configData)
		{
			buildRssHeader(inputData,configData);
			buildArticleURL(inputData,configData);
		}
		protected void buildRssHeader(InputData inputData,ConfigData configData)
		{	
			buildChannelName(inputData,configData);
			buildChannelDesc(inputData,configData);

			_factivaChannelLink = configData.getItem("//rssChannel/factivaChannelLink");
			_customerServiceLink = configData.getItem("//rssChannel/factivaCustomerServiceLink");
			_factivaCopyright = configData.getItem("//rssChannel/factivaCopyright");
			_rssChannelDocs = configData.getItem("//rssChannel/rssChannelDocs");
			_rssChannelLanguage = configData.getItem("//rssChannel/rssChannelLanguage");
		    _rssChannelImage = configData.getItem("//rssChannel/rssChannelImage/url");
            _channelttl = configData.getItem("//rssChannel/rssChannelttl");
            _rssChannelImageHeight = configData.getItem("//rssChannel/rssChannelImage/height");
            _rssChannelImageWidth = configData.getItem("//rssChannel/rssChannelImage/width");
            _channelAuthor = configData.getItem("//rssChannel/rssChannelAuthor");
			DateTime today = DateTime.UtcNow;
			_channelLastBuildDate = today.ToString("R");

		}
		protected virtual void buildChannelName(InputData inputData , ConfigData configData)
		{
			// prefix
			_channelTitle = configData.getItem("//rssChannel/channelHeader/channelName/prefix");
			
			// User Values specified in EID
			_channelTitle = _channelTitle + inputData.getItem(configData.getItem("//rssChannel/channelHeader/channelName/param[@type='eid']"));
            _channelTitle = channelTitle + configData.getItem("//rssChannel/channelHeader/channelName/suffix");
        }

		protected virtual void buildChannelDesc(InputData inputData , ConfigData configData)
		{
			// prefix
			_channelDesc = configData.getItem("//rssChannel/channelHeader/channelDesc/prefix");
			
			// User Values specified in EID
			_channelDesc = _channelDesc + inputData.getItem(configData.getItem("//rssChannel/channelHeader/channelDesc/param[@type='eid']"));
            _channelDesc = _channelDesc + configData.getItem("//rssChannel/channelHeader/channelDesc/suffix");

		}
        protected virtual void buildArticleURL(InputData inputData, ConfigData configData)
        {
            // DirectURL
            if (!string.IsNullOrEmpty(inputData.getItem("app")) && inputData.getItem("app").ToUpper() == "WSJ")
            {
                _articleURL = configData.getItem("//articleURL/WSJDirectURL");
            }
            else
            {
                _articleURL = configData.getItem("//articleURL/DirectURL");

                // User Predefined Values
                string[] predefinedValues = configData.getItemValues("//articleURL/predefined/params/param");
                if (predefinedValues != null)
                {
                    foreach (string predefinedValue in predefinedValues)
                    {
                        if (_articleURL.IndexOf("?") > 0)
                            _articleURL = _articleURL + "&" + predefinedValue;
                        else
                            _articleURL = _articleURL + "?" + predefinedValue;
                    }
                }

                // User Values specified in EID
                //string[] eidValues = configData.getItemValues("//articleURL/eid/params/param");
                XmlNodeList eidNodes;
                eidNodes = configData.getNodeList("//articleURL/eid/params/param");
                if (eidNodes != null)
                {
                    foreach (XmlNode eidNode in eidNodes)
                    {
                        string eidMapTo = eidNode.Attributes["mapTo"].InnerText;
                        string eidValue = eidNode.InnerText;
                        string _strMapNameValue = string.Empty;

                        if (!string.IsNullOrEmpty(inputData.getItem(eidValue)))
                            if (eidMapTo != null)
                            {
                                _strMapNameValue = string.Format("{0}={1}", eidMapTo, HttpContext.Current.Server.UrlEncode(inputData.getItem(eidValue)));

                                if (_articleURL.IndexOf("?") > 0)
                                    _articleURL = _articleURL + "&";
                                else
                                    _articleURL = _articleURL + "?";

                                _articleURL = _articleURL + _strMapNameValue;
                            }
                    }
                }
                _articleURL = _articleURL.Replace("?&", "?").Replace("&&", "&");
            }
        }
	}
}
