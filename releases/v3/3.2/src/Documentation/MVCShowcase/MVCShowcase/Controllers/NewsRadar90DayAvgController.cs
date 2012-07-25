using System.Collections.ObjectModel;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.NewsRadar;
using DowJones.Web.Mvc.Routing;

namespace DowJones.MvcShowcase.Controllers
{
    public class NewsRadar90DayAvgController : Controller
    {
        public ActionResult Index()
        {
            #region Data
            var data = new Collection<EntityModel>
                       {
                           new EntityModel
                           {
                               CompanyName = "Dupont",
                               OwnershipType = OwnershipType.Private,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 47},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 203},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 457},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1165},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2519},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3458}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 19},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 70},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 154},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 394},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 749},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1067}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 32},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 77},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 222},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 375},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 531}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 50},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 163},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 220},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 297}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 65},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 117}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 35},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 57},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 142},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 439},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 566}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 36},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 65},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 135},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 430},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 583}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 8},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 42},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 96},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 123}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 31},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 47},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 145},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 174}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 19},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 70},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 154},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 394},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 749},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1067}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "766517072",
                                                         FCode = "DPNT",
                                                         Type = 0,
                                                         Source = 0
                                                     },
                               IsNewsCoded = false,
                               NewsSearch = "dupont"
                           },
                           new EntityModel
                           {
                               CompanyName = "3M Co.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 7},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 53},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 135},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 449},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1267},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1620}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 18},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 33},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 111},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 357},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 446}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 23},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 69},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 152},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 202}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 28},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 64},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 75}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 17},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 43},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 53}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 18},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 33},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 99},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 357},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 446}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 31},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 96},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 318},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 402}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 17},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 27},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 36}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 59},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 66}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 111},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 247},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 340}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "006173082",
                                                         FCode = "MMMUK",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "MMM"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "3m co"
                           },
                           new EntityModel
                           {
                               CompanyName = "Alcoa, Inc.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 6},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 138},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 205},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 391},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1386},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2063}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 45},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 62},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 108},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 498},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 712}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 23},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 35},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 63},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 132},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 214}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 31},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 65},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 107}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 43},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 79}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 37},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 49},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 80},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 381},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 552}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 45},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 62},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 108},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 498},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 712}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 43},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 49}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 74},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 127}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 35},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 75},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 150},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 223}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "001339472",
                                                         FCode = "ALMAM",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "AA"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "alcoa inc"
                           },
                           new EntityModel
                           {
                               CompanyName = "American Express Co.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 68},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 474},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 713},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1717},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 4336},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 7317}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 23},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 150},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 214},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 500},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1344},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2354}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 31},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 60},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 203},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 435},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 791}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 118},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 223},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 361}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 85},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 208},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 391}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 72},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 95},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 212},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 785},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1310}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 150},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 214},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 500},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1344},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2354}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 29},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 37},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 73},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 173},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 286}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 55},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 63},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 117},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 346},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 616}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 23},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 107},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 182},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 409},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 822},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1208}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "006979900",
                                                         FCode = "AMEXPR",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "AXP"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "american express"
                           },
                           new EntityModel
                           {
                               CompanyName = "AT&T, Inc.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 38},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 240},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 440},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1231},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2500},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3580}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 24},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 65},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 123},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 284},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 557},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 844}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 36},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 77},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 237},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 440},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 610}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 19},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 37},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 140},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 270},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 350}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 27},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 51},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 96},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 132}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 39},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 64},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 218},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 532},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 710}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 65},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 100},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 268},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 557},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 844}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 39},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 59}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 43},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 63}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 60},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 123},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 284},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 523},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 812}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "108024050",
                                                         FCode = "SBCATT",
                                                         Type = 0,
                                                         Source = 0
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "at t"
                           },
                           new EntityModel
                           {
                               CompanyName = "Bank of America Corp.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 448},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 3221},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 5370},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 13743},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 28818},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 44974}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 128},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 982},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1544},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 4212},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 8868},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 14145}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 62},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 454},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 853},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1998},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3963},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 6332}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 46},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 216},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 386},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 847},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1803},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2668}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 19},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 291},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 586},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1370},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2573},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 4219}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 74},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 483},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 717},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 2003},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 4740},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 7081}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 128},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 982},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1544},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 4212},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 8868},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 14145}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 76},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 100},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 216},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 497},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 746}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 73},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 432},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 651},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1660},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3569},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 5869}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 34},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 287},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 533},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1437},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2805},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3914}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "055169452",
                                                         FCode = "NCNBCO",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "BAC"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "bank of america"
                           },
                           new EntityModel
                           {
                               CompanyName = "Caterpillar, Inc.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 69},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 334},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 458},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 780},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1803},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2527}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 21},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 97},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 129},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 194},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 432},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 604}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 54},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 84},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 147},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 295},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 407}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 42},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 56},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 114},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 210},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 244}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 22},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 26},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 70},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 132}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 11},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 68},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 80},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 125},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 387},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 492}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 97},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 129},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 194},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 432},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 604}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 39}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 33},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 66},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 172},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 258}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 31},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 49},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 105},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 224},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 351}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "005070479",
                                                         FCode = "CATRA",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "CAT"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "caterpillar inc"
                           },
                           new EntityModel
                           {
                               CompanyName = "Chevron Corp.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 48},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 338},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 580},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1378},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3069},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 4427}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 136},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 199},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 467},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1019},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1543}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 52},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 177},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 424},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 622}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 111},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 223},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 299}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 11},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 74},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 130}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 60},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 114},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 253},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 693},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 930}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 136},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 199},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 467},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1019},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1543}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 29},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 54},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 77},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 94}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 30},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 54},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 128},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 216}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 11},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 54},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 112},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 241},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 431},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 593}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "001382555",
                                                         FCode = "SOCAL",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "CVX"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "chevron corp"
                           },
                           new EntityModel
                           {
                               CompanyName = "Citigroup, Inc.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 115},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 1035},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1836},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 3863},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 8341},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 13736}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 31},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 267},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 469},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1039},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2195},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3575}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 227},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 362},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 721},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1460},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2520}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 114},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 172},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 330},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 648},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1087}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 11},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 124},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 213},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 446},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1036},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1722}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 89},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 198},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 459},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1155},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1742}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 31},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 267},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 469},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1039},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2195},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3575}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 37},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 93},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 211},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 331}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 44},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 118},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 244},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 526},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 879}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 146},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 267},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 531},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1110},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1880}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "197329691",
                                                         FCode = "CCRED",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "C"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "citigroup inc"
                           },
                           new EntityModel
                           {
                               CompanyName = "Exxon Mobil Corp.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 111},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 913},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1447},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 2934},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 7899},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 11798}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 42},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 386},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 521},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1069},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2965},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 4599}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 154},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 249},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 459},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1066},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1576}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 118},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 188},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 331},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 728},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 990}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 18},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 31},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 71},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 176},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 358}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 80},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 149},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 368},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1392},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1958}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 42},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 386},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 521},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1069},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2965},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 4599}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 49},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 79},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 122},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 195},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 241}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 65},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 132},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 613},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 942}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 70},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 165},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 382},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 764},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1134}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "001213214",
                                                         FCode = "EXXN",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "XOM"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "exxon mobil"
                           },
                           new EntityModel
                           {
                               CompanyName = "General Electric Co.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 43},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 432},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 801},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 2161},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 4302},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 8093}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 16},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 160},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 278},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 678},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1403},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3681}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 62},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 120},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 346},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 579},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 940}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 56},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 219},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 326},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 525}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 34},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 76},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 167},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 271}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 51},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 98},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 286},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 728},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1016}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 160},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 278},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 678},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1403},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3681}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 18},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 33},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 90},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 155},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 233}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 22},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 35},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 90},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 185},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 274}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 74},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 147},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 376},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 759},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1153}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "001367960",
                                                         FCode = "GNELC",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "GE"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "general electric"
                           },
                           new EntityModel
                           {
                               CompanyName = "General Motors Corp",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 101},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 626},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 988},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 2528},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 4253},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 6155}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 21},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 148},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 220},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 572},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1007},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1475}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 85},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 169},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 537},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 795},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1123}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 49},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 72},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 245},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 373},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 571}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 34},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 91},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 336},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 431},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 604}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 100},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 150},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 341},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 717},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 995}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 148},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 220},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 572},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1007},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1475}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 35},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 45},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 63},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 94},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 134}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 78},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 98},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 191},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 391},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 631}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 97},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 143},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 243},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 445},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 622}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "005356613",
                                                         FCode = "GNMOC",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "GM"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "general motors"
                           },
                           new EntityModel
                           {
                               CompanyName = "Hewlett-Packard Co.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 105},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 599},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1437},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 2299},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3247},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 4826}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 24},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 158},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 489},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 737},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 980},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1304}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 86},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 181},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 318},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 468},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 808}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 22},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 69},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 127},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 184},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 242},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 430}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 40},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 100},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 179},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 318}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 158},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 489},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 737},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 980},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1304}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 152},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 377},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 559},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 753},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1076}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 40},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 61},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 86},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 119},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 165}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 43},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 75},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 99},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 119},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 165}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 87},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 216},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 387},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 560}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "009122532",
                                                         FCode = "HEWLP",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "HPQ"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "hewlett packard"
                           },
                           new EntityModel
                           {
                               CompanyName = "Intel Corp. Iberia, S.A.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 101},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 367},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 615},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1515},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3575},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 5074}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 37},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 117},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 197},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 435},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 982},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1365}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 11},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 28},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 63},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 192},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 422},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 655}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 31},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 99},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 255},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 405}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 19},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 42},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 133},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 203}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 70},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 123},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 354},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 941},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1286}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 23},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 117},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 197},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 435},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 982},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1365}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 8},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 29},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 60},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 93}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 33},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 114},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 145}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 37},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 109},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 151},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 331},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 668},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 922}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "047897855",
                                                         FCode = "INTL",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "INTC"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "intel corp iberia"
                           },
                           new EntityModel
                           {
                               CompanyName = "International Business Machines Corp.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 30},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 342},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 793},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1833},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 5086},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 8425}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 11},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 107},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 194},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 450},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1268},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2238}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 58},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 137},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 318},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 799},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1319}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 29},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 71},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 166},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 509},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 869}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 55},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 142},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 317},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 466}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 33},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 110},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 240},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 915},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1380}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 73},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 171},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 396},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1268},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2238}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 31},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 61},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 108},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 211}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 60},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 279},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 691}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 11},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 107},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 194},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 450},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 891},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1251}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "001368083",
                                                         FCode = "IBM",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "IBM"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "international business machines"
                           },
                           new EntityModel
                           {
                               CompanyName = "Johnson & Johnson",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 115},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 478},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 813},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1915},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 5064},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 6984}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 36},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 139},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 247},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 570},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1478},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2054}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 50},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 80},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 231},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 588},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 824}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 29},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 47},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 143},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 370},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 503}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 42},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 133},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 175}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 25},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 86},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 128},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 277},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 905},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1122}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 26},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 139},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 247},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 570},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1478},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2054}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 30},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 77},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 287},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 415}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 30},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 51},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 297},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 374}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 36},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 124},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 237},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 524},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1006},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1517}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "001307081",
                                                         FCode = "JONJON",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "JNJ"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "johnson johnson"
                           },
                           new EntityModel
                           {
                               CompanyName = "JPMorgan Chase & Co.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 445},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 3813},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 7183},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 20030},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 33310},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 48049}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 135},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 1136},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1858},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 5687},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 9299},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 13071}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 50},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 592},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1262},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 3167},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 5171},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 7726}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 270},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 646},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1679},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2588},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3653}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 26},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 388},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 795},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1789},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3109},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 4829}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 84},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 655},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1068},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 3214},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 5725},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 7860}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 135},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 1136},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1858},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 5687},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 9299},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 13071}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 80},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 162},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 537},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 812},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1063}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 92},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 488},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 866},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 2397},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3745},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 5401}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 23},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 204},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 526},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1560},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2861},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 4446}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "047675947",
                                                         FCode = "CNYC",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "JPM"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "jpmorgan chase"
                           },
                           new EntityModel
                           {
                               CompanyName = "Kraft Foods, Inc.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 17},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 197},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 309},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 570},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 860},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1224}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 6},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 60},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 85},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 133},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 236},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 329}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 60},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 85},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 133},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 170},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 245}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 19},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 29},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 75},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 95},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 139}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 34},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 44},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 52},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 69},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 90}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 32},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 52},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 83},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 153},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 209}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 30},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 40},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 81},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 119},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 181}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 7}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 11},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 24}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 49},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 132},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 236},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 329}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "877147228",
                                                         FCode = "DK",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "KFT"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "kraft foods"
                           },
                           new EntityModel
                           {
                               CompanyName = "McDonalds Corp.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 31},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 213},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 490},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1123},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2219},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3397}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 74},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 160},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 411},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 777},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1239}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 29},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 80},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 147},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 236},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 350}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 27},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 63},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 91},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 127}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 44},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 58},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 88},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 128}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 34},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 81},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 240},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 567},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 832}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 74},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 160},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 411},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 777},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1239}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 8},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 19},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 72},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 154}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 113},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 171}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 76},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 147},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 275},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 396}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "041534264",
                                                         FCode = "BIGMAC",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "MCD"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "mcdonalds corp"
                           },
                           new EntityModel
                           {
                               CompanyName = "Merck & Co., Inc.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 53},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 173},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 339},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 736},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1710},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2631}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 17},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 47},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 107},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 220},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 421},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 684}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 35},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 60},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 120},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 220},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 361}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 27},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 75},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 136},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 195}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 18},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 53},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 83}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 28},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 57},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 123},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 364},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 516}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 33},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 58},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 140},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 348},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 534}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 11},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 30},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 87},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 138}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 81},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 120}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 17},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 47},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 107},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 220},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 421},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 684}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "001317064",
                                                         FCode = "MSDMH",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "MRK"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "merck co"
                           },
                           new EntityModel
                           {
                               CompanyName = "Microsoft Corp.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1082},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 5988},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 10562},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 24234},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 49748},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 70073}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 523},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 2383},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 4133},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 9789},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 18552},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 26567}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 118},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 805},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1482},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 3309},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 6547},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 9627}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 46},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 286},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 552},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1251},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2695},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3786}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 80},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 526},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 903},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 2000},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3763},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 5654}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 108},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 682},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1243},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 3049},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 7333},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 9837}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 156},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 961},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 1667},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 3685},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 8269},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 11173}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 29},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 153},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 249},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 585},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1135},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1578}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 22},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 192},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 333},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 566},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1454},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1851}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 523},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 2383},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 4133},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 9789},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 18552},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 26567}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "081466849",
                                                         FCode = "MCROST",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "MSFT"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "microsoft corp"
                           },
                           new EntityModel
                           {
                               CompanyName = "Pfizer Inc.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 43},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 186},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 464},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1139},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3363},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 4668}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 12},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 44},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 117},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 310},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 794},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1106}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 28},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 65},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 169},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 633},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 904}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 32},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 91},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 461},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 629}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 19},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 36},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 113},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 171}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 61},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 184},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 601},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 822}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 44},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 115},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 310},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 794},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1106}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 27},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 48},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 67},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 127},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 152}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 25},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 101},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 149}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 44},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 117},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 257},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 533},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 735}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "001326495",
                                                         FCode = "PFIZ",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "PFE"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "pfizer inc"
                           },
                           new EntityModel
                           {
                               CompanyName = "Procter & Gamble Co.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 85},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 488},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 996},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 2200},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 5083},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 7156}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 28},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 113},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 285},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 650},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1516},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2175}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 91},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 161},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 344},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 695},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 968}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 74},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 120},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 245},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 402},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 537}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 17},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 26},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 62},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 183},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 279}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 63},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 177},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 376},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1030},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1335}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 113},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 285},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 650},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1516},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2175}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 36},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 71},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 161},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 295}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 36},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 102},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 360},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 474}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 28},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 90},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 155},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 350},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 736},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1093}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "001316827",
                                                         FCode = "PRGML",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "PG"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "procter gamble"
                           },
                           new EntityModel
                           {
                               CompanyName = "The Boeing Co.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 54},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 263},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 444},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1166},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2560},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3948}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 20},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 79},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 123},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 327},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 730},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1124}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 33},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 53},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 157},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 308},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 522}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 8},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 80},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 156},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 255}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 17},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 50},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 100},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 166}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 37},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 61},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 200},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 544},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 761}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 8},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 61},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 121},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 327},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 730},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1124}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 30},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 54},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 82},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 116}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 61},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 172},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 260}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 79},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 123},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 237},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 468},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 744}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "009256819",
                                                         FCode = "BOEING",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "BA"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "the boeing"
                           },
                           new EntityModel
                           {
                               CompanyName = "The Coca-Cola Co.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 26},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 194},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 360},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 764},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2236},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3254}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 10},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 52},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 99},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 238},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 540},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 774}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 28},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 48},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 94},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 292},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 449}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 151},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 232}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 92},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 171}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 52},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 84},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 144},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 498},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 651}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 47},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 90},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 180},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 540},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 774}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 8},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 8},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 28},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 58},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 106}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 11},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 18},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 139},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 192}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 46},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 99},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 238},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 466},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 679}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "003296175",
                                                         FCode = "COCA",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "KO"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "the coca cola"
                           },
                           new EntityModel
                           {
                               CompanyName = "The Home Depot, Inc.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 96},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 341},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 627},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 2147},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3202},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 4829}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 39},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 133},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 259},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 808},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1258},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1918}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 40},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 74},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 207},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 299},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 483}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 32},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 111},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 149},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 241}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 6},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 60},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 81},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 135}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 26},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 76},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 121},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 543},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 781},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1039}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 39},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 133},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 259},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 808},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1258},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1918}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 25},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 49},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 83}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 62},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 233},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 307},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 527}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 34},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 56},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 160},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 278},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 403}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "072271711",
                                                         FCode = "HOMD",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "HD"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "the home depot"
                           },
                           new EntityModel
                           {
                               CompanyName = "The Walt Disney Co.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 57},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 351},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 516},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1665},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 2752},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 3774}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 17},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 97},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 131},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 427},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 676},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 896}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 29},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 51},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 190},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 336},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 466}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 19},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 31},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 124},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 191},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 244}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 49},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 98},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 153}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 63},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 83},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 427},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 676},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 896}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 15},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 97},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 130},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 403},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 614},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 817}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 40},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 49},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 83},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 163},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 243}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 27},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 85},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 130},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 174}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 17},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 78},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 131},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 304},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 544},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 781}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "932660376",
                                                         FCode = "DSNYW",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "DIS"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "the walt disney"
                           },
                           new EntityModel
                           {
                               CompanyName = "United Technologies Corp.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 19},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 219},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 484},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 743},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1485},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2324}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 8},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 61},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 128},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 172},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 326},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 508}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 3},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 58},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 128},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 171},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 294},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 481}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 21},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 59},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 91},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 157},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 287}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 34},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 98},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 101},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 130},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 152}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 29},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 59},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 214},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 335}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 61},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 108},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 172},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 326},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 508}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 0},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 4},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 18},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 32}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 30},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 64},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 106}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 8},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 14},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 106},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 282},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 423}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "001344142",
                                                         FCode = "UTC",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "UTX"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "united technologies"
                           },
                           new EntityModel
                           {
                               CompanyName = "Verizon Communications, Inc.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 98},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 576},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 763},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 1688},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 3796},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 5062}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 28},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 151},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 222},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 513},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1154},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1672}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 99},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 117},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 227},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 415},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 548}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 11},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 85},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 87},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 143},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 247},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 305}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 9},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 22},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 84},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 113}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 13},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 77},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 105},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 269},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 758},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 925}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 20},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 151},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 222},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 513},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1154},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1672}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 7},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 17},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 35},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 43}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 10},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 35},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 39},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 69},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 182},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 216}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 28},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 115},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 177},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 428},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 921},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1240}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "107212169",
                                                         FCode = "BELATT",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "VZ"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "verizon communications"
                           },
                           new EntityModel
                           {
                               CompanyName = "Wal-Mart Stores, Inc.",
                               OwnershipType = OwnershipType.Public,
                               TotalNewsItems = new Collection<NewsVolumeModel>
                                                {
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 88},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 471},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 957},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 2516},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 4645},
                                                    new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 6486}
                                                },
                               MaxNewsItems = new Collection<NewsVolumeModel>
                                              {
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 26},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 197},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 389},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 838},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1639},
                                                  new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2238}
                                              },
                               NewsEntities = new Collection<NewsEntityModel>
                                              {
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 38},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 89},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 268},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 433},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 721}
                                                                    },
                                                      SubjectCode = "CACTIO",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Corporate Actions",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "CACTIO",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 19},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 47},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 139},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 225},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 410}
                                                                    },
                                                      SubjectCode = "C18",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Ownership Changes",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C18",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 1},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 5},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 17},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 64},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 110},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 184}
                                                                    },
                                                      SubjectCode = "C17",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Funding/Capital",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C17",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 24},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 93},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 179},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 671},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1175},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 1433}
                                                                    },
                                                      SubjectCode = "C15",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Performance",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C15",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 26},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 197},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 389},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 838},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 1639},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 2238}
                                                                    },
                                                      SubjectCode = "MCAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Commodity/Financial Market News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "MCAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 2},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 45},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 86},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 163},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 213}
                                                                    },
                                                      SubjectCode = "C411",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Management Moves",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "C411",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 12},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 40},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 74},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 166},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 306},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 431}
                                                                    },
                                                      SubjectCode = "ECAT",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Economic News",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "ECAT",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  },
                                                  new NewsEntityModel
                                                  {
                                                      EntityType = EntityType.NewsSubject,
                                                      NewsVolumes = new Collection<NewsVolumeModel>
                                                                    {
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Day, HitCount = 16},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Week, HitCount = 67},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoWeeks, HitCount = 117},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.Month, HitCount = 284},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.TwoMonth, HitCount = 594},
                                                                        new NewsVolumeModel {TimeFrame = TimeFrame.ThreeMonth, HitCount = 856}
                                                                    },
                                                      SubjectCode = "NPRESS",
                                                      RadarSearchQuery = new RadarSearchQueryModel
                                                                         {
                                                                             Name = "Press Release",
                                                                             QueryType = QueryType.NewsSubject,
                                                                             SearchString = "NPRESS",
                                                                             Scope = Scope.ns,
                                                                             SearchMode = 2
                                                                         }
                                                  }
                                              },
                               InstrumentReference = new InstrumentReferenceModel
                                                     {
                                                         DunsNumber = "051957769",
                                                         FCode = "WLMRT",
                                                         Type = 0,
                                                         Source = 0,
                                                         Ticker = "WMT"
                                                     },
                               IsNewsCoded = true,
                               NewsSearch = "wal mart stores"
                           }
                       };
            #endregion
            
            var model = new NewsRadarModel { Data = data };

            return View(model);
        }

        [Route("NewsRadar90DayAvg/data/{mode}")]
        public ActionResult Data(string mode = "cs")
        {
            var view = mode == "cs" ? "_cSharpData" : "_jsData";

            return PartialView(view);
        }
    }
}