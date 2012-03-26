using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;

using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using DowJones.Utilities.Formatters;
using DowJones.Web.Mvc.UI.Components.Models;


namespace DowJones.Web.Showcase.Controllers
{
    public class CompanyProfileGraphController : DowJonesControllerBase
    {

        public ActionResult Index()
        {
            //return Components("Index",GetModel());
            return Components("Index",GetModel());
        }
        private ContentContainerModel GetModel()
        {

            return new ContentContainerModel(new IViewComponentModel [] { new NewsChartModel {
              
                ID="companyOverviewChart",
                IsFullChart = false,
                onPointClick="ClickFired", 
                OnZoomSelect= "GetZoomAreaDates",
                OnZoomReset= "GraphReset"

            }
            
            //,
            //new CompanyProfileGraphModel {
              
            //    IsFullChart = true,
            //      OnZoomSelect= "GetZoomAreaDates",
            //      onPointClick="ClickFired", 
            //    OnZoomReset= "GraphReset", 
            //    GraphHeight= 250,
            //}
            //,
            //new CompanyProfileGraphModel {
            //    IsFullChart = true,
            //    OnZoomSelect= "GetZoomAreaDates",
            //    onPointClick="ClickFired", 
            //    OnZoomReset= "GraphReset", 
            //    GraphHeight=400
            //}
            });
        }
    }
}
