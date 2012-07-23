using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.UI.Exceptions;

namespace DowJones.Web.Mvc.UI
{
    public abstract class RazorTemplateBase : CustomAttributeProvider
    {
        private IDictionary<string, Action> _sections;
        private HtmlTextWriter _writer;

        protected RazorTemplateBase()
        {
            _sections = new Dictionary<string, Action>();
        }

        public virtual void ExecuteTemplate()
        {
            // Do nothing.  
            // This will be overridden by the
            // Razor template generator
        }

        public void ExecuteTemplate(StringBuilder buffer)
        {
            _writer = new HtmlTextWriter(new StringWriter(buffer));
            ExecuteTemplate();
        }

        public bool IsSectionDefined(string sectionName)
        {
            return _sections.ContainsKey(sectionName);
        }

        protected void RegisterSection(string sectionName, Action<HtmlTextWriter> section)
        {
            RegisterSection(sectionName, () => section(_writer));
        }

        protected void RegisterSection(string sectionName, Action section)
        {
            Guard.IsNotNull(section, "section");

            if(IsSectionDefined(sectionName))
                throw new DuplicateRazorSectionDefinitionException(sectionName);

            _sections.Add(sectionName, section);
        }

        protected void RenderSection(string sectionName, HtmlTextWriter writer, bool isRequired = false)
        {
            var currentWriter = _writer;
            _writer = writer;
            RenderSection(sectionName, isRequired);
            _writer = currentWriter;
        }

        protected void RenderSection(string sectionName, bool isRequired = false)
        {
            var isSectionDefined = IsSectionDefined(sectionName);

            if (!isSectionDefined && isRequired)
                throw new SectionNotImplementedException(GetType(), sectionName);

            if (isSectionDefined)
                _sections[sectionName].Invoke();
        }

        protected virtual void DefineSection(string sectionName, Action action)
        {
            if (_sections.ContainsKey(sectionName))
                throw new DuplicateRazorSectionDefinitionException(sectionName);

            _sections.Add(sectionName, action);
        }

        protected void Write(object value)
        {
            if (value == null)
                return;

            if (value is IHtmlString)
                _writer.Write(((IHtmlString) value).ToHtmlString());
            else
                _writer.Write(value);
        }

        protected void WriteLiteral(object value)
        {
            Write(value);
        }
    }
}
