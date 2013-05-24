// -----------------------------------------------------------------------
// <copyright file="HtmlExtensions.cs" company="Dow Jones & Co.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

public static class HtmlHelpers
{
    public enum ListType
    {
        Ordered,
        Unordered,
    }

    public static MvcHtmlString RadioButtonListForEnum<TModel, TProperty>(
        this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression, 
        ListType listType,
        string className)
    {
        var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        var names = Enum.GetNames(metaData.ModelType);
        var sb = new StringBuilder();
        var tagName = (listType == ListType.Ordered) ? "ol" : "ul";
        sb.AppendFormat("<{1} class=\"{0}\">", className, tagName);

        foreach (var name in names)
        {
            var id = string.Format(
                            "{0}_{1}_{2}",
                            htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix,
                            metaData.PropertyName,
                            name);

            var radio = htmlHelper.RadioButtonFor(
                            expression,
                            name,
                            new
                            {
                                id = id
                            }).ToHtmlString();
            sb.AppendFormat(
                            "<li class=\"item\"><label for=\"{0}\">{1}</label> {2}</li>",
                            id,
                            HttpUtility.HtmlEncode(name),
                            radio);
        }

        sb.AppendFormat("</{0}>", tagName);
        return MvcHtmlString.Create(sb.ToString());
    }


    public static MvcHtmlString RadioButtonList(
        this HtmlHelper htmlhelper, 
        string prefix, 
        IEnumerable<SelectListItem> items,
        ListType listType,
        string className)
    {
        var sb = new StringBuilder();
        var tagName = (listType == ListType.Ordered) ? "ol" : "ul";
        sb.AppendFormat("<{1} class=\"{0}\">", className, tagName);
        var index = 0;
        foreach (var li in items)
        {
            var fieldName = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}_{1}", prefix, index++);
            sb.Append("<li class=\"item\">");
            sb.Append(htmlhelper.RadioButton(
                prefix, 
                li.Value, 
                li.Selected, 
                new
                    {
                        @id = fieldName
                    }));
            sb.AppendFormat("<label class=\"label\" for=\"{0}\">{1}</label>", fieldName, li.Text);
            sb.Append("</li>");
        }

        sb.AppendFormat("</{0}>", tagName);
        return MvcHtmlString.Create(sb.ToString());
    }


    public static MvcHtmlString RadioButtonForEnum<TModel, TProperty>(
         this HtmlHelper<TModel> htmlHelper,
         Expression<Func<TModel, TProperty>> expression)
    {
        var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        var names = Enum.GetNames(metaData.ModelType);
        var sb = new StringBuilder();
        foreach (var name in names)
        {
            var id = string.Format(
                            "{0}_{1}_{2}",
                            htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix,
                            metaData.PropertyName,
                            name);

            var radio = htmlHelper.RadioButtonFor(
                            expression, 
                            name, 
                            new
                                {
                                    id = id
                                }).ToHtmlString();
            sb.AppendFormat(
                "<label for=\"{0}\">{1}</label> {2}",
                id,
                HttpUtility.HtmlEncode(name),
                radio);
        }

        return MvcHtmlString.Create(sb.ToString());
    }

    public static MvcHtmlString HiddenFor<TValue>(this HtmlHelper htmlhelper, string name, IEnumerable<TValue> items)
    {
        if (items == null || String.IsNullOrEmpty(name))
        {
            return MvcHtmlString.Empty;
        }

        var sb = new StringBuilder();
        int index = 0;
        foreach (var filter in items)
        {
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("name", String.Format( "{0}[{1}]",  name, index++) );
            tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
            tagBuilder.MergeAttribute("value", filter.ToString());
            sb.Append(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
        return MvcHtmlString.Create(sb.ToString());
    }
}