using System;
using System.ComponentModel;
using System.Linq.Expressions;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI
{
    public abstract class ComponentBuilderBase<TComponent, TBuilder> : IAmFluent
        where TBuilder : ComponentBuilderBase<TComponent, TBuilder>
    {
        private static readonly Func<TComponent, TBuilder> Creator = GetCreator();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public TComponent Component
        {
            get;
            private set;
        }

        protected ComponentBuilderBase(TComponent component)
        {
            Component = component;
        }


        public static TBuilder Create(TComponent component)
        {
            return Creator(component);
        }

        private static Func<TComponent, TBuilder> GetCreator()
        {
            var componentType = typeof(TComponent);
            var targetType = typeof(TBuilder);

            var argumentExpression = Expression.Parameter(componentType, "component");
            var constructor = targetType.GetConstructor(new [] { componentType });
            var newExpression = Expression.New(constructor, argumentExpression);

            return Expression.Lambda<Func<TComponent, TBuilder>>(newExpression, argumentExpression).Compile();
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
