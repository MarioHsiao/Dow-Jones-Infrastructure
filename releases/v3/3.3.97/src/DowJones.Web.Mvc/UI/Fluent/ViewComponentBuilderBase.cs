using System.ComponentModel;
using System.Web;
using System.Web.UI;

namespace DowJones.Web.Mvc.UI
{
    public abstract class ViewComponentBuilderBase<TViewComponent, TBuilder> : ComponentBuilderBase<TViewComponent, TBuilder>, IHtmlString
        where TViewComponent : ViewComponentBase, IHtmlString
        where TBuilder : ComponentBuilderBase<TViewComponent, TBuilder>
    {
        protected ViewComponentBuilderBase(TViewComponent component)
            : base(component)
        {
        }

        public TBuilder Named(string name)
        {
            Component.Name = name;
            return this as TBuilder;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual IHtmlString ToHtml()
        {
            return Component;
        }

        public TBuilder WithId(string id)
        {
            Component.ClientID = id;

            return this as TBuilder;
        }

        public string ToHtmlString()
        {
            return ToHtml().ToHtmlString();
        }

        public void Render(HtmlTextWriter writer)
        {
            Component.Render(writer);
        }
    }
}