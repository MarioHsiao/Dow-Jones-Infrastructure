using System;
using System.Globalization;
using Factiva.Utility.Formatters;
using Factiva.Utility.Interfaces;

namespace Factiva.BusinessLayerLogic.Formatters
{
    /// <summary>
    /// Summary description for NumberFormatter.
    /// </summary>
    public class NumberFormatter : IFormatter
    {
        private readonly NumberFormatInfo _dataSourceNumberFormat = null;

        public NumberFormatter()
        {
            _dataSourceNumberFormat = new NumberFormatInfo();
            _dataSourceNumberFormat.NumberDecimalDigits = 2;
        }

        #region IFormatter Members

        public bool IsFormattable(object obj)
        {
            return (obj is Number);
        }

        public void Format(Object obj)
        {
            Number number = (Number) obj;
            double value;
            string currency;
            int exp = number.exp;

            if (number is LongNumber)
            {
                value = ((LongNumber) number).value;
                value = value*(Math.Pow(10, exp));

                if (IsValid(value))
                {
                    number.text = GetFormattedText(value, NumberFormatType.Raw);
                    number.rawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.isPositive = (value >= 0);
                    _dataSourceNumberFormat.NumberDecimalDigits = 0;
                    ((LongNumber) number).value = long.Parse(number.text.Value, _dataSourceNumberFormat);
                }
                number.exp = 0;
            }
            else if (number is WholeNumber)
            {
                value = number.value;
                number.value = value = value*(Math.Pow(10, exp));
                if (IsValid(value))
                {
                    number.text = GetFormattedText(value, NumberFormatType.Whole);
                    number.rawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.isPositive = (value >= 0);
                }
                number.exp = 0;
            }
            else if (number is DoubleNumber)
            {
                value = number.value;
                number.value = value = value*(Math.Pow(10, exp));
                if (IsValid(value))
                {
                    number.text = GetFormattedText(value, NumberFormatType.Cents);
                    number.rawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.isPositive = (value >= 0);
                }
                number.exp = 0;
            }
            else if (number is DoubleNumberStockAbsolute)
            {
                value = number.value;
                number.value = value = value*(Math.Pow(10, exp));
                if (IsValid(value))
                {
                    number.text = GetFormattedText(value, NumberFormatType.CentsStockAbsolute);
                    number.rawText = GetFormattedText(value, NumberFormatType.RawAbsolute);
                    number.isPositive = (value >= 0);
                }
                number.exp = 0;
            }
            else if (number is DoubleNumberStock)
            {
                value = number.value;
                number.value = value = value*(Math.Pow(10, exp));
                if (IsValid(value))
                {
                    number.text = GetFormattedText(value, NumberFormatType.CentsStock);
                    number.rawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.isPositive = (value >= 0);
                }
                number.exp = 0;
            }
            else if (number is PrecisionNumber)
            {
                value = number.value;
                number.value = value = value*(Math.Pow(10, exp));
                int precision = ((PrecisionNumber) number).precision;
                if (IsValid(value))
                {
                    number.text = new Text();
                    number.text.Value = GetFormattedString(value, NumberFormatType.Precision, precision);
                    number.rawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.isPositive = (value >= 0);
                }
            }
            else if (number is DoubleMoney)
            {
                value = number.value;
                currency = ((DoubleMoney) number).currency;
                number.value = value = value*(Math.Pow(10, exp));
                if (IsValid(value))
                {
                    number.text = new Text();
                    number.text.Value = GetFormattedStringByCurrency(value, currency);
                    number.rawText = GetFormattedText(value, NumberFormatType.Raw);
                    _dataSourceNumberFormat.NumberDecimalDigits = 6;
                    value = double.Parse(number.text.Value, _dataSourceNumberFormat);
                    number.isPositive = (value >= 0);
                }
                number.exp = 0;
            }
            else if (number is DoubleMoneyCurrency)
            {
                value = number.value;
                currency = ((DoubleMoneyCurrency) number).currency;
                number.value = value = value*(Math.Pow(10, exp));
                if (IsValid(value))
                {
                    number.text = new Text();
                    number.text.Value = GetFormattedStringWithCurrency(value, currency);
                    number.rawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.isPositive = (value >= 0);
                }
                number.exp = 0;
            }
            else if (number is Percent)
            {
                value = number.value;
                number.value = value = value*(Math.Pow(10, exp));
                if (IsValid(value))
                {
                    number.text = GetFormattedText(value, NumberFormatType.Percentage);
                    number.rawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.isPositive = (value >= 0);
                }
                number.exp = 0;
            }
            else if (number is PercentStock)
            {
                value = number.value;
                number.value = value = value*(Math.Pow(10, exp));
                if (IsValid(value))
                {
                    number.text = GetFormattedText(value, NumberFormatType.PercentageStock);
                    number.rawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.isPositive = (value >= 0);
                }
                number.exp = 0;
            }
            else
            {
                if (number.text == null || number.text.Value == null || number.text.Value.Length == 0)
                {
                    return;
                }
                string text = number.text.Value;
                _dataSourceNumberFormat.NumberDecimalDigits = 6;
                value = double.Parse(text, _dataSourceNumberFormat);
                value = value*(Math.Pow(10, exp));
                if (IsValid(value))
                {
                    number.text = GetFormattedText(value, NumberFormatType.Raw);
                    number.rawText = GetFormattedText(value, NumberFormatType.Raw);
                    number.isPositive = (value > 0);
                }
                number.exp = 0;
            }
        }

        #endregion

        public string Format(double value, NumberFormatType formatType)
        {
            return GetFormattedString(value, formatType, 1);
        }

        public string Format(double value, int exp, NumberFormatType formatType)
        {
            value = value*(Math.Pow(10, exp));
            return GetFormattedString(value, formatType, 4);
        }

        public string Format(double value, int exp, NumberFormatType formatType, int percision)
        {
            value = value*(Math.Pow(10, exp));
            return GetFormattedString(value, formatType, percision);
        }

        public string Format(double value, int exp, int percision)
        {
            value = value*(Math.Pow(10, exp));
            return GetFormattedString(value, NumberFormatType.Precision, percision);
        }

        public string FormatAsWholeNumber(string s)
        {
            double value;
            if (double.TryParse(s, out value))
            {
                return GetFormattedString(value, NumberFormatType.Whole, 1);
            }
            throw new ArgumentException("Unable to cast argument to base double type.");
        }

        private static bool IsValid(double d)
        {
            return (!double.IsInfinity(d) && !double.IsNaN(d));
        }

        private static Text GetFormattedText(double val, NumberFormatType type)
        {
            Text text = new Text();
            text.Value = GetFormattedString(val, type, 1);
            return text;
        }

        private static string GetFormattedString(double val, NumberFormatType type, int precision)
        {
            NumberFormatInfo nf = new NumberFormatInfo();
            nf.NumberDecimalSeparator = ".";

            string raw = val.ToString("#.################", nf);
            byte decimalDigits = 0;
            if (raw.IndexOf(".") >= 0)
            {
                decimalDigits = (byte) (raw.Length - (raw.IndexOf(".") + 1));
            }

            nf.NumberDecimalDigits = decimalDigits;
            switch (type)
            {
                case NumberFormatType.Raw:
                    nf.NumberDecimalDigits = decimalDigits;
                    return val.ToString("N", nf);
                case NumberFormatType.RawAbsolute:
                    nf.NumberDecimalDigits = decimalDigits;
                    return Math.Abs(val).ToString("N", nf);
                case NumberFormatType.Cents:
                    nf.NumberDecimalDigits = 2;
                    return (val.ToString("N", nf));
                case NumberFormatType.CentsStock:
                    nf.NumberDecimalDigits = 2;
                    return Math.Abs(val).ToString("N", nf);
                case NumberFormatType.CentsStockAbsolute:
                    return Math.Abs(val).ToString("N", nf);
                case NumberFormatType.Percentage:
                    nf.NumberDecimalDigits = 2;
                    return string.Concat(val.ToString("N", nf), "%");
                case NumberFormatType.PercentageStock:
                    nf.NumberDecimalDigits = 2;
                    if (val > 0)
                        return (string.Concat("+", val.ToString("N", nf), "%"));
                    return (val.ToString("N", nf));
                case NumberFormatType.Whole:
                    nf.NumberDecimalDigits = 0;
                    return val.ToString("N", nf);
                case NumberFormatType.Precision:
                    nf.NumberDecimalDigits = precision;
                    return val.ToString("N", nf);
            }
            return val.ToString("N", nf);
        }

        private static string GetFormattedStringByCurrency(double val, string currency)
        {
            if (Enum.IsDefined(typeof (CurrenciesByBillion), currency.ToUpper()))
            {
                val = val/1000000000;
            }
            else
            {
                val = val/1000000;
            }
            NumberFormatInfo nf = new NumberFormatInfo();
            nf.NumberDecimalDigits = 1;
            return (val.ToString("N", nf));
        }

        private static string GetFormattedStringWithCurrency(double val, string currency)
        {
            NumberFormatInfo nf = new NumberFormatInfo();
            nf.NumberDecimalDigits = 2;
            return (val.ToString("N", nf) + " " + currency);
        }
    }
}