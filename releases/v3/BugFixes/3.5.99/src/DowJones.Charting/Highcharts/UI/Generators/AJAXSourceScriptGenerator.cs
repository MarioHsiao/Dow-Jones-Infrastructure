using System;

namespace DowJones.Charting.Highcharts.UI.Generators
{
    [Serializable]
    public class AJAXSourceScriptGenerator : IScriptGenerator
    {
        public AJAXSourceScriptGenerator(string source)
        {
            Source = source;
            Delay = 500;
            ClearAll = false;
            OnlyOnce = false;
            Shift = false;
        }

        public AJAXSourceScriptGenerator(string source, int uSeconds)
        {
            Source = source;
            Delay = uSeconds;
            ClearAll = false;
            OnlyOnce = false;
            Shift = false;
        }

        public AJAXSourceScriptGenerator(string source, int uSeconds, bool clearAll, bool onlyOnce, bool shift)
        {
            Source = source;
            Delay = uSeconds;
            ClearAll = clearAll;
            OnlyOnce = onlyOnce;
            Shift = shift;
        }

        public string ClientId { get; set; }
        public string Source { get; set; }
        public int Delay { get; set; }
        public bool ClearAll { get; set; }
        public bool OnlyOnce { get; set; }
        public bool Shift { get; set; }
        public string CustomFunction { get; set; }

        public string RenderScript()
        {
            string script;

            if (!string.IsNullOrEmpty(CustomFunction))
                script = String.Format("function(){{ {0} }}", CustomFunction);
            else
            {
                script = @"chart[@Id]_JSONUpdate = function() {
                jQuery.get('[@DataSource]', function (data) {
		            var allSeries = jQuery.parseJSON(data);
            "
                         +
                         ((ClearAll) ?
                              // remove all existing series
                              @"for (var i = 0; i < chart[@Id].series.length; i++) {
		            chart[@Id].series[i].remove(true);
                }"
                              : ""
                         )
                         +
                         @"
		            //// Iterate over the lines and add categories or series
		            jQuery.each(allSeries, function (recordNo, currentSerie) {
		                
                        // if there is no serie added to the chart
                        // ,insert the one just received
                        if(typeof(chart[@Id].series[0]) == 'undefined')
                            chart[@Id].addSeries(currentSerie);
                        else
                        {
                            var serieToUpdate = new Object();
                            if(typeof(currentSerie.id) != 'undefined')
                                serieToUpdate = chart[@Id].get(currentSerie.id);    // if 'id' is defined for the serie, try to update corresponding chart serie
                            else
                                serieToUpdate = chart[@Id].series[0];   // if 'id' is not defined, update the first serie that you can find
                            
                            if(typeof(serieToUpdate) != 'undefined')
                            {
                                jQuery.each(currentSerie.data, function (pointNo, point) {
                                    serieToUpdate.addPoint(point, false, " + Shift.ToString().ToLower() + @");
                                });
                            }
                        }
                    });
                    chart[@Id].redraw();
		        });     
            };
            " +
                         ((OnlyOnce) ?
                              "setTimeout(function() {"
                              : "setInterval(function() {"
                         ) + @"
                chart[@Id]_JSONUpdate();
            }, " + Delay + ");";
            }

            script = script.Replace("@Id", ClientId);
            return !string.IsNullOrEmpty(Source) ? script.Replace("[@DataSource]", Source) : "";
        }
    }
}