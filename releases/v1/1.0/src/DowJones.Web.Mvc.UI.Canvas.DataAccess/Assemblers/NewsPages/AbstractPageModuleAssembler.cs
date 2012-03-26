using System;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using DataAccessModel = DowJones.Web.Mvc.UI.Models.NewsPages.Modules;
using FactivaDataModel = Factiva.Gateway.Messages.Assets.Pages.V1_0;
using DataAccessModelNewsPages= DowJones.Web.Mvc.UI.Models.NewsPages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.NewsPages
{
    public abstract class AbstractPageModuleAssembler         
    {
        protected readonly Factiva.Gateway.Utils.V1_0.ControlData ControlData;
        protected readonly IPreferences Preferences;

        protected AbstractPageModuleAssembler(Factiva.Gateway.Utils.V1_0.ControlData controlData, IPreferences preferences)
        {
            ControlData = controlData;
            Preferences = preferences;
        }

        /// <summary>
        /// Initializes the specified module.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public DataAccessModel.NewsPageModule Initialize(DataAccessModel.NewsPageModule module, ModuleEx source)
        {
           
            if (module == null)
            {
                throw new Exception("Data object cannot be empty");
            }

            if (source != null)
            {
                module.Description = source.Description;
                module.Id = source.Id;
                module.LastModifiedDate = source.LastModifiedDateUtc;
                module.ModuleProperties = ConvertModuleProperties(module.ModuleProperties, source.ModuleProperties);
                module.ModuleQualifier = PageListManager.MapAccessQualifier(source.ModuleQualifier); // (DataAccessModelNewsPages.AccessQualifier)Enum.Parse(typeof(DataAccessModelNewsPages.AccessQualifier), Enum.GetName(source.ModuleQualifier.GetType(), source.ModuleQualifier));
                //  module.ModuleState = (DataAccessModel.ModuleState)Enum.Parse(typeof(DataAccessModel.ModuleState), Enum.GetName(source.ModuleState.GetType(), source.ModuleState));
                module.Position = source.Position;

                // module.ShareProperties = ConvertShareProperties(module.ShareProperties, source.ShareProperties);
                if (source.TagCollection != null)
                {
                   // module.TagCollection.AddRange(source.TagCollection.ToArray());
                }

                module.Title = source.Title;
            }

            return module;
        }

        public  DataAccessModel.ModuleProperties ConvertModuleProperties(DataAccessModel.ModuleProperties moduleProperties, ModuleProperties sourcemoduleProperties)
        {
            if (moduleProperties == null)
            {
                throw new Exception("Data object moduleProperties cannot be empty");
            }

             if (sourcemoduleProperties != null)
             {
                // moduleProperties.PlaceHolder = sourcemoduleProperties.PlaceHolder;
                 if (sourcemoduleProperties.ModuleMetaData != null)
                 {
                     if (moduleProperties.ModuleMetaData == null)
                     {
                        // moduleProperties.ModuleMetaData = new DataAccessModel.Metadata();
                     }
                   //  moduleProperties.ModuleMetaData = ConvertMetadata(moduleProperties.ModuleMetaData, sourcemoduleProperties.ModuleMetaData);
                 }
             }
             return moduleProperties;
        }

       public ModuleEx Initialize(ModuleEx module, DataAccessModel.NewsPageModule source)
       {

           if (module == null)
           {
               throw new Exception("Data object cannot be empty");
           }

           if (source != null )
           {
               module.Description = source.Description;
               //module.Id = source.Id;
               module.LastModifiedDateUtc = source.LastModifiedDate;
               //module.ModuleProperties = ConvertModuleProperties(module.ModuleProperties, source.ModuleProperties);
               //module.ModuleQualifier = PageListManager.MapAccessQualifier(source.ModuleQualifier); // (DataAccessModelNewsPages.AccessQualifier)Enum.Parse(typeof(DataAccessModelNewsPages.AccessQualifier), Enum.GetName(source.ModuleQualifier.GetType(), source.ModuleQualifier));
               //  module.ModuleState = (DataAccessModel.ModuleState)Enum.Parse(typeof(DataAccessModel.ModuleState), Enum.GetName(source.ModuleState.GetType(), source.ModuleState));
               module.Position = source.Position;
               // module.ShareProperties = ConvertShareProperties(module.ShareProperties, source.ShareProperties);
              
               module.Title = source.Title;
           }
           return module;
       }

       //public FactivaDataModel.ModuleProperties ConvertModuleProperties(FactivaDataModel.ModuleProperties moduleProperties, DataAccessModel.ModuleProperties sourcemoduleProperties)
       //{
       //    if (moduleProperties == null)
       //    {
       //        throw new Exception("Data object moduleProperties cannot be empty");
       //    }

       //    if (sourcemoduleProperties != null )
       //    {
       //        moduleProperties.__moduleMetaData. = sourcemoduleProperties.c;
       //        if (sourcemoduleProperties.ModuleMetaData != null)
       //        {
       //            if (moduleProperties.ModuleMetaData == null)
       //            {
       //                // moduleProperties.ModuleMetaData = new DataAccessModel.Metadata();
       //            }
       //            //  moduleProperties.ModuleMetaData = ConvertMetadata(moduleProperties.ModuleMetaData, sourcemoduleProperties.ModuleMetaData);
       //        }
       //    }
       //    return moduleProperties;
       //}

    

       //public DataAccessModel.Metadata ConvertMetadata(DataAccessModel.Metadata resultMetadata,  FactivaDataModel.Metadata SourceMetadata)
       //{
       //    //Check for null of metadata proporties
       //    if (resultMetadata != null && SourceMetadata != null)
       //    {
       //        resultMetadata.AuthorCollection.AddRange(SourceMetadata.AuthorCollection != null ? SourceMetadata.AuthorCollection.ToArray() : null);
       //        resultMetadata.CategoryCollection.AddRange(SourceMetadata.CategoryCollection != null ? SourceMetadata.CategoryCollection.ToArray() : null);
       //        resultMetadata.CompanyCollection.AddRange(SourceMetadata.CompanyCollection != null ? SourceMetadata.CompanyCollection.ToArray() : null);
       //        resultMetadata.ExecutiveCollection.AddRange(SourceMetadata.ExecutiveCollection != null ? SourceMetadata.ExecutiveCollection.ToArray() : null);
       //        resultMetadata.FreeTextCollection.AddRange(SourceMetadata.ExecutiveCollection != null ? SourceMetadata.FreeTextCollection.ToArray() : null);
       //        resultMetadata.IndustryCollection.AddRange(SourceMetadata.IndustryCollection != null ? SourceMetadata.IndustryCollection.ToArray() : null);
       //        resultMetadata.NewsSubjectCollection.AddRange(SourceMetadata.NewsSubjectCollection != null ? SourceMetadata.NewsSubjectCollection.ToArray() : null);
       //        resultMetadata.RegionCollection.AddRange(SourceMetadata.RegionCollection != null ? SourceMetadata.RegionCollection.ToArray() : null);
       //        resultMetadata.SourceCollection.AddRange(SourceMetadata.SourceCollection != null ? SourceMetadata.SourceCollection.ToArray() : null);
       //        resultMetadata.TopicCollection.AddRange(SourceMetadata.SourceCollection != null ? SourceMetadata.TopicCollection.ToArray() : null);

       //    }
       //    return resultMetadata;
       //}

       //public DataAccessModel.ShareProperties ConvertShareProperties(DataAccessModel.ShareProperties resultShareProperties, FactivaDataModel.ShareProperties SourceShareProperties)
       //{
       //    if (resultShareProperties != null && SourceShareProperties != null)
       //    {
       //        resultShareProperties.AccessControlScope = (DataAccessModel.AccessControlScope)Enum.Parse(typeof(DataAccessModel.AccessControlScope), Enum.GetName(SourceShareProperties.AccessControlScope.GetType(), SourceShareProperties.AccessControlScope));
       //        resultShareProperties.AssignedScope = (DataAccessModel.ShareScope)Enum.Parse(typeof(DataAccessModel.ShareScope), Enum.GetName(SourceShareProperties.AssignedScope.GetType(), SourceShareProperties.AssignedScope));
       //        resultShareProperties.ListingScope = (DataAccessModel.ShareScope)Enum.Parse(typeof(DataAccessModel.ShareScope), Enum.GetName(SourceShareProperties.ListingScope.GetType(), SourceShareProperties.ListingScope));
       //        resultShareProperties.SharePromotion = (DataAccessModel.ShareScope)Enum.Parse(typeof(DataAccessModel.ShareScope), Enum.GetName(SourceShareProperties.SharePromotion.GetType(), SourceShareProperties.SharePromotion));
       //    }
       //    return resultShareProperties;
       //}

       //public DataAccessModel.QueryEntity ConvertQueryEntity(DataAccessModel.QueryEntity resultQueryEntity, FactivaDataModel.QueryEntity SourceQueryEntity)
       //{
       //    //mismatch in the classes
       //    if (resultQueryEntity == null)
       //    {
       //        resultQueryEntity = new DataAccessModel.QueryEntity();
       //    }
       //    //resultQueryEntity.Id = SourceQueryEntity.

       //    return resultQueryEntity;
       //}
    }
}
