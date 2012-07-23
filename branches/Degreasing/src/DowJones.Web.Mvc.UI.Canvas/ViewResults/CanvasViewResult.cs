using System.Web.Mvc;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public class CanvasViewResult : ViewResult
    {
        public Canvas Canvas { get; set; }

        public CanvasViewResult(Canvas canvas)
        {
            Canvas = canvas;
            ViewName = "Canvas";
        }
    }
}