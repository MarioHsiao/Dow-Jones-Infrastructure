using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using Newtonsoft.Json;
using DowJones.Converters;
using DowJones.Web.Mvc.UI.Components.Common.Types;
using DowJones.Extensions;
namespace DowJones.Web.Mvc.UI.Components.Models
{
    public class HeadlineListCarouselModel : ViewComponentModel
    {
        
        public HeadlineListModel HeadlineList { get; set; }
        
        public int NumberOfHeadlinesToScrollBy { get; set; }
        
        public string SelectedAccessionNo { get; set; }
        
        public string AutoScrollSpeed { get; set; }
        
    }
}
