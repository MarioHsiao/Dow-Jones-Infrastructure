// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBox.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the TextBox type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using DowJones.Managers.Core;

namespace DowJones.Charting.Core.Data
{
    public class TextBox
    {
        private readonly string name;
        private Color color = Color.Black;
        private bool visible = true;
        
        internal TextBox(string name)
        {
            this.name = name;
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Text { get; set; }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        internal string ToITXML()
        {
            if (string.IsNullOrEmpty(Text) || string.IsNullOrEmpty(Text.Trim()) || visible == false)
            {
                return string.Format("<cit:textbox name=\"{0}\" common=\"visible:false\" />", Name);
            }

            return string.Format("<cit:textbox name=\"{0}\"><cit:text content=\"{1}\"/><cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"false\" font=\"name:Arial Unicode MS;color:{2}\"/></cit:textbox>", Name, StringUtilitiesManager.XmlEncode(Text), ColorTranslator.ToHtml(Color));
        }

        internal string ToITXMLTimeLine()
        {
            if (string.IsNullOrEmpty(Text) || string.IsNullOrEmpty(Text.Trim()) || visible == false)
            {
                return string.Format("<cit:textbox name=\"{0}\" common=\"visible:false\" />", Name);
            }

            return string.Format("<cit:textbox name=\"{0}\"><cit:text content=\"{1}\"/><cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"false\" font=\"name:Arial Unicode MS;color:#8C8C8C\"/></cit:textbox>", Name, StringUtilitiesManager.XmlEncode(Text));
        }
    }
}
