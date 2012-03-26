// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Extension methods for all kind of Lists implementing the IList&lt;T&gt; interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using DowJones;
using DowJones.Utilities.Exceptions;

/// <summary>
/// Extension methods for all kind of Lists implementing the IList&lt;T&gt; interface
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Inserts an item uniquely to to a list and returns a value whether the item was inserted or not.
    /// </summary>
    /// <typeparam name="T">The generic list item type.</typeparam>
    /// <param name="list">The list to be inserted into.</param>
    /// <param name="index">The index to insert the item at.</param>
    /// <param name="item">The item to be added.</param>
    /// <returns>Indicates whether the item was inserted or not</returns>
    public static bool InsertUnqiue<T>(this IList<T> list, int index, T item)
    {
        if (list.Contains(item) == false)
        {
            list.Insert(index, item);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Inserts a range of items uniquely to a list starting at a given index and returns the amount of items inserted.
    /// </summary>
    /// <typeparam name="T">The generic list item type.</typeparam>
    /// <param name="list">The list to be inserted into.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="items">The items to be inserted.</param>
    /// <returns>The amount if items that were inserted.</returns>
    public static int InsertRangeUnique<T>(this IList<T> list, int startIndex, IEnumerable<T> items)
    {
        var index = startIndex + items.Count(item => list.InsertUnqiue(startIndex, item));
        return index - startIndex;
    }

    /// <summary>
    /// Return the index of the first matching item or -1.
    /// </summary>
    /// <typeparam name="T">The generic type</typeparam>
    /// <param name="list">The target list.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>The item index</returns>
    public static int IndexOf<T>(this IList<T> list, Func<T, bool> comparison)
    {
        Guard.IsNotNull(list, "list");
        Guard.IsNotNull(comparison, "comparison");

        var match = list.SingleOrDefault(comparison);

        if (match == null)
            return -1;

        return list.IndexOf(match);
    }

    public static IList<T> GetUniques<T>(this IList<T> list)
    {
        return list.Distinct().ToList();
    }

    /// <summary>
    /// Removes a range of items from a list
    /// </summary>    
    /// <typeparam name="T">The generic type</typeparam>
    /// <param name="list">
    /// The list item.
    /// </param>
    /// <param name="itemsToRemove">
    /// The items To Remove.
    /// </param>
    public static void RemoveRange<T>(this IList<T> list, IEnumerable<T> itemsToRemove)
    {
        Guard.IsNotNull(list, "list");

        if (itemsToRemove == null)
            return;

        foreach (var item in itemsToRemove.ToArray())
        {
            if (!list.Remove(item))
            {
                throw new DowJonesUtilitiesException(-1);
            }
        }
    }
}
