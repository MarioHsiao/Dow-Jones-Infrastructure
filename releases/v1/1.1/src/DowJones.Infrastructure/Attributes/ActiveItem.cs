using System;

namespace DowJones.Utilities.Attributes
{
    /// <summary>
    /// When this attribute is added to an enumeration it indicates that enumeration item is _active
    /// and should be processed in a meaningful way.
    /// </summary> 
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ActiveItem : Attribute
    {
        private bool _active = true;


        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get { return _active; }
            set { _active = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveItem"/> class.
        /// </summary>
        public ActiveItem()
        {
            _active = true;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveItem"/> class.
        /// </summary>
        /// <param name="status">if set to <c>true</c> [status].</param>
        public ActiveItem(bool status)
        {
            _active = status;
        }
    }
}
