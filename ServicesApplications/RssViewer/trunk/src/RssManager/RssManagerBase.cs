using System;
using System.Xml;
using Data;
using System.Text;

namespace FactivaRssManager
{
	public class RssManager
	{
		protected string _from = "";
		
		public string from
		{
			get{return _from;}
			set{_from = value;}
		}

		// Input Data
		protected InputData _inputData = new InputData();
		public  InputData inputData
		{
			get{return _inputData;}
			set{_inputData  = value;}
		}
	
		// Config Data
		protected  ConfigData _configData = null;
		public   ConfigData configData
		{
			get{return _configData;}
		}
		public  void loadConfig(string fileName)
		{
			_configData = new ConfigData(fileName);
		}
		public  void loadConfig(string fileName,string productVersion)
		{
			_configData = new ConfigData(fileName,productVersion);
		}
		public  void loadConfigXML(string xmlData)
		{
			_configData = new ConfigData();
			_configData.LoadXML(xmlData);
		}
		public  void loadConfigXML(string xmlData,string productVersion)
		{
			_configData = new ConfigData();
			_configData.LoadXML(xmlData,productVersion);
		}
		

		// Data Retrieval
		protected XmlDocument _xmlHeadlines = new XmlDocument();
		protected string _strRequestXML; 
		public virtual void retrieveData(string remoteHost)
		{
			
		}

		// Rss Request
		private RssRequest _rssRequest = new  RssRequest();
		public void buildRssRequest()
		{
			_rssRequest.buildRequest(_inputData,_configData);
		}

		// Generate RSS
		private RssGenerator _rssGenerator = new  RssGenerator();
		public virtual string getRss()
		{
            _rssGenerator.TranName = _from;
			_rssGenerator.ChannelTitle = _rssRequest.channelTitle;
			_rssGenerator.ChannelDesc = _rssRequest.channelDesc;
			_rssGenerator.ChannelLink = _rssRequest.factivaChannelLink;
			_rssGenerator.ChannelCopyright = _rssRequest.factivaCopyright;
			_rssGenerator.ChannelDocs = _rssRequest.rssChannelDocs;
			_rssGenerator.ChannelLanguage = _rssRequest.rssChannelLanguage;
			_rssGenerator.ChannelLastBuildDate = _rssRequest.channelLastBuildDate;
			_rssGenerator.ChannelManagingEditor = _rssRequest.channelManagingEditor;
            _rssGenerator.ChannelRssChannelImage = _rssRequest.rssChannelImage;
            _rssGenerator.ChannelImageHeight = _rssRequest.rssChannelImageHeight;
            _rssGenerator.ChannelImageWidth = _rssRequest.rssChannelImageWidth;
            _rssGenerator.Channelttl = _rssRequest.channelttl;
            _rssGenerator.ArticleUrl = _rssRequest.articleURL;
            _rssGenerator.ChannelAuthor = _rssRequest.channelAuthor;
			return _rssGenerator.Convert(_xmlHeadlines,_configData, _inputData);
		}

	
	}	

	public class RssErrorManager
	{
	
		private string channelTitle;
		private string channelDesc;
		private string channelLink;
		private string channelLanguage;
		private string channelCopyright;
		private string channelDocs;
		private  ErrorData _errorData = null;
		public string ErrorCode;
		public string ErrorDescription;
		public bool DisplayError= false;

		public ErrorData errorData
		{
			get{return _errorData;}
		}
		public  void loadErrorConfig(string fileName)
		{
			_errorData = new ErrorData(fileName);
		}
		public  void loadErrorConfigXML(string xmlData)
		{
			_errorData = new ErrorData();
			_errorData.LoadErrorDataXML(xmlData);
		}

		public string createRssError(ErrorData Data)
		{
			StringBuilder channel = new StringBuilder();
			string title = "";
			string desc  = "";
			string link  = "";
			string lastBuildDate = "";
			string language = "";
			string copyright = "";
			string docs = "";
			string managingEditor = "";

						
			channelTitle = errorData.getItem("//rssChannel/channelName");
			channelDesc = errorData.getItem("//rssChannel/channelDesc");
			channelLink = errorData.getItem("//rssChannel/factivaChannelLink");
			channelCopyright = errorData.getItem("//rssChannel/factivaCopyright");
			channelDocs = errorData.getItem("//rssChannel/rssChannelDocs");
			channelLanguage = errorData.getItem("//rssChannel/rssChannelLanguage");
			

			channel.Append("<?xml version='1.0' encoding='utf-8'?>");
			channel.Append("<rss version='2.0'>");

			try
			{
				//header
				DateTime today = DateTime.UtcNow;
				lastBuildDate = today.ToString("R");

				title = "<channel><title><![CDATA[" +channelTitle + "]]></title>";
				desc =  "<description><![CDATA[" + channelDesc + "]]></description>";
				link =  "<link>" +channelLink + "</link>";
				lastBuildDate = "<lastBuildDate>" + lastBuildDate + "</lastBuildDate>";
				language = "<language>" + channelLanguage + "</language>";
				copyright = "<copyright>" +  channelCopyright+ "</copyright>";
				docs = "<docs>" + channelDocs + "</docs>";
				managingEditor = "<managingEditor>" + managingEditor + "</managingEditor>";
				
				channel.Append(title);
				channel.Append(desc);
				channel.Append(link);
				channel.Append(lastBuildDate);
				channel.Append(language);
				channel.Append(copyright);
				channel.Append(docs);

				//detail section for error
				if (DisplayError == true)
				{
					channel.Append("<item>");
					channel.Append("<title><![CDATA[");
					channel.Append(ErrorDescription + "(" + ErrorCode + ")");
					channel.Append("]]></title>");
					channel.Append("</item>");
				}

				//Footer
				channel.Append("</channel>");
				channel.Append("</rss>");

				return channel.ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		
		
	}
	public class ErrorData: XmlData
	{
		public ErrorData(){}
		public ErrorData(string fileName)
		{
			XmlDocument tmpDoc = new XmlDocument();
			XmlNode productNode;
			tmpDoc.Load(fileName);	
			productNode = data.ImportNode(tmpDoc.SelectSingleNode("//Error"),true);
			data.AppendChild(productNode);
		}
		public void LoadErrorDataXML(string xmlData)
		{
			XmlDocument tmpDoc = new XmlDocument();
			XmlNode productNode;
			tmpDoc.LoadXml(xmlData);	
			productNode = data.ImportNode(tmpDoc.SelectSingleNode("//Error"),true);
			data.AppendChild(productNode);
		}
				
	}

	


	


}