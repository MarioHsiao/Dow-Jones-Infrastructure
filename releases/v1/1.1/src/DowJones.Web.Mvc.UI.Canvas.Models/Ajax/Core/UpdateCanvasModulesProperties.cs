using System.Collections.Generic;
using DowJones.Tools.Ajax;
using DowJones.Utilities.Ajax.Canvas;

namespace DowJones.Web.Mvc.UI.Canvas.Ajax.Core
{
    public class UpdateCanvasModulesPropertiesRequestDelegate : BaseCanvasAjaxRequestDelegate
    {
        public string moduleId;
        public List<Property> properties;
    }

    public class UpdateCanvasModulesPropertiesResponseDelegate : AbstractAjaxResponseDelegate
    {
        
    }
}
