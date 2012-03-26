using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Models.NewsPages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    public interface IPageRequest : IRequest
    {
        string PageId { get; set; }
    }

    public interface IUpdatePageRequest : IPageRequest
    {
    }

    public interface IAddPageByIdPageRequest : IPageRequest
    {
        int Position { get; set; }
    }
    
    public interface IAddModuleToPageRequest : IPageRequest
    {
        string ModuleId { get; set; }
    }
    public interface IDeletePageRequest : IPageRequest
    {
        AccessScope PageAccessScope { get; set; }
    }
    public interface IReplaceModuleReqeust : IPageRequest
    {
        string ModuleIdToRemove { get; set; }
        string ModuleIdToAdd { get; set; }
    }
    public interface IUpdatePagePositionsRequest : IRequest
    {
        List<PagePosition> PagePositions { get; set; }
    }
    
}
