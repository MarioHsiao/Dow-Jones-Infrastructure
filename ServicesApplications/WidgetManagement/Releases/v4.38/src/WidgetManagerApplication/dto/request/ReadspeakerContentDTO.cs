using Factiva.BusinessLayerLogic.Attributes;
using EMG.widgets.ui.dto.lob;

namespace EMG.widgets.ui.dto.request
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadspeakerContentDTO : AbstractWidgetRequestDTO
    {
        /// <summary>
        /// 
        /// </summary>
        [ParameterName("tkn")]
        public string token = string.Empty;


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
    }
}
