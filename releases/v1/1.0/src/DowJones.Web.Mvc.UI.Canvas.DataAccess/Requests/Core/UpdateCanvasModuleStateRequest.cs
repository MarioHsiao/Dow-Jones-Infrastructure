// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCanvasModuleState.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using State = Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleState;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core
{
    public class UpdateCanvasModuleStateRequest : BaseModuleRequest
    {
        public State State { get; set; }
    }
}