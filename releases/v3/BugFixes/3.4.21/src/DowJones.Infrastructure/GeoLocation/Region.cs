using System;

namespace DowJones.GeoLocation
{
    /// <summary>
    /// 
    /// </summary>
    public class Region
    {
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        /// <value>
        /// The name of the country.
        /// </value>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region" /> class.
        /// </summary>
        public Region()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region" /> class.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="countryName">Name of the country.</param>
        /// <param name="region">The region.</param>
        public Region(String countryCode, String countryName, String region)
        {
            CountryCode = countryCode;
            CountryName = countryName;
            Name = region;
        }
    }
}

