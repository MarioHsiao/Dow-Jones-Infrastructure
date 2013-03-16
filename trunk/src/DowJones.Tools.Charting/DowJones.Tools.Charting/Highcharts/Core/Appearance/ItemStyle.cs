using System;

namespace DowJones.Tools.Charting.Highcharts.Core.Appearance
{
    [Serializable]
    public class ItemStyle
    {
        /// <summary>
        ///     The color property is used to set the color of the text.
        /// </summary>
        public string Color { get; set; }

        public void CopyStyles(ItemStyle model)
        {
            CopyStyles(model, false);
        }

        public void CopyStyles(ItemStyle model, bool overrideValues)
        {
            if (model == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(Color) || overrideValues)
            {
                Color = model.Color;
            }
        }
    }
}