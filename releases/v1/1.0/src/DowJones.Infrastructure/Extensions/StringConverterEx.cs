﻿
/// <summary>
/// Universal conversion and parsing methods for strings.
/// These methods are avaiblable throught the generic object.Convert method:
/// Feel free to provide additional converns for string or any other object type.
/// </summary>
/// <example><code>
/// var value = "123";
/// var numeric = value.Convert().ToInt32();
/// </code></example>
public static class StringConverterEx
{

    /// <summary>
    /// Converts a string to an Int32 value
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    /// <example><code>
    /// var value = "123";
    /// var numeric = value.Convert().ToInt32();
    /// </code></example>
    public static int ToInt32(this IConverter<string> value)
    {
        return ToInt32(value, 0, false);
    }

    /// <summary>
    /// Converts a string to an Int32 value
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="ignoreException">if set to <c>true</c> any parsing exception will be ignored.</param>
    /// <returns></returns>
    /// <example><code>
    /// var value = "123";
    /// var numeric = value.Convert().ToInt32();
    /// </code></example>
    public static int ToInt32(this IConverter<string> value, int defaultValue, bool ignoreException)
    {
        if (ignoreException)
        {
            int result;
            return int.TryParse(value.Value, out result) ? result : defaultValue;
        }

        return int.Parse(value.Value);
    }
}


