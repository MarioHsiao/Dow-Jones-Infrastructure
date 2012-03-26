// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsPagesListServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "newsPagesListServiceResult", Namespace = "")]
    public class NewsPagesListServiceResult : AbstractServiceResult, IPopulate<NewsPagesListRequest>
    {
        [DataMember(Name = "package")]
        public NewsPagesListPackage Package { get; set; }

        #region Implementation of IPopulate

        public void Populate(ControlData controlData, NewsPagesListRequest request, IPreferences preferences)
        {
            Process(controlData, request, preferences);
        }

        #endregion

        private void Process(ControlData controlData, NewsPagesListRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                {
                    List<NewsPage> newsPages = null;
                    NewsPage requestedNewsPage = null;

                    if (request.PageId.IsNullOrEmpty())
                    {
                        RecordTransaction(
                            "PageListManager.GetUserNewsPagesList",
                            null,
                            manager =>
                            {
                                newsPages = manager.GetUserNewsPagesList(request.CacheState == CacheState.ForceRefresh);
                            },
                            new PageListManager(controlData, preferences));
                        
                    }
                    else
                    {
                        NewsPageListWithNewsPage newspagesWithPage = null;
                        RecordTransaction(
                            "PageListManager.GetUserNewsPagesListWithAPage",
                            null,
                            manager =>
                            {
                                newspagesWithPage = manager.GetUserNewsPagesListWithAPage(request.PageId);
                            },
                            new PageListManager(controlData, preferences));
                         
                        newsPages = newspagesWithPage.NewsPages;
                        requestedNewsPage = newspagesWithPage.RequestedNewsPage;
                    }

                    Package = new NewsPagesListPackage
                                  {
                                      NewsPages = newsPages,
                                      RequestedNewsPage = requestedNewsPage
                                  };
                },
                preferences);
        }
    }
}
