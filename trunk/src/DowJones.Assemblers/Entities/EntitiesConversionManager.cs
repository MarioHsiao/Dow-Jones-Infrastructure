using DowJones.Ajax.HeadlineList;
using DowJones.Models.Common;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Assemblers.Entities
{
    public class EntitiesConversionManager : IAssembler<DowJones.Models.Common.Entities, NavigatorSet>
    {
        public DowJones.Models.Common.Entities Convert(NavigatorSet navigatorSet)
        {
            DowJones.Models.Common.Entities entities = new Models.Common.Entities();
            if (navigatorSet.NavigatorCollection != null)
            {
                foreach (Navigator objNavigator in navigatorSet.NavigatorCollection)
                {
                    ParentNewsEntity parentNewsEntity = new ParentNewsEntity();
                    Models.Common.NewsEntities newsEntities = new Models.Common.NewsEntities();

                    foreach (Bucket objBucket in objNavigator.BucketCollection)
                    {
                        NewsEntity objNewsEntity = new NewsEntity();
                        objNewsEntity.Code = objBucket.Id;
                        objNewsEntity.Descriptor = objBucket.Value;
                        objNewsEntity.CurrentTimeFrameNewsVolume = new Formatters.WholeNumber(objBucket.HitCount);
                        switch (objNavigator.Id)
                        {
                            case "co":
                                {
                                    objNewsEntity.TypeDescriptor = "Company";
                                    objNewsEntity.Type = EntityType.Company;
                                    break;
                                }
                            case "in":
                                {
                                    objNewsEntity.TypeDescriptor = "Industry";
                                    objNewsEntity.Type = EntityType.Industry;
                                    break;
                                }
                            case "ns":
                                {
                                    objNewsEntity.TypeDescriptor = "Subject";
                                    objNewsEntity.Type = EntityType.NewsSubject;
                                    break;
                                }
                            case "pe":
                                {
                                    objNewsEntity.TypeDescriptor = "Person";
                                    objNewsEntity.Type = EntityType.Person;
                                    break;
                                }
                            case "re":
                                {
                                    objNewsEntity.TypeDescriptor = "Region";
                                    objNewsEntity.Type = EntityType.Region;
                                    break;
                                }
                            //case "orgt":
                            //    {
                            //        objNewsEntity.TypeDescriptor = "Organization";
                            //        objNewsEntity.Type = EntityType.Organization;
                            //        break;
                            //    }
                            case "au":
                                {
                                    objNewsEntity.TypeDescriptor = "Author";
                                    objNewsEntity.Type = EntityType.Author;
                                    break;
                                }
                            case "sc":
                                {
                                    objNewsEntity.TypeDescriptor = "Source";
                                    objNewsEntity.Type = EntityType.Source;
                                    break;
                                }
                            //default:
                            //    {
                            //        objNewsEntity.TypeDescriptor = "UnSpecified";
                            //        objNewsEntity.Type = EntityType.UnSpecified;
                            //        break;
                            //    }
                        }
                        newsEntities.Add(objNewsEntity);
                    }
                    parentNewsEntity.NewsEntities = newsEntities;
                    
                    switch (objNavigator.Id)
                    {
                        case "co":
                            {
                                parentNewsEntity.Title = "companies";
                                parentNewsEntity.Type = EntityType.Company;
                                entities.CompanyNewsEntities = parentNewsEntity;
                                break;
                            }
                        case "in":
                            {
                                parentNewsEntity.Title = "industries";
                                parentNewsEntity.Type = EntityType.Industry;
                                entities.IndustryNewsEntities = parentNewsEntity;
                                break;
                            }
                        case "ns":
                            {
                                parentNewsEntity.Title = "newsSubjects";
                                parentNewsEntity.Type = EntityType.NewsSubject;
                                entities.SubjectNewsEntities = parentNewsEntity;
                                break;
                            }
                        case "pe":
                            {
                                parentNewsEntity.Title = "executives";
                                parentNewsEntity.Type = EntityType.Person;
                                entities.PersonNewsEntities = parentNewsEntity;
                                break;
                            }
                        case "re":
                            {
                                parentNewsEntity.Title = "regions";
                                parentNewsEntity.Type = EntityType.Region;
                                entities.RegionNewsEntities = parentNewsEntity;
                                break;
                            }
                        //case "orgt":
                        //    {
                        //        parentNewsEntity.Title = "organizations";
                        //        parentNewsEntity.Type = EntityType.Organization;
                        //        entities.OrganizationNewsEntities = parentNewsEntity;
                        //        break;
                        //    }
                        case "au":
                            {
                                parentNewsEntity.Title = "authors";
                                parentNewsEntity.Type = EntityType.Author;
                                entities.AuthorNewsEntities = parentNewsEntity;
                                break;
                            }
                        case "sc":
                            {
                                parentNewsEntity.Title = "sources";
                                parentNewsEntity.Type = EntityType.Source;
                                entities.SourceNewsEntities = parentNewsEntity;
                                break;
                            }
                        //default:
                        //    {
                        //        parentNewsEntity.Title = "UnSpecified";
                        //        parentNewsEntity.Type = EntityType.UnSpecified;
                        //        entities.UnSpecifiedNewsEntities = parentNewsEntity;
                        //        break;
                        //    }
                    }
                }
            }
            return entities;
        }
    }
}
