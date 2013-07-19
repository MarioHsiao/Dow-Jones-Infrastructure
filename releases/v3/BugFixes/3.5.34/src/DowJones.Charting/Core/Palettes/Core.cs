/* Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 * Infosys -            8/06/2009               Bar Chart - Changes to integrate EMG.utility call to Corda
 */

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DowJones.Charting.Core.Palettes
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class LineChartColorPalette : AbstractColorPalette
    {
        private const string name = "LineChartColorPalette";

        public LineChartColorPalette()
        {
            palette = new List<Color>(
                new Color[]
                    {
                        ColorTranslator.FromHtml("#5E0B68"),
                        ColorTranslator.FromHtml("#99CC00"),
                        ColorTranslator.FromHtml("#8086BD"),
                        ColorTranslator.FromHtml("#71C44A"),
                        ColorTranslator.FromHtml("#8CCBAB"),
                        ColorTranslator.FromHtml("#F4D954"),
                        ColorTranslator.FromHtml("#89B1F6"),
                        ColorTranslator.FromHtml("#983131"),
                        ColorTranslator.FromHtml("#184E4E"),
                        ColorTranslator.FromHtml("#929BC3"),
                        ColorTranslator.FromHtml("#CEB58A"),
                        ColorTranslator.FromHtml("#9A809A"),
                        ColorTranslator.FromHtml("#D4CBBA"),
                        ColorTranslator.FromHtml("#98752E"),
                        ColorTranslator.FromHtml("#4E6C93"),
                        ColorTranslator.FromHtml("#CCCCFF"),
                    }
                );
        }

        #region Overrides of AbstractColorPalette

        public override string Name
        {
            get { return name; }
        }

        #endregion
    }


    public class StackedBarChartColorPalette : AbstractColorPalette
    {
        private const string name = "StackBarColorPalette";

        public StackedBarChartColorPalette()
        {
            palette = new List<Color>(
                new Color[]
                    {
                        ColorTranslator.FromHtml("#99CC00"),
                        ColorTranslator.FromHtml("#2288AE"),
                        ColorTranslator.FromHtml("#5E0B68"),
                        ColorTranslator.FromHtml("#ED451E"),
                        ColorTranslator.FromHtml("#891946"),
                        ColorTranslator.FromHtml("#0E9A2C"),
                        ColorTranslator.FromHtml("#333333"),
                        ColorTranslator.FromHtml("#FFD015"),
                        ColorTranslator.FromHtml("#8CD8D7"),
                        ColorTranslator.FromHtml("#EF0069"),
                        ColorTranslator.FromHtml("#CEB58A"),
                        ColorTranslator.FromHtml("#9A809A"),
                        ColorTranslator.FromHtml("#D4CBBA"),
                        ColorTranslator.FromHtml("#98752E"),
                        ColorTranslator.FromHtml("#4E6C93"),
                        ColorTranslator.FromHtml("#CCCCFF"),
                    }
                );
        }

      

        

        #region Overrides of AbstractColorPalette

        public override string Name
        {
            get { return name; }
        }

        #endregion
    } 
    		 
    //Mayank added the coloured palette class for the bar chart implementation
    public class BarChartColorPalette : AbstractColorPalette
    {
        private const string name = "BarColorPalette";

        public BarChartColorPalette()
        {
            palette = new List<Color>(
                new Color[]
                    {
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#C97060"),
                        ColorTranslator.FromHtml("#7BAE5F"),
                        ColorTranslator.FromHtml("#C866B9"),
                        ColorTranslator.FromHtml("#CEA861"),
                        ColorTranslator.FromHtml("#845807"),
                        ColorTranslator.FromHtml("#4DDC68"),
                        ColorTranslator.FromHtml("#800900"),
                        ColorTranslator.FromHtml("#04841C"),
                        ColorTranslator.FromHtml("#81B6C8"),
                        ColorTranslator.FromHtml("#F96280"),
                        ColorTranslator.FromHtml("#6D036D"),
                        ColorTranslator.FromHtml("#2C9696"),
                        ColorTranslator.FromHtml("#CECE1C"),
                        ColorTranslator.FromHtml("#9C9C9C"),
                        ColorTranslator.FromHtml("#2D39F3"),
                    }
                    );
        } 
            #region Overrides of AbstractColorPalette

        public override string Name
        {
            get { return name; }
        }

        #endregion
    }

     //Mayank added the coloured palette class for the combo bar chart implementation
    public class ComboBarChartColorPalette : AbstractColorPalette
    {      
         private const string name = "ComboBarColorPalette";

        public ComboBarChartColorPalette()
        {
            palette = new List<Color>(
                new Color[]
                    {
                        ColorTranslator.FromHtml("#999999"),
                        ColorTranslator.FromHtml("#555555"),
                        ColorTranslator.FromHtml("#A5A5A5"),
                        ColorTranslator.FromHtml("#333333"),
                        ColorTranslator.FromHtml("#AAAAAA"),
                        ColorTranslator.FromHtml("#666666"),
                        ColorTranslator.FromHtml("#DDDDDD"),
                        ColorTranslator.FromHtml("#222222"),
                        ColorTranslator.FromHtml("#BBBBBB"),
                        ColorTranslator.FromHtml("#444444"),
                        ColorTranslator.FromHtml("#F8F8F8"),
                        ColorTranslator.FromHtml("#888888"),
                        ColorTranslator.FromHtml("#CCCCCC"),
                        ColorTranslator.FromHtml("#111111"),
                        ColorTranslator.FromHtml("#777777"),
                        ColorTranslator.FromHtml("#040404"),
                    }
                    );
        } 
            #region Overrides of AbstractColorPalette

        public override string Name
        {
            get { return name; }
        }

        #endregion

    }


    public class BubbleMapColorPalette : AbstractColorPalette
    {
        private const string name = "BubbleMapColorPalette";

        public BubbleMapColorPalette()
        {
            palette = new List<Color>(
                new Color[]
                    {
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                        ColorTranslator.FromHtml("#FF9122"),
                    }
                );
        }

        #region Overrides of AbstractColorPalette

        public override string Name
        {
            get { return name; }
        }

        #endregion
    }

    public class PieChartColorPalette : AbstractColorPalette
    {
        private const string name = "PieColorPalette";

        public PieChartColorPalette()
        {
            palette = new List<Color>(
                new Color[]
                    {
                        ColorTranslator.FromHtml("#99CC00"),
                        ColorTranslator.FromHtml("#2288AE"),
                        ColorTranslator.FromHtml("#5E0B68"),
                        ColorTranslator.FromHtml("#ED451E"),
                        ColorTranslator.FromHtml("#891946"),
                        ColorTranslator.FromHtml("#0E9A2C"),
                        ColorTranslator.FromHtml("#333333"),
                        ColorTranslator.FromHtml("#FFD015"),
                        ColorTranslator.FromHtml("#8CD8D7"),
                        ColorTranslator.FromHtml("#EF0069"),
                        ColorTranslator.FromHtml("#666666"),
                        ColorTranslator.FromHtml("#0D5FC1"),
                        ColorTranslator.FromHtml("#782C23"),
                        ColorTranslator.FromHtml("#E06169"),
                        ColorTranslator.FromHtml("#6FB900"),
                        ColorTranslator.FromHtml("#7852A7"),
                    }
                );
        }

        #region Overrides of AbstractColorPalette

        public override string Name
        {
            get { return name; }
        }

        #endregion
    }

    public class DiscoveryBarChartColorPalette : AbstractColorPalette
    {
        private const string name = "DiscoveryBarColorPalette";

        public DiscoveryBarChartColorPalette()
        {
            palette = new List<Color>(
                new Color[]
                    {
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                        ColorTranslator.FromHtml("#54559B"),
                    }
                    );
        }
        #region Overrides of AbstractColorPalette

        public override string Name
        {
            get { return name; }
        }

        #endregion
    }

}