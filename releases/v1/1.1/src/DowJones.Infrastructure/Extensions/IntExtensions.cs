// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Extension methods for the string data type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

/// <summary>
/// Extension methods for the string data type
/// </summary>
public static class IntExtensions
{

    /// <summary>
    /// Performs the specified action n times based on the underlying int value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="action">The action.</param>
    public static void Times(this int value, Action action)
    {
        for (var i = 0; i < value; i++)
        {
            action();
        }
    }

    /// <summary>
    /// Performs the specified action n times based on the underlying int value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="action">The action.</param>
    public static void Times(this int value, Action<int> action)
    {
        for (var i = 0; i < value; i++)
        {
            action(i);
        }
    }
}

