// -----------------------------------------------------------------------
// <copyright file="StringSplitModelBinder.cs" company="Dow Jones">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.ModelBinders
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StringSplitModelBinder : IModelBinder
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
