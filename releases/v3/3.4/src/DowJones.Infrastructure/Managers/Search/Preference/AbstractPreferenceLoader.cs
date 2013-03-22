using System.Collections.Generic;
using System.Linq;
using DowJones.DependencyInjection;
using DowJones.Preferences;
using Factiva.Gateway.Messages.Preferences.V1_0;

namespace DowJones.Managers.Search.Preference
{
    public abstract class AbstractPreferenceLoader : IPreferencesLoader
    {
        private PreferenceResponse _preferenceResponse;

        [Inject("Avoiding constructor injection in abstract class")]
        protected IPreferenceService Service { get; set; }

        public virtual IEnumerable<PreferenceClassID> PreferenceClassIds
        {
            get { return Enumerable.Empty<PreferenceClassID>(); }
        }

        public PreferenceResponse PreferenceResponse
        {
            get
            {
                if (_preferenceResponse == null)
                {
                    Load();
                }
                return _preferenceResponse;
            }
            set { _preferenceResponse = value; }
        }

        private void Load()
        {
            _preferenceResponse = Service.GetItemsByClassId(PreferenceClassIds) ?? new PreferenceResponse();
        }
    }
}