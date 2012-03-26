// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModuleRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    public enum CacheState
    {
        ConfigurationDefault,

        Off,
        
        ForceRefresh,
    }


    public interface ICacheState
    {
        CacheState CacheState { get; set; }
    }

    public interface IModuleBaseRequest : IRequest
    {
        /// <summary>
        /// Gets or sets PageId.
        /// </summary>
        string PageId { get; set; }
    }

    /// <summary>
    /// The base interface for a module request.
    /// </summary>
    public interface IModuleRequest : IModuleBaseRequest
    {
        /// <summary>
        /// Gets or sets ModuleId.
        /// </summary>
        string ModuleId { get; set; }
    }

    /// <summary>
    /// The The interface for a module get data request.
    /// </summary>
    public interface IModuleGetRequest : IModuleRequest
    {
    }

    /// <summary>
    /// The interface for a module update request.
    /// </summary>
    public interface IModuleUpdateRequest : IModuleRequest
    {
    }

    /// <summary>
    /// Describes the module create request
    /// </summary>
    public interface IModuleCreateRequest: IModuleBaseRequest 
    {
    }
}