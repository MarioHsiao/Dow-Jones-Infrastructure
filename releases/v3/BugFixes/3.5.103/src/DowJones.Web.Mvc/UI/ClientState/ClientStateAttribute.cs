using System;

namespace DowJones.Web.Mvc.UI
{
    public abstract class ClientStateAttribute : Attribute
    {
        public string Name { get; set; }

        /// <summary>
        /// When applied to a dictionary, should
        /// the value be merged into the target 
        /// client state or directly added as a value
        /// </summary>
        protected internal bool Merge { get; set; }

        /// <summary>
        /// When <see cref="Merge"/> is enabled,
        /// should existing destination client state 
        /// values be overwritten?
        /// If [true] (the default setting), existing 
        /// destination client state values will be overwritten.
        /// If [false], existing destination client
        /// state values will not be replaced.
        /// </summary>
        protected internal bool ReplaceExistingValuesDuringMerge { get; set; }

        /// <summary>
        /// The TypeMapper used to convert this value to
        /// another type prior to adding it to the ClientState
        /// </summary>
        public object TypeConverter { get; set; }

        protected ClientStateAttribute(string name)
        {
            Name = name;
            Merge = false;
            ReplaceExistingValuesDuringMerge = true;
        }

        protected ClientStateAttribute() : this(default(string))
        {
        }
    }
}