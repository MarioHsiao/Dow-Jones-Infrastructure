using System.Globalization;
using DowJones.DependencyInjection;
using DowJones.Globalization;
using DowJones.Newsletters.App.Web.Models.Api;
using Newtonsoft.Json;

namespace DowJones.Prod.X.Web.Models.Api
{
    public class BasicApiResult : IApiResultModel
    {
        [Inject("")]
        [JsonIgnore]
        public IResourceTextManager ResourceTextManager { get; set; }

        private string _errorMessage;

        public BasicApiResult() {}

        public BasicApiResult(long returnCode)
        {
            ReturnCode = returnCode;
        }

        [JsonProperty("returnCode") ]
        public long ReturnCode { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage {
            get
            {
                return _errorMessage ?? (ReturnCode == 0 ? null : ResourceTextManager.GetErrorMessage(ReturnCode.ToString(CultureInfo.InvariantCulture)));
            }

            set { _errorMessage = value; }
        }
    }
}