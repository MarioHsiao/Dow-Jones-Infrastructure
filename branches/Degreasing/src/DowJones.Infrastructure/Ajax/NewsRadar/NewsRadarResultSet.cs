using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DowJones.Ajax.NewsRadar
{
    [Serializable]
    public class NewsRadarResultSet : IListDataResult
    {
        /// <summary>
        /// Stores colum deatils
        /// </summary>
        private List<RadarViewQuery> _queryViews;

        /// <summary>
        /// stores rows details
        /// </summary>
        private List<Company> _companies;

        [XmlElement("QueryViews")]
        public List<RadarViewQuery> QueryViews
        {
            get { return _queryViews; }
            set { _queryViews = value; }
        }

        [XmlElement("Companies")]
        public List<Company> Companies
        {
            get { return _companies; }
            set { _companies = value;}
        }

        public NewsRadarResultSet()
        {
            _queryViews = new List<RadarViewQuery>();
            _companies = new List<Company>();
        }
    }

    public class NewsDataType
    {
        //public string fcode;
        //public string ID;
        [XmlElement("day")]
        public int day{ get; set;}
        [XmlElement("week")]
        public int week { get; set; }
        [XmlElement("month")]
        public int month { get; set; }
        [XmlElement("twoMonth")]
        public int twoMonth { get; set; }
        [XmlElement("threeMonth")]
        public int threeMonth { get; set; }
        [XmlElement("Uri")]
        public string Uri { get; set; }
        //public string Value;
    }



    public class Company 
    {
        /// <remarks/>
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("RadarValues")]
        public List<NewsDataType> RadarValues { get; set; }

        [XmlElement("Uri")]
        public string Uri { get; set; }

        [XmlElement("IsNewsCoded")]
        public bool IsNewsCoded { get; set; }


        public Company()
        {
            RadarValues = new List<NewsDataType>();
        }

    }

    public class RadarViewQuery
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("QueryType")]
        public RadarViewQueryType QueryType { get; set; }
        [XmlElement("Query")]
        public string Query { get; set; }
        [XmlElement("Position")]
        public int Position { get; set; }
        [XmlElement("Id")]
        public string Id { get; set; }
        [XmlElement("Uri")]
        public string Uri { get; set; }
    }

    public enum RadarViewQueryType
    {
        Company,
        Custom,
        Industry,
        NewsSubject,
        Region
    }
}
