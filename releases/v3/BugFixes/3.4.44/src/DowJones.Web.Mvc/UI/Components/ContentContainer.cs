using System;
using System.Web.UI;

namespace DowJones.Web.Mvc.UI
{
    public class ContentContainer : CompositeComponent<ContentContainerModel>
    {
        public override string ClientPluginName
        {
            get { return null; }
        }

        protected override void WriteHtml(HtmlTextWriter writer)
        {
            RenderChildren(writer);
        }

        protected override void WriteContent(HtmlTextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}