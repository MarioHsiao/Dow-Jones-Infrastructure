using DowJones.Utilities.Formatters;

namespace DowJones.Web.Showcase.Extensions
{
    public static class WholeNumberExtensions
    {
        public static WholeNumber ToWholeNumber(this decimal number)
        {
            return new WholeNumber(number);
        }

        public static WholeNumber ToWholeNumber(this double number)
        {
            return new WholeNumber(number);
        }

        public static WholeNumber ToWholeNumber(this float number)
        {
            return new WholeNumber(number);
        }

        public static WholeNumber ToWholeNumber(this int number)
        {
            return new WholeNumber(number);
        }

        public static WholeNumber ToWholeNumber(this string number)
        {
            return new WholeNumber(number);
        }
    }
}