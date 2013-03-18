// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Line.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using System.Xml.Serialization;

namespace DowJones.Charting.Core.Data
{
    /// <summary>
    /// The fill type.
    /// </summary>
    public enum FillType
    {
        /// <summary>
        /// The mountain.
        /// </summary>
        [XmlEnum("mountain")] Mountain, 

        /// <summary>
        /// The no fill type.
        /// </summary>
        [XmlEnum("none")] None, 
    }

    /// <summary>
    /// The representation of the ITXML line object.
    /// </summary>
    public class Line
    {
        /// <summary>
        /// The mi n_ lin e_ thickness.
        /// </summary>
        private const int MIN_LINE_THICKNESS = 1;

        /// <summary>
        /// The ma x_ lin e_ thickness.
        /// </summary>
        private const int MAX_LINE_THICKNESS = 4;

        /// <summary>
        /// The fill color.
        /// </summary>
        private Color fillColor = ColorTranslator.FromHtml("#000000");

        /// <summary>
        /// The fill type.
        /// </summary>
        private FillType fillType = FillType.Mountain;

        /// <summary>
        /// The line color.
        /// </summary>
        private Color lineColor = ColorTranslator.FromHtml("#000000");

        /// <summary>
        /// The line style.
        /// </summary>
        private LineStyle lineStyle = LineStyle.Solid;

        /// <summary>
        /// The line thickness.
        /// </summary>
        private int lineThickness = MIN_LINE_THICKNESS;

        #region Properties

        /// <summary>
        /// Gets or sets FillColor.
        /// </summary>
        public Color FillColor
        {
            get { return fillColor; }
            set { fillColor = value; }
        }

        /// <summary>
        /// Gets or sets FillType.
        /// </summary>
        public FillType FillType
        {
            get { return fillType; }
            set { fillType = value; }
        }

        /// <summary>
        /// Gets or sets LineStyle.
        /// </summary>
        public LineStyle LineStyle
        {
            get { return lineStyle; }
            set { lineStyle = value; }
        }

        /// <summary>
        /// Gets or sets LineColor.
        /// </summary>
        public Color LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }

        /// <summary>
        /// Gets or sets LineThickness.
        /// </summary>
        public int LineThickness
        {
            get
            {
                return lineThickness;
            }

            set
            {
                // Do Simmple validation routine.
                if (value <= MIN_LINE_THICKNESS)
                {
                    value = MIN_LINE_THICKNESS;
                }
                else if (value >= MAX_LINE_THICKNESS)
                {
                    value = MAX_LINE_THICKNESS;
                }
                
                // set it to the new value
                lineThickness = value;
            }
        }

        #endregion
    }
}