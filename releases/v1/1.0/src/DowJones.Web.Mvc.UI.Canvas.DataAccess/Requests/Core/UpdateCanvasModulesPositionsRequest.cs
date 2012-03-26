// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCanvasModulesPositions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core
{
    [DataContract(Name = "updateCanvasModulesPositionsRequest", Namespace = "")]
    public class UpdateCanvasModulesPositionsRequest : BaseModuleRequest
    {
        [DataMember(Name = "Modules")]
        public ICollection<ICollection<int>> Modules { get; set; }
    }
}
