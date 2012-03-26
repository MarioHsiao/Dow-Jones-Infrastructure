using DowJones.Tools.Ajax;

namespace DowJones.Web.Mvc.UI.Canvas.Ajax.Core
{
    public class BaseCanvasAjaxRequestDelegate : IAjaxRequestDelegate
    {
        public string canvasId;

        /// <summary>
        /// Default value is true; if the items is not in session cache, the transaction will go all the to the 
        /// back end to fill the cache item
        /// If set to false, if not found in cache, returns an error and the CacheHit Flag in response control data
        /// will be Miss
        /// </summary>
        public bool hitBackEndViaCache = true;
    }
}
