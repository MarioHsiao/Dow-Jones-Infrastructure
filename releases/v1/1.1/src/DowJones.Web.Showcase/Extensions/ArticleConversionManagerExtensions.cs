using DowJones.Tools.Ajax.Converters;
using Factiva.Gateway.Messages.Archive.V1_0;
using Factiva.Gateway.Services.V1_0;
using DowJones.Utilities.Managers;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Utils.V1_0;
using DowJones.Web.Mvc.UI.Components.Models.Article;

namespace DowJones.Web.Showcase.Extensions
{
    public static class ArticleConversionManagerExtensions
    {



        public static ArticleModel GetArticle(this ArticleConversionManager manager, string accessionNumber)
        {
            GetArticleRequest articleRequest = GetArticleRequest(accessionNumber);
            ControlData cData = Factiva.Gateway.Managers.ControlDataManager.GetLightWeightUserControlData("dacostad", "brian", "16");
            GetArticleResponse response = GetArticleResponse(articleRequest, cData);
            return new ArticleModel(manager.Process(response, cData));
        }

        private static GetArticleRequest GetArticleRequest(string accessionNumber)
        {
            return new GetArticleRequest
            {
                accessionNumbers = new[] { accessionNumber },
                responseDataSet = new Factiva.Gateway.Messages.Archive.V1_0.ResponseDataSet
                {
                    articleFormat = ArticleFormatType.Custom,
                    fids = new[]{
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
                }
            };
        }
        
        private static GetArticleResponse GetArticleResponse(GetArticleRequest request,ControlData cData)
        {
            
            var serviceResponse = ArchiveService.GetArticle(ControlDataManager.Clone(cData), request);
            
           GetArticleResponse response =null;
            if (serviceResponse.rc == 0)
            {
                object responseObj;
                var responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);

                if (responseObjRC == 0)
                {
                    response = responseObj as GetArticleResponse;
                    if (response == null ||
                        response.articleResponseSet == null ||
                        !response.articleResponseSet.countSpecified ||
                        response.articleResponseSet.count == 0 ||
                        response.articleResponseSet.article[0] == null)
                    {
                        response = null;
                    }
                }
            }
            return response;
        }
        
    }
}