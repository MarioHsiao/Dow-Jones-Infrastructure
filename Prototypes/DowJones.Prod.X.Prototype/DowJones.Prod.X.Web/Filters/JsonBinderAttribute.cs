using System.Web.Mvc;
using Newtonsoft.Json;

namespace DowJones.Prod.X.Web.Filters
{
    public class JsonBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new JsonModelBinder();
        }

        public class JsonModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                try
                {
                    var json = controllerContext.HttpContext.Request[bindingContext.ModelName];
                    return JsonConvert.DeserializeObject(json, bindingContext.ModelType);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}