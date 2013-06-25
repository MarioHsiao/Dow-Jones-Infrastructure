using DowJones.Extensions;
using DowJones.Preferences;

namespace DowJones.Prod.X.Web.Models
{
    public class GenericActionProperties
    {
        private readonly IPreferences _preferences;
        private string _bodyClassName;

        public GenericActionProperties(IPreferences session)
        {
            IncludeFooter = true;
            _preferences = session;
        }

        public string Description { get; set; }
        public string Title { get; set; }
        public string BodyClassName
        {
            get
            {
                return _bodyClassName.IsNullOrEmpty() ? _preferences.InterfaceLanguage : string.Concat(_bodyClassName, " ", _preferences.InterfaceLanguage);
            }
            set
            {
                _bodyClassName = value;
            }
        }
        public bool IncludeFooter { get; set; }
    }
}