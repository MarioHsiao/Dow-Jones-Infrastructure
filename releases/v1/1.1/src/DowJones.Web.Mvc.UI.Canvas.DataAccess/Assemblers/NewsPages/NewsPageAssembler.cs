// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsPageAssembler.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DataAccessModel = DowJones.Web.Mvc.UI.Models.NewsPages;
using FactivaDataModel = Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.NewsPages
{
    public class NewsPageAssembler :         
       IAssembler<DataAccessModel.NewsPage, FactivaDataModel.NewsPage>,
       IAssembler<DataAccessModel.MetaData, FactivaDataModel.Metadata>
    {
        public DataAccessModel.NewsPage Convert(FactivaDataModel.NewsPage source)
        {
            return Convert(source, null);
        }

        public DataAccessModel.NewsPage Convert(FactivaDataModel.NewsPage source, string cachekey)
        {
            var returnNewsPage = new DataAccessModel.NewsPage
                                     {
                                         AccessQualifier = (DataAccessModel.AccessQualifier)Enum.Parse(typeof(DataAccessModel.AccessQualifier), source.PageQualifier.ToString(), true), 
                                         Description = source.PageProperties != null ? !string.IsNullOrEmpty(source.PageProperties.CustomDescription) ? source.PageProperties.CustomDescription : source.PageProperties.Description : string.Empty, 
                                         ID = cachekey.IsNotEmpty() ? cachekey : source.Id.ToString(), 
                                         IsActive = true, 
                                         MetaData = source.PageProperties != null ? Convert(source.PageProperties.PageMetaData) : null, 
                                         OwnerNamespace = source.PageProperties != null ? !string.IsNullOrEmpty(source.PageProperties.CreatedByNamespace) ? source.PageProperties.CreatedByNamespace : string.Empty : string.Empty, 
                                         OwnerUserId = source.PageProperties != null && string.IsNullOrEmpty(source.PageProperties.CreatedByAccountId) ? source.PageProperties.CreatedByAccountId : string.Empty, 
                                         Position = source.PageProperties != null ? source.PageProperties.Position : 0, 
                                         Title = source.PageProperties != null ? string.IsNullOrEmpty(source.PageProperties.CustomTitle) ? source.PageProperties.CustomTitle : source.PageProperties.Title : string.Empty,
                                     };
            return returnNewsPage;
        }

        public DataAccessModel.MetaData Convert(FactivaDataModel.Metadata source)
        {
            var returnNMetadata = new DataAccessModel.MetaData();
            return returnNMetadata;
        }
    }
}
