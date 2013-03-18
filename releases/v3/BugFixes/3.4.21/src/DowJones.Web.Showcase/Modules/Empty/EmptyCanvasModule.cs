using System.Web.UI;
using DowJones.Web;

[assembly: WebResource("DowJones.Web.Showcase.Modules.Empty.EmptyCanvasModule.js", KnownMimeTypes.JavaScript)]

namespace DowJones.Web.Showcase.Modules.Empty
{
    [ScriptResourceAttribute(null, ResourceName="DowJones.Web.Showcase.Modules.Empty.EmptyCanvasModule.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Showcase.Modules.Empty.EmptyCanvasModule), DependsOn=new string[] {
            "AbstractCanvasModule",
            "Pager"})]

    public class EmptyCanvasModule : DowJones.Web.Mvc.UI.Canvas.AbstractCanvasModule<DowJones.Web.Showcase.Modules.Empty.EmptyModule>
    {
#line hidden

        public EmptyCanvasModule()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_EmptyCanvasModule";
            }
        }
        public override void ExecuteTemplate()
        {




WriteLiteral("\r\n");


DefineSection("EditArea", () => {

WriteLiteral(" ");


});

WriteLiteral("\r\n");


DefineSection("ContentArea", () => {

WriteLiteral("\r\n    <div>If I were a real module, I would be showing some real data here!</div>" +
"\r\n");


});


        }
    }
}
