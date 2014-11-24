using System.Web.Services;

namespace EMG.widgets.ui.services.ajax
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseWebService : WebService
    {
        /// <summary>
        /// Sets the header for client side browser cache duration.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        public void SetHeaderCacheDuration(int minutes)
        {
            utility.Utility.SetHeaderCacheDuration(minutes);
        }
    }
}