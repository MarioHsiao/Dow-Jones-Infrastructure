﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Newsstand.Packages
{
    [DataContract(Name = "newsstandHeadlinesPackage", Namespace = "")]
    public class NewsstandHeadlinesPackage : AbstractNewsstandPackage
    {
        [DataMember(Name = "newsstandSections")]
        public List<NewsstandSection> NewsstandSections { get; set; }
    }
}