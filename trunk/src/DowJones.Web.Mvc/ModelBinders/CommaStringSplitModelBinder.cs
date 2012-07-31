// -----------------------------------------------------------------------
// <copyright file="CommaStringSplitModelBinder.cs" company="Dow Jones">
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.ModelBinders
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommaStringSplitModelBinder : IModelBinder
    {
        #region Implementation of IModelBinder

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName))
            {
                return null;
            }

            var attemptedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;
            return !String.IsNullOrEmpty(attemptedValue) ? attemptedValue.Split(',') : new string[] { };
        }

        #endregion
    }
}

namespace DowJones.Web.Mvc
{
    using ModelBinders;

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class StringSplitModelBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new CommaStringSplitModelBinder();
        }
    }
}
