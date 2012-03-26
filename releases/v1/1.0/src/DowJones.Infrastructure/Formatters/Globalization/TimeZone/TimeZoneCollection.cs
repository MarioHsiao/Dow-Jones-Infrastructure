// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeZoneCollection.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace DowJones.Utilities.Formatters.Globalization.TimeZone
{
    /// <summary>
    /// Represents a collection of named time zones.
    /// </summary>
    public class TimeZoneCollection : ReadOnlyCollection<System.TimeZone>
    {
        /// <summary>
        /// The dictionary mapping time zones with their names.
        /// </summary>
        private readonly Dictionary<string, System.TimeZone> nameMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZoneCollection"/> class.
        /// </summary>
        /// <param name="data">The registry data to use when populating the collection.</param>
        internal TimeZoneCollection(RegistryKey data) : base(new List<System.TimeZone>())
        {
            var subKeyNames = data.GetSubKeyNames();
            nameMap = new Dictionary<string, System.TimeZone>();
            foreach (var name in subKeyNames)
            {
                using (RegistryKey key = data.OpenSubKey(name))
                {
                    var item = new UITimeZone(name, key);
                    Items.Add(item);
                    nameMap.Add(item.StandardName, item);
                }
            }
        }

        internal TimeZoneCollection() : base(new List<System.TimeZone>())
        {
            nameMap = new Dictionary<string, System.TimeZone>();
        }

        internal void Add(UITimeZone timeZone)
        {
            Items.Add(timeZone);
            nameMap.Add(timeZone.StandardName, timeZone);
            nameMap.Add(timeZone.FactivaCode, timeZone);
        }

        /// <summary>
        /// Gets the time zone associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the time zone to return.</param>
        public System.TimeZone this[string name]
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return nameMap[name]; }
        }
    }
}
