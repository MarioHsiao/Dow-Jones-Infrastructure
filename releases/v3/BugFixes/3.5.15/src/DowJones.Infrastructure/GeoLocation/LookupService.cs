/**
 * LookupService.cs
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
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using DowJones.DependencyInjection;
using DowJones.Managers.Abstract;
using log4net;

namespace DowJones.GeoLocation
{

    /// <summary>
    /// 
    /// </summary>
    public class LookupService : IExternalService, IDisposable
    {
        private readonly Stream _resourceStream;
        private DatabaseInfo _databaseInfo;
        private DatabaseInfo.DatabaseType _databaseType = DatabaseInfo.DatabaseType.CountryEdition;
        private int[] _databaseSegments;
        private int _recordLength;
        private readonly DataBaseOptions _dbOptions;
        private byte[] _dbbuffer;
        private bool _disposed;

        private static readonly Country UnknownCountry = new Country("--", "N/A");
        private const string ResourceName = "GeoIPCountryWhois.csv";
        private static readonly ILog Log = LogManager.GetLogger(typeof(LookupService));

        private const int CountryBegin = 16776960;
        private const int StructureInfoMaxSize = 20;
        private const int DatabaseInfoMaxSize = 100;
        private const int FullRecordLength = 100; //???
        private const int SegmentRecordLength = 3;
        private const int StandardRecordLength = 3;
        private const int OrgRecordLength = 4;
        private const int MaxRecordLength = 4;
        private const int MaxOrgRecordLength = 1000; //???
        private const int FipsRange = 360;
        private const int StateBeginRev0 = 16700000;
        private const int StateBeginRev1 = 16000000;
        private const int UsOffset = 1;
        private const int CanadaOffset = 677;
        private const int WorldOffset = 1353;



        /// <summary>
        /// 
        /// </summary>
        public enum DataBaseOptions
        {
            /// <summary>
            /// Standard
            /// </summary>
            Standard,

            /// <summary>
            ///  Memory Cache
            /// </summary>
            MemoryCache,
        }

        /// <summary>
        /// 
        /// </summary>
        public enum Speed
        {
            /// <summary>
            /// Unknown Speed
            /// </summary>
            Unknown,
            /// <summary>
            /// Dial up  
            /// </summary>
            Dialup,
            /// <summary>
            /// Cable
            /// </summary>
            Cable,
            /// <summary>
            /// Corpan WAN or LAN
            /// </summary>
            Corporate 
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; private set; }

        private static readonly string[] CountryCode =
            {
                "--", "AP", "EU", "AD", "AE", "AF", "AG", "AI", "AL", "AM", "AN", "AO", "AQ", "AR",
                "AS", "AT", "AU", "AW", "AZ", "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BJ",
                "BM", "BN", "BO", "BR", "BS", "BT", "BV", "BW", "BY", "BZ", "CA", "CC", "CD", "CF",
                "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "CR", "CU", "CV", "CX", "CY", "CZ",
                "DE", "DJ", "DK", "DM", "DO", "DZ", "EC", "EE", "EG", "EH", "ER", "ES", "ET", "FI",
                "FJ", "FK", "FM", "FO", "FR", "FX", "GA", "GB", "GD", "GE", "GF", "GH", "GI", "GL",
                "GM", "GN", "GP", "GQ", "GR", "GS", "GT", "GU", "GW", "GY", "HK", "HM", "HN", "HR",
                "HT", "HU", "ID", "IE", "IL", "IN", "IO", "IQ", "IR", "IS", "IT", "JM", "JO", "JP",
                "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW", "KY", "KZ", "LA", "LB", "LC",
                "LI", "LK", "LR", "LS", "LT", "LU", "LV", "LY", "MA", "MC", "MD", "MG", "MH", "MK",
                "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY",
                "MZ", "NA", "NC", "NE", "NF", "NG", "NI", "NL", "NO", "NP", "NR", "NU", "NZ", "OM",
                "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "PR", "PS", "PT", "PW", "PY",
                "QA", "RE", "RO", "RU", "RW", "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ",
                "SK", "SL", "SM", "SN", "SO", "SR", "ST", "SV", "SY", "SZ", "TC", "TD", "TF", "TG",
                "TH", "TJ", "TK", "TM", "TN", "TO", "TL", "TR", "TT", "TV", "TW", "TZ", "UA", "UG",
                "UM", "US", "UY", "UZ", "VA", "VC", "VE", "VG", "VI", "VN", "VU", "WF", "WS", "YE",
                "YT", "RS", "ZA", "ZM", "ME", "ZW", "A1", "A2", "O1", "AX", "GG", "IM", "JE", "BL",
                "MF"
            };

        private static readonly string[] CountryName =
            {
                "N/A", "Asia/Pacific Region", "Europe", "Andorra", "United Arab Emirates",
                "Afghanistan", "Antigua and Barbuda", "Anguilla", "Albania", "Armenia",
                "Netherlands Antilles", "Angola", "Antarctica", "Argentina", "American Samoa",
                "Austria", "Australia", "Aruba", "Azerbaijan", "Bosnia and Herzegovina",
                "Barbados", "Bangladesh", "Belgium", "Burkina Faso", "Bulgaria", "Bahrain",
                "Burundi", "Benin", "Bermuda", "Brunei Darussalam", "Bolivia", "Brazil", "Bahamas",
                "Bhutan", "Bouvet Island", "Botswana", "Belarus", "Belize", "Canada",
                "Cocos (Keeling) Islands", "Congo, The Democratic Republic of the",
                "Central African Republic", "Congo", "Switzerland", "Cote D'Ivoire",
                "Cook Islands", "Chile", "Cameroon", "China", "Colombia", "Costa Rica", "Cuba",
                "Cape Verde", "Christmas Island", "Cyprus", "Czech Republic", "Germany",
                "Djibouti", "Denmark", "Dominica", "Dominican Republic", "Algeria", "Ecuador",
                "Estonia", "Egypt", "Western Sahara", "Eritrea", "Spain", "Ethiopia", "Finland",
                "Fiji", "Falkland Islands (Malvinas)", "Micronesia, Federated States of",
                "Faroe Islands", "France", "France, Metropolitan", "Gabon", "United Kingdom",
                "Grenada", "Georgia", "French Guiana", "Ghana", "Gibraltar", "Greenland", "Gambia",
                "Guinea", "Guadeloupe", "Equatorial Guinea", "Greece",
                "South Georgia and the South Sandwich Islands", "Guatemala", "Guam",
                "Guinea-Bissau", "Guyana", "Hong Kong", "Heard Island and McDonald Islands",
                "Honduras", "Croatia", "Haiti", "Hungary", "Indonesia", "Ireland", "Israel", "India",
                "British Indian Ocean Territory", "Iraq", "Iran, Islamic Republic of",
                "Iceland", "Italy", "Jamaica", "Jordan", "Japan", "Kenya", "Kyrgyzstan", "Cambodia",
                "Kiribati", "Comoros", "Saint Kitts and Nevis",
                "Korea, Democratic People's Republic of", "Korea, Republic of", "Kuwait",
                "Cayman Islands", "Kazakhstan", "Lao People's Democratic Republic", "Lebanon",
                "Saint Lucia", "Liechtenstein", "Sri Lanka", "Liberia", "Lesotho", "Lithuania",
                "Luxembourg", "Latvia", "Libyan Arab Jamahiriya", "Morocco", "Monaco",
                "Moldova, Republic of", "Madagascar", "Marshall Islands",
                "Macedonia, the Former Yugoslav Republic of", "Mali", "Myanmar", "Mongolia",
                "Macau", "Northern Mariana Islands", "Martinique", "Mauritania", "Montserrat",
                "Malta", "Mauritius", "Maldives", "Malawi", "Mexico", "Malaysia", "Mozambique",
                "Namibia", "New Caledonia", "Niger", "Norfolk Island", "Nigeria", "Nicaragua",
                "Netherlands", "Norway", "Nepal", "Nauru", "Niue", "New Zealand", "Oman", "Panama",
                "Peru", "French Polynesia", "Papua New Guinea", "Philippines", "Pakistan",
                "Poland", "Saint Pierre and Miquelon", "Pitcairn", "Puerto Rico", "Palestinian Territory, Occupied", "Portugal", "Palau", "Paraguay", "Qatar",
                "Reunion", "Romania", "Russian Federation", "Rwanda", "Saudi Arabia",
                "Solomon Islands", "Seychelles", "Sudan", "Sweden", "Singapore", "Saint Helena",
                "Slovenia", "Svalbard and Jan Mayen", "Slovakia", "Sierra Leone", "San Marino",
                "Senegal", "Somalia", "Suriname", "Sao Tome and Principe", "El Salvador",
                "Syrian Arab Republic", "Swaziland", "Turks and Caicos Islands", "Chad",
                "French Southern Territories", "Togo", "Thailand", "Tajikistan", "Tokelau",
                "Turkmenistan", "Tunisia", "Tonga", "Timor-Leste", "Turkey", "Trinidad and Tobago",
                "Tuvalu", "Taiwan", "Tanzania, United Republic of", "Ukraine", "Uganda",
                "United States Minor Outlying Islands", "United States", "Uruguay", "Uzbekistan",
                "Holy See (Vatican City State)", "Saint Vincent and the Grenadines",
                "Venezuela", "Virgin Islands, British", "Virgin Islands, U.S.", "Vietnam",
                "Vanuatu", "Wallis and Futuna", "Samoa", "Yemen", "Mayotte", "Serbia",
                "South Africa", "Zambia", "Montenegro", "Zimbabwe", "Anonymous Proxy",
                "Satellite Provider", "Other",
                "Aland Islands", "Guernsey", "Isle of Man", "Jersey", "Saint Barthelemy",
                "Saint Martin"
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupService" /> class.
        /// </summary>
        /// <param name="option">The option.</param>
        public LookupService(DataBaseOptions option)
        {
            try
            {
                _resourceStream = GetType().Assembly.GetManifestResourceStream(GetType(), ResourceName);
                _dbOptions = option;
                Init();
            }
            catch (SystemException)
            {
                Console.Write("initialization of lookup service failed");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupService" /> class.
        /// </summary>
        [Inject("This is the primary construtor")]
        public LookupService()
            : this(DataBaseOptions.Standard)
        {
        }

        private void Init()
        {
            int i;
            var delim = new byte[3];
            var buf = new byte[SegmentRecordLength];
            _databaseType = DatabaseInfo.DatabaseType.CountryEdition;
            _recordLength = StandardRecordLength;
            //_resourceStream.Seek(_resourceStream.Length() - 3,SeekOrigin.Begin);
            _resourceStream.Seek(-3, SeekOrigin.End);
            for (i = 0; i < StructureInfoMaxSize; i++)
            {
                _resourceStream.Read(delim, 0, 3);
                if (delim[0] == 255 && delim[1] == 255 && delim[2] == 255)
                {
                    _databaseType = (DatabaseInfo.DatabaseType)Convert.ToByte(_resourceStream.ReadByte());
                    if ((int)_databaseType >= 106)
                    {
                        // Backward compatibility with databases from April 2003 and earlier
                        _databaseType -= 105;
                    }
                    // Determine the database type.
                    if (_databaseType == DatabaseInfo.DatabaseType.RegionEditionRev0)
                    {
                        _databaseSegments = new int[1];
                        _databaseSegments[0] = StateBeginRev0;
                        _recordLength = StandardRecordLength;
                    }
                    else if (_databaseType == DatabaseInfo.DatabaseType.RegionEditionRev1)
                    {
                        _databaseSegments = new int[1];
                        _databaseSegments[0] = StateBeginRev1;
                        _recordLength = StandardRecordLength;
                    }
                    else if (_databaseType == DatabaseInfo.DatabaseType.CityEditionRev0 ||
                             _databaseType == DatabaseInfo.DatabaseType.CityEditionRev1 ||
                             _databaseType == DatabaseInfo.DatabaseType.OrgEdition ||
                             _databaseType == DatabaseInfo.DatabaseType.OrgEditionV6 ||
                             _databaseType == DatabaseInfo.DatabaseType.IspEdition ||
                             _databaseType == DatabaseInfo.DatabaseType.IspEditionV6 ||
                             _databaseType == DatabaseInfo.DatabaseType.AsnumEdition ||
                             _databaseType == DatabaseInfo.DatabaseType.AsnumEditionV6 ||
                             _databaseType == DatabaseInfo.DatabaseType.NetspeedEditionRev1||
                             _databaseType == DatabaseInfo.DatabaseType.NetspeedEditionRev1V6 ||
                             _databaseType == DatabaseInfo.DatabaseType.CityEditionRev0V6 ||
                             _databaseType == DatabaseInfo.DatabaseType.CityEditionRev1V6
                        )
                    {
                        _databaseSegments = new int[1];
                        _databaseSegments[0] = 0;
                        if (_databaseType == DatabaseInfo.DatabaseType.CityEditionRev0 ||
                            _databaseType == DatabaseInfo.DatabaseType.CityEditionRev1 ||
                            _databaseType == DatabaseInfo.DatabaseType.AsnumEditionV6 ||
                            _databaseType == DatabaseInfo.DatabaseType.NetspeedEditionRev1 ||
                            _databaseType == DatabaseInfo.DatabaseType.NetspeedEditionRev1V6 ||
                            _databaseType == DatabaseInfo.DatabaseType.CityEditionRev0V6 ||
                            _databaseType == DatabaseInfo.DatabaseType.CityEditionRev1V6 ||
                            _databaseType == DatabaseInfo.DatabaseType.AsnumEdition
                            )
                        {
                            _recordLength = StandardRecordLength;
                        }
                        else
                        {
                            _recordLength = OrgRecordLength;
                        }
                        _resourceStream.Read(buf, 0, SegmentRecordLength);
                        int j;
                        for (j = 0; j < SegmentRecordLength; j++)
                        {
                            _databaseSegments[0] += (UnsignedByteToInt(buf[j]) << (j*8));
                        }
                    }
                    break;
                }
                //_resourceStream.Seek(_resourceStream.get_resourceStreamPointer() - 4);
                _resourceStream.Seek(-4, SeekOrigin.Current);
                //_resourceStream.Seek(_resourceStream.position-4,SeekOrigin.Begin);
            }
            if ((_databaseType == DatabaseInfo.DatabaseType.CountryEdition) ||
                (_databaseType == DatabaseInfo.DatabaseType.CountryEditionV6) ||
                (_databaseType == DatabaseInfo.DatabaseType.ProxyEdition) ||
                (_databaseType == DatabaseInfo.DatabaseType.NetspeedEdition))
            {
                _databaseSegments = new int[1];
                _databaseSegments[0] = CountryBegin;
                _recordLength = StandardRecordLength;
            }
            if (_dbOptions == DataBaseOptions.Standard) return;
            var l = (int) _resourceStream.Length;
            _dbbuffer = new byte[l];
            _resourceStream.Seek(0, SeekOrigin.Begin);
            _resourceStream.Read(_dbbuffer, 0, l);
        }

        

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        public Country GetCountry(IPAddress ipAddress)
        {
            return GetCountry(BytestoLong(ipAddress.GetAddressBytes()));
        }

        /// <summary>
        /// Gets the country v6.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        public Country GetCountryV6(string ipAddress)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(ipAddress);
            }
                //catch (UnknownHostException e) {
            catch (Exception e)
            {
                Console.Write(e.Message);
                return UnknownCountry;
            }
            return GetCountryV6(addr);
        }

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        public Country GetCountry(string ipAddress)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(ipAddress);
            }
                //catch (UnknownHostException e) {
            catch (Exception e)
            {
                Console.Write(e.Message);
                return UnknownCountry;
            }
            //  return getCountry(bytestoLong(addr.GetAddressBytes()));
            return GetCountry(BytestoLong(addr.GetAddressBytes()));
        }

        /// <summary>
        /// Gets the country v6.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public Country GetCountryV6(IPAddress ipAddress)
        {
            if (_resourceStream == null)
            {
                //throw new IllegalStateException("Database has been closed.");
                throw new Exception("Database has been closed.");
            }
            if ((_databaseType == DatabaseInfo.DatabaseType.CityEditionRev1) |
                (_databaseType == DatabaseInfo.DatabaseType.CityEditionRev0))
            {
                var l = GetLocation(ipAddress);
                return l == null ? UnknownCountry : new Country(l.CountryCode, l.CountryName);
            }
            var ret = SeekCountryV6(ipAddress) - CountryBegin;
            return ret == 0 ? UnknownCountry : new Country(CountryCode[ret], CountryName[ret]);
        }

        /// <summary>
        /// Gets the country.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public Country GetCountry(long ipAddress)
        {
            if (_resourceStream == null)
            {
                //throw new IllegalStateException("Database has been closed.");
                throw new Exception("Database has been closed.");
            }
            if ((_databaseType == DatabaseInfo.DatabaseType.CityEditionRev1) |
                (_databaseType == DatabaseInfo.DatabaseType.CityEditionRev0))
            {
                var l = GetLocation(ipAddress);
                return l == null ? UnknownCountry : new Country(l.CountryCode, l.CountryName);
            }
            var ret = SeekCountry(ipAddress) - CountryBegin;
            return ret == 0 ? UnknownCountry : new Country(CountryCode[ret], CountryName[ret]);
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        public int GetId(string ipAddress)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(ipAddress);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return 0;
            }
            return GetId(BytestoLong(addr.GetAddressBytes()));
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        public int GetId(IPAddress ipAddress)
        {

            return GetId(BytestoLong(ipAddress.GetAddressBytes()));
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public int GetId(long ipAddress)
        {
            if (_resourceStream == null)
            {
                throw new Exception("Database has been closed.");
            }
            var ret = SeekCountry(ipAddress) - _databaseSegments[0];
            return ret;
        }

        /// <summary>
        /// Gets the database info.
        /// </summary>
        /// <returns></returns>
        public DatabaseInfo GetDatabaseInfo()
        {
            if (_databaseInfo != null)
            {
                return _databaseInfo;
            }
            try
            {
                // Synchronize since we're accessing the database _resourceStream.
                lock (this)
                {
                    var hasStructureInfo = false;
                    var delim = new byte[3];
                    
                    // Advance to part of _resourceStream where database info is stored.
                    _resourceStream.Seek(-3, SeekOrigin.End);
                    for (var i = 0; i < StructureInfoMaxSize; i++)
                    {
                        _resourceStream.Read(delim, 0, 3);
                        if (delim[0] != 255 || delim[1] != 255 || delim[2] != 255) continue;
                        hasStructureInfo = true;
                        break;
                    }

                    _resourceStream.Seek(-3, hasStructureInfo ? SeekOrigin.Current : SeekOrigin.End);

                    // Find the database info string.
                    for (var i = 0; i < DatabaseInfoMaxSize; i++)
                    {
                        _resourceStream.Read(delim, 0, 3);
                        if (delim[0] == 0 && delim[1] == 0 && delim[2] == 0)
                        {
                            var dbInfo = new byte[i];
                            var dbInfo2 = new char[i];
                            _resourceStream.Read(dbInfo, 0, i);
                            for (var a0 = 0; a0 < i; a0++)
                            {
                                dbInfo2[a0] = Convert.ToChar(dbInfo[a0]);
                            }
                            // Create the database info object using the string.
                            _databaseInfo = new DatabaseInfo(new string(dbInfo2));
                            return _databaseInfo;
                        }
                        _resourceStream.Seek(-4, SeekOrigin.Current);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                //e.printStackTrace();
            }
            return new DatabaseInfo("");
        }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        public Region GetRegion(IPAddress ipAddress)
        {
            return GetRegion(BytestoLong(ipAddress.GetAddressBytes()));
        }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public Region GetRegion(string str)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(str);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }

            return GetRegion(BytestoLong(addr.GetAddressBytes()));
        }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <param name="ipnum">The ipnum.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Region GetRegion(long ipnum)
        {
            var record = new Region();
            int seekRegion;
            if (_databaseType == DatabaseInfo.DatabaseType.RegionEditionRev0)
            {
                seekRegion = SeekCountry(ipnum) - StateBeginRev0;
                var ch = new char[2];
                if (seekRegion >= 1000)
                {
                    record.CountryCode = "US";
                    record.CountryName = "United States";
                    ch[0] = (char) (((seekRegion - 1000)/26) + 65);
                    ch[1] = (char) (((seekRegion - 1000)%26) + 65);
                    record.Name = new string(ch);
                }
                else
                {
                    record.CountryCode = CountryCode[seekRegion];
                    record.CountryName = CountryName[seekRegion];
                    record.Name = "";
                }
            }
            else if (_databaseType == DatabaseInfo.DatabaseType.RegionEditionRev1)
            {
                seekRegion = SeekCountry(ipnum) - StateBeginRev1;
                var ch = new char[2];
                if (seekRegion < UsOffset)
                {
                    record.CountryCode = "";
                    record.CountryName = "";
                    record.Name = "";
                }
                else if (seekRegion < CanadaOffset)
                {
                    record.CountryCode = "US";
                    record.CountryName = "United States";
                    ch[0] = (char) (((seekRegion - UsOffset)/26) + 65);
                    ch[1] = (char) (((seekRegion - UsOffset)%26) + 65);
                    record.Name = new string(ch);
                }
                else if (seekRegion < WorldOffset)
                {
                    record.CountryCode = "CA";
                    record.CountryName = "Canada";
                    ch[0] = (char) (((seekRegion - CanadaOffset)/26) + 65);
                    ch[1] = (char) (((seekRegion - CanadaOffset)%26) + 65);
                    record.Name = new string(ch);
                }
                else
                {
                    record.CountryCode = CountryCode[(seekRegion - WorldOffset)/FipsRange];
                    record.CountryName = CountryName[(seekRegion - WorldOffset)/FipsRange];
                    record.Name = "";
                }
            }
            return record;
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <param name="addr">The addr.</param>
        /// <returns></returns>
        public Location GetLocation(IPAddress addr)
        {
            return GetLocation(BytestoLong(addr.GetAddressBytes()));
        }

        /// <summary>
        /// Gets the location v6.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public Location GetLocationV6(string str)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(str);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }

            return GetLocationV6(addr);
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public Location GetLocation(string str)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(str);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }

            return GetLocation(BytestoLong(addr.GetAddressBytes()));
        }

        /// <summary>
        /// Gets the location v6.
        /// </summary>
        /// <param name="addr">The addr.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Location GetLocationV6(IPAddress addr)
        {
            var recordBuf = new byte[FullRecordLength];
            var recordBuf2 = new char[FullRecordLength];
            var recordBufOffset = 0;
            var record = new Location();
            var strLength = 0;
            double latitude = 0, longitude = 0;

            try
            {
                var seekCountry = SeekCountryV6(addr);
                if (seekCountry == _databaseSegments[0])
                {
                    return null;
                }

                var recordPointer = seekCountry + ((2*_recordLength - 1)*_databaseSegments[0]);
                if (_dbOptions == DataBaseOptions.MemoryCache)
                {
                    Array.Copy(_dbbuffer, recordPointer, recordBuf, 0, Math.Min(_dbbuffer.Length - recordPointer, FullRecordLength));
                }
                else
                {
                    _resourceStream.Seek(recordPointer, SeekOrigin.Begin);
                    _resourceStream.Read(recordBuf, 0, FullRecordLength);
                }

                for (var a0 = 0; a0 < FullRecordLength; a0++)
                {
                    recordBuf2[a0] = Convert.ToChar(recordBuf[a0]);
                }

                // get country
                record.CountryCode = CountryCode[UnsignedByteToInt(recordBuf[0])];
                record.CountryName = CountryName[UnsignedByteToInt(recordBuf[0])];
                recordBufOffset++;

                // get region
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.Region = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += strLength + 1;
                strLength = 0;

                // get region_name
                record.RegionName = RegionName.GetRegionName(record.CountryCode, record.Region);

                // get city
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.City = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += (strLength + 1);
                strLength = 0;

                // get postal code
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.PostalCode = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += (strLength + 1);

                // get latitude
                for (var j = 0; j < 3; j++)
                    latitude += (UnsignedByteToInt(recordBuf[recordBufOffset + j]) << (j*8));
                record.Latitude = (float) latitude/10000 - 180;
                recordBufOffset += 3;

                // get longitude
                for (var j = 0; j < 3; j++)
                    longitude += (UnsignedByteToInt(recordBuf[recordBufOffset + j]) << (j*8));
                record.Longitude = (float) longitude/10000 - 180;

                record.MetroCode = record.DmaCode= 0;
                record.AreaCode = 0;
                if (_databaseType == DatabaseInfo.DatabaseType.CityEditionRev1
                    || _databaseType == DatabaseInfo.DatabaseType.CityEditionRev1V6)
                {
                    // get metro_code
                    var metroareaCombo = 0;
                    if (record.CountryCode == "US")
                    {
                        recordBufOffset += 3;
                        for (var j = 0; j < 3; j++)
                            metroareaCombo += (UnsignedByteToInt(recordBuf[recordBufOffset + j]) << (j*8));
                        record.MetroCode = record.DmaCode = metroareaCombo/1000;
                        record.AreaCode = metroareaCombo%1000;
                    }
                }
            }
            catch (IOException)
            {
                Console.Write("IO Exception while seting up segments");
            }
            return record;
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <param name="ipnum">The ipnum.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Location GetLocation(long ipnum)
        {
            var recordBuf = new byte[FullRecordLength];
            var recordBuf2 = new char[FullRecordLength];
            var recordBufOffset = 0;
            var record = new Location();
            var strLength = 0;
            double latitude = 0, longitude = 0;

            try
            {
                var seekCountry = SeekCountry(ipnum);
                if (seekCountry == _databaseSegments[0])
                {
                    return null;
                }
                var recordPointer = seekCountry + ((2*_recordLength - 1)*_databaseSegments[0]);
                if (_dbOptions == DataBaseOptions.MemoryCache )
                {
                    Array.Copy(_dbbuffer, recordPointer, recordBuf, 0, Math.Min(_dbbuffer.Length - recordPointer, FullRecordLength));
                }
                else
                {
                    _resourceStream.Seek(recordPointer, SeekOrigin.Begin);
                    _resourceStream.Read(recordBuf, 0, FullRecordLength);
                }
                for (var a0 = 0; a0 < FullRecordLength; a0++)
                {
                    recordBuf2[a0] = Convert.ToChar(recordBuf[a0]);
                }
                // get country
                record.CountryCode = CountryCode[UnsignedByteToInt(recordBuf[0])];
                record.CountryName = CountryName[UnsignedByteToInt(recordBuf[0])];
                recordBufOffset++;

                // get region
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.Region = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += strLength + 1;
                strLength = 0;

                // get region_name
                record.RegionName = RegionName.GetRegionName(record.CountryCode, record.Region);

                // get city
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.City = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += (strLength + 1);
                strLength = 0;

                // get postal code
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.PostalCode = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += (strLength + 1);

                // get latitude
                for (var j = 0; j < 3; j++)
                    latitude += (UnsignedByteToInt(recordBuf[recordBufOffset + j]) << (j*8));
                record.Latitude = (float) latitude/10000 - 180;
                recordBufOffset += 3;

                // get longitude
                for (var j = 0; j < 3; j++)
                    longitude += (UnsignedByteToInt(recordBuf[recordBufOffset + j]) << (j*8));
                record.Longitude = (float) longitude/10000 - 180;

                record.MetroCode = record.DmaCode = 0;
                record.AreaCode = 0;
                if (_databaseType == DatabaseInfo.DatabaseType.CityEditionRev1)
                {
                    // get metro_code
                    var metroareaCombo = 0;
                    if (record.CountryCode == "US")
                    {
                        recordBufOffset += 3;
                        for (var j = 0; j < 3; j++)
                            metroareaCombo += (UnsignedByteToInt(recordBuf[recordBufOffset + j]) << (j*8));
                        record.MetroCode = record.DmaCode = metroareaCombo/1000;
                        record.AreaCode = metroareaCombo%1000;
                    }
                }
            }
            catch (IOException)
            {
                Console.Write("IO Exception while seting up segments");
            }
            return record;
        }

        /// <summary>
        /// Gets the org.
        /// </summary>
        /// <param name="addr">The addr.</param>
        /// <returns></returns>
        public string GetOrg(IPAddress addr)
        {
            return GetOrg(BytestoLong(addr.GetAddressBytes()));
        }

        /// <summary>
        /// Gets the org v6.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public string GetOrgV6(string str)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(str);
            }
                //catch (UnknownHostException e) {
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
            return GetOrgV6(addr);
        }

        /// <summary>
        /// Gets the org.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public string GetOrg(string str)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(str);
            }
                //catch (UnknownHostException e) {
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
            return GetOrg(BytestoLong(addr.GetAddressBytes()));
        }

        /// <summary>
        /// Gets the org v6.
        /// </summary>
        /// <param name="addr">The addr.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetOrgV6(IPAddress addr)
        {
            var strLength = 0;
            var buf = new byte[MaxOrgRecordLength];
            var buf2 = new char[MaxOrgRecordLength];

            try
            {
                var seekOrg = SeekCountryV6(addr);
                if (seekOrg == _databaseSegments[0])
                {
                    return null;
                }

                var recordPointer = seekOrg + (2*_recordLength - 1)*_databaseSegments[0];
                if (_dbOptions == DataBaseOptions.MemoryCache)
                {
                    Array.Copy(_dbbuffer, recordPointer, buf, 0, Math.Min(_dbbuffer.Length - recordPointer, MaxOrgRecordLength));
                }
                else
                {
                    _resourceStream.Seek(recordPointer, SeekOrigin.Begin);
                    _resourceStream.Read(buf, 0, MaxOrgRecordLength);
                }
                while (buf[strLength] != 0)
                {
                    buf2[strLength] = Convert.ToChar(buf[strLength]);
                    strLength++;
                }
                buf2[strLength] = '\0';
                var orgBuf = new string(buf2, 0, strLength);
                return orgBuf;
            }
            catch (IOException)
            {
                Console.Write("IO Exception");
                return null;
            }
        }

        /// <summary>
        /// Gets the org.
        /// </summary>
        /// <param name="ipnum">The ipnum.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetOrg(long ipnum)
        {
            var strLength = 0;
            var buf = new byte[MaxOrgRecordLength];
            var buf2 = new char[MaxOrgRecordLength];

            try
            {
                var seekOrg = SeekCountry(ipnum);
                if (seekOrg == _databaseSegments[0])
                {
                    return null;
                }

                var recordPointer = seekOrg + (2*_recordLength - 1)*_databaseSegments[0];
                if (_dbOptions == DataBaseOptions.MemoryCache)
                {
                    Array.Copy(_dbbuffer, recordPointer, buf, 0, Math.Min(_dbbuffer.Length - recordPointer, MaxOrgRecordLength));
                }
                else
                {
                    _resourceStream.Seek(recordPointer, SeekOrigin.Begin);
                    _resourceStream.Read(buf, 0, MaxOrgRecordLength);
                }
                while (buf[strLength] != 0)
                {
                    buf2[strLength] = Convert.ToChar(buf[strLength]);
                    strLength++;
                }
                buf2[strLength] = '\0';
                var orgBuf = new string(buf2, 0, strLength);
                return orgBuf;
            }
            catch (IOException)
            {
                Console.Write("IO Exception");
                return null;
            }
        }

        /// <summary>
        /// Seeks the country v6.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private int SeekCountryV6(IPAddress ipAddress)
        {
            var v6Vec = ipAddress.GetAddressBytes();
            var buf = new byte[2*MaxRecordLength];
            var x = new int[2];
            var offset = 0;
            for (var depth = 127; depth >= 0; depth--)
            {
                try
                {
                    if (_dbOptions == DataBaseOptions.MemoryCache)
                    {
                        for (var i = 0; i < (2*MaxRecordLength); i++)
                        {
                            buf[i] = _dbbuffer[i + (2*_recordLength*offset)];
                        }
                    }
                    else
                    {
                        _resourceStream.Seek(2*_recordLength*offset, SeekOrigin.Begin);
                        _resourceStream.Read(buf, 0, 2*MaxRecordLength);
                    }
                }
                catch (IOException)
                {
                    Console.Write("IO Exception");
                }
                for (var i = 0; i < 2; i++)
                {
                    x[i] = 0;
                    for (var j = 0; j < _recordLength; j++)
                    {
                        int y = buf[(i*_recordLength) + j];
                        if (y < 0)
                        {
                            y += 256;
                        }
                        x[i] += (y << (j*8));
                    }
                }


                var bnum = 127 - depth;
                var idx = bnum >> 3;
                var bMask = 1 << (bnum & 7 ^ 7);
                if ((v6Vec[idx] & bMask) > 0)
                {
                    if (x[1] >= _databaseSegments[0])
                    {
                        return x[1];
                    }
                    offset = x[1];
                }
                else
                {
                    if (x[0] >= _databaseSegments[0])
                    {
                        return x[0];
                    }
                    offset = x[0];
                }
            }

            // shouldn't reach here
            Console.Write("Error Seeking country while Seeking " + ipAddress);
            return 0;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private int SeekCountry(long ipAddress)
        {
            var buf = new byte[2*MaxRecordLength];
            var x = new int[2];
            var offset = 0;
            for (var depth = 31; depth >= 0; depth--)
            {
                try
                {
                    if (_dbOptions == DataBaseOptions.MemoryCache)
                    {
                        for (var i = 0; i < (2*MaxRecordLength); i++)
                        {
                            buf[i] = _dbbuffer[i + (2*_recordLength*offset)];
                        }
                    }
                    else
                    {
                        _resourceStream.Seek(2*_recordLength*offset, SeekOrigin.Begin);
                        _resourceStream.Read(buf, 0, 2*MaxRecordLength);
                    }
                }
                catch (IOException)
                {
                    Console.Write("IO Exception");
                }
                for (var i = 0; i < 2; i++)
                {
                    x[i] = 0;
                    for (var j = 0; j < _recordLength; j++)
                    {
                        int y = buf[(i*_recordLength) + j];
                        if (y < 0)
                        {
                            y += 256;
                        }
                        x[i] += (y << (j*8));
                    }
                }

                if ((ipAddress & (1 << depth)) > 0)
                {
                    if (x[1] >= _databaseSegments[0])
                    {
                        return x[1];
                    }
                    offset = x[1];
                }
                else
                {
                    if (x[0] >= _databaseSegments[0])
                    {
                        return x[0];
                    }
                    offset = x[0];
                }
            }

            // shouldn't reach here
            Console.Write("Error Seeking country while Seeking " + ipAddress);
            return 0;

        }

        private static long Swapbytes(long ipAddress)
        {
            return (((ipAddress >> 0) & 255) << 24) | (((ipAddress >> 8) & 255) << 16)
                   | (((ipAddress >> 16) & 255) << 8) | (((ipAddress >> 24) & 255) << 0);
        }

        private static long BytestoLong(byte[] address)
        {
            long ipnum = 0;
            for (var i = 0; i < 4; ++i)
            {
                long y = address[i];
                if (y < 0)
                {
                    y += 256;
                }
                ipnum += y << ((3 - i)*8);
            }
            return ipnum;
        }

        private static int UnsignedByteToInt(byte b)
        {
            return b & 0xFF;
        }

         #region IDisposable Members

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Code to dispose managed resources held by the class
                    if (_resourceStream != null)
                    {
                        _resourceStream.Close();
                        _resourceStream.Dispose();
                    }
                }
            }
            // Code to dispose unmanaged resources held by the class
            _disposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="LookupService"/> is reclaimed by garbage collection.
        /// </summary>
        ~LookupService()
        {
            Dispose(false);
        }
        #endregion
    }
}