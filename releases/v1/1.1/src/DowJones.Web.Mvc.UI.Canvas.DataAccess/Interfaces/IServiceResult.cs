// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    public interface IServiceResult : IAudit
    {
        string StatusMessage { get; set; }

        Audit Audit { get; set;  }
    }

    public interface IAudit
    {
        long ReturnCode { get; set; }

        long ElapsedTime { get; set; }
    }

    public interface IPopulate<in TGetRequest> where TGetRequest : IRequest
    {
        void Populate(ControlData controlData, TGetRequest request, IPreferences preferences);
    }
    public interface IPreferencesPopulate<in TGetRequest> where TGetRequest : IRequest
    {
        void Populate(ControlData controlData, TGetRequest request);
    }
    public interface IUpdateDefinition<in T> where T : IModuleUpdateRequest
    {
        void Update(ControlData controlData, T request, IPreferences preferences);
    }

    public interface IUpdatePage<in T> where T : IUpdatePageRequest
    {
        void Update(ControlData controlData, T request, IPreferences preferences);
    }

    public interface IDeletePageAssignment<in T> where T : IPageRequest 
    {
        void DeleteAssignment(ControlData controlData, T request, IPreferences preferences);
    }

    public interface ICopyPageAssignment<in T> where T : IAddPageByIdPageRequest
    {
        void CopyPage(ControlData controlData, T request, IPreferences preferences);
    }
    public interface IAddPageById<in T> where T : IAddPageByIdPageRequest
    {
        void AddPageById(ControlData controlData, T request, IPreferences preferences);
    }

    public interface IReplaceModule<in T> where T : IReplaceModuleReqeust
    {
        void ReplaceModule(ControlData controlData, T request, IPreferences preferences);
    }

    public interface IAddModuleToPage<in T> where T : IAddModuleToPageRequest
    {
        void AddModuleToPage(ControlData controlData, T request, IPreferences preferences);
    }

    public interface IUnsubscribePage<in T> where T : IPageRequest
    {
        void Unsubscribe(ControlData controlData, T request, IPreferences preferences);
    }

    public interface IDeletePage<in T> where T : IDeletePageRequest
    {
        void Delete(ControlData controlData, T request, IPreferences preferences);
    }

    public interface ISubscribePage<in T> where T : IAddPageByIdPageRequest
    {
        void Subscribe(ControlData controlData, T request, IPreferences preferences);
    }

    public interface IUpdatePagePositions<in T> where T : IUpdatePagePositionsRequest
    {
        void Update(ControlData controlData, T request, IPreferences preferences);
    }

    public interface IValidate<in T> where T : IValidate
    {
        void Validate(ControlData controlData, T request, IPreferences preferences);
    }

    public interface IUpdateDefinitionAndPopulate<in TUpdateRequest, in TGetRequest> where TUpdateRequest : IModuleUpdateRequest where TGetRequest : IModuleGetRequest
    {
        void UpdateDefinitionAndPopulate(ControlData controlData, TUpdateRequest updateRequest, TGetRequest getRequest, IPreferences preferences);
    }

    public interface ICreateDefinition<in T> where T : IModuleBaseRequest
    {
        void Create(ControlData controlData, T request, IPreferences preferences);
    }

    public interface IDeleteModule<in T> where T : IModuleBaseRequest
    {
        void DeleteModule(ControlData controlData, T request, IPreferences preferences);
    }
}
