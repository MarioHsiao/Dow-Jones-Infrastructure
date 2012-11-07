using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DowJones.Factiva.Currents.Aggregrator
{
    public class EnumConverter1<T>
    {
        //public static T ConvertStringToEnum(string enumString, long errorCode, bool isRequired, T defaultValue)
        //{
        //    if (string.IsNullOrWhiteSpace(enumString))
        //    {
        //        if (isRequired)
        //            throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
        //        return defaultValue;
        //    }
        //    try
        //    {
        //        return (T)Enum.Parse(typeof(T), enumString, true);
        //    }
        //    catch
        //    {
        //        //686206:invalid parts
        //        throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
        //    }
        //}

        //public static T ConvertStringToEnum(string enumString, long errorCode, bool isRequired)
        //{
        //    if (string.IsNullOrWhiteSpace(enumString))
        //    {
        //        if (isRequired)
        //            throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
        //    }
        //    try
        //    {
        //        return (T)Enum.Parse(typeof(T), enumString, true);
        //    }
        //    catch
        //    {
        //        //686206:invalid parts
        //        throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
        //    }
        //}

        //public static T ConvertStringToEnum(string enumString)
        //{

        //    try
        //    {
        //        return (T)Enum.Parse(typeof(T), enumString, true);
        //    }
        //    catch
        //    {
        //        //686206:invalid parts
        //        throw ExceptionHandlerUtility.GetWebFaultByServiceException(ErrorConstants.EnumConverstionFailed, HttpStatusCode.BadRequest);
        //    }
        //}

        //public static List<T> ConvertStringToEnumList(string stringOfEnums, long errorCode, bool isRequired, List<T> defaultValue)
        //{
        //    var enumList = new List<T>();

        //    if (string.IsNullOrWhiteSpace(stringOfEnums))
        //    {
        //        if (isRequired)
        //            throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);

        //        return defaultValue;
        //    }

        //    var arrayOfEnums = stringOfEnums.Split(Constants.DefaultDelimiter);
        //    foreach (var enumString in arrayOfEnums)
        //    {
        //        if (!string.IsNullOrWhiteSpace(enumString))
        //        {
        //            try
        //            {
        //                enumList.Add((T)(Enum.Parse(typeof(T), enumString, true)));
        //            }
        //            catch
        //            {
        //                //686206:invalid parts
        //                throw ExceptionHandlerUtility.GetWebFaultByServiceException(errorCode, HttpStatusCode.BadRequest);
        //            }
        //        }
        //    }

        //    return enumList;
        //}

        //public static List<T> ConvertStringToEnumList(string stringOfEnums)
        //{
        //    var enumList = new List<T>();

        //    var arrayOfEnums = stringOfEnums.Split(Constants.DefaultDelimiter);
        //    foreach (var enumString in arrayOfEnums)
        //    {
        //        if (!string.IsNullOrWhiteSpace(enumString))
        //        {
        //            try
        //            {
        //                enumList.Add((T)(Enum.Parse(typeof(T), enumString, true)));
        //            }
        //            catch
        //            {
        //                //686206:invalid parts
        //                throw ExceptionHandlerUtility.GetWebFaultByServiceException(ErrorConstants.EnumConverstionFailed, HttpStatusCode.BadRequest);
        //            }
        //        }
        //    }

        //    return enumList;
        //}
    }
}
