using System;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Data;
using FactivaEncryption;

public partial class encrypt : System.Web.UI.Page
{
    FactivaRssManager_2_0.RssManager rssManager2 = new FactivaRssManager_2_0.RssManager();

    FactivaEncryption.encryption e = new FactivaEncryption.encryption();

    private InputData inputData = new InputData();

    private ConfigData eidData = new ConfigData();
    private string configFile = "";
    private string verRssManager = "";
    private string from = "";
    string rss = "";
    private string decryptKey = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        eidData.LoadXML(GetXMLFromCache("encrypt.xml", "encrypt", "encrypt.xml"));      //Cache["EIDSCache"].ToString());
        decryptKey = eidData.getItem("//Encrypt/@key");
        NameValueCollection nvData = new NameValueCollection();
        string strName = eidData.getItem("//Param/@name");
        string strValue = eidData.getItem("//Param/@value");
        XmlNodeList nodeList = eidData.getNodeList("//Param/value");
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].InnerText != "")
                nvData.Add(nodeList[i].Attributes.GetNamedItem("name").Value, nodeList[i].InnerText);
        }
        //nvData.Add("accountid", txtAccountID.Text.Trim());
        //nvData.Add("userID", txtUserID.Text.Trim());
        //nvData.Add("namespace", txtNameSpace.Text.Trim());
        //nvData.Add("from", txtfrom.Text.Trim());
        //nvData.Add("contid", txtfolderid.Text.Trim());
        //nvData.Add("fname", txtfolderName.Text.Trim());
        //if (!string.IsNullOrEmpty(tokenID.Text.Trim()))
        //    nvData.Add("tk", tokenID.Text.Trim());
        encryption E = new encryption();
        string eToken = E.encrypt(nvData, decryptKey);
        lnkRss.NavigateUrl = string.Format("http://rss.dev.factiva.com/headlines.aspx?{0}={1}&eid5={2}",strName,strValue, Server.UrlEncode(eToken));
        lnkRss.Text = string.Format("http://rss.dev.factiva.com/headlines.aspx?{0}={1}&eid5={2}", strName, strValue, Server.UrlEncode(eToken));
        lnkRss.Visible = true;

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
    //    NameValueCollection nvData = new NameValueCollection();
    //    XmlNodeList nodeList = eidData.getNodeList("//Param/value");
    //    for (int i = 0; i < nodeList.Count; i++)
    //    {
    //        nvData.Add(nodeList[i].Attributes.GetNamedItem("name").value, nodeList[i].InnerText);
    //    }
    //    //nvData.Add("accountid", txtAccountID.Text.Trim());
    //    //nvData.Add("userID", txtUserID.Text.Trim());
    //    //nvData.Add("namespace", txtNameSpace.Text.Trim());
    //    //nvData.Add("from", txtfrom.Text.Trim());
    //    //nvData.Add("contid", txtfolderid.Text.Trim());
    //    //nvData.Add("fname", txtfolderName.Text.Trim());
    //    //if (!string.IsNullOrEmpty(tokenID.Text.Trim()))
    //    //    nvData.Add("tk", tokenID.Text.Trim());
    //    encryption E = new encryption();
    //    string eToken = E.encrypt(nvData, decryptKey);
    //    lnkRss.NavigateUrl = "http://rss.dev.factiva.com/FactivaRss/headlines.aspx&eid3=" + Server.UrlEncode(eToken);
    //    lnkRss.Text = "http://rss.dev.factiva.com/FactivaRss/headlines.aspx&eid3=" + Server.UrlEncode(eToken);
    //    lnkRss.Visible = true;
    }
    private string GetXMLFromCache(string filename, string cacheItemName, string dependencyFileName)
    {
        if (Cache[cacheItemName] == null)
        {
            XmlDocument x = new XmlDocument();
            x.Load(Server.MapPath(filename));
            Cache.Insert(cacheItemName, x.InnerXml, new CacheDependency(Server.MapPath(dependencyFileName)));
        }
        return Cache[cacheItemName].ToString();
    }

}
