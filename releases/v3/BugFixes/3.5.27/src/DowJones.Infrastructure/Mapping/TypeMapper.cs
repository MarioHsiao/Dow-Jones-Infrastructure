using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Exceptions;
using DowJones.Infrastructure;

namespace DowJones.Mapping
{
    public class TypeMapper : TypeMapper<object, object>
    {
        public TypeMapper(Func<object, object> conversionFunction)
            : base(conversionFunction)
        {
        }

        public static implicit operator TypeMapper(Func<object, object> conversionFunction)
        {
            return new TypeMapper(conversionFunction);
        }
    }

    public abstract class TypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
    {
        private readonly Func<TSource, TTarget> conversionFunction;

        protected TypeMapper(Func<TSource,TTarget> conversionFunction = null)
        {
            this.conversionFunction = conversionFunction;
        }

        public object Map(object source)
        {
            Guard.IsNotNull(source, "source");

            if(!(source is TSource))
                throw new DowJonesUtilitiesException(
                    string.Format("{0} expects source objects of type {1} but was given a {2}", 
                                  GetType(), typeof(TSource), source.GetType() ));

            return Map((TSource)source);
        }

        public virtual TTarget Map(TSource source)
        {
            Guard.IsNotNull(conversionFunction, "conversionFunction");
            return conversionFunction(source);
        }

        public virtual IEnumerable<object> Map(IEnumerable<object> sources)
        {
            var mappedSources = sources.Select(Map);
            return mappedSources.ToArray();
        }

        public virtual IEnumerable<TTarget> Map(IEnumerable<TSource> sources)
        {
            var mappedSources = sources.Select(Map);
            return mappedSources.ToArray();
        }
    }
}