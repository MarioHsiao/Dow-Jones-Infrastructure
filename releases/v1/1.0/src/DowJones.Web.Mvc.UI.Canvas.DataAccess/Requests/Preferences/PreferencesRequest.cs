using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Preferences
{
    public enum PreferencesParts
    {
        /// <summary>
        /// Time Zone part.
        /// </summary>
        TimeZone,

        /// <summary>
        /// Time Format part.
        /// </summary>
        TimeFormat,

        /// <summary>
        /// Search Language part.
        /// </summary>
        SearchLanguage,
    }
       
    public class PreferencesRequest : IRequest
    {
        private List<PreferencesParts> parts = new List<PreferencesParts>();
        
        /// <summary>
        /// Gets or Sets the Preference Parts
        /// </summary>
        public List<PreferencesParts> Parts { get; set; }

        public bool IsValid()
        {
            return true;
        }
    }
}
