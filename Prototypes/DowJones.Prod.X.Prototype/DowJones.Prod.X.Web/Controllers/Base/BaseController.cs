using System;
using System.Web.Mvc;
using System.Web.SessionState;
using DowJones.DependencyInjection;
using DowJones.Prod.X.Web.Filters;
using DowJones.Prod.X.Web.Models.Interfaces;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Prod.X.Web.Controllers.Base
{
    [SessionState(SessionStateBehavior.Disabled)]
    [RequireAuthentication(Order = 0)]
    [SessionTimeoutExceptionFilter(Order = 1)]
    [Filters.HandleError(Order = 2, ExceptionType = typeof(Exception), View = "Error")]
    public abstract class BaseController : ControllerBase, IContentController
    {
        [Inject("")]
        public IBasicSiteRequestDto BasicSiteRequestDto { get; set; }
    }
}