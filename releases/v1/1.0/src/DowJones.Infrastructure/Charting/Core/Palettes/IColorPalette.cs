using System.Collections.Generic;
using System.Drawing;

namespace DowJones.Tools.Charting.Core.Palettes
{
    internal interface IColorPalette
    {
        List<Color> Palette { get; }
        string Name { get; }
    }
}