// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageListManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.NewsPages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Properties;
using DowJones.Web.Mvc.UI.Models.Common;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using DowJones.Web.Mvc.UI.Models.NewsPages.Modules;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Messages.Assets.V1_0;
using Factiva.Gateway.Messages.Track.V1_0;
using log4net;
using AccessQualifier = Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier;
using AlertsNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.AlertsNewspageModule;
using CompanyOverviewNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.CompanyOverviewNewspageModule;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using Filter = Factiva.Gateway.Messages.Assets.Pages.V1_0.Filter;
using ModuleType = DowJones.Web.Mvc.UI.Models.NewsPages.ModuleType;
using NewsPage = DowJones.Web.Mvc.UI.Models.NewsPages.NewsPage;
using PagePosition = DowJones.Web.Mvc.UI.Models.NewsPages.PagePosition;
using ShareProperties = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareProperties;
using ShareScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope;
using SortOrder = Factiva.Gateway.Messages.Assets.Common.V2_0.SortOrder;
using SyndicationNewspageModule = Factiva.Gateway.Messages.Assets.Pages.V1_0.SyndicationNewspageModule;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common
{
    /// <summary>
    /// The page list manager.
    /// </summary>
    public class PageListManager : PageAssetsManager
    {
        /// <summary>
        /// The internal log.
        /// </summary>
        private static readonly ILog InternalLog = LogManager.GetLogger(typeof(PageListManager));

        #region ..:: Constructor ::..

        /// <summary>
        /// Initializes a new instance of the <see cref="PageListManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="preferences">The interface language.</param>
        public PageListManager(IControlData controlData, IPreferences preferences)
            : base(controlData, preferences)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageListManager"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="preferences">The preferences.</param>
        public PageListManager(ControlData controlData, IPreferences preferences)
            : base(controlData, preferences)
        {
        }

        #endregion

        #region Overrides of AbstractAggregationManager

        /// <summary>
        /// Gets current Log.
        /// </summary>
        protected override ILog Log
        {
            get { return InternalLog; }
        }

        #endregion

        #region Page Methods

        /// <summary>
        /// Get user news page by id
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="forceRefresh">if set to <c>true</c> [force refresh].</param>
        /// <returns>
        /// A News Page object.
        /// </returns>
        public NewsPage GetUserNewsPage(string pageId, bool forceRefresh = false)
        {
            var prm = GetPage(pageId, forceRefresh, true);
            return AssembleNewsPageModel(prm as Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsPage, Preferences);
        }

        /// <summary>
        /// Delete the Page
        /// </summary>
        /// <param name="pageId">The page id.</param>
        public void DeleteUserNewsPage(string pageId)
        {
            DeletePamPage(pageId);
        }

        /// <summary>
        /// Updates the user news page title and description
        /// </summary>
        /// <param name="page">A page object.</param>
        public void UpdateUserNewsPageTitleDescription(NewsPage page)
        {
            UpdateUserPageTitleDescription(page);
        }

        /// <summary>
        /// Publish the user news page
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="personalAlertIds">The personal alert ids.</param>
        public void PublishUserNewsPage(string pageId, List<int> personalAlertIds)
        {
            PublishPage(pageId, personalAlertIds);
        }

        /// <summary>
        /// UnPublish the user news page
        /// </summary>
        /// <param name="pageId">The page id.</param>
        public void UnPublishUserNewsPage(string pageId)
        {
            UnPublishPage(pageId);
        }

        #endregion

        #region PageList Methods

        /// <summary>
        /// The get user news pages list.
        /// </summary>
        /// <param name="forceCacheRefresh">The force Cache Refresh.</param>
        /// <returns>
        /// A list of <see cref="NewsPage">NewsPage</see>objects.
        /// </returns>
        public List<NewsPage> GetUserNewsPagesList(bool forceCacheRefresh = false)
        {
            var pageTypes = new List<PageType>
                                {
                                    PageType.NewsPage
                                };

            var pageListColl = GetUserPageList(pageTypes, SortOrder.Ascending, SortBy.Position, forceCacheRefresh);
            return ReorderNewspages(AssembleNewsPagesListModel(WeedOutNonOwnerAndNonUserPagesFromUserPageList(pageListColl)));
        }

        /// <summary>
        /// The get user news pages list with a page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="position">The position.</param>
        /// <returns>A NewsPage List with a newspage object.</returns>
        public NewsPageListWithNewsPage GetUserNewsPagesListWithAPage(string pageId, int position = 1)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var response = GetUserPageListWithDefaultPage(
                new List<PageType> { PageType.NewsPage },
                SortOrder.Ascending,
                SortBy.Position,
                pId,
                pageId.IsNotEmpty() ? PageDefaultBy.PageId : PageDefaultBy.Position);

            var newsPageListWithNewsPage = new NewsPageListWithNewsPage
                                               {   
                                                   NewsPages = ReorderNewspages(AssembleNewsPagesListModel(WeedOutNonOwnerAndNonUserPagesFromUserPageList(response.PageListInfoCollection))),
                                                   RequestedNewsPage = AssembleNewsPageModel(response.RequestedPage as Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsPage, Preferences)
                                               };
            return newsPageListWithNewsPage;
        }

        /// <summary>
        /// The update page positions.
        /// </summary>
        /// <param name="pagePositions">
        /// The page positions.
        /// </param>
        public void UpdatePagePositions(List<PagePosition> pagePositions)
        {
            UpdatePamPagePositions(pagePositions);
        }

        /// <summary>
        /// Removes an assigned page from the user's list. This is when user is dismissing a page from his/her list.
        /// Not an admin action
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>A Boolean.</returns>
        public bool RemoveAsssigedPage(string pageId)
        {
            return EnableDisable.Disable == EnableDisablePage(pageId, EnableDisable.Disable);
        }

        #endregion

        #region Page GardenPage Methods
        /// <summary>
        /// Get all subscribed news page list.
        /// </summary>
        /// <param name="accessQualifiers">The access qualifiers.</param>
        /// <param name="metaDataFilter">The meta data filter.</param>
        /// <returns>A list of <see cref="NewsPage"/> objects.</returns>
        public List<NewsPage> GetSubscribableNewsPagesList(List<AccessQualifier> accessQualifiers, MetadataFilter metaDataFilter)
        {
            var coll = GetSubscribablePages(PageType.NewsPage, SortOrder.Ascending, SortBy.Name, accessQualifiers, metaDataFilter);
            return coll.Select(AssembleNewsPageModelFromPageListInfo).ToList();
        }

        /// <summary>
        /// The get industry subscribed news pages list.
        /// </summary>
        /// <returns>A list of <see cref="NewsPage"/> objects.</returns> <returns></returns>
        public List<NewsPage> GetIndustrySubscribableNewsPagesList()
        {
            var coll = GetIndustrySubScribableNewsPages();
            return coll.Select(AssembleNewsPageModelFromPageListInfo).ToList();
        }

        /// <summary>
        /// The get geographic sub-scribable news pages list.
        /// </summary>        
        /// <returns>A list of <see cref="NewsPage"/> objects.</returns>
        public List<NewsPage> GetGeographicSubscribableNewsPagesList()
        {
            var coll = GetGeographicSubScribableNewsPages();
            return coll.Select(AssembleNewsPageModelFromPageListInfo).ToList();
        }

        /// <summary>
        /// The get topic sub-scribable news pages list.
        /// </summary>
        /// <returns>A list of <see cref="NewsPage"/> objects.</returns>
        public List<NewsPage> GetTopicSubscribableNewsPagesList()
        {
            var coll = GetTopicSubScribableNewsPages();
            return coll.Select(AssembleNewsPageModelFromPageListInfo).ToList();
        }

        /// <summary>
        /// The get user sub-scribable news pages list.
        /// </summary>
        /// <returns>A list of <see cref="NewsPage"/> objects.</returns>
        public List<NewsPage> GetUserSubscribableNewsPagesList()
        {
            var coll = GetUserSharedSubScribableNewsPages();
            return coll.Select(AssembleNewsPageModelFromPageListInfo).ToList();
        }

        #endregion

        #region AdminPage methods

        /// <summary>
        /// The get admin news pages list.
        /// </summary>
        /// <returns>A list of <see cref="AdminNewsPage"/> objects.</returns>
        public List<AdminNewsPage> GetAdminNewsPagesList()
        {
            var coll = GetAdminSharedSubScribableNewsPages();
            return coll.Select(AssembleAdminNewsPageModelFromPageListInfo).ToList();
        }

        public List<KeyValuePair<string, string>> GetAdminPageOwner(int pageId)
        {
            var userInformation = new List<KeyValuePair<string, string>>();
            var request = new GetOwnerByAssetIDRequest
                              {
                                  ID = pageId, 
                                  Type = AssetType.Page
                              };
            var response = Process<GetOwnerByAssetIDResponse>(request);
            if (response != null && !String.IsNullOrEmpty(response.UserID) && !String.IsNullOrEmpty(response.Namespace))
            {
                userInformation.Add(new KeyValuePair<string, string>("OwnerUserId", response.UserID));
                userInformation.Add(new KeyValuePair<string, string>("OwnerNamespace", response.Namespace));
            }

            return userInformation;
        }

        /// <summary>
        /// The get admin sub-ascribable news pages list.
        /// </summary>
        /// <returns></returns>
        public List<AdminNewsPage> GetAdminSubscribableNewsPagesList()
        {
            var pageCollection = GetAdminSharedSubScribableNewsPages();
            var collectionOfAdminListedPages = new PageListInfoExCollection();
            foreach (var pagelistinfo in pageCollection.Where(pagelistinfo => pagelistinfo.ShareProperties != null && pagelistinfo.ShareProperties.AccessControlScope == AccessControlScope.Account))
            {
                collectionOfAdminListedPages.Add(pagelistinfo);
            }

            return collectionOfAdminListedPages.Select(AssembleAdminNewsPageModelFromPageListInfo).ToList();
        }

        /// <summary>
        /// Enable or Push A Page to Assigned into the account user's list.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <returns>
        /// The push admin page.
        /// </returns>
        public void PushAdminPage(string pageId)
        {
            SetPageShareProperties(
                pageId,
                new ShareProperties 
                    {
                        AccessControlScope = AccessControlScope.Account,
                        ListingScope = ShareScope.Personal,                /*Since the Listing Scope setting was not allowing us to unpublish admin page and remove the page from user's or admin's list, the AccountAdmin got introduced
                 and so, the page should be @ Account level to get into the page garden*/

                        AssignedScope = ShareScope.Account,
                        SharePromotion = ShareScope.Personal
                    });
        }

        /// <summary>
        /// Disable or Recall a pushed/enabled page back.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        public void RecallPushedAdminPage(string pageId)
        {
            SetPageShareProperties(
                pageId,
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.Account,
                        ListingScope = ShareScope.Personal,                /*Since the Listing Scope setting was not allowing us to unpublish admin page and remove the page from user's or admin's list, the AccountAdmin got introduced
                 and so, the page should be @ Account level to get into the page garden*/

                        AssignedScope = ShareScope.Personal,
                        SharePromotion = ShareScope.Personal
                    });
        }

        /// <summary>
        /// Publish the page from the Admin Page garden
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="personalAlertIds">The personal alert ids.</param>
        /// <param name="pushToUsersList">if set to <c>true</c> [push to users list].</param>
        public void PublishAdminPage(string pageId, List<int> personalAlertIds, bool pushToUsersList = false)
        {
            // publish modules, alerts and then page. and if the page is to be pushed then set the flag according this will set the assigned scope
            PublishAndPushAdminPage(pageId, personalAlertIds, pushToUsersList);
        }

        /// <summary>
        /// UnPublish and optionally recall the pushed admin page. If Recall of a pushed admin page fails, un-publish will not go through
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="recallPushedPage">if set to <c>true</c> [recall pushed page].</param>
        public void UnPublishAdminPage(string pageId, bool recallPushedPage = false)
        {
            // unpublished the page now
            UnPublishAccountPage(pageId);
        }

        /// <summary>
        /// Publish the page from the Admin Page garden
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="personalAlertIds">The personal alert ids.</param>
        private void PublishPage(string pageId, List<int> personalAlertIds)
        {
            var page = GetPage(pageId, false, false);

            if (page != null)
            {
                if (page.ModuleCollection != null)
                {
                    MakePageModulesPublic(page.ModuleCollection);
                    if (personalAlertIds != null && personalAlertIds.Count > 0)
                    {
                        MakePersonalAlertsPublic(personalAlertIds);
                    }
                }
            }

            SetPageShareProperties(
                pageId, 
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.Account, 
                        ListingScope = ShareScope.Personal, 
                        /* Since the Listing Scope setting was not allowing us to un-publish admin page and remove the page from user's or admin's list, the AccountAdmin got introduced
                           and so, the page should be @ Account level to get into the page garden*/
                        AssignedScope = ShareScope.Personal, 
                        SharePromotion = ShareScope.Personal
                    });
        }

        /// <summary>
        /// Publish the page from the Admin Page garden
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="personalAlertIds">The personal alert ids.</param>
        /// <param name="pushToUsersList">Push the page to the users in the account</param>
        private void PublishAndPushAdminPage(string pageId, List<int> personalAlertIds, bool pushToUsersList = false)
        {
            var page = GetPage(pageId, false, false);

            if (page == null || page.ModuleCollection == null)
            {
                return;
            }
            
            MakePageModulesPublic(page.ModuleCollection);
            if (personalAlertIds != null && personalAlertIds.Count > 0)
            {
                MakePersonalAlertsPublic(personalAlertIds);
            }

            SetPageShareProperties(
                pageId,
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.Account,
                        ListingScope = ShareScope.Personal,                /*Since the Listing Scope setting was not allowing us to unpublish admin page and remove the page from user's or admin's list, the AccountAdmin got introduced
                 and so, the page should be @ Account level to get into the page garden*/

                        AssignedScope = pushToUsersList ? ShareScope.Account : ShareScope.Personal,
                        SharePromotion = ShareScope.Personal
                    });
        }

        /// <summary>
        /// Un-publish account page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        private void UnPublishAccountPage(string pageId)
        {
            var page = GetPage(pageId, false, false);

            if (page != null && page.ModuleCollection != null)
            {
                MakePageModulesPrivate(page.ModuleCollection);
            }
            
            // this is an account page. the page will remain an account page, but the page is not listed in the garden
            // and is not assigned to the users.
            SetPageShareProperties(
                pageId,
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.AccountAdmin,
                        /* Since the Listing Scope setting was not allowing us to un-publish admin page and remove the page from user's or admin's list, the AccountAdmin got introduced
                           and so, the page should be @ Account level to get into the page garden*/
                        ListingScope = ShareScope.Personal,
                        AssignedScope = ShareScope.Personal,
                        SharePromotion = ShareScope.Personal
                    });
        }

        /// <summary>
        /// Unpublished or recall the page from the Admin Page garden
        /// </summary>
        /// <param name="pageId">The page id.</param>
        private void UnPublishPage(string pageId)
        {
            // get the page, and make he modules private...
            var page = GetPage(pageId, false, false);

            if (page != null && page.ModuleCollection != null)
            {
                MakePageModulesPrivate(page.ModuleCollection);
            }

            SetPageShareProperties(
                pageId,
                new ShareProperties
                    {
                        AccessControlScope = AccessControlScope.Personal,
                        ListingScope = ShareScope.Personal,
                        AssignedScope = ShareScope.Personal,
                        SharePromotion = ShareScope.Personal
                    });
        }

        #endregion

        #region Page Action methods

        /// <summary>
        /// subscribe to the page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="pagePosition">The page position.</param>
        /// <returns>
        /// The subscribe to page.
        /// </returns>
        public string SubscribeToPage(string pageId, int pagePosition = 1)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var request = new SubscribePageRequest
                              {
                                  PagePosition = new Factiva.Gateway.Messages.Assets.Pages.V1_0.PagePosition
                                                     {
                                                         Id = pId, 
                                                         Position = pagePosition
                                                     }
                              };
            var response = Process<SubscribePageResponse>(request);
            return response.Id.ToString();
        }

        /// <summary>
        /// The un subscribe to page.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        public void UnSubscribeToPage(string pageId)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var request = new UnSubscribePageRequest
                              {
                                  Id = pId
                              };
            Process<UnSubscribePageResponse>(request);
        }

        /// <summary>
        /// The create a factiva page.
        /// </summary>
        /// <param name="newsPage">The news page.</param>
        /// <returns>
        /// The create a factiva page.
        /// </returns>
        public string CreateAFactivaPage(NewsPage newsPage)
        {
            var request = new CreatePageRequest
                              {
                                  Page = new Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsPage
                                             {
                                                 ShareProperties = new ShareProperties
                                                                       {
                                                                           AccessControlScope = AccessControlScope.Everyone,
                                                                           SharePromotion = ShareScope.Personal,
                                                                           AssignedScope = ShareScope.Personal,
                                                                           ListingScope = ShareScope.Personal,
                                                                       },
                                                 PageQualifier = AccessQualifier.Factiva,
                                                 PageProperties = new NewsPageProperties
                                                                      {
                                                                          Position = newsPage.Position,
                                                                          Description = newsPage.Description,
                                                                          Title = newsPage.Title,
                                                                          PageMetaData = new Metadata
                                                                                             {
                                                                                                 IndustryCollection = new MetadataFieldCollection { new MetadataField { Text = newsPage.MetaData.MetaDataCode } },
                                                                                                 CategoryCollection = new MetadataFieldCollection { new MetadataField { Text = newsPage.CategoryInfo.CategoryCode } }
                                                                                             }
                                                                      },
                                             }
                              };
             
            request.Page.ModuleCollection = new ModuleCollection();
            var pos = 0;
            if (newsPage.ModuleCollection != null)
            {
                foreach (var module in newsPage.ModuleCollection)
                {
                    request.Page.ModuleCollection.Add(AssembleNewsPageModule(module, ++pos));
                }
            }

            var createPageResponse = Invoke<CreatePageResponse>(request).ObjectResponse;
            if (createPageResponse.Id > 0)
            {
                if (Settings.Default.IncludeCacheKeyGeneration &&
                    Settings.Default.CachePageItems)
                {
                    // var rootAccessControlScope = GetRootAccessControlScope(createPageResponse.Id, createPageResponse.Id, AccessControlScope.Everyone, AccessQualifier.Factiva);
                    var generator = new PageCacheKeyGenerator
                    {
                        PageId = createPageResponse.Id.ToString(),
                        ParentId = createPageResponse.Id.ToString(),
                        PageAccessQualifier = AccessQualifier.Factiva,
                        RootAccessControlScope = AccessControlScope.Everyone,
                        PageAccessControlScope = AccessControlScope.Everyone,
                    };

                    return generator.ToReference();
                }

                return createPageResponse.Id.ToString();
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.UnableToCreatePage);
        }

        /// <summary>
        /// The create admin page.
        /// </summary>
        /// <param name="newsPage">
        /// The news page.
        /// </param>
        /// <returns>
        /// The create admin page.
        /// </returns>
        public string CreateAdminPage(NewsPage newsPage)
        {
            //Updated the request with the page meta data so that it is there on the clopied pages
            var request = new CreatePageRequest
                              {
                                  Page = new Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsPage
                                             {
                                                 ShareProperties = new ShareProperties
                                                                       {
                                                                           AccessControlScope = AccessControlScope.AccountAdmin,                /*Since the Listing Scope setting was not allowing us to unpublish admin page and remove the page from user's or admin's list, the AccountAdmin got introduced
                 and so, the page should be @ Account level to get into the page garden*/

                                                                           SharePromotion = ShareScope.Personal,
                                                                           AssignedScope = ShareScope.Personal,
                                                                           ListingScope = ShareScope.Personal,
                                                                       },
                                                 PageQualifier = AccessQualifier.Account,
                                                 PageProperties = new NewsPageProperties
                                                                      {
                                                                          Description = newsPage.Description,
                                                                          Title = newsPage.Title,
                                                                          PageMetaData = (newsPage.MetaData != null && newsPage.CategoryInfo != null) ? ((newsPage.MetaData.MetaDataType == MetaDataType.Industry) ? new Metadata
                                                                          {
                                                                              IndustryCollection = new MetadataFieldCollection { new MetadataField { Text = newsPage.MetaData.MetaDataCode } },
                                                                              CategoryCollection = new MetadataFieldCollection { new MetadataField { Text = newsPage.CategoryInfo.CategoryCode } }
                                                                          } : new Metadata
                                                                          {
                                                                              RegionCollection = new MetadataFieldCollection { new MetadataField { Text = newsPage.MetaData.MetaDataCode } },
                                                                              CategoryCollection = new MetadataFieldCollection { new MetadataField { Text = newsPage.CategoryInfo.CategoryCode } }
                                                                          }) : null,
                                                                      },
                                             }
                              };

            var modulesListToAdd = new List<KeyValuePair<int, int>>();
            if (newsPage.ModuleCollection != null)
            {
                request.Page.ModuleCollection = new ModuleCollection();
                var pos = 0;
                foreach (var module in newsPage.ModuleCollection)
                {
                    var newsPageModule = AssembleNewsPageModule(module, ++pos);
                    if (newsPageModule != null)
                    {
                        request.Page.ModuleCollection.Add(AssembleNewsPageModule(module, module.Position));
                    }
                    else
                    {
                        modulesListToAdd.Add(new KeyValuePair<int, int>(module.Id, module.Position));
                    }
                }
            }

            var createPageResponse = Invoke<CreatePageResponse>(request).ObjectResponse;
            if (createPageResponse.Id > 0)
            {
                if (modulesListToAdd.Count > 0)
                {
                    AddModuleIdsToPage(createPageResponse.Id.ToString(), modulesListToAdd);
                }

                if (Settings.Default.IncludeCacheKeyGeneration &&
                    Settings.Default.CachePageItems)
                {
                    // var rootAccessControlScope = GetRootAccessControlScope(createPageResponse.Id, createPageResponse.Id, AccessControlScope.Everyone, AccessQualifier.Factiva);
                    var generator = new PageCacheKeyGenerator
                    {
                        PageId = createPageResponse.Id.ToString(),
                        ParentId = createPageResponse.Id.ToString(),
                        PageAccessQualifier = AccessQualifier.Account,
                        RootAccessControlScope = AccessControlScope.Account,
                        PageAccessControlScope = AccessControlScope.Account,
                    };

                    //return generator.ToReference();
                }

                return createPageResponse.Id.ToString();
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.UnableToCreatePage);
        }

        /// <summary>
        /// Admin copy page; when the admin creates a page from the garden and creates an admin page
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public string CopyAsAdminPage (string pageId)
        {
            /*
            1. get the page with the page. if the page fails then throw error
            2. for the page assemble a PAM news-page object. 
            3. for each module if the module is alerts, company, RSS create a new module and add.
            4. for the others, do add modules to page transactions.
            */
            var thisPage = GetUserNewsPage(pageId);

            return CreateAdminPage(thisPage);
        }

        /// <summary>
        /// The create custom page.
        /// </summary>
        /// <param name="newsPage">The news page.</param>
        /// <returns>
        /// The create custom page.
        /// </returns>
        public string CreateCustomPage(NewsPage newsPage)
        {
            //Updated the request with the page meta data so that it is there on the clopied pages
            var request = new CreatePageRequest
                              {
                                  Page = new Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsPage
                                             {
                                                 ShareProperties = new ShareProperties
                                                                       {
                                                                           AccessControlScope = AccessControlScope.Personal,
                                                                           SharePromotion = ShareScope.Personal,
                                                                           AssignedScope = ShareScope.Personal,
                                                                           ListingScope = ShareScope.Personal,
                                                                       },
                                                 PageQualifier = AccessQualifier.User,
                                                 PageProperties = new NewsPageProperties
                                                                      {
                                                                          Position = newsPage.Position,
                                                                          Description = newsPage.Description,
                                                                          Title = newsPage.Title,
                                                                          PageMetaData = (newsPage.MetaData != null && newsPage.CategoryInfo != null) ? ((newsPage.MetaData.MetaDataType == MetaDataType.Industry) ? new Metadata
                                                                          {
                                                                              IndustryCollection = new MetadataFieldCollection { new MetadataField { Text = newsPage.MetaData.MetaDataCode } },
                                                                              CategoryCollection = new MetadataFieldCollection { new MetadataField { Text = newsPage.CategoryInfo.CategoryCode } }
                                                                          } : new Metadata
                                                                          {
                                                                              RegionCollection = new MetadataFieldCollection { new MetadataField { Text = newsPage.MetaData.MetaDataCode } },
                                                                              CategoryCollection = new MetadataFieldCollection { new MetadataField { Text = newsPage.CategoryInfo.CategoryCode } }
                                                                          } ): null,
                                                                      },
                                             }
                              };
            
            
            var modulesListToAdd = new List<KeyValuePair<int, int>>();
            if (newsPage.ModuleCollection != null)
            {
                request.Page.ModuleCollection = new ModuleCollection();
                var pos = 0;
                foreach (var module in newsPage.ModuleCollection)
                {
                    var newsPageModule = AssembleNewsPageModule(module, ++pos);
                    if (newsPageModule != null)
                    {
                        request.Page.ModuleCollection.Add(AssembleNewsPageModule(module, module.Position));
                    }
                    else
                    {
                        modulesListToAdd.Add(new KeyValuePair<int, int>(module.Id, module.Position));
                    }
                }
            }

            var createPageResponse = Invoke<CreatePageResponse>(request).ObjectResponse;

            if (createPageResponse.Id > 0)
            {
                if (modulesListToAdd.Count > 0)
                {
                    AddModuleIdsToPage(createPageResponse.Id.ToString(), modulesListToAdd);
                }

                if (Settings.Default.IncludeCacheKeyGeneration &&
                    Settings.Default.CachePageItems)
                {
                    // var rootAccessControlScope = GetRootAccessControlScope(createPageResponse.Id, createPageResponse.Id, AccessControlScope.Everyone, AccessQualifier.Factiva);
                    var generator = new PageCacheKeyGenerator
                    {
                        PageId = createPageResponse.Id.ToString(),
                        ParentId = createPageResponse.Id.ToString(),
                        PageAccessQualifier = AccessQualifier.User,
                        RootAccessControlScope = AccessControlScope.Personal,
                        PageAccessControlScope = AccessControlScope.Personal,
                    };

                    // Todo :: Cleaned Up dacostad
                    /// GetPage(generator.ToReference(), true);
                    return generator.ToReference();
                }

                // Todo :: Cleaned Up dacostad
                //// GetPage(createPageResponse.Id.ToString(), true);
                return createPageResponse.Id.ToString();
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.UnableToCreatePage);
        }

        public string CopyPage(string pageRef, int pagePosition = 1)
        {
            /*
             1. get the page with the page. if the page fails then throw error
             2. for the page assemble a PAM NewsPage object. 
             3. for each module if the module is alerts, company, RSS create a new module and add.
             4. for the others, do add module to page transactions
             */

            // get the page first.
            var thisPage = GetUserNewsPage(pageRef);
            thisPage.Position = pagePosition;
            return CreateCustomPage(thisPage);
        }

        public NewsPageModule ReplaceModuleOnPage(string pageId, string moduleToAdd, string moduleToRemove)
        {
            var page = GetPage(pageId, false, false) as Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsPage;

            if (page == null)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.PageIsNull);
            }

            var module = FindModule(page.ModuleCollection, moduleToRemove);

            if (module == null)
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ModuleDoesNotExistOnPage);
            }

            if (typeof(CompanyOverviewNewspageModule) == module.GetType() ||
                typeof(SyndicationNewspageModule) == module.GetType() ||
                typeof(AlertsNewspageModule) == module.GetType())
            {

                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidReplaceRequestRequest);
            }

            var newModule = GetModuleById(moduleToAdd);
            
            var allModuleIds = page.ModuleCollection.Cast<ModuleEx>().Select(m => m.Id).ToList();
            var allModuleIdsWithNewModuleId = new List<int>();

            var index = 0;
            foreach(var id in allModuleIds)
            {
                if (id == module.Id)
                {
                    allModuleIdsWithNewModuleId.Add(newModule.Id);
                    newModule.Position = index;
                }
                else
                    allModuleIdsWithNewModuleId.Add(id);
                index++;
            }

            page.ModuleCollection.Remove(module);
            page.ModuleCollection.Add(newModule);

            var factivaModuleIds = page.ModuleCollection.Cast<ModuleEx>().Where(pageModule => pageModule.ModuleQualifier == AccessQualifier.Factiva).Select(m => m.Id).ToList();

            page.ModuleIDCollection.Clear();
            page.ModuleIDCollection.AddRange(factivaModuleIds);
            page.ModuleCollection.RemoveAll(m => factivaModuleIds.Contains(m.Id));

            // update the modules on the page
            Invoke<UpdatePageResponse>(new UpdatePageRequest { Page = page });

            // update the module positions
            UpdateModulePositionsOnPage(page.Id.ToString(), new List<List<int>> { allModuleIdsWithNewModuleId });

            // return the module definition
            return NewsPageModuleFactory.GetModuleByFactivaType((ModuleEx)newModule, ControlData, Preferences);
        }


        /// <summary>
        /// The add modules to page.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="moduleCollection">
        /// The module collection.
        /// </param>
        public void AddModulesToPage(int pageId, List<NewsPageModule> moduleCollection)
        {
            var request = new AddModulesToPageRequest
                              {
                                  PageID = pageId
                              };

            const int Position = 0;
            if (moduleCollection != null)
            {
                request.ModuleCollection = new ModuleCollection();
                request.ModulePositionCollection = new ModulePositionCollection();
                foreach (var module in moduleCollection.Where(module => module.Id == 0))
                {
                    request.ModuleCollection.Add(AssembleNewsPageModule(module, Position));
                }
            }   

            Invoke<AddModulesToPageResponse>(request);
        }

        /// <summary>
        /// The get modules by module type.
        /// </summary>
        /// <param name="moduleType">The module type.</param>
        /// <param name="metaDataType">The meta data type.</param>
        /// <returns></returns>
        public List<ModuleIdByMetadata> GetModulesByModuleType(ModuleType moduleType, MetaDataType metaDataType)
        {
            var moduleIdByMetadataList = new List<ModuleIdByMetadata>();
            var request = new GetModulesListRequest
                              {
                                  InterfaceLanguage = MapInterfaceLanguage(InterfaceLanguage),
                                  ModuleQualifierCollection = new ModuleQualifierCollection
                                                                  {
                                                                      AccessQualifier.Factiva
                                                                  },
                                  ReturnType = ReturnType.Full,
                                  TypeCollection = new ModuleTypeCollection
                                                       {
                                                           AssembleModuleTypeModel(moduleType)
                                                       },
                                  SortBy = SortBy.Name,
                                  SortOrder = SortOrder.Ascending,
                                  SearchFilter = AssembleModuleMetaDataType(metaDataType)
                              };

            var getModuleListResponse = Process<GetModulesListResponse>(request);

            if (getModuleListResponse != null && getModuleListResponse.ModuleListInfoCollection != null
                && getModuleListResponse.ModuleListInfoCollection.Count > 0)
            {
                moduleIdByMetadataList = AssembleModulesListModel(getModuleListResponse.ModuleListInfoCollection);
            }

            return moduleIdByMetadataList;
        }

        /// <summary>
        /// The get modules by module type.
        /// </summary>
        /// <param name="moduleType">The module type.</param>
        /// <returns></returns>
        public List<ModuleIdByMetadata> GetModulesByModuleType(ModuleType moduleType)
        {
            var moduleIdByMetadataList = new List<ModuleIdByMetadata>();
            var request = new GetModulesListRequest
                              {
                                  InterfaceLanguage = MapInterfaceLanguage(InterfaceLanguage),
                                  ModuleQualifierCollection = new ModuleQualifierCollection
                                                                  {
                                                                      AccessQualifier.Factiva
                                                                  },
                                  ReturnType = ReturnType.Summary,
                                  TypeCollection = new ModuleTypeCollection
                                                       {
                                                           AssembleModuleTypeModel(moduleType)
                                                       },
                                  SortBy = SortBy.Name,
                                  SortOrder = SortOrder.Ascending,
                              };

            var getModuleListResponse = Process<GetModulesListResponse>(request);
            if (getModuleListResponse != null && getModuleListResponse.ModuleListInfoCollection != null
                && getModuleListResponse.ModuleListInfoCollection.Count > 0)
            {
                moduleIdByMetadataList = AssembleModulesListModel(getModuleListResponse.ModuleListInfoCollection);
            }

            return moduleIdByMetadataList;
        }
        #endregion

        #region PAM Access methods
        /// <summary>
        /// The set page share properties.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="shareProperties">The share properties.</param>
        private void SetPageShareProperties(string pageId, ShareProperties shareProperties)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var request = new SetPageSharePropertiesRequest
                              {
                                  PageID = pId,
                                  ShareProperties = shareProperties
                              };
            Process<SetPageSharePropertiesResponse>(request);
        }

        /// <summary>
        /// The update user page title description.
        /// </summary>
        /// <param name="aPage">
        /// The a page.
        /// </param>
        private void UpdateUserPageTitleDescription(NewsPage aPage)
        {
            var dissectedPageRef = ExtractPageId(aPage.ID);
            var pId = dissectedPageRef.ExtractedId;
            var request = new UpdatePageTitleRequest
                              {
                                  Id = pId,
                                  Title = aPage.Title,
                                  Description = aPage.Description,
                                  InterfaceLanguage = MapInterfaceLanguage(InterfaceLanguage)
                              };

            Process<UpdatePageTitleResponse>(request);
        }

        /// <summary>
        /// The delete pam page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        private void DeletePamPage(string pageId)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var request = new DeletePageRequest
                              {
                                  Id = pId
                              };
            Process<DeletePageResponse>(request);
        }

        /// <summary>
        /// The enable disable page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        private EnableDisable EnableDisablePage(string pageId, EnableDisable action = EnableDisable.Enable)
        {
            var dissectedPageRef = ExtractPageId(pageId);
            var pId = dissectedPageRef.ExtractedId;
            var request = new EnableDisableAssignedPageRequest
                              {
                                  Id = pId,
                                  Action = action
                              };
            var response = Process<EnableDisableAssignedPageResponse>(request);
            return response.Result;
        }

        /// <summary>
        /// The get user page list.
        /// </summary>
        /// <param name="pageTypes">The page types.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="forceCacheRefresh">if set to <c>true</c> [force cache refresh].</param>
        /// <returns>
        /// A PageListInfo Collection.
        /// </returns>
        private IEnumerable GetUserPageList(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy, bool forceCacheRefresh = false)
        {
            var request = new GetPagesListRequest { PageQualifierCollection = new PageQualifierCollection() };
            request.TypeCollection.AddRange(pageTypes);
            request.SortBy = sortBy;
            request.SortOrder = sortOrder;

            if (Settings.Default.IncludeCacheKeyGeneration &&
                    Settings.Default.CachePageListItems)
            {
                var generator = new PageListCacheKeyGenerator
                                    {
                                        CacheForceCacheRefresh = forceCacheRefresh
                                    };

                return Invoke<GetPagesListResponse>(request, generator.GetCacheControlData(ControlData)).ObjectResponse.PageListInfoCollection;
            }

            return Invoke<GetPagesListResponse>(request).ObjectResponse.PageListInfoCollection;
        }


        /// <summary>
        /// Gets the page list info collection.
        /// </summary>
        /// <param name="pageTypes">The page types.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="pageDefaultedBy">The page defaulted by.</param>
        /// <param name="pagePosition">The page position.</param>
        /// <returns>
        /// A collection of Page Information
        /// </returns>
        private GetPagesListWithPageResponse GetUserPageListWithDefaultPage(IEnumerable<PageType> pageTypes, SortOrder sortOrder, SortBy sortBy, int pageId, PageDefaultBy pageDefaultedBy = PageDefaultBy.Position, int pagePosition = 1)
        {
            // return MockGetPageListWithPageResponse();
            var request = new GetPagesListWithPageRequest();
            request.TypeCollection.AddRange(pageTypes);
            request.SortBy = sortBy;
            request.SortOrder = sortOrder;
            if (pageDefaultedBy == PageDefaultBy.Position)
            {
                request.ReturnPageAtPosition = pagePosition;
            }

            if (pageDefaultedBy == PageDefaultBy.PageId)
            {
                request.ReturnPageWithID = pageId;
            }

            return Process<GetPagesListWithPageResponse>(request);
        }

        /// <summary>
        /// The get industry sub-scribable news pages.
        /// </summary>
        /// <returns></returns>
        protected internal PageListInfoExCollection GetIndustrySubScribableNewsPages()
        {
            return GetSubscribablePages(PageType.NewsPage, SortOrder.Ascending, SortBy.Name, new List<AccessQualifier> { AccessQualifier.Factiva }, new MetadataFilter { SearchFilterTypeCollection = new SearchFilterTypeCollection { MetadataFilterType.AllIndustry } });
        }

        /// <summary>
        /// The get geographic sub-scribable  news pages.
        /// </summary>
        /// <returns></returns>
        protected internal PageListInfoExCollection GetGeographicSubScribableNewsPages()
        {
            return GetSubscribablePages(PageType.NewsPage, SortOrder.Ascending, SortBy.Name, new List<AccessQualifier> { AccessQualifier.Factiva }, new MetadataFilter { SearchFilterTypeCollection = new SearchFilterTypeCollection { MetadataFilterType.AllRegion } });
        }

        /// <summary>
        /// The get topic subscribable news pages.
        /// </summary>
        /// <returns>
        /// </returns>
        protected internal PageListInfoExCollection GetTopicSubScribableNewsPages()
        {
            return GetSubscribablePages(PageType.NewsPage, SortOrder.Ascending, SortBy.Name, new List<AccessQualifier> { AccessQualifier.Factiva }, new MetadataFilter { SearchFilterTypeCollection = new SearchFilterTypeCollection { MetadataFilterType.AllTopic } });
        }
        
        protected internal List<NewsPage> ReorderNewspages(List<NewsPage> inputPageList)
        {
            var resultOrderedPageList = new List<NewsPage>();

            try
            {
                var firstPositionPageInfo = from pagelist in inputPageList where pagelist.Position == 1 select pagelist;
                if (firstPositionPageInfo.ToList().Count > 0)
                {
                    resultOrderedPageList.AddRange(firstPositionPageInfo);
                }
                else
                {
                    firstPositionPageInfo = (from pagelist in inputPageList where pagelist.Position != 0 select pagelist).OrderBy(item => item.Position);
                    foreach (NewsPage pages in firstPositionPageInfo)
                    {
                        resultOrderedPageList.Add(pages);
                        inputPageList.Remove(pages);
                        break;
                    }
                }



                var adminPositionPageInfo = (from pagelist in inputPageList where pagelist.Position == 0 select pagelist).OrderByDescending(exp => exp.LastModifiedDate);
                resultOrderedPageList.AddRange(adminPositionPageInfo);

                var restPositionPageInfo = (from pagelist in inputPageList where pagelist.Position != 0 && pagelist.Position != 1 select pagelist).OrderBy(exp => exp.Position);
                resultOrderedPageList.AddRange(restPositionPageInfo);
            }
            catch (Exception)
            {
                return inputPageList;
            }

            return resultOrderedPageList;
        }

        /// <summary>
        /// The get admin shared subscribable news pages.
        /// </summary>
        /// <returns>
        /// </returns>
        private IEnumerable<PageListInfoEx> GetAdminSharedSubScribableNewsPages()
        {
            return GetSubscribablePages(PageType.NewsPage, SortOrder.Descending, SortBy.LastModifiedDate, new List<AccessQualifier> {AccessQualifier.Account}, null);
        }

        private IEnumerable<PageListInfoEx> GetUserSharedSubScribableNewsPages()
        {
            return GetSubscribablePages(PageType.NewsPage, SortOrder.Descending, SortBy.LastModifiedDate, new List<AccessQualifier> {AccessQualifier.User}, null);
        }

        private PageListInfoExCollection GetSubscribablePages(PageType pageType, SortOrder sortOrder, SortBy sortBy, IEnumerable<AccessQualifier> accessQualifiers, MetadataFilter metadataFilter)
        {
            var pageQualifiers = new PageQualifierCollection();
            pageQualifiers.AddRange(accessQualifiers);
            var request = new GetSubscribablePagesRequest
                              {
                                  InterfaceLanguage = MapInterfaceLanguage(InterfaceLanguage),
                                  PageQualifierCollection = pageQualifiers,
                                  SearchFilter = metadataFilter,
                                  PageType = pageType, ShareScopeCollection = new ShareScopeCollection()
                              };
            return Process<GetSubscribablePagesResponse>(request).PageListInfoExCollection;
        }
        
        /// <summary>
        /// The update PAM page positions.
        /// </summary>
        /// <param name="pagePositions">
        /// The page positions.
        /// </param>
        private void UpdatePamPagePositions(IEnumerable<PagePosition> pagePositions)
        {
            var request = new UpdatePagePositionsRequest
                              {
                                  PagePositions = new PagePositions { PagePositionCollection = AssemblePagePositionsRequest(pagePositions) }
                              };
            Process<UpdatePagePositionsResponse>(request);
        }


        /// <summary>
        /// The make page modules public.
        /// </summary>
        /// <param name="moduleCollection">
        /// The module collection.
        /// </param>
        private void MakePageModulesPublic(ICollection moduleCollection)
        {
            if (moduleCollection != null && moduleCollection.Count > 0)
            {
                foreach (var module in moduleCollection)
                {
                    if (module.GetType() == typeof(AlertsNewspageModule) || 
                        (module.GetType() == typeof(SyndicationNewspageModule) || 
                        module.GetType() == typeof(CompanyOverviewNewspageModule)))
                    {
                        SetModuleShareProperties(
                            ((ModuleEx)module).Id, 
                            new ShareProperties
                                {
                                    AccessControlScope = AccessControlScope.Account,
                                    ListingScope = ShareScope.Personal,
                                    AssignedScope = ShareScope.Personal,
                                    SharePromotion = ShareScope.Personal
                                });
                    }
                }
            }
        }

        /// <summary>
        /// The make page modules private.
        /// </summary>
        /// <param name="moduleCollection">
        /// The module collection.
        /// </param>
        private void MakePageModulesPrivate(ICollection moduleCollection)
        {
            if (moduleCollection != null && moduleCollection.Count > 0)
            {
                foreach (var module in moduleCollection)
                {
                    if (module.GetType() == typeof(AlertsNewspageModule) || 
                        (module.GetType() == typeof(SyndicationNewspageModule) || 
                        module.GetType() == typeof(CompanyOverviewNewspageModule)))
                    {
                        SetModuleShareProperties(
                            ((ModuleEx)module).Id, 
                            new ShareProperties
                                {
                                    AccessControlScope = AccessControlScope.Personal,
                                    ListingScope = ShareScope.Personal,
                                    AssignedScope = ShareScope.Personal,
                                    SharePromotion = ShareScope.Personal
                                });
                    }
                }
            }
        }

        /// <summary>
        /// The make personal alerts public.
        /// </summary>
        /// <param name="alertIds">
        /// The alert ids.
        /// </param>
        public void MakePersonalAlertsPublic(List<int> alertIds)
        {
            if (alertIds.Count == 0)
                return;

            var request = new SetFolderSharePropertiesRequest
                              {
                                  folderShareDetails = alertIds.Select(alertId => new FolderShareDetails
                                                                                      {
                                                                                          folderId = alertId, SharingData = new Factiva.Gateway.Messages.Assets.V1_0.ShareProperties
                                                                                                                                {
                                                                                                                                    accessControlScope = ShareAccessScope.Everyone, allowCopy = false, assignedScope = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal, externalAccess = ShareAccess.Allow, listingScope = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal, sharePromotion = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal
                                                                                                                                }
                                                                                      }).ToArray()
                              };

            Process<SetFolderSharePropertiesResponse>(request);
        }

        /// <summary>
        /// The set module share properties.
        /// </summary>
        /// <param name="moduleId">The module id.</param>
        /// <param name="shareProperties">The share properties.</param>
        private void SetModuleShareProperties(int moduleId, ShareProperties shareProperties)
        {
            try
            {
                var request = new SetModuleSharePropertiesRequest
                                  {
                                      ModuleID = moduleId,
                                      ShareProperties = shareProperties
                                  };
                Process<SetModuleSharePropertiesResponse>(request);
            }
            catch
            {
            }
        }

        #endregion

        /// <summary>
        /// Assembles the type of the module meta data.
        /// </summary>
        /// <param name="metaDataType">Type of the meta data.</param>
        /// <returns></returns>
        private static Filter AssembleModuleMetaDataType(MetaDataType metaDataType)
        {
            switch (metaDataType)
            {
                case MetaDataType.Geographic:
                    {
                        return new MetadataFilter
                                   {
                                       SearchFilterTypeCollection = new SearchFilterTypeCollection
                                                                        {
                                                                            MetadataFilterType.AllRegion
                                                                        }
                                   };
                    }

                case MetaDataType.Industry:
                    {
                        return new MetadataFilter 
                                   { 
                                        SearchFilterTypeCollection = new SearchFilterTypeCollection
                                                                         {
                                                                            MetadataFilterType.AllIndustry
                                                                         } 
                                   };
                    }
                case MetaDataType.Topic:
                    {
                        return new MetadataFilter
                                   {
                                       SearchFilterTypeCollection = new SearchFilterTypeCollection
                                                                        {
                                                                            MetadataFilterType.AllTopic
                                                                        }
                                   };
                    }
            }

            return new MetadataFilter
                       {
                           SearchFilterTypeCollection = new SearchFilterTypeCollection
                                                            {
                                                                MetadataFilterType.AllTopic, MetadataFilterType.AllRegion, MetadataFilterType.AllIndustry
                                                            }
                       };
        }

        /// <summary>
        /// The assemble page positions request.
        /// </summary>
        /// <param name="pagePositions">The page positions.</param>
        /// <returns></returns>
        private PagePositionCollection AssemblePagePositionsRequest(IEnumerable<PagePosition> pagePositions)
        {
            var coll = new PagePositionCollection();
            foreach (var page in pagePositions)
            {
                var pid = ExtractPageId(page.PageId);
                coll.Add(new Factiva.Gateway.Messages.Assets.Pages.V1_0.PagePosition
                             {
                                 Position = page.Position,
                                 Id = Convert.ToInt32(pid.ExtractedId),
                             });
            }

            return coll;
        }

        /// <summary>
        /// The assemble modules list model.
        /// </summary>
        /// <param name="moduleListInfoCollection">
        /// The module list info collection.
        /// </param>
        /// <returns>
        /// </returns>
        private List<ModuleIdByMetadata> AssembleModulesListModel(IEnumerable moduleListInfoCollection)
        {
            var moduleIdByMetadataList = new List<ModuleIdByMetadata>();

            foreach (ModuleListInfo moduleListInfo in moduleListInfoCollection)
            {
                var moduleIdByMetadata = new ModuleIdByMetadata
                                             {
                                                 ModuleId = moduleListInfo.Id,
                                                 InterfaceLanguage = MapLanguageToString(moduleListInfo.InterfaceLanguage)
                                             };

                if (moduleListInfo.ModuleProperties != null)
                {
                    moduleIdByMetadata.MetaData = AssemblePageModuleMetadata(moduleListInfo.ModuleProperties.ModuleMetaData);
                }

                moduleIdByMetadataList.Add(moduleIdByMetadata);
            }

            return moduleIdByMetadataList;
        }

        /// <summary>
        /// The assemble page module metadata.
        /// </summary>
        /// <param name="metadata">
        /// The metadata.
        /// </param>
        /// <returns>
        /// </returns>
        private MetaData AssemblePageModuleMetadata(Metadata metadata)
        {
            var metaData = new MetaData();
            if (metadata != null)
            {
                if (metadata.IndustryCollection != null && metadata.IndustryCollection.Count > 0)
                {
                    metaData.MetaDataType = MetaDataType.Industry;
                    metaData.MetaDataCode = metadata.IndustryCollection[0].Text; // the code
                    metaData.MetaDataDescriptor = NewspageMetadataUtility.GetMetaDataName(metaData.MetaDataCode, InterfaceLanguage);
                }

                if (metadata.RegionCollection != null && metadata.RegionCollection.Count > 0)
                {
                    metaData.MetaDataType = MetaDataType.Geographic;
                    metaData.MetaDataCode = metadata.RegionCollection[0].Text; // the code
                    metaData.MetaDataDescriptor = NewspageMetadataUtility.GetMetaDataName(metaData.MetaDataCode, InterfaceLanguage);
                }

                if (metadata.TopicCollection != null && metadata.TopicCollection.Count > 0)
                {
                    metaData.MetaDataType = MetaDataType.Topic;
                    metaData.MetaDataCode = metadata.TopicCollection[0].Text; // the code
                    metaData.MetaDataDescriptor = NewspageMetadataUtility.GetMetaDataName(metaData.MetaDataCode, InterfaceLanguage);
                }
            }

            return metaData;
        }


        /// <summary>
        /// The assemble module type model.
        /// </summary>
        /// <param name="moduleType">
        /// The module type.
        /// </param>
        /// <returns>
        /// </returns>
        private static Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType AssembleModuleTypeModel(ModuleType moduleType)
        {
            switch (moduleType)
            {
                case ModuleType.TrendingNewsPageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.TrendingNewsPageModule;
                case ModuleType.AlertsNewspageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.AlertsNewspageModule;
                case ModuleType.CompanyOverviewNewspageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.CompanyOverviewNewspageModule;
                case ModuleType.TopNewsNewspageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.TopNewsNewspageModule;
                case ModuleType.SyndicationNewspageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.SyndicationNewspageModule;
                case ModuleType.NewsstandNewspageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.NewsstandNewspageModule;
                case ModuleType.RadarNewspageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.RadarNewspageModule;
                case ModuleType.RegionalMapNewspageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.RegionalMapNewspageModule;
                case ModuleType.SourcesNewspageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.SourcesNewspageModule;
                case ModuleType.SummaryNewspageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.SummaryNewspageModule;
                case ModuleType.CustomTopicsNewspageModule:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.ModuleType.CustomTopicsNewspageModule;
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidModuleType);
        }

        /// <summary>
        /// The map interface language code.
        /// </summary>
        /// <param name="interfaceLanguage">
        /// The interface language.
        /// </param>
        /// <returns>
        /// </returns>
        private static LanguageCode MapInterfaceLanguageCode(string interfaceLanguage)
        {
            switch (interfaceLanguage.ToLower())
            {
                case "en":
                    return LanguageCode.en;
                case "de":
                    return LanguageCode.de;
                case "fr":
                    return LanguageCode.fr;
                case "es":
                    return LanguageCode.es;
                case "it":
                    return LanguageCode.it;
                case "ru":
                    return LanguageCode.ru;
                case "ja":
                    return LanguageCode.ja;
                case "zhcn":
                    return LanguageCode.zhcn;
                case "zhtw":
                    return LanguageCode.zhtw;
            }

            return LanguageCode.en;
        }

        /// <summary>
        /// The assemble news page model.
        /// </summary>
        /// <param name="aPage">The page.</param>
        /// <param name="preferences"></param>
        /// <returns>
        /// </returns>
        private NewsPage AssembleNewsPageModel(Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsPage aPage, IPreferences preferences)
        {
            if (aPage == null)
            {
                return null;
            }

            var pageSharePropertiesResponse = aPage.ShareProperties as SharePropertiesResponse;
            var thisPage = new NewsPage
                               {
                                   Position = aPage.PageProperties.Position,
                                   ID = aPage.Id.ToString(),
                                   IsActive = pageSharePropertiesResponse != null && pageSharePropertiesResponse.ShareStatus == ShareStatus.Active,
                                   LastModifiedDate = DateTimeFormatter.ConvertToUtc(aPage.PageProperties.LastModifiedDate),
                                   OwnerNamespace = aPage.PageProperties.CreatedByNamespace,
                                   OwnerUserId = aPage.PageProperties.CreatedBy,
                                   AccessScope = MapAccessScope(pageSharePropertiesResponse),
                                   ParentID = (pageSharePropertiesResponse != null) ? pageSharePropertiesResponse.RootID.ToString() : 0.ToString(),
                                   AccessQualifier = MapAccessQualifier(aPage.PageQualifier),
                                   ModuleCollection = new List<NewsPageModule>(),
                               };

            thisPage.AccessQualifier = MapAccessQualifier(aPage.PageQualifier);
            thisPage.Title = aPage.PageProperties.Title;
            thisPage.Description = aPage.PageProperties.Description;
            //Added this so that the meta data can be carried over to the clopied pages
            thisPage.MetaData = AssemblePageModuleMetadata(aPage.PageProperties.PageMetaData);
            thisPage.CategoryInfo = AssembleNewsPageCategoryInfo(aPage.PageProperties.PageMetaData);
           
            if (String.IsNullOrEmpty(aPage.PageProperties.Title) || thisPage.AccessQualifier == UI.Models.NewsPages.AccessQualifier.Global)
            {
                GetTitleDescriptionByInterfaceLangauge(aPage.PageProperties, ref thisPage);
            }

            if (aPage.ModuleCollection != null)
            {
                foreach (var module in aPage.ModuleCollection.Cast<ModuleEx>().Where(module => module != null))
                {
                    thisPage.ModuleCollection.Add(NewsPageModuleFactory.GetModuleByFactivaType(module, ControlData, preferences));
                }
            }

            thisPage.PublishStatusScope = PublishStatusScope.Personal;
            var rootAcccesControlScope = AccessControlScope.Personal;
            if (pageSharePropertiesResponse != null)
            {
                rootAcccesControlScope = GetRootAccessControlScope(aPage.Id, pageSharePropertiesResponse.RootID, aPage.ShareProperties.AccessControlScope, aPage.PageQualifier);
           
                switch (pageSharePropertiesResponse.AccessControlScope)
                {
                    case AccessControlScope.Account:
                        thisPage.PublishStatusScope = PublishStatusScope.Account;
                        break;
                    case AccessControlScope.Everyone:
                        thisPage.PublishStatusScope = PublishStatusScope.Global;
                        break;
                }
            }

            if (thisPage.AccessQualifier == UI.Models.NewsPages.AccessQualifier.Global)
            {
            }

            // update the cache key
            if (Settings.Default.IncludeCacheKeyGeneration &&
                    Settings.Default.CachePageItems)
            {
                var generator = new PageCacheKeyGenerator
                {
                    PageId = thisPage.ID,
                    ParentId = thisPage.ParentID,
                    PageAccessQualifier = aPage.PageQualifier,
                    RootAccessControlScope = rootAcccesControlScope,
                    PageAccessControlScope = aPage.ShareProperties.AccessControlScope,
                };

                thisPage.ID = generator.ToReference();
            }

            return thisPage;
        }

        private static AccessControlScope GetRootAccessControlScope(int pageId, int parentId, AccessControlScope accessControlScope, AccessQualifier qualifier)
        {
            if (parentId != pageId && qualifier == AccessQualifier.User)
            {
                return AccessControlScope.Account;
            }

            if (qualifier == AccessQualifier.Factiva || accessControlScope == AccessControlScope.Everyone)
            {
                return AccessControlScope.Everyone;
            }

            if (qualifier == AccessQualifier.Account || accessControlScope == AccessControlScope.Account)
            {
                return AccessControlScope.Account;
            }
                
            return AccessControlScope.Personal;
        }

        /// <summary>
        /// The assemble admin news page model from page list info.
        /// </summary>
        /// <param name="aPage">The a page.</param>
        /// <returns></returns>
        private AdminNewsPage AssembleAdminNewsPageModelFromPageListInfo(PageListInfo aPage)
        {
            if (aPage == null)
            {
                return null;
            }

            var pageSharePropertiesResponse = aPage.ShareProperties as SharePropertiesResponse;
            NewsPageProperties newsPagesProperties = aPage.PageProperties as NewsPageProperties ?? new NewsPageProperties();
            var thisPage = new AdminNewsPage
                               {
                                   ID = aPage.Id.ToString(),
                                   IsActive = pageSharePropertiesResponse != null && pageSharePropertiesResponse.ShareStatus == ShareStatus.Active,
                                   Position = newsPagesProperties.Position,
                                   AccessQualifier = UI.Models.NewsPages.AccessQualifier.Account,
                                   LastModifiedDate = DateTimeFormatter.ConvertToUtc(aPage.PageProperties.LastModifiedDate),
                                   OwnerNamespace = aPage.PageProperties.CreatedByNamespace,
                                   OwnerUserId = aPage.PageProperties.CreatedBy
                               };

            if (String.IsNullOrEmpty(newsPagesProperties.Title) || thisPage.AccessQualifier == UI.Models.NewsPages.AccessQualifier.Global)
            {
                GetTitleDescriptionByInterfaceLangauge(newsPagesProperties, ref thisPage);
            }
            else
            {
                thisPage.Title = newsPagesProperties.Title;
                thisPage.Description = newsPagesProperties.Description;
            }

            if (aPage.ShareProperties != null)
            {
                switch (aPage.ShareProperties.AccessControlScope)
                {
                    /*Since the Listing Scope setting was not allowing us to un-publish admin page and remove the page from user's or admin's list, the AccountAdmin got introduced
                     and so, the page should be @ Account level to get into the page garden*/
                    case AccessControlScope.Account:
                        thisPage.PublishStatusScope = PublishStatusScope.Account;
                        thisPage.AdminPublishStatus = AdminPublishStatus.Published;
                        break;
                    default:
                        thisPage.PublishStatusScope = PublishStatusScope.Personal;
                        thisPage.AdminPublishStatus = AdminPublishStatus.UnPublished;
                        break;
                }
            }
            else
            {
                thisPage.PublishStatusScope = PublishStatusScope.Personal;
                thisPage.AdminPublishStatus = AdminPublishStatus.UnPublished;
            }

            return thisPage;
        }

        /// <summary>
        /// The assemble news page model from page list info.
        /// </summary>
        /// <param name="pageListInfo">The page list info.</param>
        /// <returns></returns>
        private NewsPage AssembleNewsPageModelFromPageListInfo(PageListInfo pageListInfo)
        {
            var aPage = pageListInfo as PageListInfoEx;
            if (aPage == null)
            {
                return null;
            }

            var pageSharePropertiesResponse = aPage.ShareProperties as SharePropertiesResponse;
            var newsPagesProperties = aPage.PageProperties as NewsPageProperties ?? new NewsPageProperties();
            var thisPage = new NewsPage
                               {
                                   ID = aPage.Id.ToString(),
                                   IsActive = pageSharePropertiesResponse != null && pageSharePropertiesResponse.ShareStatus == ShareStatus.Active,
                                   Position = newsPagesProperties.Position,
                                   LastModifiedDate = DateTimeFormatter.ConvertToUtc(aPage.PageProperties.LastModifiedDate),
                                   OwnerNamespace = aPage.PageProperties.CreatedByNamespace,
                                   OwnerUserId = aPage.PageProperties.CreatedBy,
                                   AccessScope = MapAccessScope(pageSharePropertiesResponse),
                                   ParentID = (pageSharePropertiesResponse != null) ? pageSharePropertiesResponse.RootID.ToString() : 0.ToString(),
                                   AccessQualifier = MapAccessQualifier(aPage.PageQualifier),
                                   ModuleCollection = new List<NewsPageModule>(),
                                   CategoryInfo = AssembleNewsPageCategoryInfo(newsPagesProperties.PageMetaData)
                               };

            // update the cache key

            if (String.IsNullOrEmpty(newsPagesProperties.Title) || thisPage.AccessQualifier == UI.Models.NewsPages.AccessQualifier.Global)
            {
                GetTitleDescriptionByInterfaceLangauge(newsPagesProperties, ref thisPage);
            }
            else
            {
                thisPage.Title = newsPagesProperties.Title;
                thisPage.Description = newsPagesProperties.Description;
            }

            thisPage.PublishStatusScope = PublishStatusScope.Personal;
            var rootAccessControlScope = AccessControlScope.Personal;

            if (pageSharePropertiesResponse != null)
            {
                rootAccessControlScope = GetRootAccessControlScope(aPage.Id, pageSharePropertiesResponse.RootID, aPage.ShareProperties.AccessControlScope, aPage.PageQualifier);

                switch (pageSharePropertiesResponse.AccessControlScope)
                {
                    case AccessControlScope.Account:
                        thisPage.PublishStatusScope = PublishStatusScope.Account;
                        break;
                    case AccessControlScope.Everyone:
                        thisPage.PublishStatusScope = PublishStatusScope.Global;
                        break;
                }
            }

            // update the cache key
            if (Settings.Default.IncludeCacheKeyGeneration &&
                    Settings.Default.CachePageItems)
            {
                var generator = new PageCacheKeyGenerator
                {
                    PageId = thisPage.ID,
                    ParentId = thisPage.ParentID,
                    PageAccessQualifier = aPage.PageQualifier,
                    RootAccessControlScope = rootAccessControlScope,
                    PageAccessControlScope = aPage.ShareProperties.AccessControlScope,
                };
                
                thisPage.ID = generator.ToReference();
                generator.PageId = thisPage.ParentID;
                thisPage.ParentID = generator.ToReference();
            }

            return thisPage;
        }

        /// <summary>
        /// This is not for production. PAM is currently returning emptyu.. Question to PAM - if page is not active.. what will me tadata be?
        /// </summary>
        /// <param name="pagePosition">The page Position.</param>
        /// <returns>A NewsPageProperties object.</returns>
        private static NewsPageProperties AssembleNewsPagesProperties(int pagePosition = 1)
        {
            return new NewsPageProperties
                       {
                           Title = ResourceTextManager.Instance.GetString("pageUnavailable"),
                           Description = ResourceTextManager.Instance.GetString("descUnavailable"),
                           LastModifiedDate = DateTime.Now,
                           Position = pagePosition,
                           PageMetaData = null
                       };
        }

        /// <summary>
        /// The assemble news page model from page list info.
        /// </summary>
        /// <param name="aPage">The a page.</param>
        /// <returns></returns>
        private NewsPage AssembleNewsPageModelFromPageListInfo(PageListInfoEx aPage)
        {
            if (aPage == null)
            {
                return null;
            }

            var pageSharePropertiesResponse = aPage.ShareProperties as SharePropertiesResponse;
            var newsPagesProperties = aPage.PageProperties as NewsPageProperties ?? AssembleNewsPagesProperties();
            var thisPage = new NewsPage
                               {
                                   ID = aPage.Id.ToString(),
                                   IsActive = pageSharePropertiesResponse != null && pageSharePropertiesResponse.ShareStatus == ShareStatus.Active,
                                   Position = newsPagesProperties.Position,
                                   LastModifiedDate = DateTimeFormatter.ConvertToUtc(aPage.PageProperties.LastModifiedDate),
                                   OwnerNamespace = aPage.PageProperties.CreatedByNamespace,
                                   OwnerUserId = aPage.PageProperties.CreatedBy,
                                   AccessScope = MapAccessScope(pageSharePropertiesResponse),
                                   ParentID = (pageSharePropertiesResponse != null) ? pageSharePropertiesResponse.RootID.ToString() : 0.ToString(),
                                   AccessQualifier = MapAccessQualifier(aPage.PageQualifier),
                                   CategoryInfo = AssembleNewsPageCategoryInfo(newsPagesProperties.PageMetaData),
                                   ModuleCollection = new List<NewsPageModule>(),

                               };
            if (String.IsNullOrEmpty(newsPagesProperties.Title) || thisPage.AccessQualifier == UI.Models.NewsPages.AccessQualifier.Global)
            {
                GetTitleDescriptionByInterfaceLangauge(newsPagesProperties, ref thisPage);
            }
            else
            {
                thisPage.Title = newsPagesProperties.Title;
                thisPage.Description = newsPagesProperties.Description;
            }

            thisPage.PublishStatusScope = PublishStatusScope.Personal;
            var rootAccessControlScope = AccessControlScope.Personal;
            if (pageSharePropertiesResponse != null)
            {
                rootAccessControlScope = GetRootAccessControlScope(aPage.Id, pageSharePropertiesResponse.RootID, aPage.ShareProperties.AccessControlScope, aPage.PageQualifier);

                switch (pageSharePropertiesResponse.AccessControlScope)
                {
                    case AccessControlScope.Account:
                        thisPage.PublishStatusScope = PublishStatusScope.Account;
                        break;
                    case AccessControlScope.Everyone:
                        thisPage.PublishStatusScope = PublishStatusScope.Global;
                        break;
                }
            }

            // update the cache key
            if (Settings.Default.IncludeCacheKeyGeneration &&
                    Settings.Default.CachePageItems)
            {
                var generator = new PageCacheKeyGenerator
                {
                    PageId = thisPage.ID,
                    ParentId = thisPage.ParentID,
                    PageAccessQualifier = aPage.PageQualifier,
                    RootAccessControlScope = rootAccessControlScope,
                    PageAccessControlScope = aPage.ShareProperties.AccessControlScope,
                };

                thisPage.ID = generator.ToReference();
                generator.PageId = thisPage.ParentID;
                thisPage.ParentID = generator.ToReference();
            }

            return thisPage;
        }

        /// <summary>
        /// The assemble news page category info.
        /// </summary>
        /// <param name="pageMetaData">The page meta data.</param>
        /// <returns></returns>
        private CategoryInfo AssembleNewsPageCategoryInfo(Metadata pageMetaData)
        {
            if (pageMetaData != null && pageMetaData.CategoryCollection != null && pageMetaData.CategoryCollection.Count > 0)
            {
                var code = pageMetaData.CategoryCollection[0].Text;
                return new CategoryInfo
                           {
                               CategoryCode = code,
                               CategoryDescriptor = NewspageMetadataUtility.GetPageCategoryName(code, InterfaceLanguage)
                           };
            }

            return null;
        }

        /// <summary>
        /// The get title description by interface language.
        /// </summary>
        /// <param name="pageProperties">The page properties.</param>
        /// <param name="newspage">The news page.</param>
        private void GetTitleDescriptionByInterfaceLangauge(NewsPageProperties pageProperties, ref NewsPage newspage)
        {
            if (pageProperties == null || pageProperties.TitleDescriptionsCollection == null || pageProperties.TitleDescriptionsCollection.Count <= 0)
            {
                return;
            }

            foreach (var item in pageProperties.TitleDescriptionsCollection.Where(anItem => anItem.Language == MapInterfaceLanguage(InterfaceLanguage)))
            {
                newspage.Title = item.Title;
                newspage.Description = item.Description;
                return;
            }
        }

        /// <summary>
        /// The get title description by interface langauge.
        /// </summary>
        /// <param name="pageProperties">The page properties.</param>
        /// <param name="newspage">The newspage .</param>
        private void GetTitleDescriptionByInterfaceLangauge(NewsPageProperties pageProperties, ref AdminNewsPage newspage)
        {
            if (pageProperties == null || pageProperties.TitleDescriptionsCollection == null || pageProperties.TitleDescriptionsCollection.Count <= 0)
            {
                return;
            }

            foreach (var anItem in pageProperties.TitleDescriptionsCollection.Where(anItem => anItem.Language == MapInterfaceLanguage(InterfaceLanguage)))
            {
                newspage.Title = anItem.Title;
                newspage.Description = anItem.Description;
                return;
            }
        }

        /// <summary>
        /// Weeds out non owner and non user pages from user page list.
        /// </summary>
        /// <param name="pageListColl">The page list coll.</param>
        /// <returns></returns>
        private static PageListInfoCollection WeedOutNonOwnerAndNonUserPagesFromUserPageList(IEnumerable pageListColl)
        {
            var pageListwithPersonalSubscribedAndAssignedPages = new PageListInfoCollection();
            foreach (PageListInfoEx aPage in pageListColl)
            {
                if (((SharePropertiesResponse)aPage.ShareProperties).IsOwner)
                {
                    //// here are the pages we will be adding and others are weeded out
                    /*
                     *1. Assigned pages of the Qualifier Account
                     *2. Pages owned by user and the Qualifier is User
                     *3. Account Pages of the Qualifier Factiva OR Account
                     */

                    if (aPage.PageQualifier == AccessQualifier.Account && ((SharePropertiesResponse)aPage.ShareProperties).ShareType == ShareType.Assigned)
                    {
                        pageListwithPersonalSubscribedAndAssignedPages.Add(aPage);
                    }

                    if (aPage.PageQualifier == AccessQualifier.User)
                    {
                        pageListwithPersonalSubscribedAndAssignedPages.Add(aPage);
                    }
                    else
                    {
                        if ((aPage.PageQualifier == AccessQualifier.Factiva || aPage.PageQualifier == AccessQualifier.Account) && ((SharePropertiesResponse)aPage.ShareProperties).ShareType == ShareType.Subscribed)
                        {
                            pageListwithPersonalSubscribedAndAssignedPages.Add(aPage);
                        }
                    }
                }
                else
                {
                    pageListwithPersonalSubscribedAndAssignedPages.Add(aPage);
                }
            }

            return pageListwithPersonalSubscribedAndAssignedPages;
        }

        private List<NewsPage> AssembleNewsPagesListModel(IEnumerable<PageListInfo> pageListInfoCollection)
        {
            return pageListInfoCollection.Select(AssembleNewsPageModelFromPageListInfo).ToList();
        }

        /// <summary>
        /// The map access qualifier.
        /// </summary>
        /// <param name="accessQualifier">
        /// The access qualifier.
        /// </param>
        /// <returns>
        /// </returns>
        public static UI.Models.NewsPages.AccessQualifier MapAccessQualifier(AccessQualifier accessQualifier)
        {
            switch (accessQualifier)
            {
                case AccessQualifier.User:
                    return UI.Models.NewsPages.AccessQualifier.User;
                case AccessQualifier.Account:
                    return UI.Models.NewsPages.AccessQualifier.Account;
                case AccessQualifier.Factiva:
                    return UI.Models.NewsPages.AccessQualifier.Global;
            }

            return UI.Models.NewsPages.AccessQualifier.User;
        }

        /// <summary>
        /// The map access scope.
        /// </summary>
        /// <param name="sharePropertiesResponse">
        /// The share properties response.
        /// </param>
        /// <returns>
        /// </returns>
        private static AccessScope MapAccessScope(SharePropertiesResponse sharePropertiesResponse)
        {
            switch (sharePropertiesResponse.ShareType)
            {
                case ShareType.Personal:
                    return AccessScope.OwnedByUser;
                case ShareType.Assigned:
                    return AccessScope.AssignedToUser;
                case ShareType.Subscribed:
                    return AccessScope.SubscribedByUser;
            }

            return AccessScope.OwnedByUser;
        }

        /// <summary>
        /// Assemble PAM Module from a NewsPage Model module.
        /// </summary>
        /// <param name="newspageModule">
        /// </param>
        /// <param name="position">
        /// </param>
        /// <returns>
        /// </returns>
        private static ModuleEx AssembleNewsPageModule(NewsPageModule newspageModule, int position)
        {
            switch (newspageModule.GetType().ToString())
            {
                case "DowJones.Web.Mvc.UI.Models.NewsPages.Modules.CompanyOverviewNewspageModule":
                    var companyOverviewNewspageModule = new CompanyOverviewNewspageModule
                                                            {
                                                                Position = position
                                                            };

                    companyOverviewNewspageModule.FCodeCollection.AddRange(((UI.Models.NewsPages.Modules.CompanyOverviewNewspageModule)newspageModule).Fcode.ToArray());
                    companyOverviewNewspageModule.Title = newspageModule.Title;
                    companyOverviewNewspageModule.Description = newspageModule.Description;
                    return companyOverviewNewspageModule;
                case "DowJones.Web.Mvc.UI.Models.NewsPages.Modules.AlertsNewspageModule":
                    var alertsNewspageModule = new AlertsNewspageModule
                                                   {
                                                       Title = newspageModule.Title, 
                                                       Description = newspageModule.Description, 
                                                       Position = position
                                                   };

                    foreach (string alertid in ((UI.Models.NewsPages.Modules.AlertsNewspageModule)newspageModule).AlertIDCollection)
                    {
                        alertsNewspageModule.AlertCollection.Add(new Alert
                                                                     {
                                                                         AlertID = alertid
                                                                     });
                        alertsNewspageModule.HeadlineCount = 5;
                    }

                    return alertsNewspageModule;
                case "DowJones.Web.Mvc.UI.Models.NewsPages.Modules.SyndicationNewspageModule":
                    var syndicationNewspageModule = new SyndicationNewspageModule
                                                        {
                                                            Title = newspageModule.Title,
                                                            Description = newspageModule.Description,
                                                            Position = position
                                                        };

                    foreach (var feedId in ((UI.Models.NewsPages.Modules.SyndicationNewspageModule)newspageModule).SyndicationFeedIdCollection)
                    {
                        syndicationNewspageModule.SyndicationFeedIDCollection.Add(feedId);
                        syndicationNewspageModule.HeadlineCount = 5;
                    }

                    return syndicationNewspageModule;
            }

            return null;
        }

        #region ListRelated

        /// <summary>
        /// Gets news-pages modules name and descriptions. The interface language is set when creating an instance on
        /// the Page list manager
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <returns>
        /// List of MetaData items with codes, names and descriptions; if nothing is passed, then all them of the language.
        /// </returns>
        public List<MetaData> GetNewspageModulesNameAndDescriptions(IEnumerable<string> codes = null)
        {
            return NewspageMetadataUtility.GetModulesMetaData(codes, InterfaceLanguage);
        }


        /// <summary>
        /// The map custom code to metada type.
        /// </summary>
        /// <param name="customCode">
        /// The custom code.
        /// </param>
        /// <returns>
        /// </returns>
        private static MetaDataType MapCustomCodeToMetadaType(string customCode)
        {
            switch (customCode.ToLower())
            {
                case "top":
                    return MetaDataType.Topic;
                case "reg":
                    return MetaDataType.Geographic;
                case "ind":
                    return MetaDataType.Industry;
                case "mod":
                    return MetaDataType.Custom;
            }

            return MetaDataType.Industry;
        }

        private static string MapMetadataType(MetaDataType metaDataType)
        {
            switch (metaDataType)
            {
                case MetaDataType.Topic:
                    return "Top";
                case MetaDataType.Geographic:
                    return "Reg";
                case MetaDataType.Industry:
                    return "Ind";
            }

            return string.Empty;
        }

        #endregion
    }
}
