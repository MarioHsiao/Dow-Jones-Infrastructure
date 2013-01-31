using System.Xml;
using System.Text;

namespace FactivaRssManager_2_0
{
	/// <summary>
	/// Summary description for rssManager.
	/// </summary>
	public class RssManager:FactivaRssManager.RssManager 
	{
		public override  void retrieveData(string remoteHost)
		{
			var xmlHeadlines = new XmlDocument();
			var retriever = new DataRetriever(remoteHost);
            var iso = Encoding.GetEncoding("ISO-8859-1");
            
            if (_from == "g2")
                _inputData.data.Set("userid", Encoding.UTF8.GetString(iso.GetBytes(inputData.getItem("uid"))));
            else
                _inputData.data.Set("userid", Encoding.UTF8.GetString(iso.GetBytes(inputData.getItem("userID"))));

			switch(base._from)
			{
                case "g1":
                case "g2":
                case "fdk":
                    xmlHeadlines.LoadXml(retriever.getSearch2Headlines(_inputData, _configData));
                    break;
                case "fce1":
                    xmlHeadlines.LoadXml(retriever.getSearch2Headlines(_inputData, _configData, true));
                    break;
				case "g3":
					xmlHeadlines.LoadXml(retriever.getSearchHeadlines(_inputData,_configData));
					break;	
				case "nl1":
                    xmlHeadlines.LoadXml(retriever.getNewsLetter(_inputData, _configData));
					break;
                case "pcast":
                    xmlHeadlines.LoadXml(retriever.getPocast(_inputData, _configData));
                    break;
                case "nl2":
                case "nl2pcast":
                    xmlHeadlines.LoadXml(retriever.getManualWorkpaceForNewsLetter(_inputData, _configData));
                    break;
                case "ws1":
                case "ws1pcast":
                    xmlHeadlines.LoadXml(retriever.getAutomaticWorkpaceForWorkspaces(_inputData, _configData));
                    break;					
            }
			_xmlHeadlines = xmlHeadlines;
		}
	}
		
}
