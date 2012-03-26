// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPreferences.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Utilities.Formatters.Globalization;

namespace DowJones.Session
{
    public interface IPreferences
    {
        string InterfaceLanguage { get; set; }

        ContentLanguageCollection ContentLanguages { get; set; }

        string TimeZone { get; set; }

        ClockType ClockType { get; set; } 
    }
}