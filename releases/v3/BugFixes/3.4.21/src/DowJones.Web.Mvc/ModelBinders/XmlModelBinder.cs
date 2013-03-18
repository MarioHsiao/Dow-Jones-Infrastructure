using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace DowJones.Web.Mvc.ModelBinders
{
    public class XmlModelBinder : IModelBinder
    {
        public object BindModel(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            var modelType = bindingContext.ModelType;
            var serializer = new XmlSerializer(modelType);

            var inputStream = controllerContext.HttpContext.Request.InputStream;
            using (var reader = new StreamReader(inputStream))
                return serializer.Deserialize(reader);
        }


        public class Provider : IModelBinderProvider
        {
            public IModelBinder GetBinder(Type modelType)
            {
                var contentType = HttpContext.Current.Request.ContentType;

                if (string.Compare(contentType, @"text/xml",
                    StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return null;
                }

                return new XmlModelBinder();
            }
        }
    }
}

namespace DowJones.Web.Mvc
{
    using ModelBinders;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class XmlModelBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new XmlModelBinder();
        }
    }
}
