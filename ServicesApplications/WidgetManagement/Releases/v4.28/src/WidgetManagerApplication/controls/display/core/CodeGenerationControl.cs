// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGenerationControl.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the CodeGenerationControl type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using factiva.nextgen.ui;

namespace EMG.widgets.ui.controls.display
{
    /// <summary>
    /// </summary>
    public sealed class CodeGenerationControl : AbstractWidgetDesignerControl
    {
        private readonly HtmlGenericControl container = new HtmlGenericControl("div");

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGenerationControl"/> class.
        /// </summary>
        public CodeGenerationControl() : base("div", "5")
        {
            ID = "wdgtCodeGenCntrl";
            container.ID = string.Concat(ID, "Cnt");
            Attributes.CssStyle.Add("display", "none");
            Controls.Add(container);
        }

        /// <summary>
        /// Gets the directions control.
        /// </summary>
        /// <returns></returns>
        private static Control GetDirectionsControl()
        {
            // Right part of title
            var directionsControl = new HtmlGenericControl("div");
            directionsControl.Attributes.Add("class", "directionContainer");
            directionsControl.ID = "codeGenerationDirectionsControl";
            directionsControl.InnerHtml = string.Format("{0}", ResourceText.GetInstance.GetString("displayCodeDirections"));
            directionsControl.Controls.Add(GetCodeGenerationControl());
            return directionsControl;
        }

        private static Control GetCodeGenerationControl()
        {
            // Right part of title
            var directionsControl = new HtmlGenericControl("div");
            directionsControl.Attributes.Add("class", "codeGeneContainer");
            directionsControl.ID = "codeGenControl";
            directionsControl.InnerHtml = string.Format("<a id=\"showModalPopupClientButton\" class=\"button\" href=\"javascript:void(0)\" onclick=\"getCodeGenerationPopup();return false;\"><span>{0}</span></a>", ResourceText.GetInstance.GetString("displayCode"));
            return directionsControl;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            container.Controls.Add(GetTitleControl(ResourceText.GetInstance.GetString("codeGeneration")));
            container.Controls.Add(GetDirectionsControl());
            base.OnPreRender(e);
        }
    }
}