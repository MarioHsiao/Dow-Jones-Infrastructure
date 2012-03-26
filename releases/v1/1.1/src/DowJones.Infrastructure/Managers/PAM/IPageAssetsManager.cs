// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPageAssetsManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Utilities.Ajax.Canvas;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using SortBy = Factiva.Gateway.Messages.Assets.Pages.V1_0.SortBy;

namespace DowJones.Managers.PAM
{
    public interface IPageAssetsManager
    {
        /// <summary>
        /// Gets the module by id.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <returns>A module object.</returns>
        Module GetModuleById(string pageId, string moduleId);

        /// <summary>
        /// Deletes the modules.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="moduleIds">The module ids.</param>
        void DeleteModules(string pageId, List<string> moduleIds);

        /// <summary>
        /// Creates the page.
        /// </summary>
        /// <param name="page">The new page object.</param>
        /// <returns>A updated page object</returns>
        Page CreatePage(CreatePageRequest page);

        /// <summary>
        /// Adds the module to page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="newModule">The new module.</param>
        /// <param name="routine">The routine.</param>
        /// <returns>a string of the module id.</returns>
        string AddModuleToPage(string pageId, int numberOfColumns, Module newModule, ValidationRoutine routine);

        /// <summary>
        /// Adds the module to end of page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="newModule">The new module.</param>
        /// <param name="routine">The routine.</param>
        /// <returns>a string of the module id.</returns>
        string AddModuleToEndOfPage(string pageId, Module newModule, ValidationRoutine routine);

        /// <summary>
        /// Updates the module positions on page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="zones">The modules.</param>
        void UpdateModulePositionsOnPage(string pageId, IEnumerable<IEnumerable<int>> zones);

        /// <summary>
        /// Updates the state of the module.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="state">The state.</param>
        void UpdateModuleState(string pageId, string moduleId, ModuleState state);

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>A page object.</returns>
        Page GetPage(string pageId);

        /// <summary>
        /// Deletes the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        void DeletePage(string pageId);

        /// <summary>
        /// Gets the page list info collection.
        /// </summary>
        /// <param name="pageTypes">The page types.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <returns>A collection of Page Information</returns>
        PageListInfoCollection GetPageListInfoCollection(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy);

        /// <summary>
        /// Deletes all pages.
        /// </summary>
        /// <param name="pageTypes">The page types.</param>
        void DeleteAllPages(IEnumerable<PageType> pageTypes);

        /// <summary>
        /// Deletes all pages.
        /// </summary>
        /// <param name="pageListInfoCollection">The page list info collection.</param>
        void DeleteAllPages(PageListInfoCollection pageListInfoCollection);

        /// <summary>
        /// Deletes the page from session cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheScope">The cache scope.</param>
        void DeletePageFromSessionCache(string cacheKey, Factiva.Gateway.Messages.Cache.SessionCache.V1_0.CacheScope cacheScope);

        /// <summary>
        /// Updates the module.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="properties">The properties.</param>
        void UpdateModule(string pageId, string moduleId, ICollection<Property> properties);

        /// <summary>
        /// Updates the modules on page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="module">The module.</param>
        void UpdateModulesOnPage(string pageId, Module module);

        /// <summary>
        /// The add module ids to page.
        /// </summary>
        /// <param name="pageRef">The page reference.</param>
        /// <param name="moduleIds">The module ids.</param>
        void AddModuleIdsToPage(string pageRef, List<string> moduleIds);

        /// <summary>
        /// The add module ids to end of the page.
        /// </summary>
        /// <param name="pageRef">The page reference.</param>
        /// <param name="moduleIds">The module ids.</param>
        void AddModuleIdsToEndOfPage(string pageRef, List<string> moduleIds);
    }
}