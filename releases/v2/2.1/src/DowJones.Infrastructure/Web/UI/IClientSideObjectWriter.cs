﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace DowJones.Web.UI
{
    /// <summary>
    /// Defines the basic building block of creating client side object.
    /// </summary>
    public interface IClientSideObjectWriter
    {
        /// <summary>
        /// Starts writing this instance.
        /// </summary>
        /// <returns></returns>
        IClientSideObjectWriter Start();

        /// <summary>
        /// Appends the specified key value pair to the end of this instance.
        /// </summary>
        /// <param name="keyValuePair">The key value pair.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string keyValuePair);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, string value);

        /// <summary>
        /// Appends the specified name and nullable value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IClientSideObjectWriter AppendNullableString(string name, string value);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, int value);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, int value, int defaultValue);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, int? value);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, double value);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, double? value);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, decimal value);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, decimal? value);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, bool value);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, bool value, bool defaultValue);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, Action action);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, IList<string> values);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append(string name, IList<int> values);

        IClientSideObjectWriter AppendCollection(string name, IEnumerable value);

        /// <summary>
        /// Appends the object.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IClientSideObjectWriter AppendObject(string name, object value);

        /// <summary>
        /// Appends the specified name and Action or String specified in the ClientEvent object.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="event">Client event of the component.</param>
        /// <returns></returns>
        IClientSideObjectWriter AppendClientEvent(string name, string clientEvent);
        
        IClientSideObjectWriter AppendClientEvents(IDictionary<string, string> clientEvents);

        /// <summary>
        /// Appends the specified name and value to the end of this instance.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        IClientSideObjectWriter Append<TEnum>(string name, TEnum value, TEnum defaultValue) where TEnum : IComparable, IFormattable;

        /// <summary>
        /// Completes this instance.
        /// </summary>
        void Complete();

        IClientSideObjectWriter AppendKeyValuePairs<TValue>(string name, IEnumerable<KeyValuePair<string, TValue>> keyValuePairs);
    }
}