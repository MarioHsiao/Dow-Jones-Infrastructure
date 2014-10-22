using Factiva.BusinessLayerLogic.Attributes;
using EMG.widgets.ui.dto.lob;

namespace EMG.widgets.ui.dto.request
{
    /// <summary>
    /// 
    /// </summary>
    public class RenderWidgetDTO : AbstractWidgetRequestDTO
    {
        // Callback params for JSONP format
        /// <summary>
        /// 
        /// </summary>
        [ParameterName("fnc")] public string callBackFunction;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("prm")] public string callBackParam;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("lng")] public string language = "en";

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("rspfmt")] public ResponseFormat responseFormat = ResponseFormat.JSONP;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("tkn")] public string token = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("typ")] public WidgetType type = WidgetType.AlertHeadlineWidget;
        
        /// <summary>
        /// 
        /// </summary>
        [ParameterName("target")] 
        public IntegrationTarget integrationTarget = IntegrationTarget.UnSpecified;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("st")] 
        public bool showTitle = true;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("rKey")]
        public string randomKey;

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("im")] public int isMct = 0;

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid()
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(token.Trim()))
                return false;
            return base.IsValid();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public override void Load()
        {
            if (IsValid())
            {
                // Itunes does a reduces the encoding to + and then it is sent out as a +
                // server reads it as a space --> change it to 
                token = token.Replace(' ', '+');
            }
            base.Load();
        }
    }
}