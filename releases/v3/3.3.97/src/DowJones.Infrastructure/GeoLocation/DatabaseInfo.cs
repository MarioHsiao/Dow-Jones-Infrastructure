/**
 * DatabaseInfo.java
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
    public class DatabaseInfo
    {
        #region DatabaseType enum

        /// <summary>
        /// 
        /// </summary>
        public enum DatabaseType
        {
            /// <summary>
            /// Country Edition
            /// </summary>
            CountryEdition = 1,

            /// <summary>
            /// 
            /// </summary>
            RegionEditionRev0 = 7
            ,

            /// <summary>
            /// 
            /// </summary>
            RegionEditionRev1 = 3,

            /// <summary>
            /// 
            /// </summary>
            CityEditionRev0 = 6,

            /// <summary>
            /// 
            /// </summary>
            CityEditionRev1 = 2,

            /// <summary>
            /// 
            /// </summary>
            OrgEdition = 5,

            /// <summary>
            /// 
            /// </summary>
            IspEdition = 4,

            /// <summary>
            /// 
            /// </summary>
            ProxyEdition = 8,

            /// <summary>
            /// 
            /// </summary>
            AsnumEdition = 9,

            /// <summary>
            /// 
            /// </summary>
            NetspeedEdition = 10,

            /// <summary>
            /// 
            /// </summary>
            DomainEdition = 11,

            /// <summary>
            /// 
            /// </summary>
            CountryEditionV6 = 12,

            /// <summary>
            /// 
            /// </summary>
            AsnumEditionV6 = 21,

            /// <summary>
            /// 
            /// </summary>
            IspEditionV6 = 22,

            /// <summary>
            /// 
            /// </summary>
            OrgEditionV6 = 23,

            /// <summary>
            /// 
            /// </summary>
            DomainEditionV6 = 24,

            /// <summary>
            /// 
            /// </summary>
            CityEditionRev1V6 = 30,

            /// <summary>
            /// 
            /// </summary>
            CityEditionRev0V6 = 31,

            /// <summary>
            /// 
            /// </summary>
            NetspeedEditionRev1 = 32,

            /// <summary>
            /// 
            /// </summary>
            NetspeedEditionRev1V6 = 33,
        }

        #endregion

        //private static SimpleDateFormat formatter = new SimpleDateFormat("yyyyMMdd"),

        private readonly string _info;


        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseInfo" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        public DatabaseInfo(string info)
        {
            _info = info;
        }

        /// <summary>
        /// Gets the type of the database.
        /// </summary>
        /// <returns></returns>
        public DatabaseType GetDatabaseType
        {
            get
            {
                if ((_info == null) | (_info == ""))
                {
                    return DatabaseType.CountryEdition;
                }
                // Get the type code from the database info string and then
                // subtract 105 from the value to preserve compatability with
                // databases from April 2003 and earlier.
                return (DatabaseType) Convert.ToInt32(_info.Substring(4, 3)) - 105;
            }
        }

        /**
         * Returns true if the database is the premium version.
         *
         * @return true if the premium version of the database.
         */

        /**
         * Returns the date of the database.
         *
         * @return the date of the database.
         */

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime Date
        {
            get
            {
                for (var i = 0; i < _info.Length - 9; i++)
                {
                    if (Char.IsWhiteSpace(_info[i]) != true) continue;
                    var dateString = _info.Substring(i + 1, 8);
                    try
                    {
                        //synchronized (formatter) {
                        return DateTime.ParseExact(dateString, "yyyyMMdd", null);
                        //}
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                    }
                    break;
                }
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsPremium()
        {
            return _info.IndexOf("FREE", StringComparison.Ordinal) < 0;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _info;
        }
    }
}