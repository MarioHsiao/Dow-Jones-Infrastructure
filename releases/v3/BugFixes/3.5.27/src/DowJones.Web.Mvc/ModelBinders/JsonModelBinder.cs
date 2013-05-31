using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using DowJones.Extensions;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.ModelBinders
{
    public class JsonModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                string json = GetJSON(bindingContext);

                if (IsValidJson(json))
                    return JsonConvert.DeserializeObject(json, bindingContext.ModelType);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error binding JSON model: {0}", ex);
            }

            return base.BindModel(controllerContext, bindingContext);
        }

        private static string GetJSON(ModelBindingContext bindingContext)
        {
            var providerResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            return providerResult.AttemptedValue;
        }

        private static bool IsValidJson(string json)
        {
            // Basic matching to make sure the string starts and ends with
            // JSON object ({) or array ([) characters
            return json.HasValue() && Regex.IsMatch(json, @"^(\[.*\]|{.*})$");
        }
    }
}

namespace DowJones.Web.Mvc
{
    using ModelBinders;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class JsonModelBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new JsonModelBinder();
        }
    }
}
