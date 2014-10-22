using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using EMG.widgets.ui.ajax.controls.WidgetDesigner;

[assembly: WebResource(WidgetDesignerExtender.JavaScriptBehaviorResourceLocation, "text/javascript")]
[assembly: WebResource(WidgetDesignerExtender.UIScriptLocation, "text/javascript")]
[assembly: WebResource(WidgetDesignerExtender.UICoreScriptLocation, "text/javascript")]
[assembly: WebResource(WidgetDesignerExtender.UIDraggableScriptLocation, "text/javascript")]
[assembly: WebResource(WidgetDesignerExtender.UIDroppableScriptLocation, "text/javascript")]
[assembly: WebResource(WidgetDesignerExtender.UISortableScriptLocation, "text/javascript")]
[assembly: WebResource(WidgetDesignerExtender.CssResourceLocation, "text/css")]
namespace EMG.widgets.ui.ajax.controls.WidgetDesigner
{
    /// <summary>
    /// 
    /// </summary>
    [Designer(typeof(WidgetDesignerDesigner))]
    [RequiredScript(typeof(CommonToolkitScripts), 0)]
    [ClientCssResource(CssResourceLocation)]
    /*[ClientScriptResource(BehaviorName, UIScriptLocation, LoadOrder = 0)] 
    [ClientScriptResource(BehaviorName, UICoreScriptLocation, LoadOrder = 0)]  
    [ClientScriptResource(BehaviorName, UIDraggableScriptLocation, LoadOrder = 2)]
    [ClientScriptResource(BehaviorName, UIDroppableScriptLocation, LoadOrder = 3)]
    [ClientScriptResource(BehaviorName, UISortableScriptLocation, LoadOrder = 4)]*/
    [ClientScriptResource(BehaviorName, JavaScriptBehaviorResourceLocation, LoadOrder = 0)]
    [TargetControlType(typeof(HtmlInputText))]
    [TargetControlType(typeof(TextBox))]
    [TargetControlType(typeof(HtmlInputHidden))]
    [TargetControlType((typeof(HtmlGenericControl)))]
    public class WidgetDesignerExtender : ExtenderControlBase
    {
        internal const string BaseResourceDirectory = "EMG.widgets.ui.ajax.controls.WidgetDesigner";
        internal const string BehaviorName = "EMG.widgets.ui.ajax.controls.WidgetDesigner.WidgetDesignerBehavior";
        internal const string CssResourceLocation = BaseResourceDirectory + ".WidgetDesigner.css";
        internal const string UIScriptLocation = BaseResourceDirectory + ".jquery_ui.js";
        internal const string UICoreScriptLocation = BaseResourceDirectory + ".ui.core.js";
        internal const string UIDraggableScriptLocation = BaseResourceDirectory + ".ui.draggable.js";
        internal const string UIDroppableScriptLocation = BaseResourceDirectory + ".ui.droppable.js";
        internal const string UISortableScriptLocation = BaseResourceDirectory + ".ui.sortable.js";
        internal const string JavaScriptBehaviorResourceLocation = BaseResourceDirectory + ".WidgetDesignerBehavior.js";

        #region [PROPERTIES]

        /// <summary>
        /// Set this property to specify which DOM element to append the extender to.
        /// </summary>
        /// <value>The append to ID.</value>
        [ExtenderControlProperty]
        [DefaultValue("")]
        [ClientPropertyName("appendInsideControlID")]
        public string AppendInsideControlID
        {
            get { return GetPropertyValue("AppendInsideControlID", string.Empty); }
            set { SetPropertyValue("AppendInsideControlID", value); }
        }

        /// <summary>
        /// Gets or sets the cancel.
        /// </summary>
        /// <value>The cancel.</value>
        [ExtenderControlProperty]
        [DefaultValue("ui-sortable-state-disabled")]
        [ClientPropertyName("cancel")]
        public string Cancel
        {
            get { return GetPropertyValue("Cancel", "ui-sortable-state-disabled"); }
            set { SetPropertyValue("Cancel", value); }
        }

        /// <summary>
        /// Gets or sets the containment.
        /// </summary>
        /// <value>The containment.</value>
        [ExtenderControlProperty]
        [DefaultValue("parent")]
        [ClientPropertyName("containment")]
        public string Containment
        {
            get { return GetPropertyValue("Containment", "parent"); }
            set { SetPropertyValue("Containment", value); }
        }

        /// <summary>
        /// Gets or sets the cursor.
        /// </summary>
        /// <value>The cursor.</value>
        [ExtenderControlProperty]
        [DefaultValue("w-resize")]
        [ClientPropertyName("cursor")]
        public string Cursor
        {
            get { return GetPropertyValue("Cursor", "w-resize"); }
            set { SetPropertyValue("Cursor", value); }
        }

        /// <summary>
        /// Gets or sets the placeholder.
        /// </summary>
        /// <value>The placeholder.</value>
        [ExtenderControlProperty]
        [DefaultValue("ui-sortable-placeholder")]
        [ClientPropertyName("placeholder")]
        public string Placeholder
        {
            get { return GetPropertyValue("Placeholder", "ui-sortable-placeholder"); }
            set { SetPropertyValue("Placeholder", value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WidgetDesignerExtender"/> is revert.
        /// </summary>
        /// <value><c>true</c> if revert; otherwise, <c>false</c>.</value>
        [ExtenderControlProperty]
        [DefaultValue(false)]
        [ClientPropertyName("revert")]
        public bool Revert
        {
            get { return GetPropertyValue("Revert", false); }
            set { SetPropertyValue("Revert", value); }
        }

        /// <summary>
        /// Gets or sets the list items.
        /// </summary>
        /// <value>The list items.</value>
        [ExtenderControlProperty]
        [DefaultValue("")]
        [ClientPropertyName("discoveryTabs")]
        public string DiscoveryTabs
        {
            get { return GetPropertyValue("DiscoveryTabs", string.Empty); }
            set { SetPropertyValue("DiscoveryTabs", value); }
        }

        #endregion

        #region [EVENTS]

        /// <summary>
        /// Gets or sets the on widget designer update.
        /// </summary>
        /// <value>The on widget designer update.</value>
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("widgetDesignerUpdate")]
        public string OnWidgetDesignerUpdate
        {
            get { return GetPropertyValue("OnWidgetDesignerUpdate", string.Empty); }
            set { SetPropertyValue("OnWidgetDesignerUpdate", value); }
        }

        #endregion

    }
}
