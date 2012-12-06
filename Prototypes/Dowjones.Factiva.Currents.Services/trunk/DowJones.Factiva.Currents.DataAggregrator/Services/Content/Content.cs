using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using DowJones.Factiva.Currents.Common;
using DowJones.Factiva.Currents.Common.ExceptionHandling;
using DowJones.Factiva.Currents.Common.Logging;
using CurrentUtilities = DowJones.Factiva.Currents.Common.Utilities;
using Newtonsoft.Json;
using DowJones.Factiva.Currents.Common.Utilities;

namespace DowJones.Factiva.Currents.Aggregrator.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Content" in both code and config file together.
    public class Content : IContent
    {
        public Stream GetHeadlines(string format, string searchContextRef)
        {
            string result = string.Empty;
            MemoryStream stream = null;
            try
            {
                result = Common.GetHeadlines(format, searchContextRef);
            }
            catch (Exception ex)
            {
                result = ex.Message + "_" + ex.StackTrace;
            }
            finally
            {
                byte[] byteArray = Encoding.Default.GetBytes(result);
                stream = new MemoryStream(byteArray);
                CurrentUtilities.Web.SetResponseHeaders(EnumConverter<RequestFormat>.ConvertStringToEnum(format));
            }
            return stream;
        }

        public Stream GetHeadlinesByAccessionNumber(string accessionNumber, string format)
        {
            string result = string.Empty;
            MemoryStream stream = null;
            try
            {
                result = Common.GetHeadlinesByAccessionNumber(accessionNumber, format);
            }
            catch (Exception ex)
            {
                result = ex.Message + "_" + ex.StackTrace;
            }
            finally
            {
                byte[] byteArray = Encoding.Default.GetBytes(result);
                stream = new MemoryStream(byteArray);
                CurrentUtilities.Web.SetResponseHeaders(EnumConverter<RequestFormat>.ConvertStringToEnum(format));
            }
            return stream;
        }
    }
}
