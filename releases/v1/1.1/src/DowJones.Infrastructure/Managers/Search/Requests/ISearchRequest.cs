// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Factiva.Gateway.Messages.Search;
using ConsumerAlphaPerformContentSearchRequest = Factiva.Gateway.Messages.Search.Consumer.Alpha.V1_0.PerformContentSearchRequest;
using ConsumerPerformContentSearchRequest = Factiva.Gateway.Messages.Search.Consumer.V1_0.PerformContentSearchRequest;
using FreeSearchPerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;

namespace DowJones.Utilities.Managers.Search.Requests
{
    /// <summary>
    /// </summary>
    public interface ISearchRequest
    {
        IPerformContentSearchRequest GetPerformContentSearchRequest<T>() where T : IPerformContentSearchRequest, new();
    }

}
