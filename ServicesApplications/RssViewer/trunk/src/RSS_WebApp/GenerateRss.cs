using System;
using System.Xml;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Reflection;
using System.IO;

namespace FactivaRssManager_2_0
{
	/// <summary>
	/// Summary description for GenerateRss.
	/// </summary>
	/// 
	public class GenerateRss
	{
		private XmlDocument rssMappingDocument;
		private string tranName = "";
		private string articleURL = "";
		private string channelName = "";
		private string channelDesc = "";
		private string channelLink = "";
		private string channelID = "";
		private string channelURLParam = "";
		private string channelLastBuildDate = "";
		private string channelLanguage = "";
		private string channelCopyright = "";
		private string channelDocs = "";
		private string channelManagingEditor = "";

		private string xmlTopLevelNode = "";
		private string fldTitle = "";
		private string fldDescription = "";
		private string fldSource = "";
		private string fldPubDate = "";
		private string fldPubTime = "";
		private SortedList slParams = new SortedList();


		public XmlDocument RssMappingDocument
		{
			get { return rssMappingDocument; }
			set { rssMappingDocument = value; }
		}

		public string TranName
		{
			get { return tranName; }
			set { tranName = value; }
		}

		public string ArticleUrl
		{
			get { return articleURL; }
			set { articleURL = value; }
		}

		public string ChannelName
		{
			get { return channelName; }
			set { channelName = value; }
		}

		public string ChannelDesc
		{
			get { return channelDesc; }
			set { channelDesc = value; }
		}

		public string ChannelLink
		{
			get { return channelLink; }
			set { channelLink = value; }
		}

		public string ChannelId
		{
			get { return channelID; }
			set { channelID = value; }
		}

		public string ChannelUrlParam
		{
			get { return channelURLParam; }
			set { channelURLParam = value; }
		}

		public string ChannelLastBuildDate
		{
			get { return channelLastBuildDate; }
			set { channelLastBuildDate = value; }
		}

		public string ChannelLanguage
		{
			get { return channelLanguage; }
			set { channelLanguage = value; }
		}

		public string ChannelCopyright
		{
			get { return channelCopyright; }
			set { channelCopyright = value; }
		}

		public string ChannelDocs
		{
			get { return channelDocs; }
			set { channelDocs = value; }
		}

		public string ChannelManagingEditor
		{
			get { return channelManagingEditor; }
			set { channelManagingEditor = value; }
		}

		public GenerateRss(XmlDocument rssMappingDocument)
		{
			this.rssMappingDocument = rssMappingDocument;
		}
		public GenerateRss()
		{
		}

		public string Convert(XmlDocument sourceDocument)
		{
			string rss = "";
			string hd = ""; //Headline
			string source = ""; //Source Name
			string pd = ""; //Publication Date
			string pt = ""; //Publication Time
			string snip = ""; //Snippet

			XmlNodeList nodeList;
			int i;

			try
			{
				loadTranMappings();

				rss = "<?xml version='1.0' encoding='utf-8'?>";
				rss = rss + "<rss version='2.0'>";
				rss = rss + createChannel();

				nodeList = sourceDocument.SelectNodes("//" + xmlTopLevelNode);
				for (i = 0; i < nodeList.Count; i++)
				{
					pd = "";
					pt= "";

					hd = nodeList.Item(i).SelectSingleNode(fldTitle).InnerText;
					if (fldSource.ToString() != "")
						source = nodeList.Item(i).SelectSingleNode(fldSource).InnerText;

					if (fldPubDate.ToString() != "")
					{
						pd = nodeList.Item(i).SelectSingleNode(fldPubDate).InnerText;
						//pubDate = pd;
					}

					if (fldPubTime.ToString() != "")
						if (nodeList.Item(i).SelectSingleNode(fldPubTime) != null)
							pt = nodeList.Item(i).SelectSingleNode(fldPubTime).InnerText;

					pd = formatDate(pd, pt);

					if (fldDescription != "")
						snip = nodeList.Item(i).SelectSingleNode(fldDescription).InnerText;

					rss = rss + createItem(hd, snip, pd, source, createQueryStringFromXML(nodeList.Item(i)), createQueryStringFromXML(nodeList.Item(i)));
				}
				rss = rss + "</channel>";
				rss = rss + "</rss>";


				return rss;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		private string createChannel()
		{
			string channel = "";

			try
			{
				if (channelDesc == "")
					channelDesc = channelName;

				channel = "<channel><title><![CDATA[" + channelName + "]]></title>";

				if (channelName == "")
					channelDesc = channelName;
				channel = channel + "<description><![CDATA[" + channelDesc + "]]></description>";

				if (channelLink.ToString() != "")
				{
					if ((channelURLParam != "") && (channelID != ""))
					{
						if (channelLink.IndexOf("?") > 0)
							channel = channel + "<link>" + channelLink + "&" + channelURLParam + "=" + channelID + "</link>";
						else
							channel = channel + "<link>" + channelLink + "?" + channelURLParam + "=" + channelID + "</link>";
					}
				}
				if (channelLastBuildDate != "")
					channel = channel + "<lastBuildDate>" + channelLastBuildDate + "</lastBuildDate>";

				if (channelLanguage != "")
					channel = channel + "<language>" + channelLanguage + "</language>";

				if (channelCopyright != "")
					channel = channel + "<copyright>" + channelCopyright + "</copyright>";

				if (channelDocs != "")
					channel = channel + "<docs>" + channelDocs + "</docs>";

				if (channelManagingEditor != "")
					channel = channel + "<managingEditor>" + channelManagingEditor + "</managingEditor>";

				return channel;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private string createItem(string hd, string snip, string pd, string author, string queryString, string guid)
		{
			StringBuilder item = new StringBuilder();

			try
			{
				item.Append("<item>");

				item.Append("<title><![CDATA[");
				//item.Append(hd.Replace("'", "&quot;"));
				item.Append(hd);
				item.Append("]]></title>");

				item.Append("<description><![CDATA[");
				//item.Append(snip.Replace("'", "&quot;"));
				item.Append(snip);
				item.Append("]]></description>");

				item.Append("<link>");
				if (articleURL.IndexOf("?") > 0)
					item.Append(articleURL + "&amp;" + queryString);
				else
					item.Append(articleURL + "?" + queryString);
				item.Append("</link>");


				item.Append("<author><![CDATA[");
				//item.Append(author.Replace("'", "&quot;"));
				item.Append(author);
				item.Append("]]></author>");

				item.Append("<pubDate>");
				item.Append(pd);
				item.Append("</pubDate>");

				item.Append("<guid>");
				if (articleURL.IndexOf("?") > 0)
					item.Append(articleURL + "&amp;" + queryString);
				else
					item.Append(articleURL + "?" + queryString);
				item.Append("</guid>");

				item.Append("</item>");

				return item.ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private string createQueryStringFromXML(XmlNode node)
		{
			string query = "";
			int i;

			try
			{
				for (i = 0; i < slParams.Count; i++)
				{
					query = slParams.GetKey(i) + "=" + node.SelectSingleNode(slParams.GetByIndex(0).ToString()).InnerText;
					if (i < slParams.Count - 1)
						query = query + "&amp;";
				}

				return query;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void loadTranMappings()
		{
			XmlNodeList nodeList;
			string query;
			int i;

			try
			{
				//string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
				//rssMappingDocument.Load(path + @"\config.xml");

				query = "//tran[@Name='" + tranName + "']";
				xmlTopLevelNode = rssMappingDocument.SelectSingleNode(query + "/TopLevelNode").InnerText;

				fldTitle = rssMappingDocument.SelectSingleNode(query + "/rssMapping/title").InnerText;
				if (rssMappingDocument.SelectSingleNode(query + "/rssMapping/description") != null)
					fldDescription = rssMappingDocument.SelectSingleNode(query + "/rssMapping/description").InnerText;

				if (rssMappingDocument.SelectSingleNode(query + "/rssMapping/source") != null)
					fldSource = rssMappingDocument.SelectSingleNode(query + "/rssMapping/source").InnerText;

				if (rssMappingDocument.SelectSingleNode(query + "/rssMapping/pubDate") != null)
					fldPubDate = rssMappingDocument.SelectSingleNode(query + "/rssMapping/pubDate").InnerText;

				if (rssMappingDocument.SelectSingleNode(query + "/rssMapping/pubTime") != null)
					fldPubTime = rssMappingDocument.SelectSingleNode(query + "/rssMapping/pubTime").InnerText;

				nodeList = rssMappingDocument.SelectNodes(query + "/uniqueContent/param");
				for (i = 0; i < nodeList.Count; i++)
				{
					slParams.Add(nodeList.Item(i).SelectSingleNode("urlName").InnerText, nodeList.Item(i).SelectSingleNode("fldName").InnerText);
				}
				/*
				nodeAppendList = tranXML.SelectNodes(query + "/appendFields/append");
				for(i=0;i<nodeAppendList.Count;i++)
				{
					slAppend.Add(1, nodeList.Item(i).SelectSingleNode("fields").InnerText);
				}
				*/
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private string formatDate(string pubDate, string pubTime)
		{
			string finalPubDate = "";
			string tmpDate = "";
			string tmpTime = "";
			try
			{
				if (pubDate.Length == 8)
				{
					tmpDate = Left(pubDate, 4) + "/" + (pubDate.Substring(4, 2)) + "/" + Right(pubDate, 2);
				}

				if (pubTime != "")
				{
					tmpTime = Left(pubTime, 2) + ":" + (pubTime.Substring(2, 2)); // + " GMT";
				}

				finalPubDate = tmpDate + " " + tmpTime;
				if (finalPubDate.Trim() != "")
				{
					finalPubDate = System.Convert.ToString(System.Convert.ToDateTime(finalPubDate).ToString("R"));
				}
				//if time is blank then remove 00:00:00 GMT
				if (pubTime == "")
				{
					if (finalPubDate.IndexOf("00:00:00 GMT") != -1)
						finalPubDate = Left(finalPubDate,finalPubDate.IndexOf("00:00:00 GMT"));
				}
				return finalPubDate;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static String Left(String strParam, int iLen)
		{
			try
			{
				if (iLen > 0)
					return strParam.Substring(0, iLen);
				else
					return strParam;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		//Function to get string from end
		public static String Right(String strParam, int iLen)
		{
			try
			{
				if (iLen > 0)
					return strParam.Substring(strParam.Length - iLen, iLen);
				else
					return strParam;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}

}
