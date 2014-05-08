using System;
using System.Collections.Generic;
using DowJones.Infrastructure;

namespace DowJones
{
    public class GenericEqualityComparer<T, TRet> : IEqualityComparer<T> where TRet : IComparable<TRet>
    {
        private readonly Func<T, TRet> _projector;
        private readonly IEqualityComparer<TRet> _comparer;
        private static readonly EqualityComparer<TRet> DefaultComparer = EqualityComparer<TRet>.Default;

        public GenericEqualityComparer(Func<T, TRet> projector)
            : this(projector, DefaultComparer)
        {
        }

        public GenericEqualityComparer(Func<T, TRet> projector, IEqualityComparer<TRet> comparer)
        {
            Guard.IsNotNull(projector, "projector");

            _projector = projector;
            _comparer = comparer ?? DefaultComparer;
        }

        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_projector(x), _projector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_projector(obj));
        }
    }
}