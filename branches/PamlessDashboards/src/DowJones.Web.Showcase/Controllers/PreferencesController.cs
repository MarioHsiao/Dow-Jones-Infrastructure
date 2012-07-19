using System.Collections.Generic;
using System.Web.Mvc;
using DowJones.Preferences;
using Factiva.Gateway.Messages.Preferences.V1_0;

namespace DowJones.Web.Showcase.Controllers
{
    public class PreferencesController : Controller
    {
        private readonly IPreferenceService _preferenceService;

        public PreferencesController(IPreferenceService preferenceService)
        {
            _preferenceService = preferenceService;
        }

        public ActionResult Index()
        {
            var response = _preferenceService.GetItemsByClassId(new[] {
                    PreferenceClassID.TimeZone, 
                    PreferenceClassID.TimeFormat, 
                    PreferenceClassID.SearchLanguage,
//                    PreferenceClassID.SortOption,
                });

            var preferences = new Dictionary<string, string> {
                {PreferenceClassID.TimeZone.ToString(), response.TimeZone.TimeZoneCode}, 
                {PreferenceClassID.TimeFormat.ToString(), response.TimeFormat.TimeFormat.ToString()}, 
                {PreferenceClassID.SearchLanguage.ToString(), response.SearchLanguage.SearchLanguage},
//                {PreferenceClassID.SortOption.ToString(), response.SortOption.SortOption.ToString()},
            };

            return View("index", preferences);
        }
    }
}
