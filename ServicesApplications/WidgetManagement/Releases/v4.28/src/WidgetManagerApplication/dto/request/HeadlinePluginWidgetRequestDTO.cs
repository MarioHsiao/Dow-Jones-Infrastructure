using EMG.widgets.ui.dto.lob;
using EMG.widgets.ui.Properties;
using Factiva.BusinessLayerLogic.Attributes;

namespace EMG.widgets.ui.dto.request
{
    /// <summary>
    /// 
    /// </summary>
    public class HeadlinePluginWidgetRequestDTO : AbstractWidgetRequestDTO
    {
        /// <summary>
        /// 
        /// </summary>
        [ParameterName("SA_FROM")]
        public string SA_FROM = "GL";

        /// <summary>
        /// 
        /// </summary>
        [ParameterName("returnurl")]
        public string doneUrl;



        /// <summary>
        /// 
        /// </summary>
        [ParameterName("refProd")]
        public WidgetRefererProduct refererProduct = WidgetRefererProduct.UnSpecified;

        public override void Load()
        {
            switch (SA_FROM.ToUpper())
            {
                case "GL":
                    SetWidgetRefererProduct(WidgetRefererProduct.FactivaDotCom);
                    if (string.IsNullOrEmpty(doneUrl))
                    {
                        doneUrl = Settings.Default.GL_BaseUrl;
                    }
                    break;
                case "FC":
                    SetWidgetRefererProduct(WidgetRefererProduct.FactivaCompaniesAndExecutives);
                    if (string.IsNullOrEmpty(doneUrl))
                    {
                        doneUrl = Settings.Default.FC_BaseUrl;
                    }
                    break;
                case "IF":
                    SetWidgetRefererProduct(WidgetRefererProduct.IWorksPremium);
                    if (string.IsNullOrEmpty(doneUrl))
                    {
                        doneUrl = Settings.Default.IF_BaseUrl;
                    }
                    break;
                case "IN":
                    SetWidgetRefererProduct(WidgetRefererProduct.Insight);
                    if (string.IsNullOrEmpty(doneUrl))
                    {
                        doneUrl = Settings.Default.IN_BaseUrl;
                    }
                    break;
            }
        }


        private void SetWidgetRefererProduct(WidgetRefererProduct value)
        {
            if (refererProduct == WidgetRefererProduct.UnSpecified)
            {
                refererProduct = value;
            }
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
