// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Utilities.Formatters;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Models.Common;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using System;
using Factiva.Gateway.Messages.Preferences.V1_0;
using Factiva.Gateway.Messages.Assets.SaveSearch.V1_0;

namespace DowJones.Web.Mvc.UI.Models.Search
{
    public class SearchCriteria : SaveSearch
    {
        public virtual FIICriteria AuthorCriteria { get; set; }
    }
}
