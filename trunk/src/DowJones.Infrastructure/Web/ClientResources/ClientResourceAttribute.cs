using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DowJones.Extensions;

namespace DowJones.Web
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public class ClientResourceAttribute : Attribute
	{
		/// <summary>
		/// The unique name of this resource
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The absolute Url to the external resource.
		/// </summary>
		/// <remarks>
		/// An absolute Url takes precendence over an embedded resource
		/// declared via ResourceName.
		/// </remarks>
		public string Url { get; set; }

		/// <summary>
		/// The Type in which the Web Resource is declared.
		/// </summary>
		/// <remarks>
		/// Defaults to the type on which this attribute is applied
		/// </remarks>
		public object DeclaringType { get; set; }

		/// <summary>
		/// The display name of the assembly in which the Web Resource is embedded.
		/// </summary>
		public string DeclaringAssembly { get; set; }


		/// <summary>
		/// The Dependency Level
		/// </summary>
		public ClientResourceDependencyLevel DependencyLevel { get; set; }

		/// <summary>
		/// The names of the other client resources that this resource depends on
		/// </summary>
		public string[] DependsOn { get; set; }

		/// <summary>
		/// The Embedded Resource Name
		/// </summary>
		/// <remarks>
		/// An absolute Url takes precendence over an embedded resource
		/// declared via ResourceName.
		/// </remarks>
		public string ResourceName { get; set; }

		/// <summary>
		/// The Embedded Resource Name 
		/// (relative to the namespace of the <see cref="DeclaringType"/>)
		/// </summary>
		/// <remarks>
		/// An absolute Url takes precendence over an embedded resource
		/// declared via ResourceName.
		/// </remarks>
		public string RelativeResourceName { get; set; }

		/// <summary>
		/// The id for generated script tag for Client Side Templates.
		/// </summary>
		public string TemplateId { get; set; }

		/// <summary>
		/// The Resource Kind
		/// </summary>
		public ClientResourceKind ResourceKind { get; set; }

		/// <summary>
		/// States whether this instance requires (and is lacking)
		/// the specification of a Declaring Assembly
		/// </summary>
		public bool RequiresDeclaringAssembly
		{
			get
			{
				return !string.IsNullOrWhiteSpace(ResourceName)
					&& (DeclaringType == null && string.IsNullOrWhiteSpace(DeclaringAssembly));
			}
		}

		public ClientResourceAttribute()
			: this(null)
		{
		}
		public ClientResourceAttribute(string name)
		{
			Name = name;
		}

		public ClientResourceDefinition ToClientResourceDefinition()
		{
			var definition = new ClientResourceDefinition
					   {
						   DeclaringType = DeclaringType as Type,
						   DeclaringAssembly = DeclaringAssembly,
						   DependencyLevel = DependencyLevel,
						   DependsOn = DependsOn,
						   Name = Name,
						   RelativeResourceName = RelativeResourceName,
						   ResourceKind = ResourceKind,
						   ResourceName = ResourceName,
						   Url = Url,
						   TemplateId = TemplateId
					   };

			DiscoverDerivedDependendantResources(definition);

			return definition;

		}

		private void DiscoverDerivedDependendantResources(ClientResourceDefinition definition)
		{
			if (!definition.HasDeclaringType || definition.DeclaringType.BaseType == null)
				return;

			var baseClientResources = definition.DeclaringType.BaseType.GetClientResourceAttributes().ToArray();

			if (!baseClientResources.Any()) return;

			var dependants = baseClientResources.Select(r => r.Name).ToArray();
			
			if (definition.DependsOn == null)
				definition.DependsOn = dependants;
			else
				definition.DependsOn.AddRange(dependants);
		}


		public ClientResource ToClientResource(Type declaringType = null)
		{
			if (RequiresDeclaringAssembly)
				DeclaringType = declaringType;

			ClientResourceDefinition definition = ToClientResourceDefinition();
			ClientResource resource = Mapper.Map<ClientResource>(definition);

			return resource;
		}
	}

	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public class ScriptResourceAttribute : ClientResourceAttribute
	{
		public ScriptResourceAttribute()
			: this(null)
		{
		}
		public ScriptResourceAttribute(string name)
			: base(name)
		{
			DependencyLevel = ClientResourceDependencyLevel.Component;
			ResourceKind = ClientResourceKind.Script;
		}
	}

	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public class ClientTemplateResourceAttribute : ClientResourceAttribute
	{
		public ClientTemplateResourceAttribute()
			: this(null)
		{
		}
		public ClientTemplateResourceAttribute(string name)
			: base(name)
		{
			DependencyLevel = ClientResourceDependencyLevel.Component;
			ResourceKind = ClientResourceKind.ClientTemplate;
		}
	}

	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public class StylesheetResourceAttribute : ClientResourceAttribute
	{
		public StylesheetResourceAttribute()
			: this(null)
		{
		}
		public StylesheetResourceAttribute(string name)
			: base(name)
		{
			DependencyLevel = ClientResourceDependencyLevel.Component;
			ResourceKind = ClientResourceKind.Stylesheet;
		}
	}


	public static class ClientResourceAttributeViewComponentExtensions
	{

		public static IEnumerable<ClientResourceAttribute> GetClientResourceAttributes(this ICustomAttributeProvider member)
		{
			var resources = member.GetAttributesOf<ClientResourceAttribute>();
			return resources;
		}

		public static IEnumerable<ClientResourceAttribute> GetClientResourceAttributes(this ICustomAttributeProvider member, bool inherit)
		{
			var resources = member.GetAttributesOf<ClientResourceAttribute>(inherit);
			return resources;
		}

		public static IEnumerable<ClientResourceAttribute> GetClientResourceAttributes(this IEnumerable<ICustomAttributeProvider> members)
		{
			var resources = members.SelectMany(GetClientResourceAttributes);
			return resources;
		}

		public static IEnumerable<ClientTemplateResourceAttribute> GetClientTemplateResourceAttributes(this ICustomAttributeProvider member)
		{
			var resources = member.GetAttributesOf<ClientTemplateResourceAttribute>();
			return resources;
		}

		public static IEnumerable<ClientTemplateResourceAttribute> GetClientTemplateResourceAttributes(this IEnumerable<ICustomAttributeProvider> members)
		{
			var resources = members.SelectMany(GetClientTemplateResourceAttributes);
			return resources;
		}

	}
}
