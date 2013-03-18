using System;
using System.Globalization;
using System.Runtime.Serialization;
using DowJones.Formatters.Numerical;

namespace DowJones.Formatters
{
    /// <summary>
    /// Summary description for Number.
    /// </summary>
    public enum CurrenciesByBillion
    {
        CLP,
        COP,
        VEB,
        CRC,
        JPY,
        KRW,
        IDR,
        HUF,
        TRL,
        ZWD,
        ITL,
        PTE
    }

    public enum NumberFormatType
    {
        Raw = 0,
        RawAbsolute,
        Whole,
        Percentage,
        Cents,
        Precision,
        CentsStock,
        CentsStockAbsolute,
        PercentageStock,
    }

    [DataContract(Name = "number", Namespace = "")]
    [KnownType(typeof(DoubleMoney))]
    [KnownType(typeof(DoubleMoneyCurrency))]
    [KnownType(typeof(DoubleNumber))]
    [KnownType(typeof(DoubleNumberStock))]
    [KnownType(typeof(DoubleNumberStockAbsolute))]
    [KnownType(typeof(LongNumber))]
    [KnownType(typeof(Money))]
    [KnownType(typeof(Percent))]
    [KnownType(typeof(PercentStock))]
    [KnownType(typeof(PrecisionNumber))]
    [KnownType(typeof(WholeNumber))]
    public abstract class Number
    {
        // [DataMember(Name = "exp")]
        [IgnoreDataMember]
        public int Exp { get; set; }

        // [DataMember(Name = "isPositive")]
        [IgnoreDataMember]
        public bool IsPositive { get; set; }

        // [DataMember(Name = "rawText")]
        [IgnoreDataMember]
        public Text RawText { get; set; }

        [DataMember(Name = "displayText")]
        public Text Text { get; set; }

        [DataMember(Name = "value")]
        public double Value { get; set; }
    }

    [DataContract(Name = "DoubleNumber", Namespace = "")]
    public class DoubleNumber : Number
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleNumber"/> class.
        /// </summary>
        /// <param name="val">The core value.</param>
        /// <param name="exp">The exponent multiplier.</param>
        public DoubleNumber(float val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val);
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleNumber(float? val, int exp = 0)
        {
            if (val == null)
            {
                val = 0;
            }

            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleNumber(decimal val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleNumber(decimal? val, int exp = 0)
        {
            if (val == null)
            {
                val = 0;
            }

            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleNumber(double val, int exp = 0)
        {
            Exp = exp;
            Value = val;
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleNumber(double? val, int exp = 0)
        {
            if (val == null)
            {
                val = 0;
            }

            Exp = exp;
            Value = Convert.ToDouble(val);
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleNumber()
        {
            Value = 0;
            Exp = 0;
        }
    }

    [DataContract(Name = "doubleNumberStock", Namespace = "")]
    public class DoubleNumberStock : Number
    {
        public DoubleNumberStock(float val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleNumberStock(decimal val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleNumberStock(double val, int exp = 0)
        {
            Exp = exp;
            Value = val;
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleNumberStock()
        {
            Value = 0;
            Exp = 0;
        }
    }

    [DataContract(Name = "doubleNumberStockAbsolute", Namespace = "")]
    public class DoubleNumberStockAbsolute : DoubleNumberStock
    {
        public DoubleNumberStockAbsolute(float val, int exp = 0)
            : base(val, exp)
        {
        }

        public DoubleNumberStockAbsolute(decimal val, int exp = 0)
            : base(val, exp)
        {
        }

        public DoubleNumberStockAbsolute(double val, int exp = 0)
            : base(val, exp)
        {
        }

        public DoubleNumberStockAbsolute()
        {
        }
    }

    [DataContract(Name = "precisionNumber", Namespace = "")]
    public class PrecisionNumber : Number
    {
        private int precision = 1;
        
        public PrecisionNumber(float val, int precision = 1, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            Precision = precision;
            BasicNumberFormatter.Instance.Format(this);
        }

        public PrecisionNumber(decimal val, int precision = 1, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            Precision = precision;
            BasicNumberFormatter.Instance.Format(this);
        }

        public PrecisionNumber(double val, int precision = 1, int exp = 0)
        {
            Exp = exp;
            Value = val;
            Precision = precision;
            BasicNumberFormatter.Instance.Format(this);
        }

        public PrecisionNumber()
        {
            Value = 0;
        }

        [DataMember(Name = "precision")]
        public int Precision
        {
            get { return precision; }
            set { precision = value; }
        }
    }
    
    [DataContract(Name = "money", Namespace = "")]
    [KnownType(typeof(DoubleMoney))]
    [KnownType(typeof(DoubleMoneyCurrency))]
    public abstract class Money : Number
    {
        private string currency = "USD";

        [DataMember(Name = "currency")]
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
    }

    [DataContract(Namespace = "")]
    public class Text
    {
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }

    [DataContract(Name = "doubleMoneyCurrency", Namespace = "")]
    public class DoubleMoneyCurrency : Money
    {
        public DoubleMoneyCurrency(float val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleMoneyCurrency(decimal val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleMoneyCurrency(double val, int exp = 0)
        {
            Exp = exp;
            Value = val;
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleMoneyCurrency(long? val, string currency = "USD", int exp = 0)
        {
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            Currency = currency;
            Exp = exp;
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleMoneyCurrency()
        {
            Value = 0;
        }
    }

    [DataContract(Name = "doubleMoney", Namespace = "")]
    public class DoubleMoney : Money
    {
        public DoubleMoney(float val, string currency = "USD", int exp = 0)
        {
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            Currency = currency;
            Exp = exp;
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleMoney(string val, string currency = "USD", int exp = 0)
        {
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            Currency = currency;
            Exp = exp;
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleMoney(decimal val, string currency = "USD", int exp = 0)
        {
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            Currency = currency;
            Exp = exp;
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleMoney(long? val, string currency = "USD", int exp = 0)
        {
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            Currency = currency;
            Exp = exp;
            BasicNumberFormatter.Instance.Format(this);
        }

        public DoubleMoney()
        {
            Value = 0;
            Currency = "USD";
        }
    }

    public class LongNumber : Number
    {
        public new long Value;

        public LongNumber(long val, int exp = 0)
        {
            Value = val;
            Exp = exp;
            BasicNumberFormatter.Instance.Format(this);
        }

        public LongNumber()
        {
            Value = 0;
        }
    }

    [DataContract(Name = "wholeNumber", Namespace = "")]
    public class WholeNumber : Number
    {
        public WholeNumber(float val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public WholeNumber(decimal val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public WholeNumber(double val, int exp = 0)
        {
            Exp = exp;
            Value = val;
            BasicNumberFormatter.Instance.Format(this);
        }

        public WholeNumber(int val, int exp = 0)
        {
            Exp = exp;
            Value = val;
            BasicNumberFormatter.Instance.Format(this);
        }

        public WholeNumber(string val, int exp = 0)
        {
            double output;
            if (Double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out output))
            {
                Value = output;
            }

            Exp = exp;
            BasicNumberFormatter.Instance.Format(this);
        }

        public WholeNumber()
        {
            Value = 0;
        }
    }

    [DataContract(Name = "percent", Namespace = "")]
    public class Percent : Number
    {
        public Percent(float val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public Percent(float? val, int exp = 0)
        {
            if (val == null)
            {
                val = 0;
            }

            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public Percent(decimal val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public Percent(decimal? val, int exp = 0)
        {
            if (val == null)
            {
                val = 0;
            }

            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public Percent(double val, int exp = 0)
        {
            Exp = exp;
            Value = val;
            BasicNumberFormatter.Instance.Format(this);
        }

        public Percent(double? val, int exp = 0)
        {
            if (val == null)
            {
                Value = 0;
            }

            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public Percent(string val, int exp = 0)
        {
            double output;
            if (Double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out output))
            {
                Value = output;
            }

            Exp = exp;
            BasicNumberFormatter.Instance.Format(this);
        }

        public Percent()
        {
            Value = 0;
        }
    }

    public class PercentStock : Number
    {
        public PercentStock(float val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public PercentStock(decimal val, int exp = 0)
        {
            Exp = exp;
            Value = Convert.ToDouble(val, CultureInfo.InvariantCulture);
            BasicNumberFormatter.Instance.Format(this);
        }

        public PercentStock(double val, int exp = 0)
        {
            Exp = exp;
            Value = val;
            BasicNumberFormatter.Instance.Format(this);
        }

        public PercentStock()
        {
            Value = 0;
        }
    }
}