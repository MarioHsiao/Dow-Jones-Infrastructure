using System.Xml.Serialization;

namespace EMG.Utility.Managers.Search
{
    public enum SortBy
    {
        [XmlEnum(Name = "FIFO")]
        FIFO,
        [XmlEnum(Name = "LIFO")]
        LIFO,
        [XmlEnum(Name = "PublicationDateChronological")]
        PublicationDateChronological,
        [XmlEnum(Name = "PublicationDateReverseChronological")]
        PublicationDateReverseChronological

    }
    public class AccessionNumberSearchRequest
    {
        private string[] m_Articles;
        private SortBy m_SortBy;

        public string[] Articles
        {
            get { return m_Articles; }
            set { m_Articles = value; }
        }
        public SortBy SortBy
        {
            get { return m_SortBy; }
            set { m_SortBy = value; }
        }
    }
}
