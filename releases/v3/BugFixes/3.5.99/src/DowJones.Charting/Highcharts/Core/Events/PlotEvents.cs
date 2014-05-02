using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DowJones.Charting.Highcharts.Core.Events
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class PlotEvents
    {
        [JsonIgnore] private string _click;
        [JsonIgnore] private string _mouseMove;
        [JsonIgnore] private string _mouseOut;
        [JsonIgnore] private string _mouseOver;

        public PlotEvents()
        {
            Click = String.Empty;
            MouseOver = String.Empty;
            MouseOut = String.Empty;
        }

        public string Click
        {
            get { return !string.IsNullOrEmpty(_click) ? string.Format("function(event){{ {0} }}", _click) : null; }
            set { _click = value; }
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

        public string MouseMove
        {
            get { return !string.IsNullOrEmpty(_mouseMove) ? string.Format("function(event){{ {0} }}", _mouseMove) : null; }
            set { _mouseMove = value; }
        }

        public override string ToString()
        {
            var list = new List<string>();

            if (!string.IsNullOrEmpty(Click))
            {
                list.Add(Click);
            }

            if (!string.IsNullOrEmpty(MouseOver))
            {
                list.Add(MouseOver);
            }

            if (!string.IsNullOrEmpty(MouseOut))
            {
                list.Add(MouseOut);
            }

            if (!string.IsNullOrEmpty(MouseMove))
            {
                list.Add(MouseMove);
            }

            return string.Join(",", list.ToArray());
        }
    }
}