using System.Web.Mvc;
using DowJones.Session;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using DowJones.Web.Mvc.UI.Components.PortalArticle;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class PortalArticleController : ControllerBase
    {
        public ActionResult Index(string acn = "WSJO000020101214e6cc004bl")
        {
            var article = GetPortalArticle(acn);
            return View("Index", article);
        }

        private PortalArticleModel GetPortalArticle(string acn)
        {
            var objArticle2 = GetArticle(acn).CopyTo();
            var portalModel = new PortalArticleModel
                                  {
                                      ArticleObject = objArticle2,
                                      SourceLanguage = Preferences.InterfaceLanguage
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
            var articleRequest = new GetArticleRequest
            {
                accessionNumbers = new[] { accno },
                responseDataSet = GetBaselinResponseDataSet()
            };

            // Make the call to the backend
            var serviceResponse = ArchiveService.GetArticle(ControlDataManager.Convert(ControlData), articleRequest);

            if (serviceResponse.rc == 0)
            {
                object responseObj;
                var responseObjRc = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);

                if (responseObjRc == 0)
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
        private static ResponseDataSet GetBaselinResponseDataSet()
        {
            var responseDataSet = new ResponseDataSet
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
