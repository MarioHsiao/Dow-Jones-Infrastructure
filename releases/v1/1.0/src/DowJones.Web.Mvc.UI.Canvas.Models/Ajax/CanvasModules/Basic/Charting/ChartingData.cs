using DowJones.Tools.Ajax;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Web.Mvc.UI.Canvas.Ajax.Core;

namespace DowJones.Web.Mvc.UI.Canvas.Ajax.CanvasModules.Basic
{
    public class GetChartingModuleRequestDelegate : BaseCanvasAjaxRequestDelegate
    {
        public string search = string.Empty;
        public string fcode = string.Empty;
    }

    public class GetChartingModuleResponseDelegate : AbstractAjaxResponseDelegate
    {
        public HeadlineListDataResult data;
        public string title;
    }

    public class GetChartingModuleChartRequestDelegate : BaseCanvasAjaxRequestDelegate
    {
        public string search = string.Empty;
    }

    public class GetChartingModuleChartResponseDelegate : AbstractAjaxResponseDelegate
    {
        public string[] categories;
        public DataItem[] dataItems;

    }

    public class DataItem
    {
        public string name;
        public string fcode;
        public string type;
        public string y;
    }
}
