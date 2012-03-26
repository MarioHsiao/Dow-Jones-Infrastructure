using System;
using System.CodeDom;
using System.Linq;
using DowJones.Web.Mvc.Razor.ClientResources.Spans;

namespace DowJones.Web.Mvc.Razor.ClientResources
{
    internal class ClientResourceAttributeDeclaration : CodeAttributeDeclaration
    {
        public const string DeclaringTypeAttributeName = "DeclaringType";
        public const string DeclaringAssemblyAttributeName = "DeclaringAssembly";
        public const string DependencyLevelAttributeName = "DependencyLevel";
        public const string PerformSubstitutionAttributeName = "PerformSubstitution";
        public const string RelativeResourceAttributeName = "RelativeResourceName";
        public const string ResourceKindAttributeName = "ResourceKind";
        public const string ResourceNameAttributeName = "ResourceName";
        public const string UrlAttributeName = "Url";
        public const string TemplateIdAttributeName = "TemplateId";

        public bool HasDeclaringType
        {
            get { return GetArgument(DeclaringTypeAttributeName) != null; }
        }

        public bool HasDeclaringAssembly
        {
            get { return GetArgument(DeclaringAssemblyAttributeName) != null; }
        }

        public bool HasResourceName
        {
            get { return GetArgument(ResourceNameAttributeName) != null; }
        }

        public bool HasUrl
        {
            get { return GetArgument(UrlAttributeName) != null; }
        }

        public bool HasId
        {
            get { return GetArgument(TemplateIdAttributeName) != null; }
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
            ;
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
    }
}