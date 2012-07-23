// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFramePathConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the WebServicePathConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Web;

namespace DowJones.Converters
{
    public class WebServicePathConverter : IFramePathConverter
    {
    }

    public class IFramePathConverter : StringConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var url = (string)value;

                if (string.IsNullOrEmpty(url))
                {
                    var currentContext = HttpContext.Current;
                    if (currentContext != null)
                    {
                        return currentContext.Request.FilePath;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(url))
                    {
                        return url;
                    }

                    if (url[0] != '~')
                    {
                        return url;
                    }

                    if (url.Length == 1)
                    {
                        return string.Empty;
                    }

                    var indexOfUrl = 1;
                    if (url[1] == '/' || url[1] == '\\')
                    {
                        indexOfUrl = 2;
                    }

                    return string.Concat(GetApplicationPath(), url.Substring(indexOfUrl)).Replace("//", "/");
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        private static string GetApplicationPath()
        {
            var currentContext = HttpContext.Current;
            if (currentContext != null)
            {
                var applicationPath = currentContext.Request.ApplicationPath;

                if (applicationPath.Length > 1)
                {
                    return string.Concat(applicationPath, "/");
                }
            }

            return "/";
        }
    }
}