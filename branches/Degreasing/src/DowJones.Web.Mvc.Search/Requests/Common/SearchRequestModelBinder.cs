using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Infrastructure;
using JsonValueProviderFactory = DowJones.Web.Mvc.ModelBinders.JsonValueProviderFactory;

namespace DowJones.Web.Mvc.Search.Requests
{
    public class SearchRequestModelBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return ServiceLocator.Resolve<SearchRequestModelBinder>();
        }
    }

    public class SearchRequestModelBinder : DefaultModelBinder
    {
        private static volatile IDictionary<string, Type> _searchQueryTypes;
        private static readonly Type SimpleRequestType = typeof(SimpleSearchRequest);
        private readonly IAssemblyRegistry _assemblyRegistry;


        public SearchRequestModelBinder(IAssemblyRegistry assemblyRegistry)
        {
            _assemblyRegistry = assemblyRegistry;
        }

        protected internal IDictionary<string, Type> SearchQueryTypes
        {
            get { return _searchQueryTypes ?? (_searchQueryTypes = GetSearchQueryTypes()); }
            set { _searchQueryTypes = value; }
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var name = bindingContext.ModelName;
            var hasPrefixValue = bindingContext.ValueProvider.ContainsPrefix(name);

            var hasPrefix = !String.IsNullOrEmpty(name) && !hasPrefixValue;

            if (hasPrefix && !bindingContext.FallbackToEmptyPrefix)
                return null;

            if (hasPrefix && bindingContext.FallbackToEmptyPrefix)
            {
                bindingContext = new ModelBindingContext
                {
                    ModelMetadata = bindingContext.ModelMetadata,
                    ModelState = bindingContext.ModelState,
                    PropertyFilter = bindingContext.PropertyFilter,
                    ValueProvider = bindingContext.ValueProvider
                };
            }

            Type modelType = bindingContext.ModelType;
            
            if ( modelType.IsAbstract)
            {
                modelType = GetModelType(controllerContext);
            }
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, modelType);
            var c = (ValueProviderCollection)bindingContext.ValueProvider;
            c.Add(new JsonValueProviderFactory(bindingContext.ModelName, modelType).GetValueProvider(controllerContext));
            bindingContext.ValueProvider = c;
            
            if (hasPrefixValue)
            {
                bindingContext.ModelName = "";
                bindingContext.FallbackToEmptyPrefix = false;
            }
            else
            {
                bindingContext.ModelName = name;
                bindingContext.FallbackToEmptyPrefix = true;
            }
            
            return base.BindModel(controllerContext, bindingContext);
        }

        private Type GetModelType(ControllerContext controllerContext)
        {
            var searchQueryKindName = (controllerContext.RouteData.Values["kind"] as string);

            if (searchQueryKindName.IsNullOrEmpty())
                searchQueryKindName = controllerContext.HttpContext.Request["kind"];

            if (searchQueryKindName.IsNullOrEmpty())
                searchQueryKindName = "simple";

            Type searchQueryType;
            if (SearchQueryTypes.TryGetValue(searchQueryKindName.ToLower(), out searchQueryType))
                return searchQueryType;

            return SimpleRequestType;
        }

        private IDictionary<string, Type> GetSearchQueryTypes()
        {
            IEnumerable<Type> searchQueryTypes = _assemblyRegistry.GetConcreteTypesDerivingFrom<SearchRequest>();

            var groupedTypes = searchQueryTypes
                .Select(x => new
                {
                    Name = Regex.Replace(x.Name, "SearchRequest", string.Empty).ToLower(),
                    Type = x
                });

            return groupedTypes.ToDictionary(x => x.Name, y => y.Type);
        }
    }
}