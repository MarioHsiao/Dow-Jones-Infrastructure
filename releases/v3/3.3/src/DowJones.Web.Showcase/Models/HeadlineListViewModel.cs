using DowJones.Ajax.HeadlineList;

namespace DowJones.Web.Showcase.Models
{
    public class HeadlineListViewModel
    {
        public string Query { get; set; }

        public HeadlineListDataResult Result { get; set; }
    }
}