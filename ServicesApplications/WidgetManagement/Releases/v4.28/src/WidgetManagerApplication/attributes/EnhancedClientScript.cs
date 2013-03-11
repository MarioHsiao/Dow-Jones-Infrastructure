// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnhancedClientScript.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the EnhancedClientScriptAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using factiva.nextgen.ui.page;

namespace EMG.widgets.ui.attributes
{
    /// <summary>
    /// Enhanced ClientScript Attribute
    /// </summary>
    public class EnhancedClientScriptAttribute : ClientScriptAttribute
    {
        public EnhancedClientScriptAttribute(string url, int position, bool defer) : base(Enhance(url), position, defer)
        {
            
        }

        public EnhancedClientScriptAttribute(string url, int position)
            : base(Enhance(url), position)
        {
        }

        public EnhancedClientScriptAttribute(string url)
            : base(Enhance(url) + utility.Utility.GetVersion())
        {
        }

        protected static string Enhance(string url)
        {
            return url.Contains("?") ? string.Concat(url, "&v=", utility.Utility.GetVersion()) : url;
        }
    }
}
