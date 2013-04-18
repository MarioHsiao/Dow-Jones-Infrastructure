// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Legend.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
/* Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 * Infosys -            8/06/2009               Bar Chart - Changes to integrate EMG.utility call to Corda
 */

using System;
using System.Drawing;
using System.Text;

namespace DowJones.Charting.Core.Data
{
    /// <summary>
    /// The legend.
    /// </summary>
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class Legend
    {
        /// <summary>
        /// The text format string part.
        /// </summary>
        private readonly string text = "%_SERIES_NAME";

        /// <summary>
        /// The font color.
        /// </summary>
        private Color fontColor = ColorTranslator.FromHtml("#FF0000");

        /// <summary>
        /// The font size.
        /// </summary>
        private int fontSize = 12;

        /// <summary>
        /// The font size specified.
        /// </summary>
        private bool fontSizeSpecified;

        /// <summary>
        /// The height.
        /// </summary>
        private int height;

        /// <summary>
        /// The left postion.
        /// </summary>
        private int left;

        /// <summary>
        /// The name of the legend.
        /// </summary>
        private string name = "legend";

        /// <summary>
        /// The position specified.
        /// </summary>
        private bool positionSpecified;

        /// <summary>
        /// The top position.
        /// </summary>
        private int top;

        /// <summary>
        /// The visible.
        /// </summary>
        private bool visible = true;

        /// <summary>
        /// The width.
        /// </summary>
        private int width;

        /// <summary>
        /// Initializes a new instance of the <see cref="Legend"/> class.
        /// </summary>
        /// <param name="name">The name of the legend.</param>
        /// <param name="text">The text of the titile of the legend.</param>
        public Legend(string name, string text) : this(name)
        {
            this.text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Legend"/> class.
        /// </summary>
        /// <param name="name">The name of the legend.</param>
        internal Legend(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Gets or sets a value indicating whether Visible.
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// Gets or sets FontColor.
        /// </summary>
        public Color FontColor
        {
            get { return fontColor; }
            set { fontColor = value; }
        }

        /// <summary>
        /// Gets or sets FontSize.
        /// </summary>
        public int FontSize
        {
            get
            {
                return fontSize;
            }

            set
            {
                fontSize = value;
                fontSizeSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets Top.
        /// </summary>
        public int Top
        {
            get
            {
                return top;
            }

            set
            {
                top = value;
                positionSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets Left.
        /// </summary>
        public int Left
        {
            get
            {
                return left;
            }

            set
            {
                left = value;
                positionSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets Height.
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                positionSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets Width.
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
                positionSpecified = true;
            }
        }

        /// <summary>
        /// The to itxml.
        /// </summary>
        /// <returns>A string of itxml.</returns>
        internal string ToITXML()
        {
            var sb = new StringBuilder();
            if (positionSpecified)
            {
                sb.AppendFormat("<cit:graph-legend name=\"legend\" common=\"top:{0};left:{1};height:{2};width:{3};visible:{4}\">", Top, Left, Height, Width, Visible);
            }
            else
            {
                sb.AppendFormat("<cit:graph-legend name=\"legend\" common=\"visible:{0}\">", Visible);
            }

            sb.Append("<cit:legend-settings>");
            
            if (!fontSizeSpecified)
            {
                sb.AppendFormat("<cit:legend-settings-label text=\"{0}\" minimum-font-size=\"8\" font=\"name:Arial Unicode MS;size:12;color:{1}\" />", text, ColorTranslator.ToHtml(FontColor));
            }
            else
            {
                sb.AppendFormat("<cit:legend-settings-label text=\"{0}\" minimum-font-size=\"{2}\" font=\"name:Arial Unicode MS;size:{2};color:{1}\" />", text, ColorTranslator.ToHtml(FontColor), fontSize);
            }

            sb.Append("</cit:legend-settings>");
            sb.Append("</cit:graph-legend>");
            return sb.ToString();
        }

        /// <summary>
        /// The to itxml for a barchart.
        /// </summary>
        /// <returns>A string of itxml for a barchart.</returns>
        internal string ToITXMLBarchart()
        {
            var sb = new StringBuilder();
            if (positionSpecified)
            {
                sb.AppendFormat("<cit:graph-legend name=\"legend\" common=\"top:{0};left:{1};height:{2};width:{3};visible:{4}\">", Top, Left, Height, Width, Visible);
            }
            else
            {
                sb.AppendFormat("<cit:graph-legend name=\"legend\" common=\"visible:{0}\">", Visible);
            }

            sb.Append("<cit:legend-settings>");
            
            // sb.AppendFormat("<cit:legend-settings-layout layout-items=\"vertical\" truncate-down-to=\"20\" reverse-order=\"false\" specify-column-count=\"false\" number-of-columns=\"1\" grow-vertically=\"false\" max-height=\"0\" grow-horizontally=\"false\" max-width=\"0\" align=\"false\" align-to-fixed-left-margin=\"10\" space-between=\"5\" minimum-dot-leader-width=\"30\" right-margin=\"10\"/>");
            sb.AppendFormat("<cit:legend-settings-layout layout-items=\"vertical\" space-between=\"0\"/>");
            if (!fontSizeSpecified)
            {
                sb.AppendFormat("<cit:legend-settings-label text=\"{0}\" minimum-font-size=\"8\" font=\"name:Arial Unicode MS;size:12;color:{1}\" />", text, ColorTranslator.ToHtml(FontColor));
            }
            else
            {
                sb.AppendFormat("<cit:legend-settings-label text=\"{0}\" minimum-font-size=\"8\" font=\"name:Arial Unicode MS;size:{2};color:{1}\" />", text, ColorTranslator.ToHtml(FontColor), fontSize);
            }

            sb.Append("</cit:legend-settings>");
            sb.Append("</cit:graph-legend>");
            return sb.ToString();
        }
    }
}