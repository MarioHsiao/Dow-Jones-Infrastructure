using DowJones.Infrastructure;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Ninject;

namespace DowJones.Preferences
{
    public class CachedPreferenceServiceFactory : Factory<IPreferenceService>
    {
        public override IPreferenceService Create()
        {
            var baseService = Kernel.Get<PreferenceService>();
            var primedCache = GeneratePrimedCacheResponse(baseService);

            return new CachedPreferenceService(baseService, primedCache);
        }

        /// <summary>
        /// Primes the cache by retrieving preferences used by the framework libraries.
        /// Feel free to override and replace this call with additional application-specific preferences.
        /// </summary>
        protected virtual PreferenceResponse GeneratePrimedCacheResponse(IPreferenceService service)
        {
            return service.GetItemsByClassId(new[] {
                    PreferenceClassID.TimeZone, 
                    PreferenceClassID.TimeFormat, 
                    PreferenceClassID.SearchLanguage,
                });
        }
    }
}
