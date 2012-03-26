using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Token;
using DowJones.Web.Mvc.UI.Exceptions;
using DowJones.Web.UI;
using log4net;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentFactory : IAmFluent, IInitializable
    {
        private int _globalIdCounter;

        private readonly ScriptRegistryBuilder _scriptRegistry;
        private readonly StylesheetRegistryBuilder _stylesheetRegistry;
        private readonly IAssemblyRegistry _assemblyRegistry;
        private readonly ITokenRegistry _tokenRegistry;
        private readonly IViewComponentRegistry _componentRegistry;
        private readonly IClientSideObjectWriterFactory _clientSideObjectWriterFactory;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IClientSideObjectWriterFactory ClientSideObjectWriterFactory
        {
            get { return _clientSideObjectWriterFactory; }
        }

        public HtmlHelper HtmlHelper { get; set; }

        [Inject("Injected to avoid a base constructor call in derived classes")]
        protected ILog Log { get; set; }

        protected ViewComponentFactory(
                IClientSideObjectWriterFactory clientSideObjectWriterFactory, 
                ScriptRegistryBuilder scriptRegistry, 
                StylesheetRegistryBuilder stylesheetRegistry, 
                IViewComponentRegistry componentRegistry,
                IAssemblyRegistry assemblyRegistry,
                ITokenRegistry tokenRegistry
            )
        {
            _scriptRegistry = scriptRegistry;
            _clientSideObjectWriterFactory = clientSideObjectWriterFactory;
            _stylesheetRegistry = stylesheetRegistry;
            _assemblyRegistry = assemblyRegistry;
            _tokenRegistry = tokenRegistry;
            _componentRegistry = componentRegistry;
        }

        [DebuggerStepThrough]
        public ScriptRegistryBuilder ScriptRegistry()
        {
            _scriptRegistry.HtmlHelper = HtmlHelper;
            return _scriptRegistry;
        }

        [DebuggerStepThrough]
        public StylesheetRegistryBuilder StylesheetRegistry()
        {
            return _stylesheetRegistry;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual IViewComponent Create(object model, IDictionary<string, object> htmlAttributes = null)
        {
            Guard.IsNotNull(model, "model");

            var component = Mapper.Map<IViewComponent>(model);

            return Create(component, model, htmlAttributes);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual TViewComponent Create<TViewComponent>(object model = null, IDictionary<string, object> htmlAttributes = null)
            where TViewComponent : class, IViewComponent
        {
            var component = Create(typeof(TViewComponent), model, htmlAttributes);
            return component as TViewComponent;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual IViewComponent Create(Type componentType, object model, IDictionary<string,object> htmlAttributes = null)
        {
            Guard.IsNotNull(componentType, "componentType");

            IViewComponent component = ServiceLocator.Resolve<IViewComponent>(componentType);

            return Create(component, model, htmlAttributes);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual IViewComponent Create(IViewComponent component, object model, IDictionary<string, object> htmlAttributes = null)
        {
            Guard.IsNotNull(component, "component");

            _componentRegistry.Register(component);

            InitializeComponent(component, model, htmlAttributes);

            PopulateChildren(component, model as IViewComponentModel);

            return component;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GenerateComponentID(IViewComponent component)
        {
            if (component == null)
                return null;

            var componentState = component.Model as IViewComponentModel;
            if (componentState != null && !string.IsNullOrWhiteSpace(componentState.ID))
                return componentState.ID;

            // No, there is no logical correlation between the component type and the
            // _globalIdCounter so it is possible to get IDs like "canvas-1" and "myModule-2".
            // But, what this does do is provide a simple way to guarantee unique IDs.
            var componentId = string.Format("{0}-{1}", component.GetType().Name, _globalIdCounter++);

            IViewComponent parent = component.Parent;
            if (parent != null && !string.IsNullOrWhiteSpace(parent.ClientID))
            {
                parent.EnsureClientID();
                componentId = string.Format("{0}_{1}", parent.ClientID, componentId);
            }

            return componentId;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Initialize()
        {
            // Define global includes
            // DO NOT ADD CONTROL-SPECIFIC ITEMS HERE
            // Instead, use Html.DJ().ScriptRegistry() and 
            // .StylesheetRegistry() in your control markup

            ScriptRegistry()
                .WithjQuery()
                .WithDowJonesCommon()
                .WithDateFormat();
        }

        public IHtmlString Render(object data, object htmlAttributes = null)
        {
            if(data == null)
            {
                Log.Warn("Data is null, rendering blank contents instead");
                return new HtmlString(string.Empty);
            }

            if (data is IEnumerable)
                return RenderComponentCollection((IEnumerable)data);

            return Create(data, ConvertObjectToDictionary(htmlAttributes));
        }

        [Obsolete]
        public IHtmlString RenderComponent(string componentName, object model = null, object htmlAttributes = null)
        {
            var componentTypes =
                _assemblyRegistry
                    .GetConcreteTypesDerivingFrom<IViewComponent>()
                    .WithTypeName(componentName);

            if (!componentTypes.Any())
                throw new UnknownComponentTypeException(componentName);

            if (componentTypes.Count() > 1)
                throw new AmbiguousComponentTypeException(componentName, componentTypes);

            var component = Create(componentTypes.SingleOrDefault(), model, ConvertObjectToDictionary(htmlAttributes));

            return component;
        }

        public IHtmlString RenderComponent<TViewComponent>(object model = null, object htmlAttributes = null)
            where TViewComponent : class, IViewComponent
        {
            return Create<TViewComponent>(model, ConvertObjectToDictionary(htmlAttributes));
        }

        /// <summary>
        /// Render a token value
        /// </summary>
        public IHtmlString Token(string name)
        {
            return new HtmlString(_tokenRegistry.Get(name));
        }

        /// <summary>
        /// Render a token value
        /// </summary>
        public IHtmlString Token(Enum value)
        {
            return new HtmlString(_tokenRegistry.Get(value));
        }

        public IHtmlString TokenForErrorNumber(int errorNumber)
        {
            return new HtmlString(_tokenRegistry.GetErrorMessage(errorNumber));
        }

        public IHtmlString TokenForErrorNumber(string errorNumber)
        {
            return new HtmlString( _tokenRegistry.GetErrorMessage( errorNumber ) );
        }

        private static IDictionary<string, object> ConvertObjectToDictionary(object source)
        {
            if (source is IDictionary<string, object>)
                return (IDictionary<string, object>)source;

            return HtmlHelper.AnonymousObjectToHtmlAttributes(source);
        }

        protected virtual void InitializeComponent(IViewComponent component, object model = null, IEnumerable<KeyValuePair<string, object>> htmlAttributes = null)
        {
            component.Html = HtmlHelper;

            if(model != null)
                component.Model = model;

            if(htmlAttributes != null)
                component.HtmlAttributes.Merge(htmlAttributes);

            var viewComponent = component as ViewComponentBase;
            if (viewComponent != null)
            {
                viewComponent.ClientSideObjectWriterFactory = ClientSideObjectWriterFactory;
                viewComponent.ComponentFactory = this;
            }
        }

        protected virtual void PopulateChildren(IViewComponent component, IViewComponentModel componentModel)
        {
            var container = component;

            if (container == null || componentModel == null || componentModel.Children == null)
                return;

            foreach (var childState in componentModel.Children)
            {
                IViewComponent childComponent = Create(childState);

                childComponent.Parent = component;
                container.AddChild(childComponent);
            }
        }

        protected internal IHtmlString RenderComponentCollection(IEnumerable models)
        {
            Guard.IsNotNull(models, "models");

            StringBuilder viewBuilder = new StringBuilder();

            foreach (var model in models)
            {
                viewBuilder.AppendLine(Render(model).ToHtmlString());
            }

            return new HtmlString(viewBuilder.ToString());
        }

        
        public IHtmlString GlobalHeaders()
        {
            const string ScriptFormat = @"<script type=""text/javascript"">(function(w) {{ var $dj = w.$dj || {{}}; $dj.globalHeaders = {0}; }}(window))</script>";
            return new HtmlString(ScriptFormat.FormatWith(JsonConvert.SerializeObject(HtmlHelper.ViewData["GlobalHeaders"])));
        }
    }
}
