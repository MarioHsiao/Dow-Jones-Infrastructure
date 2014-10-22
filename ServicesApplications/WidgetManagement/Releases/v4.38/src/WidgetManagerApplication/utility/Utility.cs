using System;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using MimeTypeAttribute = EMG.widgets.ui.attributes.MimeType;
using factiva.nextgen.ui;
using EMG.Utility.Attributes;
using Factiva.BusinessLayerLogic.Attributes;

namespace EMG.widgets.ui.utility
{
    /// <summary>
    /// 
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Gets the type of the MIME.
        /// </summary>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <returns></returns>
        public static string GetMimeType(MimeType mimeType)
        {
            FieldInfo fieldInfo = typeof (MimeType).GetField(mimeType.ToString());
            if (fieldInfo != null)
            {
                MimeTypeAttribute mType = (MimeTypeAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof (MimeTypeAttribute));
                if (mType != null)
                {
                    return mType.Value;
                }
            }
            return string.Empty;
        }

        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Sets the header for client side browser cache duration.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        public static void SetHeaderCacheDuration(int minutes)
        {
            TimeSpan cacheDuration = TimeSpan.FromMinutes(1);
            FieldInfo maxAge = HttpContext.Current.Response.Cache.GetType().GetField("_maxAge",
                                                                         BindingFlags.Instance | BindingFlags.NonPublic);
            maxAge.SetValue(HttpContext.Current.Response.Cache, cacheDuration);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.Add(cacheDuration));
            HttpContext.Current.Response.Cache.AppendCacheExtension(
                "must-revalidate, proxy-revalidate");
        }

        public static DropDownList GetDropDownBaseOnEnum(string dropDownListId, Type enumType, string itemValue,
                                                         string onChangeJavascriptScript)
        {
            ListItemCollection lc = new ListItemCollection();
            foreach (String s in Enum.GetNames(enumType))
            {
                AssignedToken assignedToken =
                    (AssignedToken)Attribute.GetCustomAttribute(enumType.GetField(s), typeof(AssignedToken));

                ValidIWorksEntry _isValidForIWorksEntry = (ValidIWorksEntry)
                    Attribute.GetCustomAttribute(enumType.GetField(s), typeof(ValidIWorksEntry));

                if (!s.Equals("_Undefined"))
                {
                    if (assignedToken != null)
                        lc.Add(
                            new ListItem(assignedToken.Token,
                                         ((int)Enum.Parse(enumType, s)).ToString()));
                    else
                        lc.Add(new ListItem(ResourceText.GetInstance.GetString(s),
                                            ((int)Enum.Parse(enumType, s)).ToString()));
                }
            }

            DropDownList ddList = new DropDownList();
            ddList.ID = dropDownListId;
            ddList.DataSource = lc;
            ddList.DataTextField = "Text";
            ddList.DataValueField = "Value";
            ddList.DataBind();

            if (!string.IsNullOrEmpty(onChangeJavascriptScript))
            {
                ddList.Attributes.Add("onchange", onChangeJavascriptScript);
            }

            if (!string.IsNullOrEmpty(itemValue))
            {
                ListItem item = ddList.Items.FindByValue(((int)Enum.Parse(enumType, itemValue)).ToString());
                if (item != null)
                    item.Selected = true;
            }
            return ddList;
        }
    }
}