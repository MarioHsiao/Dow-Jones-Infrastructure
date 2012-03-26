using System.Collections.Generic;
using DowJones.Tools.Ajax;
using DowJones.Utilities.Ajax.Canvas;
using DowJones.Web.Mvc.UI.Canvas.Ajax.Core;

namespace DowJones.Web.Mvc.UI.Canvas.Ajax.CanvasModules
{

    public class AbstractAddModuleRequestDelegate : BaseCanvasAjaxRequestDelegate
    {
        public string moduleType;
        public List<Property> properties;

    }

    public class AbstractAddModuleResponseDelegate : AbstractAjaxResponseDelegate
    {
        public string clientSideStateScriptBlock;
        public string[] moduleInitializationScriptsBlock;
        public string moduleHtml;
        public string moduleClientId;
        public bool hasClientData;
    }
}
