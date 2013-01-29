using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc
{
    public static class SelectListHelpers
    {
        public static IEnumerable<SelectListItem> ToSelectList<TValue>(this IEnumerable<KeyValuePair<TValue, string>> source, TValue selectedValue = default(TValue))
        {
            return source.Select(x =>
                new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString(),
                    Selected = Equals(x.Key, selectedValue)
                }
            );
        }
    }

    public static class DropDownListHelpers
    {
        public static MvcHtmlString DropDownList<TEnum>(this ViewComponentFactory factory, string name, TEnum selectedValue = default(TEnum), string optionLabel = null, object htmlAttributes = null)
            where TEnum : struct
        {
            var selectList = new EnumSelectList<TEnum>(selectedValue);
            return factory.HtmlHelper.DropDownList(name, selectList, optionLabel, htmlAttributes);
        }
    }
}
