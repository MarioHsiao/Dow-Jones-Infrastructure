To test the newly created component. In project `DowJones.Web.Showcase`, create the following 2 files (Create folder if needed)

*	`SampleComponentController.cs`

<pre><code>
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.SampleComponent;

namespace DowJones.Web.Showcase.Controllers
{
    public class SampleComponentController : Controller
    {
        public ActionResult Index()
        {
            var model = new SampleComponentModel
            {
                TextColor = "red",
                TextSize = "20px",
                Data = new SampleComponentData
                {
                    TextOne = "This is text one",
                    TextTwo = "This is text two"
                }
            };

            return View(model);
        }
    }
}
</code></pre>

*	`Views\SampleComponent\Index.cshtml`


<pre><code>
@@using DowJones.Extensions
@@{
    ViewBag.Title = "Sample Component";
}

@@Html.DJ().Render(Model)
</code></pre>

After compiling and hosting the `DowJones.Web.Showcase` site, opening the following link

http://localhost/DowJones.Web.ShowCase/SampleComponent

Should give you the following:

<div class="dj_SampleComponentContent" style="border: 1px solid black;">    Text One:    <div style="color: red; font-size: 20px;" class="textOne">        This is text one    </div>    <br><br>    Text Two:    <div style="color: red; font-size: 20px;" class="textTwo">        This is text two    </div></div>
