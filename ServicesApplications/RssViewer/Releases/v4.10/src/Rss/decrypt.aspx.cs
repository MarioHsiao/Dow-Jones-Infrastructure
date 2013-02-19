using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using FactivaEncryption;
using Data;
using System.Web.Caching;
using System.Xml;


namespace FactivaRSS
{
	/// <summary>
	/// Summary description for decrypt.
	/// </summary>
	public partial class decrypt : System.Web.UI.Page
	{

		FactivaRssManager_2_0.RssManager rssManager2 = new FactivaRssManager_2_0.RssManager(); 

		FactivaEncryption.encryption  e = new  FactivaEncryption.encryption();

		private InputData inputData = new  InputData();
		
		private ConfigData eidData = new ConfigData();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			eidData.LoadXML(GetXMLFromCache("EIDs.xml","EIDSCache","EIDs.xml"))   ;      //Cache["EIDSCache"].ToString());
			Response.Write(decryptvalue());

			// Put user code to initialize the page here
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion

		public string decryptvalue()
		{
				string decryptKey = "";
				string eidName	  = "";
				string eidValue	  = "";
				string strdecryptValue = "";
				bool   status	  = false;
				string[] data = new string[6];
			try
			{


				if (Request["eid1"] != null)
				{
					eidName = "eid1";
					eidValue = Request["eid1"].ToString();
				}

				else if (Request["eid2"] != null)
				{
					eidName = "eid2";
					eidValue = Request["eid2"].ToString();
				}
				else if (Request["eid3"] != null)
				{
					eidName = "eid3";
					eidValue = Request["eid3"].ToString();
				}
				decryptKey = eidData.getItem("//" + eidName + "/@key");
				rssManager2.inputData.data = e.decrypt(eidValue,decryptKey);
				for ( int i=0; i< rssManager2.inputData.data.Count; i++)
				{
					strdecryptValue += rssManager2.inputData.data.AllKeys[i] + " - " + rssManager2.inputData.data[i] + "<BR>";
				}
			}
			catch(Exception ex)
			{
				CreateRssForError("550006", ex.ToString(),false);
			}
			return strdecryptValue;


		}

		private string GetXMLFromCache(string filename, string cacheItemName, string dependencyFileName)
		{
			if(Cache[cacheItemName] == null)
			{
				XmlDocument x = new XmlDocument();
				x.Load(Server.MapPath(filename));
				Cache.Insert(cacheItemName,x.InnerXml,new CacheDependency(Server.MapPath(dependencyFileName)));
			}	
			return Cache[cacheItemName].ToString();
		}


		private void CreateRssForError (string errorCode, string errorDescription,bool displayError)
		{
		
			try
			{
				FactivaRssManager.RssErrorManager oRssError = new FactivaRssManager.RssErrorManager();
				oRssError.loadErrorConfigXML(GetXMLFromCache("config.xml","ConfigCache","config.xml"));
				oRssError.ErrorCode = errorCode;
				oRssError.ErrorDescription = errorDescription;
				if (Request.QueryString["debug"] == null)
					oRssError.DisplayError=displayError;
				else
					oRssError.DisplayError=true;
				Response.Clear();
				Response.ContentType = "text/xml";
				Response.Write(oRssError.createRssError(oRssError.errorData));
				Response.End();
			}
			catch (Exception ex)
			{
				//
			}
		
			
			

		}


	}

}
