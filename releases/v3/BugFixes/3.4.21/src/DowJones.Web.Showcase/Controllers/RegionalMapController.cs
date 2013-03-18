using System.Collections.Generic;  
using System.Web.Mvc;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.RegionalMap;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;
using DowJones.Models.Common;
using DowJones.Formatters; 
using DowJones.Ajax.HeadlineList;
using Newtonsoft.Json;
using System.IO;

namespace DowJones.Web.Showcase.Controllers
{
    public class RegionalMapController : ControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Regional Map";
            return Components("Index", GetRegionalMapModel());
        }

        private IViewComponentModel GetRegionalMapModel()
        {
            string s = @"[
                            {
                            'code': 'AUSTR',
                            'currentTimeFrameNewsVolume': {
                            'displayText': {
                                'value': '9,217'
                            },
                            'value': 9217
                          },
                        'descriptor': null,
                        'type': 6,
                        'newEntrant': false,
                        'percentVolumeChange': {
                            'displayText': {
                                'value': '-24.52%'
                            },
                            'value': -24.518876422897385
                        },
                        'previousNewsVolume': {
                            'displayText': {
                                'value': '12,211'
                            },
                            'value': 12211
                        }
                    },
                  {
                      'code': 'APACZ',
                      'currentTimeFrameNewsVolume': {
                          'displayText': {
                              'value': '94,598'
                          },
                          'value': 94598
                      },
                      'descriptor': null,
                      'type': 6,
                      'newEntrant': false,
                      'percentVolumeChange': {
                          'displayText': {
                              'value': '-14.62%'
                          },
                          'value': -14.618120114807665
                      },
                      'previousNewsVolume': {
                          'displayText': {
                              'value': '110,794'
                          },
                          'value': 110794
                      }
                  },
                  {
                      'code': 'AFRICAZ',
                      'currentTimeFrameNewsVolume': {
                          'displayText': {
                              'value': '23,515'
                          },
                          'value': 23515
                      },
                      'descriptor': null,
                      'type': 6,
                      'newEntrant': false,
                      'percentVolumeChange': {
                          'displayText': {
                              'value': '2.31%'
                          },
                          'value': 2.3103028193525832
                      },
                      'previousNewsVolume': {
                          'displayText': {
                              'value': '22,984'
                          },
                          'value': 22984
                      }
                  },
                  {
                      'code': 'MEASTZ',
                      'currentTimeFrameNewsVolume': {
                          'displayText': {
                              'value': '28,431'
                          },
                          'value': 28431
                      },
                      'descriptor': null,
                      'type': 6,
                      'newEntrant': false,
                      'percentVolumeChange': {
                          'displayText': {
                              'value': '4.62%'
                          },
                          'value': 4.6218951241950412
                      },
                      'previousNewsVolume': {
                          'displayText': {
                              'value': '27,175'
                          },
                          'value': 27175
                      }
                  },
                  {
                      'code': 'RUSS',
                      'currentTimeFrameNewsVolume': {
                          'displayText': {
                              'value': '10,166'
                          },
                          'value': 10166
                      },
                      'descriptor': null,
                      'type': 6,
                      'newEntrant': false,
                      'percentVolumeChange': {
                          'displayText': {
                              'value': '-7.43%'
                          },
                          'value': -7.4303405572755388
                      },
                      'previousNewsVolume': {
                          'displayText': {
                              'value': '10,982'
                          },
                          'value': 10982
                      }
                  },
                  {
                      'code': 'EURZ',
                      'currentTimeFrameNewsVolume': {
                          'displayText': {
                              'value': '133,759'
                          },
                          'value': 133759
                      },
                      'descriptor': null,
                      'type': 6,
                      'newEntrant': false,
                      'percentVolumeChange': {
                          'displayText': {
                              'value': '-7.64%'
                          },
                          'value': -7.6383949841528516
                      },
                      'previousNewsVolume': {
                          'displayText': {
                              'value': '144,821'
                          },
                          'value': 144821
                      }
                  },
                  {
                      'code': 'NAMZ',
                      'currentTimeFrameNewsVolume': {
                          'displayText': {
                              'value': '273,787'
                          },
                          'value': 273787
                      },
                      'descriptor': null,
                      'type': 6,
                      'newEntrant': false,
                      'percentVolumeChange': {
                          'displayText': {
                              'value': '-22.87%'
                          },
                          'value': -22.866472086366606
                      },
                      'previousNewsVolume': {
                          'displayText': {
                              'value': '354,952'
                          },
                          'value': 354952
                      }
                  },
                  {
                      'code': 'INDIA',
                      'currentTimeFrameNewsVolume': {
                          'displayText': {
                              'value': '14,338'
                          },
                          'value': 14338
                      },
                      'descriptor': null,
                      'type': 6,
                      'newEntrant': false,
                      'percentVolumeChange': {
                          'displayText': {
                              'value': '-27.08%'
                          },
                          'value': -27.077611636659547
                      },
                      'previousNewsVolume': {
                          'displayText': {
                              'value': '19,662'
                          },
                          'value': 19662
                      }
                  },
                  {
                      'code': 'SAMZ',
                      'currentTimeFrameNewsVolume': {
                          'displayText': {
                              'value': '11,675'
                          },
                          'value': 11675
                      },
                      'descriptor': null,
                      'type': 6,
                      'newEntrant': false,
                      'percentVolumeChange': {
                          'displayText': {
                              'value': '-18.24%'
                          },
                          'value': -18.242296918767508
                      },
                      'previousNewsVolume': {
                          'displayText': {
                              'value': '14,280'
                          },
                          'value': 14280
                      }
                  },
                  {
                      'code': 'CAMZ',
                      'currentTimeFrameNewsVolume': {
                          'displayText': {
                              'value': '6,548'
                          },
                          'value': 6548
                      },
                      'descriptor': null,
                      'type': 6,
                      'newEntrant': false,
                      'percentVolumeChange': {
                          'displayText': {
                              'value': '-14.68%'
                          },
                          'value': -14.684039087947888
                      },
                      'previousNewsVolume': {
                          'displayText': {
                              'value': '7,675'
                          },
                          'value': 7675
                      }
                  }
                ]";
            TextReader textReader = new StringReader(s);
            JsonReader jsonreader = new JsonTextReader(textReader);        
            List<NewsEntityNewsVolumeVariation> newsvolume = new JsonSerializer().Deserialize<List<NewsEntityNewsVolumeVariation>>(jsonreader);
             
            var regionalMapModel = new RegionalMapModel{
                                       ShowTextLabels = true,
                                       Size = MapSize.Large,
                                       ShowTooltips = true ,
                                       Data = new RegionNewsVolumeResult {
                                                            RegionNewsVolume = newsvolume //GetMockNewsEntityVolumeVariation()  
                                                      }                                                 
                                   };
            return new ContentContainerModel(new IViewComponentModel[] { regionalMapModel });
        }

        private IList<NewsEntityNewsVolumeVariation> GetMockNewsEntityVolumeVariation() {      
            
            return new List<NewsEntityNewsVolumeVariation> {
                                                new NewsEntityNewsVolumeVariation {
                                                            Code = "Africaz",
                                                            CurrentTimeFrameNewsVolume = new WholeNumber(5568), 
                                                            PercentVolumeChange  = new Percent(2.3103028193525832), 
                                                            PreviousTimeFrameNewsVolume = new WholeNumber(22984),
                                                            Type = EntityType.Region,
                                                            TypeDescriptor = null
                                                } , 

                                                new NewsEntityNewsVolumeVariation {
                                                            Code = "MEASTZ",
                                                            CurrentTimeFrameNewsVolume = new WholeNumber(28431), 
                                                            PercentVolumeChange  = new Percent(4.6218951241950412), 
                                                            PreviousTimeFrameNewsVolume = new WholeNumber(27175),
                                                            Type = EntityType.Region,
                                                            TypeDescriptor = null
                                                } , 

                                                new NewsEntityNewsVolumeVariation {
                                                            Code = "RUSS",
                                                            CurrentTimeFrameNewsVolume = new WholeNumber(10166), 
                                                            PercentVolumeChange  = new Percent(-7.4303405572755388), 
                                                            PreviousTimeFrameNewsVolume = new WholeNumber(10982),
                                                            Type = EntityType.Region,
                                                            TypeDescriptor = null
                                                } , 

                                                new NewsEntityNewsVolumeVariation {
                                                            Code = "EURZ",
                                                            CurrentTimeFrameNewsVolume = new WholeNumber(133759), 
                                                            PercentVolumeChange  = new Percent(-7.6383949841528516), 
                                                            PreviousTimeFrameNewsVolume = new WholeNumber(144821) ,
                                                            Type = EntityType.Region,
                                                            TypeDescriptor = null
                                                } 
                                            }  ;

         
        }

    }
}
