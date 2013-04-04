using System;

namespace DowJones.Web.Mvc.UI
{
    /// <summary>
    /// Represents a client state Option value
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ClientPropertyAttribute : ClientStateAttribute
    {
        /// <summary>
        /// When applied to an IDictionary value,
        /// should that value be merged into the 
        /// client state options dictionary, or
        /// mapped directly as with any other type.
        /// If [true] (the default) then the
        /// value will be mapped directly.
        /// If [false], the dictionary values will
        /// be merged with the target client state 
        /// options.
        /// </summary>
        public bool Nested
        {
            get { return !Merge; }
            set { Merge = !value; }
        }

        public ClientPropertyAttribute()
            : this(default(string))
        {

        }

        public ClientPropertyAttribute(string name)
            : base(name)
        {
            Nested = true;
        }
    }
}