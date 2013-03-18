// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSlice.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;

namespace DowJones.Search.Attributes
{
    /// <summary>
    /// Summary description for Timeslice.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TimeSlice : Attribute
    {
        /// <summary>
        /// The _slice.
        /// </summary>
        private int _slice = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSlice"/> class.
        /// </summary>
        /// <param name="slice">
        /// The slice.
        /// </param>
        public TimeSlice(int slice)
        {
            _slice = slice;
        }

        /// <summary>
        /// Gets the slice.
        /// </summary>
        /// <value>The slice.</value>
        public int Slice
        {
            get { return _slice; }
        }
    }
}