using DowJones.Formatters;
using DowJones.Formatters.Numerical;

namespace DowJones.Extensions
{
    public static class NumberExtensions
    {
        public static void UpdateWithAbbreviatedText(this Number num)
        {

            num.Text = NumberFormatter.GetFormattedRoundedText(num.Value);
        }
    }
}