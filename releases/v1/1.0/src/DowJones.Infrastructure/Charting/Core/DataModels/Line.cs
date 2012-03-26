using System.Drawing;
using System.Xml.Serialization;

namespace EMG.Tools.Charting.DataModels
{
    public enum LineType
    {
        [XmlEnum("mountain")]
        Mountain,
        [XmlEnum("line")]
        Line,
    }

    public class Line
    {
        private const int MIN_LINE_THICKNESS = 1;
        private const int MAX_LINE_THICKNESS = 4;
        private LineStyle m_LineStyle = LineStyle.Solid;
        private Color m_LineColor = ColorTranslator.FromHtml("#000000");
        private int m_LineThickness = MIN_LINE_THICKNESS;
        private LineType m_LineType = LineType.Mountain;
        private Color m_FillColor = ColorTranslator.FromHtml("#000000");


        #region Properties

        public Color FillColor
        {
            get { return m_FillColor; }
            set { m_FillColor = value; }
        }

        public LineType LineType
        {
            get { return m_LineType; }
            set { m_LineType = value; }
        }

        public LineStyle LineStyle
        {
            get { return m_LineStyle; }
            set { m_LineStyle = value; }
        }

        public Color LineColor
        {
            get { return m_LineColor; }
            set { m_LineColor = value; }
        }

        public int LineThickness
        {
            get { return m_LineThickness; }
            set
            {
                // Do Simmple validation routine.
                if (value <= MIN_LINE_THICKNESS)
                    value = MIN_LINE_THICKNESS;
                else if (value >= MAX_LINE_THICKNESS)
                    value = MAX_LINE_THICKNESS;
                // set it to the new value
                m_LineThickness = value;
            }
        }

        #endregion
    }
}
