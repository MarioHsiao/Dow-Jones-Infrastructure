namespace DowJones.Web.Mvc.UI.Canvas.Editors
{
    public class SwapModuleEditor : AbstractEditorModel
    {
        [ClientProperty("moduleId")]
        public int ModuleId { get; set; }

        [ClientProperty("moduleType")]
        public string ModuleType { get; set; }

    }
}
