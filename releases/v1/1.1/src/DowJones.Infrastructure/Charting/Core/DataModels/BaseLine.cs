
namespace EMG.Tools.Charting.DataModels
{
    public class BaseLine : Line
    {
        private double value;
        private bool isEnabled;

        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public BaseLine()
        {
            LineType = LineType.Line;
        }
    }
}
