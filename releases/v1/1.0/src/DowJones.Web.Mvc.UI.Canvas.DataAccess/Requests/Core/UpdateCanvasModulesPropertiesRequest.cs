// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCanvasModulesProperties.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Utilities.Ajax.Canvas;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core
{
    public class UpdateCanvasModulesPropertiesRequest : BaseModuleRequest
    {
        public List<Property> Properties { get; set; }
    }
}
