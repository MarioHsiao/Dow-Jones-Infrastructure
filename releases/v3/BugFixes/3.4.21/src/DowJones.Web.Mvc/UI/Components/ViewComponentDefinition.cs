using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Infrastructure;
using DowJones.Web.Mvc.Extensions;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentDefinition
    {
        public object Data { get; set; }

        public virtual ViewComponentDefinition Parent { get; set; }

        public virtual int? Position { get; set; }

        public virtual IDictionary<string, object> Options
		{
			get { return _options; }
		}
		private readonly IDictionary<string, object> _options;

        public virtual IDictionary<string, string> HtmlAttributes
		{
            get { return _htmlAttributes; }
		}
        private readonly IDictionary<string, string> _htmlAttributes;

        public virtual IEnumerable<ViewComponentDefinition> Children
        {
            get { return _children; }
        }
        private readonly IList<ViewComponentDefinition> _children;


        public ViewComponentDefinition()
            : this(null)
        {
        }

        public ViewComponentDefinition(IEnumerable<ViewComponentDefinition> children)
        {
            _children = (children ?? Enumerable.Empty<ViewComponentDefinition>()).ToList();
			_options = new Dictionary<string,object>();
            _htmlAttributes = new Dictionary<string, string>();
        }

        public virtual IDictionary<string, object> BuildOptionsHierarchy()
        {
            var options = new Dictionary<string, object>();

            options.Merge(Options);

            if (Parent != null)
            {
                var parentOptions = Parent.BuildOptionsHierarchy();
                options.Merge(parentOptions);
            }

            return options;
        }

        public virtual void AddChild(ViewComponentDefinition viewComponent)
        {
            Guard.IsNotNull(viewComponent, "viewComponent");

            viewComponent.Parent = this;
            _children.Add(viewComponent);
        }
    }
}