using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI
{
    /// <summary>
    /// Registry to keep track of all stylesheets required
    /// for an individual request
    /// </summary>
    public interface IStylesheetRegistry : IHtmlString, IClientResourceRegistry
    {
        void AddPageStyle(PageStyle pageStyle);
        IEnumerable<PageStyle> GetPageStyles();
        IEnumerable<ClientResource> GetStylesheets();
        void Render(HtmlTextWriter writer);
    }

    /// <summary>
    /// Registry to keep track of all stylesheets required
    /// for an individual request
    /// </summary>
    public class StylesheetRegistry : ViewComponentClientResourceRegistry, IStylesheetRegistry
    {
        private readonly ClientResourceCombiner _combiner;
        private readonly IList _renderedResources;
        private readonly IList<PageStyle> _pageStyles;

        public StylesheetRegistry(IClientResourceManager globalResourceManager, IViewComponentRegistry componentRegistry, ClientResourceCombiner combiner)
            : base(globalResourceManager, componentRegistry)
        {
            _combiner = combiner;
            _pageStyles = new List<PageStyle>();
            _renderedResources = new ArrayList();
        }

        public void AddPageStyle(PageStyle pageStyle)
        {
            Guard.IsNotNull(pageStyle, "pageStyle");
            Guard.IsNotNullOrEmpty(pageStyle.Selector, "pageStyle.Selector");
            Guard.IsNotNullOrEmpty(pageStyle.Style, "pageStyle.Style");

            _pageStyles.Add(pageStyle);
        }

        public IEnumerable<ClientResource> GetStylesheets()
        {
            var stylesheets = base.GetResources(x => x.ResourceKind == ClientResourceKind.Stylesheet);

            // HACK: Exclude embedded stylesheets altogether until we figure 
            //       out what we want to do with them
            var embeddedStylesheets = stylesheets.OfType<EmbeddedClientResource>();

            return stylesheets.Except(embeddedStylesheets);
        }

        public virtual IEnumerable<PageStyle> GetPageStyles()
        {
            return _pageStyles;
        }

        public virtual void Render(HtmlTextWriter htmlWriter)
        {
            WriteStylesheetIncludes(htmlWriter);
            WritePageStyles(htmlWriter);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual string ToHtmlString()
        {
            var htmlStringBuilder = new StringBuilder();
            using (var htmlWriter = new HtmlTextWriter(new StringWriter(htmlStringBuilder)))
            {
                htmlWriter.Indent = 1;
                Render(htmlWriter);
            }

            return htmlStringBuilder.ToString();
        }

        protected override void OnRegistered(ClientResource resource)
        {
            resource.ResourceKind = ClientResourceKind.Stylesheet;
        }

        protected void WriteStylesheetIncludes(HtmlTextWriter htmlWriter)
        {
            var unrenderedStyleSheets =
                GetStylesheets().Where(x => !_renderedResources.Contains(x));

            IEnumerable<string> unrenderedStyleSheetUrls =
               _combiner.GenerateCombinedResourceUrls(unrenderedStyleSheets);

            // Write out includes
            foreach (var url in unrenderedStyleSheetUrls)
            {
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, KnownMimeTypes.Stylesheet);
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Href, url);
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Link);
                htmlWriter.RenderEndTag();
                htmlWriter.WriteLine();

                _renderedResources.Add(url);
            }
        }

        protected void WritePageStyles(HtmlTextWriter writer)
        {
            IEnumerable<PageStyle> pageStyles =
                GetPageStyles().Where(x => !_renderedResources.Contains(x));

            if (pageStyles.Any())
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, KnownMimeTypes.Stylesheet);
                writer.RenderBeginTag(HtmlTextWriterTag.Style);

                foreach (var pageStyle in pageStyles)
                {
                    string style = pageStyle.Style;

                    if (!style.EndsWith(";"))
                        style += ";";

                    writer.WriteLine("{0} {{ {1} }}", pageStyle.Selector, style);

                    _renderedResources.Add(pageStyle);
                }

                writer.RenderEndTag();
            }
        }
    }

    public class PageStyle
    {
        public string Selector { get; set; }
        public string Style { get; set; }
    }
}
