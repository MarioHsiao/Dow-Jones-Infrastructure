// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFramePathConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the WebServicePathConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;

namespace DowJones.Converters
{
    public class PercentageConverter : StringConverter
    {
        public int Precision { get; set; }

        public PercentageConverter(int? precision = null)
        {
            Precision = precision.GetValueOrDefault();
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            double? doubleValue = value as double?;

            if(doubleValue == null)
                return base.ConvertTo(context, culture, value, destinationType);
            
            return ConvertDouble(doubleValue.Value, culture);
        }

        protected virtual string ConvertDouble(double doubleValue, CultureInfo culture)
        {
            var stringValue = doubleValue.ToString("p" + Precision, (culture ?? CultureInfo.CurrentCulture).NumberFormat);
            
            // Default format is like "123 %", whereas we want "123%" - remove the space
            return stringValue.Replace(" ", string.Empty);
        }
    }

    public class ChangePercentageConverter : PercentageConverter
    {
        public ChangePercentageConverter(int? precision = null)
            : base(precision)
        {
        }

        protected override string ConvertDouble(double doubleValue, CultureInfo culture)
        {
            var convertedString = base.ConvertDouble(doubleValue, culture);

            return (doubleValue > 0) ? ("+" + convertedString) : convertedString;
        }
    }
}