using System;
using System.Collections.Generic;
using System.Text;
using Factiva.Gateway.Messages.MarketData.V1_0;
namespace EMG.Utility.StockPriceMessageModel
{
   public  class StockPriceResultSet
    {
       public string companyName;
       
       public  string ticker;
     
       public string ric;

       [System.Xml.Serialization.XmlElement("historicalDataResult")]
       public  HistoricalDataResult historicalDataResult;

       public Quote quote;

       public string[] marketIndices;

    }  
}
