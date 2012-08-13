using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Infrastructure;
using DowJones.Preferences;
using DowJones.Security;
using DowJones.Security.Interfaces;
using DowJones.Session;
using DowJones.Web.UI;

namespace DowJones.Web.Mvc.UI
{
    /// <summary>
    /// View component base class.
    /// </summary>
    public abstract class ViewComponentBase : RazorTemplateBase, IViewComponent, IViewDataContainer, IView
    {
        private readonly Lazy<DateTimeFormatter> _dateTimeFormatter;

        internal static IDictionary<Type, IEnumerable<ClientResource>> ClientResourcesCache
        {
            get
            {
                if (clientResourcesCache == null)
                    clientResourcesCache = new Dictionary<Type, IEnumerable<ClientResource>>();

                return clientResourcesCache;
            }
            set { clientResourcesCache = value; }
        }
        private volatile static IDictionary<Type, IEnumerable<ClientResource>> clientResourcesCache;


        /// <summary>
        /// This component's child components
        /// </summary>
        public IEnumerable<IViewComponent> Children
        {
            get { return _children; }
        }
        private readonly IList<IViewComponent> _children;

        /// <summary>
        /// Indicates whether or not this component requires
        /// a valid ClientID.
        /// </summary>
        /// <remarks>Most do... some don't.</remarks>
        protected virtual bool ClientIDRequired
        {
            get
            {
                return ClientPluginName != null
                    || Children.Any();
            }
        }

        /// <summary>
        /// The client (jQuery) plugin name.
        /// If this component does not have a plugin,
        /// this should be [null]
        /// </summary>
        /// <remarks>
        /// A ClientPluginName name of "dj_MyComponent"
        /// would render out an initialization script such as:
        /// $('#MyComponent').dj_MyComponent({ ... });
        /// </remarks>
        public abstract string ClientPluginName { get; }

        /// <summary>
        /// The Client Resources (scripts, stylesheets, etc.)
        /// that this component depends on
        /// </summary>
        public virtual IEnumerable<ClientResource> ClientResources
        {
            get { return _clientResources; }
        }
        private readonly IList<ClientResource> _clientResources;

        /// <summary>
        /// Gets the client side object writer factory.
        /// </summary>
        [Inject("Injected to avoid a base constructor call in derived classes")]
        public virtual IClientSideObjectWriterFactory ClientSideObjectWriterFactory
        {
            get
            {
                if (_clientSideObjectWriterFactory == null)
                {
                    var parent = Parent as ViewComponentBase;
                    if (parent != null)
                        return parent.ClientSideObjectWriterFactory;
                }

                return _clientSideObjectWriterFactory;
            }
            set { _clientSideObjectWriterFactory = value; }
        }
        private IClientSideObjectWriterFactory _clientSideObjectWriterFactory;

        /// <summary>
        /// Gets the client side object writer factory.
        /// </summary>
        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected virtual IClientStateMapper ClientStateMapper
        {
            get
            {
                if (_clientStateMapper == null)
                {
                    var parent = Parent as ViewComponentBase;
                    if (parent != null)
                        return parent.ClientStateMapper;
                }

                return _clientStateMapper;
            }
            set { _clientStateMapper = value; }
        }
        private IClientStateMapper _clientStateMapper;

        /// <summary>
        /// The ViewComponentFactory that created this component
        /// </summary>
        protected internal ViewComponentFactory ComponentFactory
        {
            get; 
            set;
        }

        /// <summary>
        /// Alias for ComponentFactory (to match the signature on DJ.WebViewPage)
        /// </summary>
        protected internal ViewComponentFactory DJ
        {
            get { return ComponentFactory; }
        }

        /// <summary>
        /// The default Css class
        /// </summary>
        public string CssClass
        {
            [DebuggerStepThrough]
            get { return GetHtmlAttribute<string>("class") ?? string.Empty; }
            [DebuggerStepThrough]
            set { HtmlAttributes["class"] = value; }
        }

        /// <summary>
        /// Gets the HTML attributes.
        /// </summary>
        /// <value>The HTML attributes.</value>
        public IDictionary<string, object> HtmlAttributes
        {
            get;
            private set;
        }

        /// <summary>
        /// The current Html Helper
        /// </summary>
        public HtmlHelper Html
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public virtual string ClientID
        {
            [DebuggerStepThrough]
            get
            {
                // Return from htmlattributes if user has specified
                // otherwise build it from name
                string id = GetHtmlAttribute<string>("id");

                if (string.IsNullOrWhiteSpace(id) && !string.IsNullOrEmpty(Name))
                {
                    id = Name.Replace(".", HtmlHelper.IdAttributeDotReplacement);
                    HtmlAttributes["id"] = id;
                }

                return id;
            }
            [DebuggerStepThrough]
            set
            {
                Guard.IsNotNullOrEmpty(value, "id");
                HtmlAttributes["id"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            [DebuggerStepThrough]
            get { return GetHtmlAttribute<string>("name"); }

            [DebuggerStepThrough]
            set
            {
                Guard.IsNotNullOrEmpty(value, "value");
                HtmlAttributes["name"] = value;
            }
        }

        /// <summary>
        /// This component's parent
        /// </summary>
        public IViewComponent Parent
        {
            get;
            set;
        }

        /// <summary>
        /// The component's model
        /// </summary>
        public object Model
        {
            get; 
            set;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected string TemplateContent { get; private set; }

        public string TagName
        {
            get { return _tagName; }
            set { _tagName = value; }
        }
        private string _tagName = HtmlTextWriterTag.Div.ToString();

        protected UrlHelper Url
        {
            get
            {
                return new UrlHelper(ViewContext.RequestContext);
            }
        }

        public IUserContext UserContext
        {
            get; 
            set;
        }

        protected IPrinciple Entitlements
        {
            get { return UserContext.Principle; }
        }

        protected IControlData ControlData
        {
            get { return UserContext.ControlData; }
        }

        protected IPreferences Preferences
        {
            get { return UserContext.Preferences; }
        }


        /// <summary>
        /// Gets or sets the view context to rendering a view.
        /// </summary>
        /// <value>The view context.</value>
        public ViewContext ViewContext
        {
            get { return Html.ViewContext; }
        }


        public ViewDataDictionary ViewData
        {
            get;
            set;
        }


        protected ViewComponentBase()
            : this(null)
        {
        }

        protected ViewComponentBase(IEnumerable<IViewComponent> children)
        {
            _children = new List<IViewComponent>((children ?? Enumerable.Empty<IViewComponent>()));

            var clientResources = AutoDiscoverClientResources();
            _clientResources = new List<ClientResource>(clientResources);

            HtmlAttributes = new RouteValueDictionary();
            _dateTimeFormatter = new Lazy<DateTimeFormatter>(() => new DateTimeFormatter(Preferences));
        }


        public void AddClientResource(ClientResource clientResource)
        {
            Guard.IsNotNull(clientResource, "clientResource");
            _clientResources.Add(clientResource);
        }

        /// <summary>
        /// Include the Client Resources for the given component type
        /// without requiring an instance.
        /// 
        /// DependsOn<PortalHeadlineListComponent>()
        /// </summary>
        /// <typeparam name="TComponent">
        /// Type of IViewComponent whose Client Resources this component depends on
        /// </typeparam>
        [Obsolete("Use the DependsOn property of the ClientResource instead")]
        protected void DependsOn<TComponent>() 
            where TComponent : class, IViewComponent
        {
            var componentType = typeof (TComponent);
            
            var clientResources = 
                componentType
                    .GetClientResourceAttributes()
                    .Select(x => x.ToClientResource(componentType));

            _clientResources.AddRange(clientResources);
        }

        /// <summary>
        /// If this component requires a client ID and none
        /// was provided, be nice and generate one
        /// </summary>
        public void EnsureClientID()
        {
            if (ClientIDRequired && string.IsNullOrEmpty(ClientID))
                ClientID = ComponentFactory.GenerateComponentID(this);
        }

        /// <summary>
        /// Renders the component.
        /// </summary>
        public void Render()
        {
            using (HtmlTextWriter textWriter = new HtmlTextWriter(ViewContext.HttpContext.Response.Output))
            {
                Render(textWriter);
            }
        }

        /// <summary>
        /// Satisfies the ASP.NET MVC IView contract
        /// </summary>
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            Html = new HtmlHelper(viewContext, this);

            var htmlWriter = writer as HtmlTextWriter;
            Render(htmlWriter ?? new HtmlTextWriter(writer));
        }

        /// <summary>
        /// Render this control to an HtmlTextWriter
        /// </summary>
        public void Render(HtmlTextWriter writer)
        {
            WriteHtml(writer);
        }

        /// <summary>
        /// Render this control to Html
        /// </summary>
        public string ToHtmlString()
        {
            return new HtmlTextWriterRenderer().Render(WriteHtml);
        }

        /// <summary>
        /// Render this control to a string
        /// </summary>
        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(ClientID))
                return ClientID;

            return base.ToString();
        }

        /// <summary>
        /// Writes the initialization script.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public virtual void WriteInitializationScript(TextWriter writer)
        {
            InitializeClientPlugin(writer);
        }

        /// <summary>
        /// Writes the cleanup script.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public virtual void WriteCleanupScript(TextWriter writer)
        {
        }

        /// <summary>
        /// Read a value from the HtmlAttributes property
        /// </summary>
        private T GetHtmlAttribute<T>(string attributeName)
        {
            object attribute;
            HtmlAttributes.TryGetValue(attributeName, out attribute);

            return (attribute is T) ? (T)attribute : default(T);
        }

        /// <summary>
        /// Render the client-side intialization script.
        /// </summary>
        /// <remarks>
        /// By default, this will render a call to the jQuery plugin 
        /// specified by the ClientPluginName and pass in a metadata
        /// object with the options, data, and event handlers.
        /// </remarks>
        protected virtual void InitializeClientPlugin(TextWriter writer)
        {
            if (string.IsNullOrWhiteSpace(ClientPluginName))
                return;

            EnsureClientID();

            ClientState clientState = GetClientState();

            if (clientState == null)
                return;

            IClientSideObjectWriter objectWriter = ClientSideObjectWriterFactory.Create(ClientID, ClientPluginName, writer);

            objectWriter.Start();

            objectWriter.AppendKeyValuePairs("options", clientState.Options);
            objectWriter.AppendObject("data", clientState.Data);
            objectWriter.AppendClientEvents(clientState.EventHandlers);
            if (Children.Any()) objectWriter.AppendCollection("childComponents", Children.Select(x => new { type = x.GetType().Name, id = x.ClientID }));
            objectWriter.Complete();
        }

        protected virtual ClientState GetClientState()
        {
            ClientState clientState = ClientStateMapper.Map(Model);
            return clientState;
        }

        /// <summary>
        /// Traverse the object hierarchy and map
        /// all of the ClientResourceAttributes
        /// to the local ClientResources collection
        /// </summary>
        protected virtual IEnumerable<ClientResource> AutoDiscoverClientResources()
        {
            var declaringType = GetType();

            if (ClientResourcesCache.ContainsKey(declaringType))
                return ClientResourcesCache[declaringType];

            var clientResourceAttributes = this.GetClientResourceAttributes();
            
            // Reverse the order so the anscestors are first
            clientResourceAttributes = clientResourceAttributes.Reverse();

            var resources = clientResourceAttributes.Select(x => x.ToClientResource(declaringType));

            ClientResourcesCache.Add(declaringType, resources);

            return resources;
        }


        /// <summary>
        /// Writes the HTML.
        /// </summary>
        protected virtual void WriteHtml(HtmlTextWriter writer)
        {
            // Execute the template method and save the results in a buffer.
            // This will allow any template code snippets to execute
            // against this instance and populate properties prior to 
            // executing the methods below that may depend on them.
            // e.g. the template might set the ClientID or TagName.
            StringBuilder templateContent = new StringBuilder();
            ExecuteTemplate(templateContent);
            TemplateContent = templateContent.ToString();

            EnsureClientID();

            WriteAttributes(writer);

            writer.RenderBeginTag(TagName);

            WriteContent(writer);

            writer.RenderEndTag();

            TemplateContent = null;
        }

        /// <summary>
        /// Writes the HTML attributes for the component's wrapper
        /// </summary>
        /// <remarks>
        /// By default, this will render out all of the attributes
        /// registered in the HtmlAttributes property
        /// </remarks>
        protected virtual void WriteAttributes(HtmlTextWriter writer)
        {
            foreach (var attribute in HtmlAttributes)
            {
                if (attribute.Value != null)
                    writer.AddAttribute(attribute.Key, attribute.Value.ToString());
            }
        }

        protected virtual void WriteContent(HtmlTextWriter writer)
        {
            writer.Write(TemplateContent);
        }

        public virtual void AddChild(IViewComponent child)
        {
            Guard.IsNotNull(child, "child");

            if (!_children.Contains(child))
            {
                child.Parent = this;
                _children.Add(child);

                EnsureClientID();
                child.EnsureClientID();
            }
        }

        public void RemoveChild(IViewComponent child)
        {
            Guard.IsNotNull(child, "child");

            if (_children.Contains(child))
                _children.Remove(child);
        }

        protected void RenderChildren(HtmlTextWriter writer)
        {
            foreach (var child in Children ?? Enumerable.Empty<IViewComponent>())
            {
                child.Render(writer);
                writer.WriteLine();
            }
        }

        protected string Format(object value, string format = null)
        {
            if(value == null)
                return string.Empty;

            if (!format.HasValue())
            {
                if (value is DateTime)
                    return _dateTimeFormatter.Value.Format((DateTime) value);
                
                if (value.IsNumeric())
                    format = "{0:N0}";
                else
                    format = "{0}";
            }

            return string.Format(format, value);
        }
    }

    public abstract class ViewComponentBase<TModel> : ViewComponentBase
        where TModel : class
    {

        /// <summary>
        /// The component's model
        /// </summary>
        public new TModel Model
        {
            get { return (TModel)base.Model; }
            set { base.Model = value; }
        }

        protected ViewComponentBase()
            : this(null)
        {
        }

        protected ViewComponentBase(IEnumerable<IViewComponent> children)
            : base(children)
        {
        }

        protected IViewComponent CreateChildControl(IViewComponentModel model = null, IDictionary<string, object> htmlAttributes = null)
        {
            var component = ComponentFactory.Create(model, htmlAttributes);
            component.Parent = this;
            AddChild(component);
            return component;
        }

        protected TViewComponent CreateChildControl<TViewComponent>(IViewComponentModel model = null, IDictionary<string, object> htmlAttributes = null)
            where TViewComponent : class, IViewComponent
        {
            var component = ComponentFactory.Create<TViewComponent>(model, htmlAttributes);
            component.Parent = this;
            AddChild(component);
            return component;
        }
    }
}