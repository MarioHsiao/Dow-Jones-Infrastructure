/**
 * Location.cs
 *
 * Copyright (C) 2008 MaxMind Inc.  All Rights Reserved.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */


using System;

namespace DowJones.GeoLocation
{

    /// <summary>
    /// 
    /// </summary>
    public class Location
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
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the dma code.
        /// </summary>
        /// <value>
        /// The dma code.
        /// </value>
        public int DmaCode { get; set; }

        /// <summary>
        /// Gets or sets the area code.
        /// </summary>
        /// <value>
        /// The area code.
        /// </value>
        public int AreaCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the region.
        /// </summary>
        /// <value>
        /// The name of the region.
        /// </value>
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the metro code.
        /// </summary>
        /// <value>
        /// The metro code.
        /// </value>
        public int MetroCode { get; set; }

        private const double EarthDiameter = 2*6378.2;
        private const double PI = 3.14159265;
        private const double RadConvert = PI/180;

        /// <summary>
        /// Distances the specified loc.
        /// </summary>
        /// <param name="loc">The loc.</param>
        /// <returns></returns>
        public double Distance(Location loc)
        {
            var lat1 = Latitude;
            var lon1 = Longitude;
            var lat2 = loc.Latitude;
            var lon2 = loc.Longitude;

            // convert degrees to radians
            lat1 *= RadConvert;
            lat2 *= RadConvert;

            // find the deltas
            var deltaLat = lat2 - lat1;
            var deltaLon = (lon2 - lon1)*RadConvert;

            // Find the great circle distance
            var temp = Math.Pow(Math.Sin(deltaLat/2), 2) + Math.Cos(lat1)*Math.Cos(lat2)*Math.Pow(Math.Sin(deltaLon/2), 2);
            return EarthDiameter*Math.Atan2(Math.Sqrt(temp), Math.Sqrt(1 - temp));
        }
    }
}