using System;
using System.Reflection;
using Factiva.BusinessLayerLogic.Attributes;
using Factiva.BusinessLayerLogic.DataTransferObject.WebWidgets;

namespace EMG.widgets.ui.delegates.core
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class WidgetDefinition
    {
        /// <summary>
        /// Accent color of widget.
        /// </summary>
        public string AccentColor = "#B4B4CC";

        /// <summary>
        /// Accent font color of the widget.
        /// </summary>
        public string AccentFontColor = "#171717";

        /// <summary>
        /// Auxiliary Information Font Color
        /// </summary>
        public string AuxInfoFontColor = "#666666";

        /// <summary>
        /// Background color for widget
        /// </summary>
        public string BackgroundColor = "#FFFFFF";

        /// <summary>
        /// Content font color of widget.
        /// </summary>
        public string ContentFontColor = "#333333";

        /// <summary>
        /// Description
        /// </summary>
        public string Description = string.Empty;

        /// <summary>
        /// Font Family
        /// </summary>
        public WidgetFontFamily FontFamily = WidgetFontFamily.Verdana;

        /// <summary>
        /// Font Size
        /// </summary>
        public WidgetFontSize FontSize = WidgetFontSize.Medium;

        /// <summary>
        /// Widget Template
        /// </summary>
        public WidgetTemplate WidgetTemplate = WidgetTemplate.EditDesign;

        /// <summary>
        /// Gets the widget template value.
        /// </summary>
        /// <value>The widget template value.</value>
        public string WidgetTemplateValue
        {
            get { return WidgetTemplate.ToString(); }
        }

        /// <summary>
        /// Id.
        /// </summary>
        public string Id = string.Empty;

        /// <summary>
        /// Language
        /// </summary>
        public string Language = string.Empty;

        /// <summary>
        /// Main color of widget
        /// </summary>
        public string MainColor = "#03468C";

        /// <summary>
        /// Main font color of widget.
        /// </summary>
        public string MainFontColor = "#F7F7F7";

        /// <summary>
        /// Name.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Snippet font color for widget.
        /// </summary>
        public string SnippetFontColor = "#999999";

        /// <summary>
        /// Gets the font family value.
        /// </summary>
        /// <value>The font family value.</value>
        public string FontFamilyValue
        {
            get { return FontFamily.ToString(); }
        }

        /// <summary>
        /// Gets the font size value.
        /// </summary>
        /// <value>The font size value.</value>
        public string FontSizeValue
        {
            get { return FontSize.ToString(); }
        }

        /// <summary>
        /// Gets the CSS font family.
        /// </summary>
        /// <value>The CSS font family.</value>
        public string CssFontFamily
        {
            get { return GetCSSValue(typeof (WidgetFontFamily), FontFamily.ToString()); }
        }

        /// <summary>
        /// Gets the size of the CSS font.
        /// </summary>
        /// <value>The size of the CSS font.</value>
        public string CssFontSize
        {
            get { return GetCSSValue(typeof (WidgetFontSize), FontSize.ToString()); }
        }

        /// <summary>
        /// Gets the content font size value.
        /// </summary>
        /// <value>The content font size value.</value>
        public int ContentFontSizeValue
        {
            get { return GetContentFontSize(FontSize); }
        }

        /// <summary>
        /// Gets the title font size value.
        /// </summary>
        /// <value>The title font size value.</value>
        public int TitleFontSizeValue
        {
            get { return GetTitleFontSize(FontSize); }
        }

        /// <summary>
        /// Gets the CSS value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        private static string GetCSSValue(Type type, string fieldName)
        {
            FieldInfo fieldInfo = type.GetField(fieldName);
            if (fieldInfo != null)
            {
                CssValue cssValue = (CssValue) Attribute.GetCustomAttribute(fieldInfo, typeof (CssValue));
                if (cssValue != null)
                {
                    return cssValue.Value;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the size of the title font.
        /// </summary>
        /// <param name="fontSize">Size of the font.</param>
        /// <returns></returns>
        private static int GetTitleFontSize(WidgetFontSize fontSize)
        {
            FieldInfo fieldInfo = typeof (WidgetFontSize).GetField(fontSize.ToString());
            if (fieldInfo != null)
            {
                TitleFontSize titleFontSize = (TitleFontSize) Attribute.GetCustomAttribute(fieldInfo, typeof (TitleFontSize));
                if (titleFontSize != null)
                {
                    return titleFontSize.Size;
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets the size of the content font.
        /// </summary>
        /// <param name="fontSize">Size of the font.</param>
        /// <returns></returns>
        private static int GetContentFontSize(WidgetFontSize fontSize)
        {
            FieldInfo fieldInfo = typeof (WidgetFontSize).GetField(fontSize.ToString());
            if (fieldInfo != null)
            {
                ContentFontSize contentFontSize = (ContentFontSize) Attribute.GetCustomAttribute(fieldInfo, typeof (ContentFontSize));
                if (contentFontSize != null)
                {
                    return contentFontSize.Size;
                }
            }
            return 0;
        }


        /// <summary>
        /// Distribution method of the widget. 
        /// </summary>
        public WidgetDistributionType DistributionType = WidgetDistributionType.OnlyUsersInMyAccount;

        /// <summary>
        /// Gets the distribution type value.
        /// </summary>
        /// <value>The distribution type value.</value>
        public string DistributionTypeValue
        {
            get { return DistributionType.ToString(); }
        }

        /// <summary>
        /// Authentication credentials for the widget
        /// </summary>
        public AuthenticationCredentials AuthenticationCredentials;

        protected WidgetDefinition()
        {
        }
    }
}

/// <summary>
/// 
/// </summary>
public class AuthenticationCredentials
{
    /// <summary>
    /// Authentication scheme for proxy credentials
    /// </summary>
    public WidgetAuthenticationScheme AuthenticationScheme = WidgetAuthenticationScheme.UserId;

    /// <summary>
    /// Gets the distribution type value.
    /// </summary>
    /// <value>The distribution type value.</value>
    public string AuthenticationSchemeValue
    {
        get { return AuthenticationScheme.ToString(); }
    }

    /// <summary>
    ///  proxy credentials user id;
    /// </summary>
    public string ProxyUserId;

    /// <summary>
    ///  proxy credentials password
    /// </summary>
    public string ProxyPassword;

    /// <summary>
    /// proxy credentials Email Address
    /// </summary>
    public string ProxyEmailAddress;

    /// <summary>
    /// proxy credentials namespace
    /// </summary>
    public string ProxyNamespace;

    /// <summary>
    /// proxy credentials profile id.
    /// </summary>
    public string ProfileId;

    /// <summary>
    /// 
    /// </summary>
    public string EncryptedToken;
}