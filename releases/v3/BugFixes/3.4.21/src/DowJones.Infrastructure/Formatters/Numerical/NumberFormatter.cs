// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberFormatter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Summary description for NumberFormatter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace DowJones.Formatters.Numerical
{
    /// <summary>
    /// Baseline Class for formatting numbers.
    /// </summary>
    public class NumberFormatter : IFormatter<Number>
    {
        /// <summary>
        /// Data Source Number Format
        /// </summary>
        private readonly NumberFormatInfo _dataSourceNumberFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberFormatter"/> class.
        /// </summary>
        public NumberFormatter()
        {
            _dataSourceNumberFormat = new NumberFormatInfo
                                          {
                                              NumberDecimalDigits = 2
                                          };
        }

        #region IFormatter Members

        /// <summary>
        /// Determines whether the specified formattableObject is format able.
        /// </summary>
        /// <param name="formattableObject">The formattableObject.</param>
        /// <returns>
        /// <c>true</c> if the specified formattableObject is format able; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFormattable(object formattableObject)
        {
            return formattableObject is Number;
        }

        /// <summary>
        /// Formats the specified formattableObject.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        public void Format(Number number)
        {
            double value;
            var exp = number.Exp;

            var longNumber = number as LongNumber;
            if (longNumber != null)
            {
                value = longNumber.Value;
                value = value * Math.Pow(10, exp);

                if (IsValid(value))
                {
                    longNumber.Text = GetFormattedText(value, NumberFormatType.Raw);
                    longNumber.RawText = GetFormattedText(value, NumberFormatType.Raw);
                    longNumber.IsPositive = value >= 0;
                    _dataSourceNumberFormat.NumberDecimalDigits = 0;
                    longNumber.Value = long.Parse(longNumber.Text.Value, _dataSourceNumberFormat);
                }

                longNumber.Exp = 0;
            }
            else if (number is WholeNumber)
            {
                var num = number as WholeNumber;

                value = num.Value;
                num.Value = value = value * Math.Pow(10, exp);
                if (IsValid(value))
                {
                    num.Text = GetFormattedText(value, NumberFormatType.Whole);
                    num.RawText = GetFormattedText(value, NumberFormatType.Raw);
                    num.IsPositive = value >= 0;
                }

                num.Exp = 0;
            }
            else if (number is DoubleNumber)
            {
                value = number.Value;
                number.Value = value = value * Math.Pow(10, exp);
                if (IsValid(value))
                {
                    number.Text = GetFormattedText(value, NumberFormatType.Cents);
                    number.RawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.IsPositive = value >= 0;
                }

                number.Exp = 0;
            }
            else if (number is DoubleNumberStockAbsolute)
            {
                value = number.Value;
                number.Value = value = value * Math.Pow(10, exp);
                
                if (IsValid(value))
                {
                    number.Text = GetFormattedText(value, NumberFormatType.CentsStockAbsolute);
                    number.RawText = GetFormattedText(value, NumberFormatType.RawAbsolute);
                    number.IsPositive = value >= 0;
                }

                number.Exp = 0;
            }
            else if (number is DoubleNumberStock)
            {
                value = number.Value;
                number.Value = value = value * Math.Pow(10, exp);
                
                if (IsValid(value))
                {
                    number.Text = GetFormattedText(value, NumberFormatType.CentsStock);
                    number.RawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.IsPositive = value >= 0;
                }

                number.Exp = 0;
            }
            else
            {
                var precisionNumber = number as PrecisionNumber;
                if (precisionNumber != null)
                {
                    value = precisionNumber.Value;
                    precisionNumber.Value = value = value * Math.Pow(10, exp);
                
                    var precision = precisionNumber.Precision;
                    if (IsValid(value))
                    {
                        precisionNumber.Text = new Text
                                          {
                                              Value = GetFormattedString(value, NumberFormatType.Precision, precision)
                                          };
                        precisionNumber.RawText = GetFormattedText(value, NumberFormatType.Raw);
                        precisionNumber.IsPositive = value >= 0;
                    }
                }
                else
                {
                    string currency;
                    var money = number as DoubleMoney;
                    if (money != null)
                    {
                        value = money.Value;
                        currency = money.Currency;
                        money.Value = value = value * Math.Pow(10, exp);
                
                        if (IsValid(value))
                        {
                            money.Text = new Text
                                              {
                                                  Value = GetFormattedStringByCurrency(value, currency)
                                              };
                            money.RawText = GetFormattedText(value, NumberFormatType.Raw);
                            _dataSourceNumberFormat.NumberDecimalDigits = 6;
                            value = double.Parse(money.Text.Value, _dataSourceNumberFormat);
                            money.IsPositive = value >= 0;
                        }

                        money.Exp = 0;
                    }
                    else
                    {
                        var moneyCurrency = number as DoubleMoneyCurrency;
                        if (moneyCurrency != null)
                        {
                            value = moneyCurrency.Value;
                            currency = moneyCurrency.Currency;
                            moneyCurrency.Value = value = value * Math.Pow(10, exp);
                
                            if (IsValid(value))
                            {
                                moneyCurrency.Text = new Text
                                                  {
                                                      Value = GetFormattedStringWithCurrency(value, currency)
                                                  };
                                moneyCurrency.RawText = GetFormattedText(value, NumberFormatType.Raw);
                                moneyCurrency.IsPositive = value >= 0;
                            }

                            moneyCurrency.Exp = 0;
                        }
                        else if (number is Percent)
                        {
                            value = number.Value;
                            number.Value = value = value * Math.Pow(10, exp);
                
                            if (IsValid(value))
                            {
                                number.Text = GetFormattedText(value, NumberFormatType.Percentage);
                                number.RawText = GetFormattedText(value, NumberFormatType.Raw);
                                number.IsPositive = value >= 0;
                            }

                            number.Exp = 0;
                        }
                        else if (number is PercentStock)
                        {
                            value = number.Value;
                            number.Value = value = value * Math.Pow(10, exp);
                
                            if (IsValid(value))
                            {
                                number.Text = GetFormattedText(value, NumberFormatType.PercentageStock);
                                number.RawText = GetFormattedText(value, NumberFormatType.Raw);
                                number.IsPositive = value >= 0;
                            }

                            number.Exp = 0;
                        }
                        else
                        {
                            if (number.Text == null || string.IsNullOrEmpty(number.Text.Value))
                            {
                                return;
                            }

                            var text = number.Text.Value;
                            _dataSourceNumberFormat.NumberDecimalDigits = 6;
                            value = double.Parse(text, _dataSourceNumberFormat);
                            value = value * Math.Pow(10, exp);
                            if (IsValid(value))
                            {
                                number.Text = GetFormattedText(value, NumberFormatType.Raw);
                                number.RawText = GetFormattedText(value, NumberFormatType.Raw);
                                number.IsPositive = value > 0;
                            }

                            number.Exp = 0;
                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Formats the specified Value.
        /// </summary>
        /// <param name="value">The Value.</param>
        /// <param name="formatType">Type of the format.</param>
        /// <returns>The formatted string.</returns>
        public string Format(double value, NumberFormatType formatType)
        {
            return GetFormattedString(value, formatType, 1);
        }

        /// <summary>
        /// Formats the specified Value.
        /// </summary>
        /// <param name="value">The Value.</param>
        /// <param name="exponent">The Exponent.</param>
        /// <param name="formatType">Type of the format.</param>
        /// <returns>The formatted string.</returns>
        public string Format(double value, int exponent, NumberFormatType formatType)
        {
            value = value * Math.Pow(10, exponent);
            return GetFormattedString(value, formatType, 4);
        }

        /// <summary>
        /// Formats the specified Value.
        /// </summary>
        /// <param name="value">The Value.</param>
        /// <param name="exponent">The Exponent.</param>
        /// <param name="formatType">Type of the format.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The formatted string.</returns>
        public string Format(double value, int exponent, NumberFormatType formatType, int precision)
        {
            value = value * Math.Pow(10, exponent);
            return GetFormattedString(value, formatType, precision);
        }

        /// <summary>
        /// Formats the specified Value.
        /// </summary>
        /// <param name="value">The Value.</param>
        /// <param name="exponent">The Exponent.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The formatted string.</returns>
        public string Format(double value, int exponent, int precision)
        {
            value = value * Math.Pow(10, exponent);
            return GetFormattedString(value, NumberFormatType.Precision, precision);
        }

        /// <summary>
        /// Formats as whole number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>The formatted string.</returns>
        public string FormatAsWholeNumber(string number)
        {
            double value;
            if (double.TryParse(number, out value))
            {
                return GetFormattedString(value, NumberFormatType.Whole, 1);
            }

            throw new ArgumentException("Unable to cast argument to base double type.");
        }

        /// <summary>
        /// Gets the formatted Text.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="numberFormatType">Type of the number format.</param>
        /// <returns>The formatted string.</returns>
        public static Text GetFormattedText(double number, NumberFormatType numberFormatType)
        {
            var text = new Text
                           {
                               Value = GetFormattedString(number, numberFormatType, 1)
                           };
            return text;
        }

         /// <summary>
        /// Gets the formatted Text.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>The formatted string.</returns>
        public static Text GetFormattedRoundedText(double number)
        {
            var text = new Text
                           {
                               Value = GetRoundedHitCount(number)
                           };
            return text;
        }


        /// <summary>
        /// Gets the formatted string.
        /// </summary>
        /// <param name="value">The Value.</param>
        /// <param name="numberFormatType">Type of the number format.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The formatted string.</returns>
        internal static string GetFormattedString(double value, NumberFormatType numberFormatType, int precision = 0)
        {
            var nf = new NumberFormatInfo
                         {
                             NumberDecimalSeparator = "."
                         };

            var raw = value.ToString("#.################", nf);
            byte decimalDigits = 0;
            if (raw.IndexOf(".", StringComparison.Ordinal) >= 0)
            {
                decimalDigits = (byte)(raw.Length - (raw.IndexOf(".", StringComparison.Ordinal) + 1));
            }

            nf.NumberDecimalDigits = decimalDigits;
            switch (numberFormatType)
            {
                case NumberFormatType.Raw:
                    nf.NumberDecimalDigits = decimalDigits;
                    return value.ToString("N", nf);
                case NumberFormatType.RawAbsolute:
                    nf.NumberDecimalDigits = decimalDigits;
                    return Math.Abs(value).ToString("N", nf);
                case NumberFormatType.Cents:
                    nf.NumberDecimalDigits = 2;
                    return value.ToString("N", nf);
                case NumberFormatType.CentsStock:
                    nf.NumberDecimalDigits = 2;
                    return Math.Abs(value).ToString("N", nf);
                case NumberFormatType.CentsStockAbsolute:
                    return Math.Abs(value).ToString("N", nf);
                case NumberFormatType.Percentage:
                    nf.NumberDecimalDigits = 2;
                    return string.Concat(value.ToString("N", nf), "%");
                case NumberFormatType.PercentageStock:
                    nf.NumberDecimalDigits = 2;
                    return value > 0 ? string.Concat("+", value.ToString("N", nf), "%") : string.Concat(value.ToString("N", nf), "%");
                case NumberFormatType.Whole:
                    nf.NumberDecimalDigits = 0;
                    return value.ToString("N", nf);
                case NumberFormatType.Precision:
                    nf.NumberDecimalDigits = precision;
                    return value.ToString("N", nf);
            }

            return value.ToString("N", nf);
        }

        internal static string GetRoundedHitCount(double value)
        {

            if (value >= 100000000) // > billion
            {
                return GetFormattedString(value / 1000000, NumberFormatType.Precision, 1) + "B";
            }

            if (value >= 10000000) // > ten million
            {
                return GetFormattedString(value / 1000000, NumberFormatType.Precision, 1) + "M";
            }

            if (value >= 1000000) // > ten million
            {
                return GetFormattedString(value / 1000000, NumberFormatType.Precision, 0) + "M";
            }

            if (value >= 100000) // > hundred thousand
            {
                return GetFormattedString(value / 1000, NumberFormatType.Precision, 0) + "K";
            }

            if (value >= 10000) // > ten thousand
            {
                return GetFormattedString(value / 1000, NumberFormatType.Precision, 1) + "K";
            }

            return GetFormattedString(value, NumberFormatType.Whole);
        }
 

        /// <summary>
        /// Gets the formatted string by currency.
        /// </summary>
        /// <param name="value">The Value.</param>
        /// <param name="currency">The currency.</param>
        /// <returns>The formatted string.</returns>
        internal static string GetFormattedStringByCurrency(double value, string currency)
        {
            if (Enum.IsDefined(typeof(CurrenciesByBillion), currency.ToUpper()))
            {
                value = value / 1000000000;
            }
            else
            {
                value = value / 1000000;
            }

            var nf = new NumberFormatInfo
                         {
                             NumberDecimalDigits = 1
                         };
            return value.ToString("N", nf);
        }

        /// <summary>
        /// Gets the formatted string with currency.
        /// </summary>
        /// <param name="val">The format-able value.</param>
        /// <param name="currency">The currency.</param>
        /// <returns>A string value.</returns>
        internal static string GetFormattedStringWithCurrency(IFormattable val, string currency)
        {
            var nf = new NumberFormatInfo
                         {
                             NumberDecimalDigits = 2
                         };
            return val.ToString("N", nf) + " " + currency;
        }

        /// <summary>
        /// Determines whether the specified d is valid.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>
        /// <c>true</c> if the specified d is valid; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValid(double number)
        {
            return !double.IsInfinity(number) && !double.IsNaN(number);
        }
    }

    /// <summary>
    /// </summary>
    internal class BasicNumberFormatter
    {

        private static readonly BasicNumberFormatter HiddenInstance = new BasicNumberFormatter();
        private static readonly NumberFormatter Formatter = new NumberFormatter();

        private BasicNumberFormatter()
        {
        }

        public static BasicNumberFormatter Instance
        {
            get { return HiddenInstance; }
        }

        public void Format(Number number)
        {
            Formatter.Format(number);
        }
    }
}