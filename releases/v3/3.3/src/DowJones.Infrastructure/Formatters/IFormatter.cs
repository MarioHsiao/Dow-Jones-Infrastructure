// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFormatter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Formatters
{
    /// <summary>
    /// IFormatter is interface implementation for various formatters.
    /// </summary>
    /// <typeparam name="T"> The formatable type
    /// </typeparam>
    public interface IFormatter<T>
    {
        /// <summary>
        /// Determines whether the specified formattableObject is format able.
        /// </summary>
        /// <param name="formatableObject">The formattableObject.</param>
        /// <returns>
        /// <c>true</c> if the specified format able object is format able; otherwise, <c>false</c>.
        /// </returns>
        bool IsFormattable(object formatableObject);

        /// <summary>
        /// Formats the specified object.
        /// </summary>
        /// <param name="formatableObject">The formatable object.</param>
        void Format(T formatableObject);
    }
}