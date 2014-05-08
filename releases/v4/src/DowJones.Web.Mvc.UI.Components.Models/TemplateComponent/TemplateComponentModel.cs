using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.UI.Components.TemplateComponent
{
    public class TemplateComponentModel : ViewComponentModel
    {
        #region ::: Client Properties :::
        // Options to be passed to the component
        // In the client side component code, these options will be accessible under
        // this.options.<clientPropertyName>

        [ClientProperty("textColor")]
        public string TextColor { get; set; }

        [ClientProperty("textSize")]
        public string TextSize { get; set; }

        #endregion

        #region ::: Client Data :::
        // If Data is not null, it will be passed to the client side component
        // as this.data

        [ClientData]
        public TemplateComponentData Data { get; set; }

        #endregion
    }
}
