using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Utilities.Managers;
using DowJones.Infrastructure;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;

using DowJones.Web.Mvc.UI.Components.PortalArticle;

namespace DowJones.Web.Showcase.Controllers
{
    public class PortalArticleController : Controller
    {
        //
        // GET: /PortalArticle/

        public ActionResult Index(string acn = "WSJO000020101214e6cc004bl")
        {
            return View("Index",GetPortalArticleModel(acn));
        }

        private PortalArticleModel GetPortalArticleModel(string acn)
        {
            ControlData cd = ControlDataManager.GetLightWeightUserControlData("dacostad", "brian", "16");
            Session["cData"] = cd;
            
            var objArticle2 = GetArticle(acn).CopyTo();
            var portalModel = new PortalArticleModel()
            {
                ArticleObject= objArticle2,
                SourceLanguage="en"
            };
            return portalModel;
        }

        /// <summary>
        /// The get article.
        /// </summary>
        /// <param name="accno">
        /// The accno.
        /// </param>
        /// <returns>
        /// </returns>
        protected Article GetArticle(string accno)
        {
            var cData = (ControlData)Session["cData"];

            var articleRequest = new GetArticleRequest
            {
                accessionNumbers = new[] { accno },
                responseDataSet = GetBaselinResponseDataSet()
            };

            // DJDN000020100310e63a0ljs4 J000000020090814e58e00013

            // Make the call to the backend
            var serviceResponse = ArchiveService.GetArticle(ControlDataManager.Clone(cData), articleRequest);

            if (serviceResponse.rc == 0)
            {
                object responseObj;
                var responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);

                if (responseObjRC == 0)
                {
                    var response = (GetArticleResponse)responseObj;
                    if (response != null &&
                        response.articleResponseSet != null &&
                        response.articleResponseSet.countSpecified &&
                        response.articleResponseSet.count > 0 &&
                        response.articleResponseSet.article[0] != null)
                    {
                        return response.articleResponseSet.article[0];
                    }
                }
            }


            return null;
        }

        /// <summary>
        /// The get baselin response data set.
        /// </summary>
        /// <returns>
        /// </returns>
        private static Factiva.Gateway.Messages.Archive.V1_0.ResponseDataSet GetBaselinResponseDataSet()
        {
            var responseDataSet = new Factiva.Gateway.Messages.Archive.V1_0.ResponseDataSet
            {
                articleFormat = ArticleFormatType.Custom,
                fids = new[]
                                                     {
                                                         DistDocField.Logo, DistDocField.Sn, DistDocField.Ct, 
                                                         DistDocField.Clm, DistDocField.Ed, DistDocField.Rf, 
                                                         DistDocField.Se, DistDocField.Pg, DistDocField.An, 
                                                         DistDocField.Hd, DistDocField.Vol, DistDocField.In, 
                                                         DistDocField.By, DistDocField.La, DistDocField.Co, 
                                                         DistDocField.Cr, DistDocField.Art, DistDocField.Re, 
                                                         DistDocField.Wc, DistDocField.Cy, DistDocField.Ns, 
                                                         DistDocField.Pd, DistDocField.Cx, DistDocField.AdocTOC, 
                                                         DistDocField.Et, DistDocField.Lp, DistDocField.Sc, 
                                                         DistDocField.Td
                                                     }
            };
            return responseDataSet;
        }

    }
}
