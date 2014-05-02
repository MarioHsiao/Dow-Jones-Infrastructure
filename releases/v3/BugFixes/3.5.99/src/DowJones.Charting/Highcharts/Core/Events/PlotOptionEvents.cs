using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DowJones.Charting.Highcharts.Core.Events
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class PlotOptionEvents
    {
        [JsonIgnore] private string _click;
        [JsonIgnore] private string _hide;
        [JsonIgnore] private string _legendItemClick;
        [JsonIgnore] private string _mouseOut;
        [JsonIgnore] private string _mouseOver;
        [JsonIgnore] private string _show;

        public PlotOptionEvents()
        {
            Click = string.Empty;
            Hide = string.Empty;
            LegendItemClick = string.Empty;
            MouseOver = string.Empty;
            MouseOut = string.Empty;
            Show = string.Empty;
        }

        public string Click
        {
            get { return !string.IsNullOrEmpty(_click) ? string.Format("function(event){{ {0} }}", _click) : null; }
            set { _click = value; }
        }

        public string Hide
        {
            get { return !string.IsNullOrEmpty(_hide) ? string.Format("function(event){{ {0} }}", _hide) : null; }
            set { _hide = value; }
        }

        public string LegendItemClick
        {
            get { return !string.IsNullOrEmpty(_legendItemClick) ? string.Format("function(event){{ {0} }}", _legendItemClick) : null; }
            set { _legendItemClick = value; }
        }

        public string MouseOver
        {
            get { return !string.IsNullOrEmpty(_mouseOver) ? string.Format("function(event){{ {0} }}", _mouseOver) : null; }
            set { _mouseOver = value; }
        }

        public string MouseOut
        {
            get { return !string.IsNullOrEmpty(_mouseOut) ? string.Format("function(event){{ {0} }}", _mouseOut) : null; }
            set { _mouseOut = value; }
        }

        public string Show
        {
            get { return !string.IsNullOrEmpty(_show) ? string.Format("function(event){{ {0} }}", _show) : null; }
            set { _show = value; }
        }

        public override string ToString()
        {
            var list = new List<string>();

            if (!string.IsNullOrEmpty(Click))
            {
                list.Add(Click);
            }

            if (!string.IsNullOrEmpty(Hide))
            {
                list.Add(Hide);
            }

            if (!string.IsNullOrEmpty(LegendItemClick))
            {
                list.Add(LegendItemClick);
            }

            if (!string.IsNullOrEmpty(MouseOver))
            {
                list.Add(MouseOver);
            }

            if (!string.IsNullOrEmpty(MouseOut))
            {
                list.Add(MouseOut);
            }

            if (!string.IsNullOrEmpty(Show))
            {
                list.Add(Show);
            }

            return string.Join(",", list.ToArray());
        }
    }
}