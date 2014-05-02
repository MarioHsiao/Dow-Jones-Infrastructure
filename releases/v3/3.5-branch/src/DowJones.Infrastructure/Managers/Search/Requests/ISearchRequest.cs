// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Factiva.Gateway.Messages.Search;

namespace DowJones.Managers.Search.Requests
{
    /// <summary>
    /// </summary>
    public interface ISearchRequest
    {
        IPerformContentSearchRequest GetPerformContentSearchRequest<T>() where T : IPerformContentSearchRequest, new();
    }

}
