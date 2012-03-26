using System;
using System.Linq;
using System.Web.Mvc;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Web.Mvc.UI.Components.Radar;

namespace DowJones.Web.Showcase.Controllers
{
    public class RadarController : Controller
    {
        public ActionResult Index()
        {
            //var random = new Random();
            //var companies = new[] { "Microsoft", "Google Inc.", "Dow Jones Incorporated, a subsidiary of News Corp", "Apple Inc", "Sony Inc", "Yahoo!", "News Corp", "Disney", "Coca-Cola", "Pepsi", "General Motors" };
            //var subjects = new[] { "Performance", "Bankruptcy", "Some subject with a very long name", "Earnings", "Press Releases", "Funding & capital", "Corporate Crime", "Analyst Comment", "IPO" };

            //var radarColumns =
            //    from companyName in companies
                
            //    let subjectCounts =
            //        from subject in subjects
            //        let total = random.Next(1000)
            //        let value = random.Next(total)
            //        select new RadarValue
            //                   {
            //                       Name = subject, 
            //                       Total = total, 
            //                       Value = value,
            //                   }

            //    let totalCount = subjectCounts.Sum(x => x.Total.GetValueOrDefault())

            //    select new RadarColumn
            //               {
            //                   Name = companyName, 
            //                   TotalCount = (int)totalCount, 
            //                   Unit = "Pages",
            //                   Values = subjectCounts.ToArray(),
            //                   Reference = new Reference
            //                                   {
            //                                       @ref = companyName, 
            //                                       guid = Guid.NewGuid().ToString(),
            //                                       type = "company"
            //                                   },
            //               };


            var radar = new RadarModel {
                    //Columns = radarColumns.ToArray(),
                    //Subjects = subjects.ToArray(),
                    OnClick = "OnRadarClicked"
                };

            return View("Index", radar);
        }

    }
}
