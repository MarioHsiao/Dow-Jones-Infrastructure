using System.Collections.Generic;
using System.Web.UI;
using DowJones.Web;
using DowJones.Utilities.Extensions.Web;
using DowJones.Web.Mvc.UI.Canvas;

[assembly: WebResource(AbstractCanvasModule.ScriptFile, KnownMimeTypes.JavaScript)]
[assembly: WebResource(AbstractCanvasModule.PagerScriptFile, KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Mvc.UI.Canvas
{
    internal sealed class AbstractCanvasModule
    {
        internal const string BaseDirectory = "DowJones.Web.Mvc.UI.Canvas.Components";
        internal const string ScriptFile = BaseDirectory + ".AbstractCanvasModule.js";
        internal const string PagerScriptFile = BaseDirectory + ".AbstractModulePager.js";
    }

    [ScriptResource(
        "AbstractCanvasModule",
        ResourceName = AbstractCanvasModule.ScriptFile,
        DeclaringType = typeof(AbstractCanvasModule),
        DependencyLevel = ClientResourceDependencyLevel.MidLevel,
        PerformSubstitution=true
    )]
    public abstract class AbstractCanvasModule<TModule> : ViewComponentBase<TModule>, ICanvasModule
        where TModule : class, IModule
    {
        protected TModule Module
        {
            get { return Model; }
        }


        protected AbstractCanvasModule()
            : this(null)
        {
        }

        protected AbstractCanvasModule(IEnumerable<IViewComponent> children)
            : base(children)
        {
            CssClass = "module dj_module wrap-9";
        }

        protected override void WriteAttributes(HtmlTextWriter writer)
        {
            if (Model.CanEdit)
                CssClass += " dj_module-movable";

            HtmlAttributes.Add("data-module-id", Model.ModuleId);

            base.WriteAttributes(writer);
        }

        protected sealed override void WriteContent(HtmlTextWriter writer)
        {
            ComponentFactory.ScriptRegistry()
                .WithErrorManager();

            writer.RenderSection("module-header dj_module-header dj_module-handle", WriteHeader);
            
            if (Model.CanEdit)
                writer.RenderHiddenSection("module-edit-options dj_module-edit-options", WriteEditAreaContainer, tag: HtmlTextWriterTag.Form);

            writer.RenderSection("module-core dj_module-core", WriteModuleBody);
        }

        #region Customizable Content Sections

        protected virtual void WriteContentArea(HtmlTextWriter writer)
        {
            RenderSection("ContentArea", writer, isRequired: true);
        }

        protected virtual void WriteEditArea(HtmlTextWriter writer)
        {
            // If we have an editor model specified, render that component
            if (Model.Editor != null)
                CreateChildControl(Model.Editor, new Dictionary<string, object> { { "class", "dj_Editor" } }).Render(writer);
            // Otherwise, render the "static" Razor content area
            else
                RenderSection("EditArea", writer, isRequired: true);
        }

        protected virtual void WriteHeaderArea(HtmlTextWriter writer)
        {
            RenderSection("HeaderArea", writer);
        }

        protected virtual void WriteHeaderIconsArea(HtmlTextWriter writer)
        {
            RenderSection("HeaderIconsArea", writer);
        }

        protected virtual void WriteInfoArea(HtmlTextWriter writer)
        {
            RenderSection("InfoArea", writer);
        }

        protected virtual void WriteMessageArea(HtmlTextWriter writer)
        {
            RenderSection("MessageArea", writer);
        }

        protected virtual void WriteLoadingArea(HtmlTextWriter writer)
        {
            RenderSection("LoadingArea", writer);
        }

        protected virtual void WriteFooterArea(HtmlTextWriter writer)
        {
            RenderSection("FooterArea", writer);
        }

        #endregion

        private void WriteHeader(HtmlTextWriter writer)
        {
            writer.RenderElement(HtmlTextWriterTag.H3, "module-title dj_module-title", (Model.Title ?? string.Empty).Trim());

            writer.RenderSection("scope-container", WriteHeaderArea);

            if (Model.CanEdit)
            {
                writer.RenderSection("actions-container",
                    x =>
                    {
                        WriteHeaderIconsArea(x);
                        
                        writer.RenderSection(
                            tag: HtmlTextWriterTag.Span,
                            className: "fi fi_reload-white dj_module-refresh{0}".FormatWith(Model.CanRefresh ? "" : " hide")
                        );
                        writer.RenderSection(
                            tag: HtmlTextWriterTag.Span,
                            className: "fi fi_gear settings"
                        );
                    });
            }
        }

        private void WriteModuleBody(HtmlTextWriter writer)
        {
            writer.RenderHiddenSection("module-content dj_module-content two-row", WriteContentArea);
            writer.RenderSection("dj_module-loading-area", WriteLoadingArea);
            writer.RenderHiddenSection("dj_module-message-area", WriteMessageArea);
            writer.RenderHiddenSection("dj_module-info-area", WriteInfoArea);
            writer.RenderSection("dj_module-footer module-footer", WriteFooterArea);
        }

        private void WriteEditAreaContainer(HtmlTextWriter writer)
        {
            writer.RenderSection("dj_edit-box", editSection =>
            {
                WriteEditArea(editSection);

                writer.RenderSection("button-pane",
                    x => writer.RenderSection("dc_list dc_list-lg",
                        y =>
                        {
                            writer.RenderSection("dc_item",
                                z => writer.RenderLink(Token("saveModuleOptions"), "#save-module-options", "dashboard-control dc_btn dc_btn-2 dc_btn-save")
                                , tag: HtmlTextWriterTag.Li);

                            writer.RenderSection("dc_item",
                                z => writer.RenderLink(Token("cancelModuleOptions"), "#cancel-module-options", "dashboard-control dc_btn dc_btn-3 dc_btn-cancel")
                                , tag: HtmlTextWriterTag.Li);

                        }, tag: HtmlTextWriterTag.Ul));
            });
        }


        private string Token(string tokenName)
        {
            return ComponentFactory.Token(tokenName).ToString();
        }
    }
}