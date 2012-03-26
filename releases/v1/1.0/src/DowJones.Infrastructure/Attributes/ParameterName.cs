// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterName.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace DowJones.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ParameterName : Attribute
    {
        public readonly string Value;

        public ParameterName(string parameterName)
        {
            Value = parameterName;
        }
    }
}