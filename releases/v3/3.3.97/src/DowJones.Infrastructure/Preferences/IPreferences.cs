// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPreferences.cs" company="Dow Jones & Co">
//   
// </copyright>
// <summary>
//   The i interface lanauge.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
   
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Globalization;

namespace DowJones.Preferences
{
    /// <summary>
    /// The i interface lanauge.
    /// </summary>
    public interface IInterfaceLanauge
    {
        /// <summary>
        /// Gets or sets InterfaceLanguage.
        /// </summary>
        string InterfaceLanguage { get; set; }
    }

    /// <summary>
    /// The i preferences.
    /// </summary>
    public interface IPreferences : IInterfaceLanauge
    { 
        /// <summary>
        /// Gets or sets ContentLanguages.
        /// </summary>
        ContentLanguageCollection ContentLanguages { get; set; }

        /// <summary>
        /// Gets or sets TimeZone.
        /// </summary>
        string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets ClockType.
        /// </summary>
        ClockType ClockType { get; set; }
    }
}