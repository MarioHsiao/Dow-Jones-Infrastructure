namespace DowJones.Tools.Charting.Highcharts.UI
{
    public class ThemeScriptGenerator: IScriptGenerator
    {
        private readonly string _theme;

        public ThemeScriptGenerator(string themeName)
        {
            _theme = themeName;
        }

        public string RenderScript()
        {
            if (!string.IsNullOrEmpty(_theme))
            {
                const string themeAPI = @"
    var highchartsOptions = Highcharts.setOptions(themes['{0}']);";
                return string.Format(themeAPI, _theme);
            }
            return null;
        }
    }
}