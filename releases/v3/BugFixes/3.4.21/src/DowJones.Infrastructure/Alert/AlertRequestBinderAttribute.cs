using System;

namespace DowJones.Infrastructure.Alert
{
    /// <summary>
    /// Translates an enum value to a valid alert request string value
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class AlertRequestBinderAttribute : Attribute
    {
        private string _translateTo;

        public AlertRequestBinderAttribute(string translateTo)
        {
            this._translateTo = translateTo;
        }

        public string TranslateTo
        {
            get
            {
                return _translateTo;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    this._translateTo = value;
            }
        }
    }
}
