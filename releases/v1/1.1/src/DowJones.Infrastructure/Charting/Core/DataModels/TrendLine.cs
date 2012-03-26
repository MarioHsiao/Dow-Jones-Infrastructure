
namespace EMG.Tools.Charting.DataModels
{
    public class TrendLine : Line
    {
        private string m_Value;
        private bool m_IsEnabled;

        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public bool IsEnabled
        {
            get { return m_IsEnabled; }
            set { m_IsEnabled = value; }
        }

        public TrendLine()
        {
            LineType = LineType.Line;
        }
    }
}
