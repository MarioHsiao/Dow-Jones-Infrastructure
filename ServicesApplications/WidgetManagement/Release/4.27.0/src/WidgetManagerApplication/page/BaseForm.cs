using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using log4net;

namespace EMG.widgets.ui.page
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseForm : HtmlForm
    {
        //private static readonly ILog m_Log = LogManager.GetLogger(typeof (BaseForm));

        private HtmlHead m_HtmlHead; 

        /// <summary>
        /// Gets or sets the target url of the form post.
        /// </summary>
        /// <remarks>Leave blank to postback to the same page.</remarks>
        new public virtual String Action
        {
            get
            {
                Object savedState = ViewState["Action"];
                if (savedState != null)
                {
                    return (String) savedState;
                }
                return GetBaseObjectActionAttribute();
            }
            set { ViewState["Action"] = value; }
        }

        /// <summary>
        /// Uses reflection to access the ClientOnSubmitEvent property of the Page class
        /// </summary>
        private String Page_ClientOnSubmitEvent
        {
            get { return (String) GetHideProperty(Page, typeof (Page), "ClientOnSubmitEvent"); }
        }

        /// <summary>
        /// purpose: Overridden to render custom Action attribute
        /// The main purpose of the Overridding is to grab the "action" attribute of the original 
        /// form
        /// </summary>
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            // Prepare our own action , method and name
            writer.WriteAttribute("name", Name);
            writer.WriteAttribute("method", Method.ToLower());
            writer.WriteAttribute("action", ResolveUrl(Action), true);
            // Remove From HtmlForm, with changes to Action
            Attributes.Remove("name");
            Attributes.Remove("method");
            Attributes.Remove("action");

            string submitEvent = Page_ClientOnSubmitEvent;
            // Now check the onsubmit event associated with Htmlform 
            if (!string.IsNullOrEmpty(submitEvent))
            {
                // ok.. this for has a "OnSubmit" 
                if (Attributes["onsubmit"] != null)
                {
                    submitEvent = submitEvent + Attributes["onsubmit"];
                    Attributes.Remove("onsubmit");
                }
                //Add some new Attributes to make the new form little more rich
                //writer.WriteAttribute("language", "javascript");
                writer.WriteAttribute("onsubmit", submitEvent);
            }
            writer.WriteAttribute("id", ClientID);

            // following is meant for HtmlContainerControl
            ViewState.Remove("innerhtml");
            // following is meant for HtmlControl
            Attributes.Render(writer);
        }

        /// <summary>
        /// Uses reflection to get the result of the private implementation of GetActionAttribute of the base class.
        /// </summary>
        /// <returns>Returns the normal action attribute of a server form.</returns>
        private String GetBaseObjectActionAttribute()
        {
            Type form = typeof (HtmlForm);
            MethodInfo actionMethod = form.GetMethod("GetActionAttribute", BindingFlags.Instance | BindingFlags.NonPublic);
            Object res = actionMethod.Invoke(this, null); //Invoke that method and get the value
            return (String) res;
        }

        /// <summary>
        /// Uses reflection to access any property on an object, even though the property is marked protected, internal, or private.
        /// </summary>
        /// <param name="target">The object being accessed</param>
        /// <param name="targetType">The Type to examine. Usually the Type of target arg, but sometimes a superclass of it.</param>
        /// <param name="propertyName">The name of the property to access.</param>
        /// <returns>The value of the property, or null if the property is not found.</returns>
        private static Object GetHideProperty(Object target, Type targetType, String propertyName)
        {
            PropertyInfo property = targetType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic);
            return property != null ? property.GetValue(target, null) : null;
        }

        /// <summary>
        /// Renders the <see cref="T:System.Web.UI.HtmlControls.HtmlForm"/> control to the specified <see cref="T:System.Web.UI.HtmlTextWriter"/> object.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> that receives the form control content.</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            // Find HtmlHeadControl
            foreach (Control child in Controls)
            {
                if (!(child is HtmlHead)) continue;
                m_HtmlHead = child as HtmlHead;
                break;
            }

            // if found take interior controls and add them to the main form.
            if (m_HtmlHead != null)
            {
                CleanUpHtmlHead();

            }

            base.RenderControl(writer);
        }

        private void CleanUpHtmlHead()
        {
            Controls.Remove(m_HtmlHead);
            for (int i = m_HtmlHead.Controls.Count - 1; i >= 0; i--)
            {
                Controls.AddAt(0, m_HtmlHead.Controls[i]);
            }
        }
    }
}