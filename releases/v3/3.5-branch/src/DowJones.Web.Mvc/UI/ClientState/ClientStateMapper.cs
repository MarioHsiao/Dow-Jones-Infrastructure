using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Infrastructure;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI
{
    public interface IClientStateMapper
    {
        /// <summary>
        /// Accepts a source object and maps the
        /// ClientStateAttribute-decorated properties
        /// to a ClientState object.
        /// </summary>
        /// <param name="source">
        /// Source object with ClientStateAttribute-decorated properties
        /// </param>
        /// <returns>A populated ClientState</returns>
        ClientState Map(object source);
    }

    public class ClientStateMapper : IClientStateMapper
    {
        public ClientState Map(object source)
        {
            if (source == null)
                return new ClientState();

            // TODO:  Great opportunity for performance improvement!
            //        Including (but certainly not limited to) the ability 
            //        to "opt out" of the mapping process altogether

            var clientState = new ClientState();

            var clientStateAttributes = GetClientStateAttributes(source);

            var clientDataAttributes = clientStateAttributes.Where(state => state.Attribute is ClientDataAttribute);

            if (clientDataAttributes != null && clientDataAttributes.Count() == 1)
            {
                clientState.Data = clientDataAttributes.FirstOrDefault().Property.GetValue(source, null);
            }
            else
            {
                var dataDictionary = new Dictionary<string, object>();
                PopulateClientStateDictionary(dataDictionary, source, clientDataAttributes);
                clientState.Data = dataDictionary;
            }

            var clientEventHandlerAttributes = clientStateAttributes.Where(state => state.Attribute is ClientEventHandlerAttribute);
            PopulateClientStateDictionary(clientState.EventHandlers, source, clientEventHandlerAttributes);

            var clientOptionAttributes = clientStateAttributes.Where(state => state.Attribute is ClientPropertyAttribute);
            PopulateClientStateDictionary(clientState.Options, source, clientOptionAttributes);

            var clientTokenAttributes = clientStateAttributes.Where(state => state.Attribute is ClientTokenAttribute);
            PopulateClientStateDictionary(clientState.Tokens, source, clientTokenAttributes);

            var clientTokensAttributes = clientStateAttributes.Where(state => state.Attribute is ClientTokensAttribute);
            PopulateClientStateDictionary(clientState.Tokens, source, clientTokensAttributes);

            return clientState;
        }


        private static void PopulateClientStateDictionary<TValue>(IDictionary<string, TValue> dictionary, object source, IEnumerable<ClientStateInfo> clientStateInfos)
        {
            foreach (var clientStateInfo in clientStateInfos)
            {
                ClientStateAttribute attribute = clientStateInfo.Attribute;
                string key = attribute.Name ?? clientStateInfo.Property.Name;
                object value = GetPropertyValue(clientStateInfo, source);

                if (string.IsNullOrWhiteSpace(key) || value == null)
                    continue;

                if (attribute.Merge && value is IEnumerable<KeyValuePair<string, TValue>>)
                {
                    bool replaceExisting = attribute.ReplaceExistingValuesDuringMerge;
                    dictionary.Merge(value as IEnumerable<KeyValuePair<string, TValue>>, replaceExisting);
                }
                else
                {
                    dictionary.Add(key, (TValue)value);
                }
            }
        }

        private static object GetPropertyValue(ClientStateInfo clientStateInfo, object source)
        {
            var value = clientStateInfo.Property.GetValue(source, null);

            // Support a type converter if one is set
            var typeConverterAttribute = clientStateInfo.TypeConverterAttribute;
            if (typeConverterAttribute != null)
            {
                Type typeConverterType = BuildManager.GetType(typeConverterAttribute.ConverterTypeName, true);
                var typeConverter = ServiceLocator.Resolve(typeConverterType) as TypeConverter;

                if (typeConverter != null)
                    value = typeConverter.ConvertTo(value, typeof(string));
            }

            // Support a json type converter if one is set
            var jsonConverterAttribute = clientStateInfo.JsonConverterAttribute;
            if (jsonConverterAttribute != null)
            {
                var typeConverter = ServiceLocator.Resolve(jsonConverterAttribute.ConverterType) as TypeConverter;

                if (typeConverter != null)
                    value = typeConverter.ConvertTo(value, typeof(string));
            }

            return value;
        }

        private static IEnumerable<ClientStateInfo> GetClientStateAttributes(object source)
        {
            Guard.IsNotNull(source, "source");

            // TODO: Optimize reflection usage

            // ref: http://blogs.msdn.com/b/weitao/archive/2009/05/28/override-properties-ii-getcustomattributes.aspx?wa=wsignin1.0
            // MemberInfo.GetCustomAttributes doesn't search the inheritance chain at all (for PropertyInfo, EventInfo, and ParameterInfo)
            // Attribute.GetCustomAttributes honors the "inherit" boolean value for PropertyInfo, EventInfo, and ParameterInfo
            return from property in source.GetType().GetProperties()
                   let attributes = Attribute.GetCustomAttributes(property, true)
                   let clientStateAttribute = attributes.OfType<ClientStateAttribute>().FirstOrDefault()
                   let typeConverterAttribute = attributes.OfType<TypeConverterAttribute>().FirstOrDefault()
                   let jsonConverterAttribute = attributes.OfType<JsonConverterAttribute>().FirstOrDefault()
                   where clientStateAttribute != null
                   select new ClientStateInfo
                   {
                       Attribute = clientStateAttribute,
                       Property = property,
                       TypeConverterAttribute = typeConverterAttribute,
                       JsonConverterAttribute = jsonConverterAttribute
                   };
        }
    }

    internal class ClientStateInfo
    {
        public PropertyInfo Property { get; set; }
        public ClientStateAttribute Attribute { get; set; }
        public TypeConverterAttribute TypeConverterAttribute { get; set; }
        public JsonConverterAttribute JsonConverterAttribute { get; set; }
    }
}