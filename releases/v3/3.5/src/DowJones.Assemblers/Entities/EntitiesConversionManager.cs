using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Ajax.HeadlineList;
using DowJones.DependencyInjection;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Numerical;
using DowJones.Globalization;
using DowJones.Models.Common;
using DowJones.Search.Core.ISO8601;
using DowJones.Extensions;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Assemblers.Entities
{
    public class EntitiesConversionManager : IAssembler<Models.Common.Entities, NavigatorSet>
    {
        private readonly DateTimeFormatter _dateTimeFormatter;
        private readonly NumberFormatter _numberFormatter;
        private readonly IResourceTextManager _resources;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitiesConversionManager"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">The interface language.</param>
        public EntitiesConversionManager(string interfaceLanguage)
            : this(new DateTimeFormatter(interfaceLanguage))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitiesConversionManager"/> class.
        /// </summary>
        public EntitiesConversionManager(DateTimeFormatter dateTimeFormatter)
            : this(dateTimeFormatter, null, null)
        {
        }

        public EntitiesConversionManager(DateTimeFormatter dateTimeFormatter, NumberFormatter numberFormatter, IResourceTextManager resources)
        {
            _dateTimeFormatter = dateTimeFormatter ?? new DateTimeFormatter("en");
            _numberFormatter = numberFormatter ?? new NumberFormatter();
            _resources = resources ?? ServiceLocator.Resolve<IResourceTextManager>();
        }

        #region Properties

        public DateTimeFormatter DateTimeFormatter
        {
            get { return _dateTimeFormatter; }
        }

        public NumberFormatter NumberFormatter
        {
            get { return _numberFormatter; }
        }

        public IResourceTextManager ResourceText
        {
            get { return _resources; }
        }

        #endregion

        public Models.Common.Entities Convert(NavigatorSet navigatorSet)
        {
            return Convert(navigatorSet, null);
        }
    
        public Models.Common.Entities Convert(NavigatorSet navigatorSet, List<string> expandedChartList)
        {
            int index = 0;
            var entities = new Models.Common.Entities();
            if (navigatorSet.NavigatorCollection != null)
            {
                foreach (Navigator objNavigator in navigatorSet.NavigatorCollection)
                {
                    switch (objNavigator.Id)
                    {
                        case "py":
                        case "pm":
                        case "pw":
                        case "pd":
                            {
                                var parentNewsEntity = new ParentDateNewsEntity
                                                           {
                                                               Position = index
                                                           };
                                if (expandedChartList != null && IsInExpandedList(objNavigator.Id, expandedChartList))
                                {
                                    parentNewsEntity.IsExpanded = true;
                                }
                                var newsEntities = new DateNewsEntities();
                                foreach (Bucket objBucket in objNavigator.BucketCollection)
                                {
                                    var objNewsEntity = new DateNewsEntity
                                                            {
                                                                Code = objBucket.Id,
                                                                Descriptor = objBucket.Value,
                                                                CurrentTimeFrameNewsVolume = new WholeNumber(objBucket.HitCount)
                                                            };
                                    objNewsEntity.CurrentTimeFrameNewsVolume.UpdateWithAbbreviatedText();
                                    switch (objNavigator.Id)
                                    {
                                        case "py":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Date Yearly";
                                                objNewsEntity.Type = EntityType.DateYearly;
                                                objNewsEntity.StartDate = new DateTime(Int32.Parse(objBucket.Id.Substring(0, 4)), 1, 1);
                                                objNewsEntity.EndDate = new DateTime(objNewsEntity.StartDate.Year, 12, 31);
                                                break;
                                            }
                                        case "pm":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Date Monthly";
                                                objNewsEntity.Type = EntityType.DateMonthly;
                                                objNewsEntity.StartDate = new DateTime(Int32.Parse(objBucket.Id.Substring(0, 4)), Int32.Parse(objBucket.Id.Substring(4, 2)), 1);
                                                objNewsEntity.EndDate = objNewsEntity.StartDate.AddMonths(1).Subtract(new TimeSpan(1, 0, 0, 0));
                                                break;
                                            }
                                        case "pw":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Date Weekly";
                                                objNewsEntity.Type = EntityType.DateWeekly;
                                                objNewsEntity.StartDate = Iso8601Date.GetIso8601Week(Int32.Parse(objBucket.Id.Substring(0, 4)), Int32.Parse(objBucket.Id.Substring(4, 2)));
                                                objNewsEntity.EndDate = objNewsEntity.StartDate.AddDays(7);
                                                break;
                                            }
                                        case "pd":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Date Daily";
                                                objNewsEntity.Type = EntityType.DateDaily;
                                                objNewsEntity.StartDate = new DateTime(Int32.Parse(objBucket.Id.Substring(0, 4)), Int32.Parse(objBucket.Id.Substring(4, 2)), Int32.Parse(objBucket.Id.Substring(6, 2)));
                                                objNewsEntity.EndDate = objNewsEntity.StartDate;
                                                break;
                                            }
                                    }
                                    objNewsEntity.TypeDescriptor = objNewsEntity.Type.ToString();
                                    objNewsEntity.StartDateFormattedString = DateTimeFormatter.FormatShortDate(objNewsEntity.StartDate);
                                    objNewsEntity.EndDateFormattedString = DateTimeFormatter.FormatShortDate(objNewsEntity.EndDate);
                                    newsEntities.Add(objNewsEntity);
                                }
                                parentNewsEntity.NewsEntities = newsEntities;
                                parentNewsEntity.Title = ResourceText.GetString("date");
                                switch (objNavigator.Id)
                                {
                                    case "py":
                                        {
                                            parentNewsEntity.Type = EntityType.DateYearly;
                                            break;
                                        }
                                    case "pm":
                                        {
                                            parentNewsEntity.Type = EntityType.DateMonthly;
                                            break;
                                        }
                                    case "pw":
                                        {
                                            parentNewsEntity.Type = EntityType.DateWeekly;
                                            break;
                                        }
                                    case "pd":
                                        {
                                            parentNewsEntity.Type = EntityType.DateDaily;
                                            break;
                                        }
                                }
                                entities.DateNewsEntities = parentNewsEntity;
                                break;
                            }
                        case "sf":
                        case "sc":
                            {
                                var parentNewsEntity = new ParentSourceNewsEntity
                                                           {
                                                               Position = index
                                                           };
                                if (expandedChartList != null && IsInExpandedList(objNavigator.Id, expandedChartList))
                                {
                                    parentNewsEntity.IsExpanded = true;
                                }
                                var newsEntities = new SourceNewsEntities();
                                foreach (Bucket objBucket in objNavigator.BucketCollection)
                                {
                                    var objNewsEntity = new SourceNewsEntity
                                                            {
                                                                Code = objBucket.Id,
                                                                Descriptor = objBucket.Value,
                                                                CurrentTimeFrameNewsVolume = new WholeNumber(objBucket.HitCount)
                                                            };
                                    objNewsEntity.CurrentTimeFrameNewsVolume.UpdateWithAbbreviatedText();
                                    switch (objNavigator.Id)
                                    {
                                        case "sc":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Source";
                                                objNewsEntity.Type = EntityType.Source;
                                                break;
                                            }
                                        case "sf":
                                            {
                                                if(objBucket.Type != null && objBucket.Type == "family")
                                                {
                                                    objNewsEntity.IsGroup = true;   //entended property for Source entity
                                                }
                                                //objNewsEntity.TypeDescriptor = "SourceFamily";
                                                objNewsEntity.Type = EntityType.SourceFamily;
                                                break;
                                            }
                                    }
                                    objNewsEntity.TypeDescriptor = objNewsEntity.Type.ToString();
                                    newsEntities.Add(objNewsEntity);
                                }
                                parentNewsEntity.NewsEntities = newsEntities;
                                parentNewsEntity.Title = ResourceText.GetString("sources");
                                switch (objNavigator.Id)
                                {
                                    case "sc":
                                        {
                                            parentNewsEntity.Type = EntityType.Source;
                                            break;
                                        }
                                    case "sf":
                                        {
                                            parentNewsEntity.Type = EntityType.SourceFamily;
                                            break;
                                        }
                                }
                                entities.SourceNewsEntities = parentNewsEntity;
                                break;
                            }
                        default:
                            {
                                var parentNewsEntity = new ParentNewsEntity
                                                           {
                                                               Position = index
                                                           };
                                if (expandedChartList != null && IsInExpandedList(objNavigator.Id, expandedChartList))
                                {
                                    parentNewsEntity.IsExpanded = true;
                                }
                                var newsEntities = new Models.Common.NewsEntities();
                                foreach (Bucket objBucket in objNavigator.BucketCollection)
                                {
                                    var objNewsEntity = new NewsEntity
                                                            {
                                                                Code = objBucket.Id,
                                                                Descriptor = objBucket.Value,
                                                                CurrentTimeFrameNewsVolume = new WholeNumber(objBucket.HitCount)
                                                            };
                                    objNewsEntity.CurrentTimeFrameNewsVolume.UpdateWithAbbreviatedText();
                                    switch (objNavigator.Id)
                                    {
                                        case "co":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Company";
                                                objNewsEntity.Type = EntityType.Company;
                                                break;
                                            }
                                        case "in":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Industry";
                                                objNewsEntity.Type = EntityType.Industry;
                                                break;
                                            }
                                        case "ns":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Subject";
                                                objNewsEntity.Type = EntityType.NewsSubject;
                                                break;
                                            }
                                        case "pe":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Person";
                                                objNewsEntity.Type = EntityType.Person;
                                                break;
                                            }
                                        case "re":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Region";
                                                objNewsEntity.Type = EntityType.Region;
                                                break;
                                            }
                                        case "au":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Author";
                                                objNewsEntity.Type = EntityType.Author;
                                                break;
                                            }
                                        case "la":
                                            {
                                                //objNewsEntity.TypeDescriptor = "Author";
                                                objNewsEntity.Type = EntityType.Language;
                                                break;
                                            }
                                    }
                                    objNewsEntity.TypeDescriptor = objNewsEntity.Type.ToString();
                                    newsEntities.Add(objNewsEntity);
                                }
                                parentNewsEntity.NewsEntities = newsEntities;

                                switch (objNavigator.Id)
                                {
                                    case "co":
                                        {
                                            parentNewsEntity.Title = ResourceText.GetString("companies");
                                            parentNewsEntity.Type = EntityType.Company;
                                            entities.CompanyNewsEntities = parentNewsEntity;
                                            break;
                                        }
                                    case "in":
                                        {
                                            parentNewsEntity.Title = ResourceText.GetString("industries");
                                            parentNewsEntity.Type = EntityType.Industry;
                                            entities.IndustryNewsEntities = parentNewsEntity;
                                            break;
                                        }
                                    case "ns":
                                        {
                                            parentNewsEntity.Title = ResourceText.GetString("newsSubjects");
                                            parentNewsEntity.Type = EntityType.NewsSubject;
                                            entities.SubjectNewsEntities = parentNewsEntity;
                                            break;
                                        }
                                    case "pe":
                                        {
                                            parentNewsEntity.Title = ResourceText.GetString("executives");
                                            parentNewsEntity.Type = EntityType.Person;
                                            entities.PersonNewsEntities = parentNewsEntity;
                                            break;
                                        }
                                    case "re":
                                        {
                                            parentNewsEntity.Title = ResourceText.GetString("regions");
                                            parentNewsEntity.Type = EntityType.Region;
                                            entities.RegionNewsEntities = parentNewsEntity;
                                            break;
                                        }
                                    case "au":
                                        {
                                            parentNewsEntity.Title = ResourceText.GetString("authors");
                                            parentNewsEntity.Type = EntityType.Author;
                                            entities.AuthorNewsEntities = parentNewsEntity;
                                            break;
                                        }
                                    case "la":
                                        {
                                            parentNewsEntity.Title = ResourceText.GetString("slanguages");
                                            parentNewsEntity.Type = EntityType.Language;
                                            entities.LanguageNewsEntities = parentNewsEntity;
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                    index++;
                }
            }
            return entities;
        }

        private static bool IsInExpandedList(string id, IEnumerable<string> expandedChartList)
        {
            return expandedChartList.Any(entityId => entityId == id);
        }

        public static string GetRoundedHitCount(int count)
        {
            if (count < 10000) {
                return new NumberFormatter().Format(count, NumberFormatType.Whole);
            }
            if (count <= 99999) {
                return String.Format("{0:##.0K}", Math.Round((double)count / 1000, 1));
            }
            if (count <= 999999) {
                return String.Format("{0:###K}", Math.Round((double)count / 1000));
            }
            //if (count <= 9999999) {
            //    return String.Format("{0:#.0M}", Math.Round((double)count / 1000000,1));
            //}
            return String.Format("{0}M", new NumberFormatter().Format(Math.Round((double)count / 1000000, 1), NumberFormatType.Precision));
        }


    }
}
