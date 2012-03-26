using System.Collections.Generic;
using DowJones.Tools.Ajax;

namespace DowJones.Web.Mvc.UI.Canvas.Ajax.Core
{

    public class UpdateCanvasModulesPositionsRequestDelegate : BaseCanvasAjaxRequestDelegate
    {
        public ICollection<ICollection<int>> modules;
    }

    public class UpdateCanvasModulesPositionsResponseDelegate : AbstractAjaxResponseDelegate
    {
    }
}
