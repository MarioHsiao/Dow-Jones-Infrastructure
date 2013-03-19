using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DowJones.Tools.Charting.Highcharts.Core.Events
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class ChartEvents
    {
        [JsonIgnore] private string _addSeries;
        [JsonIgnore] private string _click;
        [JsonIgnore] private string _load;
        [JsonIgnore] private string _redraw;
        [JsonIgnore] private string _selection;

        public ChartEvents()
        {
            AddSeries = String.Empty;
            Click = String.Empty;
            Load = String.Empty;
            Redraw = String.Empty;
            Selection = String.Empty;
        }

        public string AddSeries
        {
            get { return !string.IsNullOrEmpty(_addSeries) ? string.Format("function(event){{ {0} }}", _addSeries) : null; }
            set { _addSeries = value; }
        }

        public string Click
        {
            get { return !string.IsNullOrEmpty(_click) ? string.Format("function(event){{ {0} }}", _click) : null; }
            set { _click = value; }
        }

        public string Load
        {
            get { return !string.IsNullOrEmpty(_load) ? string.Format("function(event){{ {0} }}", _load) : null; }
            set { _load = value; }
        }

        public string Redraw
        {
            get { return !string.IsNullOrEmpty(_redraw) ? string.Format("function(event){{ {0} }}", _redraw) : null; }
            set { _redraw = value; }
        }

        public string Selection
        {
            get { return !string.IsNullOrEmpty(_selection) ? string.Format("function(event){{ {0} }}", _selection) : null; }
            set { _selection = value; }
        }

        public override string ToString()
        {
            var list = new List<string>();

            if (!string.IsNullOrEmpty(AddSeries))
            {
                list.Add(AddSeries);
            }

            if (!string.IsNullOrEmpty(Click))
            {
                list.Add(Click);
            }

            if (!string.IsNullOrEmpty(Load))
            {
                list.Add(Load);
            }

            if (!string.IsNullOrEmpty(Redraw))
            {
                list.Add(Redraw);
            }

            if (!string.IsNullOrEmpty(Selection))
            {
                list.Add(Selection);
            }

            return string.Join(",", list.ToArray());
        }
    }
}