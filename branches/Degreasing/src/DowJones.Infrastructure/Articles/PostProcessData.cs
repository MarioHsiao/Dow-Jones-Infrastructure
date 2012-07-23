using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure;

namespace DowJones.Ajax.Article
{
    public class PostProcessData
    {
        public PostProcessing Type { get; set; }
        public string ElinkValue { get; set; }
        public string ElinkText { get; set; }
    }
}
