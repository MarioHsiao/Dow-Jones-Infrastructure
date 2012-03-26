// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsPageServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "newsPageServiceResult", Namespace = "")]
    public class NewsPageServiceResult : AbstractServiceResult, IPopulate<NewsPageRequest>
    {
        #region Implementation of IPopulate

        [DataMember(Name = "package")]
        public NewsPagePackage Package { get; set; }

        public void Populate(ControlData controlData, NewsPageRequest request, IPreferences preferences)
        {
            Process(controlData, request, preferences);
        }

        #endregion

        private void Process(ControlData controlData, NewsPageRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                    {
                        NewsPage newsPage = null;
                        
                        RecordTransaction(
                            "PageAssetsManager.GetUserNewsPage",
                            null,
                            manager =>
                                {
                                    newsPage = manager.GetUserNewsPage(request.PageId, request.CacheState == CacheState.ForceRefresh);
                                },
                            new PageListManager(controlData, preferences));

                        Package = new NewsPagePackage
                                      {
                                          NewsPage = newsPage
                                      };
                    },
                    preferences);
        }
    }
}
