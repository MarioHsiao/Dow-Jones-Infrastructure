using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using DowJones.Extensions;

namespace DowJones.Utilities.Extensions.Web
{
    public static class HtmlTextWriterExtensions
    {
        private static readonly Action<HtmlTextWriter> RenderEmptyArea = x => { };

        public static void RenderSection(this HtmlTextWriter writer, string className, Action<HtmlTextWriter> renderAction = null, string title = null, HtmlTextWriterTag? tag = null)
        {
            if(title != null)
                writer.AddAttribute(HtmlTextWriterAttribute.Title, title);

            writer.RenderBeginTagWithClass(tag.GetValueOrDefault(HtmlTextWriterTag.Div), className);

                (renderAction ?? RenderEmptyArea).Invoke(writer);

            writer.RenderEndTag();
            writer.WriteLine();
        }

        public static void RenderHiddenSection(this HtmlTextWriter writer, string className, Action<HtmlTextWriter> contentRenderAction = null, string title = null, HtmlTextWriterTag? tag = null)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "display: none;");
            RenderSection(writer, className, contentRenderAction, title, tag);
        }

        public static void RenderLink(this HtmlTextWriter writer, string linkText, string href = null, string className = null, string title = null)
        {
            if (title != null) writer.AddAttribute(HtmlTextWriterAttribute.Title, title);

            writer.AddAttribute(HtmlTextWriterAttribute.Href, href ?? "javascript:void(0)");

            writer.RenderElement(HtmlTextWriterTag.A, className, linkText);
        }


        public static void RenderElement(this HtmlTextWriter writer, HtmlTextWriterTag tag, params string[] content)
        {
            RenderElement(writer, tag.ToString(), null, content);
        }

        public static void RenderElement(this HtmlTextWriter writer, HtmlTextWriterTag tag, string className, params string[] content)
        {
            RenderElement(writer, tag.ToString(), className, content);
        }

        public static void RenderElement(this HtmlTextWriter writer, string tagName, params string[] content)
        {
            RenderElement(writer, tagName, null, content);
        }

        public static void RenderElement(this HtmlTextWriter writer, string tagName, string className, params string[] content)
        {
            writer.RenderBeginTagWithClass(tagName, className);

            if (content != null)
            {
                foreach (var item in content.Where(x => !string.IsNullOrEmpty(x)))
                {
                    writer.WriteEncodedText(item);
                }
            }

            writer.RenderEndTag();
            writer.WriteLine();
        }
        
        public static void RenderBeginTagWithClass(this HtmlTextWriter writer, HtmlTextWriterTag tag, string className)
        {
            _RenderBeginTagWithClass(writer, tag.ToString(), className);
        }

        public static void RenderBeginTagWithClass(this HtmlTextWriter writer, string tagName, string className)
        {
            _RenderBeginTagWithClass(writer, tagName, className);
        }

        // ReSharper disable InconsistentNaming
        private static void _RenderBeginTagWithClass(this HtmlTextWriter writer, string tagName, string className)
        // ReSharper restore InconsistentNaming
        {
            if (!string.IsNullOrEmpty(className))
            {
                writer.AddAttribute("class", className);
            }

            writer.RenderBeginTag(tagName);
        }

        public static void AddAttributes(this HtmlTextWriter writer, IDictionary<string, object> attributes)
        {
            if (attributes.IsNullOrEmpty())
            {
                return;
            }

            foreach (var attribute in attributes)
            {
                writer.AddAttribute(attribute.Key, attribute.Value.ToString(), true);
            }
        }

        public static void RenderScriptInclude(this HtmlTextWriter writer, string scriptUrl)
        {
            writer.WriteLine("<script type=\"text/javascript\" src=\"{0}\"></script>", scriptUrl);
        }

    }
}