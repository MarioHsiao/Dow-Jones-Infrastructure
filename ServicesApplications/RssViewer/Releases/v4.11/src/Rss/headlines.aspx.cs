using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using FactivaEncryption;
using Data;
using System.Web.Caching;

namespace FactivaRSS
{
	/// <summary>
	/// Summary description for headline.
	/// </summary>
	public partial class headlines : System.Web.UI.Page
	{

		FactivaRssManager_2_0.RssManager rssManager2 = new FactivaRssManager_2_0.RssManager(); 

		FactivaEncryption.encryption  e = new  FactivaEncryption.encryption();

		private InputData inputData = new  InputData();
		
		private ConfigData eidData = new ConfigData();
		private string configFile	  = "";
		private string verRssManager  = "";
		private string from = "";
		string rss = "";
        //commeted never used 
		//private const string ENCRYPTKEY = "&)LJ(E@&@#MLU(&HDS";

		protected void Page_Load(object sender, EventArgs e)
		{
			// Put user code to initialize the page here
		   
			//CreateRssForError("1000","Invalid parameters supplied");
			Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::Page_Load Entering ...");

			eidData.LoadXML(GetXMLFromCache("EIDs.xml","EIDSCache","EIDs.xml"))   ;      //Cache["EIDSCache"].ToString());
			Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::after LoadXML EIDs ...");

			if (validate())
			{
				switch(verRssManager)
				{
					case "2":
						try
						{
							Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::before retrieveData ...");
							rssManager2.retrieveData(Request.ServerVariables["REMOTE_HOST"].ToString());
							
							Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::after retrieveData ...");
						}
						catch(Exception ex)
						{
							Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::Error(550000):retrieveData - " + ex.Message);

							CreateRssForError("550000","rssManager2.retrieveData:"+ ex.Message ,false);
								
						}
						try
						{
							Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::before buildRssRequest ");

							rssManager2.buildRssRequest();

							Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::after buildRssRequest ");
						}
						catch(Exception ex)
						{
							Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::Error(550001):buildRssRequest - " + ex.Message);

							CreateRssForError("550001","rssManager2.BuildRssRequest:"+ ex.Message ,false);
						}
						try
						{
							Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::before getRss ");

							rss = rssManager2.getRss();

							Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::after getRSS ");
						}
						catch(Exception ex)
						{
							Log(FactivaRssManager.Logger.Level.ERROR,"FactivaRSS::headlines.aspx.cs::Error(550002):getRSS - " + ex);

							CreateRssForError("550002","rssManager2.getRss:"+ ex ,false);
						}
						break;

				}

				Response.Clear();
				Response.ContentType = "text/xml";
				Response.Write(rss);
			}
			else
			{
				Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::Error(550003) - Validation failed.");

				CreateRssForError("550003","Validation failed.",false);
			}
			
			
			

		}
		private bool validate()
		{
			string decryptKey = "";
			string eidName	  = "";
			string eidValue	  = "";
			bool   status	  = false;
			string[] data = new string[6];

			
			Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:Entering validate ... ");

			Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:before getDecryptKey ");
			getDecryptKey(ref decryptKey,ref eidName , ref eidValue);
			Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:after getDecryptKey");

			/// Is EIDx present ?
			if (eidName == "")
			{
				Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:EIDx is absent");
				return status;
			}
			
			/// Is decryption a success ?
			try
			{
				Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:before decrypt");
				rssManager2.inputData.data = e.decrypt(eidValue,decryptKey);
                //This is only used to test PodCast
                //rssManager2.inputData.data.Add("type","pcast");


			}
			catch(Exception ex)
			{
				Log(FactivaRssManager.Logger.Level.ERROR,"FactivaRSS::headlines.aspx.cs::validate:Error(550004)-decrypt failed-" + ex.Message);

				CreateRssForError("550004","validate:Encryption.decrypt:"+ ex.Message ,false);
			}

			/// Does Product Identify itself by passing from?
			from = rssManager2.inputData.getItem("from");
			
			Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:from -" + from);

			if (from == "" || from == null)
			{
				Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:Error(550005)-Invalid or No product identification supplied");

				CreateRssForError("550005","Invalid or No product identification supplied.",false);
			}
			else
				rssManager2.from = from;

			
			/// Load Component Versions to determine which versions of RssManager & Encryption to use
			loadComponentVersions(eidData,eidName,from);

			///Load Config File
			string[] keys = null;

			Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:verRssManager - " + verRssManager);

			switch(verRssManager)
			{
				
				case "2":
					Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:before loadConfigXML ...");

					rssManager2.loadConfigXML(GetXMLFromCache("config.xml","ConfigCache","config.xml"),from);

					Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:after loadConfigXML ...");

					// Get Validation Keys if any?
					keys = rssManager2.configData.getValidationKeys();
					break;


			}
				
			// check URL validation rules (if any)
			if ( keys != null)
			{
				NameValueCollection nvRequest = Request.QueryString;
				NameValueCollection nvRequestKeysData = new NameValueCollection();
				for (int i=0 ; i < keys.Length ; i++)
				{
					if ( nvRequest[keys[i]] == null)
						return status;
					else
						nvRequestKeysData.Add(nvRequest.Keys[i],nvRequest[i]);
				}	
				if (e.validate(rssManager2.inputData.data,nvRequestKeysData))
				{
					Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::validate:URL validation passed.");
					status = true;
				}
				else
				{
					Log(FactivaRssManager.Logger.Level.DEBUG,"FactivaRSS::headlines.aspx.cs::validate:Error(550006) - Validation of enccrypted key failed..");
					CreateRssForError("550006","Validation of enccrypted key failed.",false);
				}
			}
			else
				status = true;

			return status;
		}

		private void getDecryptKey(ref string decryptKey,ref string eidName,ref string eidValue)
		{
			
			Log(FactivaRssManager.Logger.Level.INFO,"FactivaRSS::headlines.aspx.cs::getDecryptKey: Entering ...");

			if (Request["eid1"] != null)
			{

				eidName = "eid1";
				eidValue = Request["eid1"].ToString();
			      
			}

			else if (Request["eid2"] != null)
			{
				eidName = "eid2";
			    eidValue = Request["eid2"];
			}
			else if (Request["eid3"] != null)
			{
				eidName = "eid3";
			    eidValue = Request["eid3"];
			}
            else if (Request["eid5"] != null)
            {
                eidName = "eid5";
                eidValue = Request["eid5"];
            }
			else
			{
				//call rss error
				Log(FactivaRssManager.Logger.Level.DEBUG,"FactivaRSS::headlines.aspx.cs::getDecryptKey:Error(550007) - Invalid or No EID supplied.");
				CreateRssForError("550007","Invalid or No EID supplied.",false);
				return;
				
			}
			decryptKey = eidData.getItem("//" + eidName + "/@key");
			Log(FactivaRssManager.Logger.Level.DEBUG,"FactivaRSS::headlines.aspx.cs::getDecryptKey: Exiting ...");
			
			
			

		}


		private void loadComponentVersions(ConfigData eidData,string eidName,string from)
		{
			Log(FactivaRssManager.Logger.Level.INFO,"Entering loadComponentVersions::headlines.aspx.cs:: eidName -" + eidName + "from - " + from);
			//if (eidDoc.SelectSingleNode("//" + eidName + "/products/product[@version='" + from + "']") != null)
			try
			{
				if (eidData.getItem ("//" + eidName + "/products/product[@version='" + from + "']") != null)
				{
					XmlNode productNode = eidData.getNode("//" + eidName + "/products/product[@version='" + from + "']");
					verRssManager  = productNode.SelectSingleNode("rssManager").InnerText;
					configFile	   = Server.MapPath(productNode.SelectSingleNode("configFile").InnerText);
				}
				else
				{
					Log(FactivaRssManager.Logger.Level.DEBUG,"FactivaRSS::headlines.aspx.cs::loadComponentVersions:Error(550008) - Invalid product identification supplied.");

					CreateRssForError("550008","Invalid product identification supplied.",false);
				}
			}
			catch (Exception ex)
			{
				Log(FactivaRssManager.Logger.Level.ERROR,"FactivaRSS::headlines.aspx.cs::loadComponentVersions:Error(550009) - " + ex.Message);
				CreateRssForError("550009","loadComponentVersions:"+ ex.Message ,false);
			}
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
				if(eidData.getItem("errorDes") != null)
			
					Response.Write(oRssError.createRssError(oRssError.errorData));
				Response.End();
			}
			catch (Exception ex)
			{
				//
			}


		}

		public void Log(FactivaRssManager.Logger.Level level,string logMsg)
		{
			if(System.Configuration.ConfigurationSettings.AppSettings["logging"] == "On" || level >= FactivaRssManager.Logger.Level.ERROR)
				FactivaRssManager.Logger.Log(level, logMsg);

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
	}
}
