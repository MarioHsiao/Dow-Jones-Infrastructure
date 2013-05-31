using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DowJones.Charting.Highcharts.Core.Events
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class PointEvents
    {
        [JsonIgnore] private string _click;
        [JsonIgnore] private string _mouseOut;
        [JsonIgnore] private string _mouseOver;
        [JsonIgnore] private string _remove;
        [JsonIgnore] private string _select;
        [JsonIgnore] private string _unselect;
        [JsonIgnore] private string _update;
        [JsonIgnore] private string _legendItemClick;

        public PointEvents()
        {
            Click = string.Empty;
            MouseOver = string.Empty;
            MouseOut = string.Empty;
            Remove = string.Empty;
            Select = string.Empty;
            Unselect = string.Empty;
            Update = string.Empty;
            LegendItemClick = string.Empty;
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

        public string LegendItemClick
        {
            get { return !string.IsNullOrEmpty(_legendItemClick) ? string.Format("function(event){{ {0} }}", _legendItemClick) : null; }
            set { _legendItemClick = value; }
        }


        public string MouseOut
        {
            get { return !string.IsNullOrEmpty(_mouseOut) ? string.Format("function(event){{ {0} }}", _mouseOut) : null; }
            set { _mouseOut = value; }
        }

        public string Remove
        {
            get { return !string.IsNullOrEmpty(_remove) ? string.Format("function(event){{ {0} }}", _remove) : null; }
            set { _remove = value; }
        }

        public string Select
        {
            get { return !string.IsNullOrEmpty(_select) ? string.Format("function(event){{ {0} }}", _select) : null; }
            set { _select = value; }
        }

        public string Unselect
        {
            get { return !string.IsNullOrEmpty(_unselect) ? string.Format("function(event){{ {0} }}", _unselect) : null; }
            set { _unselect = value; }
        }

        public string Update
        {
            get { return !string.IsNullOrEmpty(_update) ? string.Format("function(event){{ {0} }}", _update) : null; }
            set { _update = value; }
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

            if (!string.IsNullOrEmpty(Remove))
            {
                list.Add(Remove);
            }

            if (!string.IsNullOrEmpty(Select))
            {
                list.Add(Select);
            }

            if (!string.IsNullOrEmpty(Unselect))
            {
                list.Add(Unselect);
            }

            if (!string.IsNullOrEmpty(Update))
            {
                list.Add(Update);
            }

            return string.Join(",", list.ToArray());
        }
    }
}