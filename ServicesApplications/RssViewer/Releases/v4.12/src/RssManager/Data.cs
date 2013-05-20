using System;
using System.Xml;
using System.Collections.Specialized;

namespace Data
{
	/// <summary>
	/// Summary description for InputData.
	/// </summary>
	/// 

	public class XmlData
	{
		protected XmlDocument data = new XmlDocument();
		protected System.Xml.XmlNamespaceManager xmlNS ;
		public XmlData(){}

		public XmlData(XmlDocument dataDoc,XmlNamespaceManager xmlNSMgr)
		{
			data = dataDoc;
			xmlNS=xmlNSMgr;
		}

		public virtual string getItem(string item)
		{
			string retValue = "";

			if (item != "")
			{
				if (data.SelectSingleNode(item,xmlNS) != null)
					retValue =  data.SelectSingleNode(item,xmlNS).InnerText;
			}
			return retValue ;
		}

		public virtual string getItem(string item,XmlNode xmlNode)
		{
			string retValue = "";
			if (item != "")
			{
				if (xmlNode.SelectSingleNode(item,xmlNS) != null)
					retValue =  xmlNode.SelectSingleNode(item,xmlNS).InnerText;
			}
			return retValue ;
		}
        //public virtual string getItems(string xpath, string connector)
        //{
        //    string retValue = "";
        //    XmlNodeList nodeList;
        //    nodeList = data.SelectNodes(xpath);
        //    foreach (XmlNode n in nodeList)
        //    {
        //        if (n.HasChildNodes)
        //        {
        //            nodeList = n.ChildNodes;
        //            for (int i = 0; i <= nodeList.Count - 1; i++)
        //            {
        //                if (data.SelectSingleNode(nodeList[i].InnerText, xmlNS) != null)
        //                    retValue = nodeList[i].Attributes.GetNamedItem("name").Value + connector + data.SelectSingleNode(nodeList[i].InnerText, xmlNS).InnerText;
                        
        //            }
        //        }
        //    }
        //    return retValue;
        //}

        public virtual bool validateNode(string item)
        {
            bool rtnValue = false;
            if (item != "")
            {
                if (data.SelectNodes(item, xmlNS) != null)
                    rtnValue = true;
            }
            return rtnValue;
        }

	    public virtual NameValueCollection getItems(string item)
		{
			NameValueCollection retValue = new  NameValueCollection();
			XmlNodeList nodeList;

			if (item != "")
			{
				if (data.SelectNodes(item,xmlNS) != null)
				{
					nodeList = data.SelectNodes(item,xmlNS);
					if (nodeList.Count > 0 )
					{
						foreach(XmlNode node in nodeList)
						{
							retValue.Add(node.Name , node.InnerText);
						}
					}
				}
			}
			return retValue ;
		}


		public virtual NameValueCollection getItems(string item,string itemNameNode,string itemValueNode)
		{
			NameValueCollection retValue = new  NameValueCollection();
			XmlNodeList nodeList;

			if (item != "")
			{
				if (data.SelectNodes(item,xmlNS) != null)
				{
					nodeList = data.SelectNodes(item,xmlNS);
					if (nodeList.Count > 0 )
					{
						foreach(XmlNode node in nodeList)
						{

							retValue.Add(getItem(itemNameNode,node),getItem(itemValueNode,node));
						}
					}
				}
			}
			return retValue ;
		}

		public virtual string[] getItemValues(string item)
		{
			string[] itemValues = null;
			XmlNodeList itemValuesNodeList;
			int i  = 0;

			if (item!= "")
			{
                if (data.SelectNodes(item, xmlNS) != null)
                {
                    itemValuesNodeList = data.SelectNodes(item, xmlNS);

                    if (itemValuesNodeList.Count > 0)
                    {
                        itemValues = new string[itemValuesNodeList.Count];
                        foreach (XmlNode itemValuesNode in itemValuesNodeList)
                        {
                            itemValues[i] = itemValuesNode.InnerText;
                            i++;
                        }
                    }
                }
			}
			return itemValues;
		}

		public virtual XmlNodeList getNodeList(string item)
		{
			XmlNodeList retValue = null;
			if (data.SelectNodes(item,xmlNS) != null)
				retValue = data.SelectNodes(item,xmlNS);

			return retValue;
		}
		public virtual XmlNode getNode(string item)
		{
			XmlNode retValue = null;
			if (data.SelectSingleNode(item,xmlNS) != null)
				retValue = data.SelectSingleNode(item,xmlNS);

			return retValue;
		}
		
	}

	public class NVData
	{
		private NameValueCollection _data = new  NameValueCollection();
		public NameValueCollection data
		{
			get{return _data;}
			set{_data = value;}
		}

		public virtual string getItem(string itemName)
		{
			string retValue = "";

			if(data[itemName] !="")
				retValue = _data[itemName];

			return retValue;
		}
	}

	public class InputData : NVData{}
	public class ConfigData: XmlData
	{
		public  ConfigData()
		{
			
		}
		public  ConfigData(string fileName)
		{
			data.Load(fileName);
		}
		public  ConfigData(string fileName,string productVersion)
		{
			XmlDocument tmpDoc = new XmlDocument();
			XmlNode productNode;
			tmpDoc.Load(fileName);	
			productNode = data.ImportNode(tmpDoc.SelectSingleNode("//products/product[@version='" + productVersion + "']"),true);
			data.AppendChild(productNode);
		}
	
		public void  LoadFile(string fileName)
		{
			data.Load(fileName);
		}
		public void  LoadFile(string fileName,string productVersion)
		{
			XmlDocument tmpDoc = new XmlDocument();
			XmlNode productNode;
			tmpDoc.Load(fileName);	
			productNode = data.ImportNode(tmpDoc.SelectSingleNode("//products/product[@version='" + productVersion + "']"),true);
			data.AppendChild(productNode);
		}
		public void  LoadXML(string xmlData)
		{
			data.LoadXml(xmlData);
		}
		public void  LoadXML(string xmlData,string productVersion)
		{
			XmlDocument tmpDoc = new XmlDocument();
			XmlNode productNode;
			tmpDoc.LoadXml(xmlData);	
			productNode = data.ImportNode(tmpDoc.SelectSingleNode("//products/product[@version='" + productVersion + "']"),true);
			data.AppendChild(productNode);
		}
		

		public virtual string[] getValidationKeys()
		{
			return getItemValues("//validate/eid/params/param");
		}
		
	}

	public class utils
	{
		public static string Left(string strParam, int iLen)
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
		public static string Right(string strParam, int iLen)
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

		// Function to Reverse the String
		public static string Reverse(string strParam)
		{
			if(strParam.Length==1)
			{
				return strParam;
			}
			else
			{
				return Reverse(strParam.Substring(1)) + strParam.Substring(0,1);
			}
		}
		public static XmlNamespaceManager CreateNsMgr( XmlDocument doc, string nsPrefix, string nsURI) 
		{
			XmlNamespaceManager nsmgr = new XmlNamespaceManager( doc.NameTable) ; 
			if (nsPrefix.Trim().Length >0 && nsURI.Trim().Length > 0)
			{
				nsmgr.AddNamespace( nsPrefix,nsURI) ; 
			}
			return nsmgr; 
		} 

	}
}
