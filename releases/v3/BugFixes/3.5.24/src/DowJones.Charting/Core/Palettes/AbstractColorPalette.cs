using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DowJones.Charting.Core.Palettes
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public abstract class AbstractColorPalette : IColorPalette, IGeneratesITXML
    {
        private const int MAX_PALETTE_SIZE = 16;
        protected static string CORDA_COLOR_PALETTE = "<cit:color-palette name=\"{0}\" {1}/>";
        protected static List<Color> palette;


        public string ToITXML()
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= Palette.Count && i <= MAX_PALETTE_SIZE; i++)
            {

                sb.AppendFormat("color-{0}=\"{1}\" ", i, ColorTranslator.ToHtml(Palette[i-1]));
            }

            return string.Format(CORDA_COLOR_PALETTE, Name, sb);
        }

        #region Implementation of IColorPalette

        public List<Color> Palette
        {
            get
            {
                if (palette == null)
                {
                    palette = new List<Color>();
                }
                return palette;
            }
        }

        public abstract string Name { get; }

        #endregion
    }
}
