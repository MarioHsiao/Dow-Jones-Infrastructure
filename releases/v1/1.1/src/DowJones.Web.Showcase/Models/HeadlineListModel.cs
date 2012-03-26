using DowJones.Tools.Ajax.HeadlineList;

namespace DowJones.Web.Showcase.Models
{
    public class HeadlineListModel
    {
        public string Query { get; set; }

        public HeadlineListDataResult Result { get; set; }
    }
}