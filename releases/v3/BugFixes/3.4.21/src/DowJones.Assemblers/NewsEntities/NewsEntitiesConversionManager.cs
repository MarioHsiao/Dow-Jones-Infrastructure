using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Models.Common;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Assemblers.NewsEntities
{
    public class NewsEntitiesConversionManager : IAssembler<DowJones.Models.Common.NewsEntities, NavigatorSet>
    {

        public DowJones.Models.Common.NewsEntities Convert(NavigatorSet navigatorSet)
        {
            DowJones.Models.Common.NewsEntities newsEntities = new Models.Common.NewsEntities();
            if (navigatorSet.NavigatorCollection != null)
            {
                foreach (Navigator objNavigator in navigatorSet.NavigatorCollection)
                {
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
                                    objNewsEntity.Type = Ajax.HeadlineList.EntityType.Company;
                                    break;
                                }
                            case "ns":
                                {
                                    objNewsEntity.Type = Ajax.HeadlineList.EntityType.NewsSubject;
                                    break;
                                }
                            case "re":
                                {
                                    objNewsEntity.Type = Ajax.HeadlineList.EntityType.Region;
                                    break;
                                }
                            case "in":
                                {
                                    objNewsEntity.Type = Ajax.HeadlineList.EntityType.Industry;
                                    break;
                                }
                            case "pe":
                                {
                                    objNewsEntity.Type = Ajax.HeadlineList.EntityType.Person;
                                    break;
                                }
                            case "orgt":
                                {
                                    objNewsEntity.Type = Ajax.HeadlineList.EntityType.Organization;
                                    break;
                                }
                            case "au":
                                {
                                    objNewsEntity.Type = Ajax.HeadlineList.EntityType.Author;
                                    break;
                                }
                            default:
                                {
                                    objNewsEntity.Type = Ajax.HeadlineList.EntityType.UnSpecified;
                                    break;
                                }
                        }

                        newsEntities.Add(objNewsEntity);
                    }
                }
            }
            return newsEntities;
        }
    }
}
