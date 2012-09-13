using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DowJones.Web.Mvc.ModelBinders
{
    public class JsonValueProviderFactory : ValueProviderFactory
    {
        public JsonValueProviderFactory(string keyName, Type modelType)
        {
            KeyName = keyName;
            ModelType = modelType;
        }

        private string KeyName { get; set; }

        private Type ModelType { get; set; }

        private static void BuildMyValueProvider(Dictionary<string, object> valueProvider, string prefix, object value)
        {
            var d = value as IDictionary<string, object>;
            if (d != null)
            {
                foreach (var entry in d)
                {
                    BuildMyValueProvider(valueProvider, GetPropertyKey(prefix, entry.Key), entry.Value);
                }
                return;
            }
            var l = value as IList;
            if (l != null)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    BuildMyValueProvider(valueProvider, GetArrayKey(prefix, i), l[i]);
                }
                return;
            }
            valueProvider[prefix] = value;
        }

        private object GetDeserializedJson(ControllerContext controllerContext)
        {
            string jsonText = controllerContext.HttpContext.Request[KeyName];
            if (String.IsNullOrEmpty(jsonText))
            {
                return null;
            }
            var serializer = new JavaScriptSerializer();
            object jsonData = serializer.DeserializeObject(jsonText);
            return jsonData;
            //return JsonConvert.DeserializeObject(jsonText, ModelType);
        }

        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            var valueProvider = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            object jsonData = GetDeserializedJson(controllerContext);
            if (jsonData != null)
            {
                BuildMyValueProvider(valueProvider, String.Empty, jsonData);
            }

            BuildMyValueProvider(valueProvider, String.Empty, jsonData);
            return new DictionaryValueProvider<object>(valueProvider, CultureInfo.CurrentCulture);
        }

        private static string GetArrayKey(string prefix, int index)
        {
            return prefix + "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
        }

        private static string GetPropertyKey(string prefix, string propertyName)
        {
            return (String.IsNullOrEmpty(prefix)) ? propertyName : prefix + "." + propertyName;
        }
    }
}