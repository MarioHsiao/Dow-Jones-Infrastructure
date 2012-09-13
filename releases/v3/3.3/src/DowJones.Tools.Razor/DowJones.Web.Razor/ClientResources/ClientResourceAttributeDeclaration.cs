using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DowJones.Web.Razor.Common;
using DowJones.Web.Razor.Keywords.ClientTemplate;
using DowJones.Web.Razor.Keywords.FrameworkResource;

namespace DowJones.Web.Razor.ClientResources
{
    internal class ClientResourceAttributeDeclaration : CodeAttributeDeclaration
    {
        public const string DeclaringTypeAttributeName = "DeclaringType";
        public const string DeclaringAssemblyAttributeName = "DeclaringAssembly";
        public const string DependencyLevelAttributeName = "DependencyLevel";
        public const string DependsOnAttributeName = "DependsOn";
        public const string PerformSubstitutionAttributeName = "PerformSubstitution";
        public const string RelativeResourceAttributeName = "RelativeResourceName";
        public const string ResourceKindAttributeName = "ResourceKind";
        public const string ResourceNameAttributeName = "ResourceName";
        public const string TemplateIdAttributeName = "TemplateId";
        public const string UrlAttributeName = "Url";

        public bool HasDeclaringType
        {
            get { return HasArgument(DeclaringTypeAttributeName); }
        }

        public bool HasDeclaringAssembly
        {
            get { return HasArgument(DeclaringAssemblyAttributeName); }
        }

        public bool HasDependsOnArgument
        {
            get { return HasArgument(DependsOnAttributeName); }
        }

        public bool HasResourceName
        {
            get { return HasArgument(ResourceNameAttributeName); }
        }

        public bool HasUrl
        {
            get { return HasArgument(UrlAttributeName); }
        }

        public bool HasId
        {
            get { return HasArgument(TemplateIdAttributeName); }
        }


        public ClientResourceAttributeDeclaration()
        {
        }

        public ClientResourceAttributeDeclaration(ClientResourceSpan span)
            : this(span, span.RootResourceNamespace)
        {
            string declaringTypeName =
                span.ContentKeyValuePairs
                    .Where(x => x.Key == DeclaringTypeAttributeName)
                    .Select(x => x.Value)
                    .SingleOrDefault();

            EnsureDeclaringType(declaringTypeName);

            string declaringAssemblyName =
                span.ContentKeyValuePairs
                    .Where(x => x.Key == DeclaringAssemblyAttributeName)
                    .Select(x => x.Value)
                    .SingleOrDefault();

            EnsureDeclaringAssembly(declaringAssemblyName);
        }

        public ClientResourceAttributeDeclaration(ClientResourceSpan span, string rootResourceNamespace)
            : base(new CodeTypeReference(span.AttributeTypeName))
        {
            ResolveRelativeName(rootResourceNamespace);
            PopulateFromClientResourceSpan(span);
        }


        public void EnsureDeclaringType(string typeName, string @namespace)
        {
            string declaringTypeName = string.Format("{0}.{1}", typeName, @namespace);
            EnsureDeclaringType(declaringTypeName);
        }

        public void EnsureDeclaringType(string declaringType)
        {
            if (HasDeclaringType || string.IsNullOrWhiteSpace(declaringType))
                return;

            Arguments.Add(
                new CodeAttributeArgument(DeclaringTypeAttributeName,
                    new CodeTypeOfExpression(new CodeTypeReference(declaringType))
                )
            );
        }

        public void EnsureDeclaringAssembly(string declaringAssemblyName)
        {
            if (HasDeclaringAssembly || string.IsNullOrWhiteSpace(declaringAssemblyName))
                return;

            AddArgument(DeclaringAssemblyAttributeName, declaringAssemblyName);
        }

        private void ResolveRelativeName(string rootResourceNamespace)
        {
            // If this is an external resource or it already 
            // has a resource name, just return
            if (HasUrl || HasResourceName)
                return;

            var relativeNameArgument = GetArgument(RelativeResourceAttributeName);
            var relativeNameExpression =
                (relativeNameArgument == null)
                    ? null
                    : relativeNameArgument.Value as CodePrimitiveExpression;

            // If we don't have a relative name argument
            // there's nothing to resolve!
            if (relativeNameExpression == null)
                return;

            var relativeName = relativeNameExpression.Value as string;

            if (string.IsNullOrWhiteSpace(relativeName))
                return;

            var resourceNameArgument = GetArgument(ResourceNameAttributeName);

            string resourceName =
                string.Format("{0}.{1}",
                    rootResourceNamespace,
                    relativeName);

            if (resourceNameArgument == null)
                AddArgument(ResourceNameAttributeName, resourceName);
            else
                ((CodePrimitiveExpression)resourceNameArgument.Value).Value = resourceName;
        }

        protected virtual void PopulateFromClientResourceSpan(ClientResourceSpan resourceSpan)
        {
            AddArgument(resourceSpan.Name);

            if (!string.IsNullOrWhiteSpace(resourceSpan.Url))
            {
                AddArgument(UrlAttributeName, resourceSpan.Url);
                return;
            }

            if (!string.IsNullOrWhiteSpace(resourceSpan.ResourceName))
                if (resourceSpan is FrameworkResourceSpan)
                    AddEmbeddedResourceReference(resourceSpan.ResourceName);
                else
                    AddArgument(ResourceNameAttributeName, resourceSpan.ResourceName);

            if (!string.IsNullOrWhiteSpace(resourceSpan.DeclaringType))
                AddArgument(DeclaringTypeAttributeName, resourceSpan.DeclaringType);

            ClientResourceDependencyLevel dependencyLevel;
            if (Enum.TryParse(resourceSpan.DependencyLevel, true, out dependencyLevel))
                AddEnumArgument(DependencyLevelAttributeName, dependencyLevel);

            EnsureDependencies(resourceSpan.DependsOn);

            if (resourceSpan.PerformSubstitution)
                AddArgument(PerformSubstitutionAttributeName, resourceSpan.PerformSubstitution);

            ClientResourceKind resourceKind;
            if (Enum.TryParse(resourceSpan.ResourceKind, true, out resourceKind))
                AddEnumArgument(ResourceKindAttributeName, resourceKind);


            var clientTemplateResourceSpan = resourceSpan as ClientTemplateResourceSpan;
            if (clientTemplateResourceSpan != null && !string.IsNullOrWhiteSpace(clientTemplateResourceSpan.TemplateId))
                AddArgument(TemplateIdAttributeName, clientTemplateResourceSpan.TemplateId);

        }

        public CodeAttributeArgument GetArgument(string argumentName)
        {
            if (Arguments == null)
                return null;

            return Arguments
                        .Cast<CodeAttributeArgument>()
                        .SingleOrDefault(x => x.Name.ToLowerInvariant() == argumentName.ToLowerInvariant());
        }

        public bool HasArgument(string argumentName)
        {
            return GetArgument(argumentName) != null;
        }

        protected void AddArgument(object value)
        {
            Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(value)));
        }

        protected void AddArgument(string name, object value)
        {
            Arguments.Add(new CodeAttributeArgument(name, new CodePrimitiveExpression(value)));
        }

        protected void AddEnumArgument<TEnum>(string name, TEnum value)
        {
            var enumType = new CodeTypeReferenceExpression(typeof(TEnum));

            Arguments.Add(
                new CodeAttributeArgument(name,
                    new CodeFieldReferenceExpression(enumType, value.ToString())
                )
            );
        }

        protected void AddEmbeddedResourceReference(string resourceName)
        {
            var typeReference = new CodeTypeReferenceExpression("DowJones.Web.Mvc.Resources.EmbeddedResources");

            Arguments.Add(
                new CodeAttributeArgument(ResourceNameAttributeName,
                    new CodeFieldReferenceExpression(typeReference, resourceName)
                )
            );
        }

        /// <summary>
        /// Sets the client resource dependencies if no dependencies exist.
        /// </summary>
        /// <param name="dependencies">
        /// List of client resource names that this resource depends on
        /// </param>
        public void EnsureDependencies(IEnumerable<string> dependencies)
        {
            if (dependencies == null || !dependencies.Any())
                return;

            if (HasDependsOnArgument)
            {
                Console.WriteLine("Resource already has dependencies - skipping...");
                return;
            }

            Arguments.Add(new ArrayCodeAttributeArgument<string>(DependsOnAttributeName, dependencies));
        }
    }
}