using System.Drawing;
using System.ComponentModel;
using EMG.Utility.Managers.Core;

namespace EMG.Tools.Charting.Data
{
    public class KNCTextBox
    {
        private bool _visible = true;
        private readonly string _name;
        private string _text;
        private string _textboxname;
        private Color _color = Color.Black;
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string drilldown;

        public KNCTextBox()
        {

        }

        

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string TextBoxName
        {
            get { return _textboxname; }
            set { _textboxname = value; }

        }
        public string Drilldown
        {
            get { return drilldown; }
            set { drilldown = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        internal string ToITXML()
        {
            if (string.IsNullOrEmpty(Text) || string.IsNullOrEmpty(Text.Trim()) || _visible == false)
            {
                return string.Format("<cit:textbox name=\"{0}\" common=\"visible:false\" />", Name);
            }
            return string.Format("<cit:textbox name=\"{0}\"><cit:text content=\"{1}\"/><cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"false\" font=\"name:Arial Unicode MS;color:{2}\"/></cit:textbox>", Name, StringUtilitiesManager.XmlEncode(Text), ColorTranslator.ToHtml(Color));
        }

        internal string ToITXMLTimeLine()
        {
            if (string.IsNullOrEmpty(Text) || string.IsNullOrEmpty(Text.Trim()) || _visible == false)
            {
                return string.Format("<cit:textbox name=\"{0}\" common=\"visible:false\" />", Name);
            }
            return string.Format("<cit:textbox name=\"{0}\"><cit:text content=\"{1}\"/><cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"false\" font=\"name:Arial Unicode MS;color:#8C8C8C\"/></cit:textbox>", Name, StringUtilitiesManager.XmlEncode(Text));
        }

        internal string ToITXMLKeywordNews()
        {
            if (string.IsNullOrEmpty(Text) || string.IsNullOrEmpty(Text.Trim()) || _visible == false)
            {
                return string.Format("<cit:textbox name=\"{0}\" common=\"visible:false\" />", Name);
            }
            return string.Format("<cit:textbox name=\"{0}\"><cit:text content=\"{1}\"/></cit:textbox>", Name, StringUtilitiesManager.XmlEncode(Text));
        }
    }
}

