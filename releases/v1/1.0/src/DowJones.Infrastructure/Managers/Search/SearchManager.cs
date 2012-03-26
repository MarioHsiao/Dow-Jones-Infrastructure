// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchManager.cs" company="Dow Jones">
//   � 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the SearchManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers.Search.Requests;
using Factiva.Gateway.Messages.Search;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Utils.V1_0;
using log4net;

namespace DowJones.Utilities.Managers.Search
{
    /// <summary>
    /// Search Manager Class
    /// </summary>
    public partial class SearchManager : AbstractAggregationManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SearchManager));

        public SearchManager(string sessionId, string clientTypeCode, string accessPointCode, string interfaceLanguage)
            : base(sessionId, clientTypeCode, accessPointCode)
        {
            InterfaceLanguage = interfaceLanguage;
        }

        public SearchManager(ControlData controlData, string interfaceLanguage)
            : base(controlData)
        {
            InterfaceLanguage = interfaceLanguage;
        }

        /// <summary>
        /// Gets or sets the interface language.
        /// </summary>
        /// <value>The interface language.</value>
        public string InterfaceLanguage { get; set; }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The logger.</value>
        protected override ILog Log
        {
            get { return _log; }
        }

        /// <summary>
        /// Generates the embargoed time.
        /// </summary>
        /// <param name="embargoTime">The embargo time.</param>
        /// <returns>A Search SearchString Object</returns>
        public static SearchString GenerateEmbargoedTime(int embargoTime)
        {
            if (embargoTime <= 0 && embargoTime >= 60)
            {
                throw new ArgumentOutOfRangeException("embargoTime", @"This can only be a value between 0-60.");
            }

            // Note: this code will only work in .NET Framework 3.5 or higher
            // Get the TimeZoneInfo for Coordinated Universal Time Zone. You need to 
            // You need to localize to UTC because the backend expects only GMT times 
            // and you must use that as a comparison for everyone.
            var utcTimeZone = TimeZoneInfo.Utc;

            // Set up a DateTime object to represent the current date. 
            var currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, utcTimeZone);

            // Subtract the embargo time in minutes
            var embDate = currentDate.Subtract(new TimeSpan(0, embargoTime, 0));
            string searchText;

            // Check to see if the time is 12:00 AM to 12:29 AM
            // else if, Check to see if time is 12:30 AM
            // else the time is greater 12:30 AM 
            if (currentDate.Hour == 0 && currentDate.Minute < embargoTime)
            {
                searchText = string.Concat("(date -1 and et < ", embDate.ToString("HHmmss"), ")", " or (date before -1)");
            }
            else if (currentDate.Hour == 0 && currentDate.Minute == embargoTime)
            {
                searchText = "date before 0";
            }
            else
            {
                searchText = string.Concat("(date +0 and et < ", embDate.ToString("HHmmss"), ")", " or (date before +0)");
            }

            var baseSrchStr = new SearchString
            {
                Value = searchText,
                Type = SearchType.Free,
                Mode = SearchMode.Traditional,
                Scope = string.Empty,
            };
            return baseSrchStr;
        }

        private IPerformContentSearchResponse GetPerformContentSearchResponse<TIPerformContentSearchRequest, TIPerformContentSearchResponse>(ISearchRequest searchRequest)
            where TIPerformContentSearchRequest : IPerformContentSearchRequest, new()
        {
            try
            {
                var sr = Invoke<TIPerformContentSearchResponse>(searchRequest.GetPerformContentSearchRequest<TIPerformContentSearchRequest>());
                if (sr != null)
                {
                    return (IPerformContentSearchResponse)sr.ObjectResponse;
                }
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
            }

            return null;
        }

        private IPerformContentSearchResponse GetPerformContentSearchResponse<TIPerformContentSearchRequest, TIPerformContentSearchResponse>(ControlData overriableControlData, ISearchRequest searchRequest) 
            where TIPerformContentSearchRequest : IPerformContentSearchRequest, new()
        {
            try
            {
                var sr = Invoke<TIPerformContentSearchResponse>(searchRequest.GetPerformContentSearchRequest<TIPerformContentSearchRequest>(), overriableControlData, true);

                if (sr != null)
                {
                    return (IPerformContentSearchResponse)sr.ObjectResponse;
                }
            }
            catch (DowJonesUtilitiesException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
            }

            return null;
        }

        private IPerformContentSearchResponse GetPerformContentSearchResponse<TIPerformContentSearchResponse>(ControlData overriableControlData, IPerformContentSearchRequest request)
    {
        try
        {
            var sr = Invoke<TIPerformContentSearchResponse>(request, overriableControlData);
            if (sr != null)
            {
                return (IPerformContentSearchResponse)sr.ObjectResponse;
            }
        }
        catch (DowJonesUtilitiesException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw DowJonesUtilitiesException.ParseExceptionMessage(ex);
        }

        return null;
    }
    }
}
