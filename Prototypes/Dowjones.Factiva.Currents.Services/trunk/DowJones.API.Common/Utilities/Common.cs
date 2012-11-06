using System;
using System.Net;
using DowJones.API.Common.ExceptionHandling;
using System.Collections.Generic;
using System.Linq;
using DowJones.API.Common.Logging;
using DowJones.Factiva.Currents.Aggregrator;

namespace DowJones.API.Common.Utilities
{
    public static class Common
    {
        public static int ToInt(this string str, long errorCode)
        {
            try
            {
                return Int32.Parse(str);
            }
            catch(Exception ex)
            {
                throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
            }
        }

        public static int? ToNullableInt(this string str, long errorCode)
        {
            int tempVal;
        
            try
            {
                return Int32.TryParse(str, out tempVal) ? tempVal : (int?)null;
            }
            catch (Exception ex)
            {
                throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
            }
        }

        public static int? ToNullableInt(this string str, long errorCode,bool isRequired, int? defaultValue)
        {
            int tempVal;
            if (string.IsNullOrWhiteSpace(str))
            {
                if (isRequired)
                    throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
                return defaultValue;
            }
            try
            {
                return Int32.TryParse(str, out tempVal) ? tempVal : (int?)null;
            }
            catch (Exception ex)
            {
                throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
            }
        }

        public static bool ToBool(this string str, long errorCode)
        {
            try
            {
                return Boolean.Parse(str.ToLower());
            }
            catch (Exception ex)
            {
               throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
            }
        }

        public static bool ToBool(this string str, long errorCode, bool isRequired, bool defaultValue)
        {
            if(string.IsNullOrWhiteSpace(str))
            {
                if(isRequired)
                    throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
                else
                    return defaultValue;
            }
            try
            {
                return Boolean.Parse(str.ToLower());
            }
            catch (Exception ex)
            {
                throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
            }
        }

        public static List<string> ConvertStringToList(string stringToSplit, long errorCode, bool removeDuplicates, bool isRequired)
        {
            List<string> codes = null;
            try
            {
                if (string.IsNullOrWhiteSpace(stringToSplit) && isRequired)
                    throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
                else
                {
                    if (!string.IsNullOrWhiteSpace(stringToSplit))
                    {
                        if (removeDuplicates)
                            codes = stringToSplit.Split(new char[] { Constants.DefaultDelimiter }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                        else
                            codes = stringToSplit.Split(new char[] { Constants.DefaultDelimiter }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
            }

            if (codes.Count == 0 && isRequired)
                throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);

            return codes;
        }

    }
}
