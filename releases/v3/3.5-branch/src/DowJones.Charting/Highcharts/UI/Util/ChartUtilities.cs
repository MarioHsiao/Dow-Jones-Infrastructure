using DowJones.Charting.Properties;
using RestSharp;

namespace DowJones.Charting.Highcharts.UI.Util
{
    public enum MimeType
    {
        Png,
        Jpeg,
        Pdf
    }

    public enum Constructor
    {
        Chart
    }

    public class ChartRequest
    {
        public string Svg { get; set; }
        public string Json { get; set; }
        public string Callback { get; set; }
        public MimeType MimeType { get; set; }
        public Constructor Constructor { get; set; }
        public int Width { get; set; }
        public int Scale { get; set; }
    }

    public class ChartUtilities
    {   
        private static string MapMimeType(MimeType type)
        {
            switch (type)
            {
                case MimeType.Pdf:
                    return "application/pdf";
                case MimeType.Png:
                    return "image/png";
                default:
                    return "image/jpeg";
            }
        }

        private static string MapConstrutor(Constructor constructor)
        {
            switch (constructor)
            {
                default:
                    return "Chart";
            }
        }

        protected internal byte[] GetBytes(string baseUrl, string resource, ChartRequest chartRequest)
        {
            var client = new RestClient(Settings.Default.BaseUrl);
            var request = new RestRequest(Settings.Default.Resource, Method.POST);
            if (!string.IsNullOrEmpty(chartRequest.Svg))
            {
                request.AddParameter("svg", chartRequest.Svg);
            }
            else
            {
                request.AddParameter("options", chartRequest.Json);
            }

            request.AddParameter("mimeType", MapMimeType(chartRequest.MimeType));
            request.AddParameter("constr", MapConstrutor(chartRequest.Constructor));
            request.AddParameter("callback", chartRequest.Callback);
            request.AddParameter("scale", chartRequest.Scale);
            request.AddParameter("idisposition", false.ToString().ToLower());
            request.AddParameter("width", chartRequest.Width);

            // add content type to header
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            // easily add HTTP Headers
            var response = client.Execute(request);
            return response.RawBytes;
        }

        public byte[] GetChartBytes(ChartRequest chartRequest)
        {
            return GetBytes(Settings.Default.BaseUrl, Settings.Default.Resource, chartRequest);
        }
    }
}