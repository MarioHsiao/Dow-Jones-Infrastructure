using System;

namespace DowJones.Charting.Highcharts.Core.Appearance
{
    /// <summary>
    ///     Additional CSS styles
    /// </summary>
    [Serializable]
    public class CSSObject
    {
        /// <summary>
        ///     The color property is used to set the color of the text.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        ///     Sets all the font properties in one declaration
        /// </summary>
        public string Font { get; set; }

        /// <summary>
        ///     Specifies the weight of a font
        ///     <example>
        ///         normal, bold, bolder, lighter, 100, 200, 300...
        ///     </example>
        /// </summary>
        public string FontWeight { get; set; }

        /// <summary>
        ///     Specifies the font size of text
        ///     <example>
        ///         xx-small, x-small, small, medium, large...
        ///     </example>
        /// </summary>
        public string FontSize { get; set; }

        /// <summary>
        ///     Specifies the font family for text
        /// </summary>
        public string FontFamily { get; set; }

        /// <summary>
        ///     Specifies the WhiteSpace for text
        /// </summary>
        public string WhiteSpace { get; set; }

        /// <summary>
        /// Specifies the Cursor
        /// </summary>
        public string Cursor { get; set; }

        /// <summary>
        /// Specifies the Cursor
        /// </summary>
        public string LineHeight { get; set; }

        public void CopyStyles(CSSObject model)
        {
            CopyStyles(model, false);
        }

        public void CopyStyles(CSSObject model, bool overrideValues)
        {
            if (model == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(Color) || overrideValues)
            {
                Color = model.Color;
            }

            if (string.IsNullOrEmpty(Font) || overrideValues)
            {
                Font = model.Font;
            }

            if (string.IsNullOrEmpty(FontWeight) || overrideValues)
            {
                FontWeight = model.FontWeight;
            }

            if (string.IsNullOrEmpty(FontSize) || overrideValues)
            {
                FontSize = model.FontSize;
            }

            if (string.IsNullOrEmpty(FontFamily) || overrideValues)
            {
                FontFamily = model.FontFamily;
            }

            if (string.IsNullOrEmpty(Cursor) || overrideValues)
            {
                Cursor = model.Cursor;
            }

            if (string.IsNullOrEmpty(Cursor) || overrideValues)
            {
                LineHeight = model.LineHeight;
            }
        }
    }
}