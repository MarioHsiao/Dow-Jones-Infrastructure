using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DowJones.Web.Mvc.UI.Components.TrendingNews;
using Newtonsoft.Json;
using DowJones.Models.Common;
using System.IO;   

namespace DowJones.Web.Showcase.Controllers
{
    public class TrendingNewsController : Controller
    {
        //
        // GET: /TrendingNews/

        public ActionResult Index()
        {
                string entities = @"[
                                      {
                                        'code': 'ohcvrc',
                                        'currentTimeFrameNewsVolume': {
                                          'displayText': {
                                            'value': '0'
                                          },
                                          'value': 0
                                        },
                                        'descriptor': 'Ohio Civil Rights Commission',
                                        'searchContextRef': '{\'pn\':\'Np\',\'sct\':\'TrendingItemSearchContext\',\'j\':\'{\\\'tf\\\':-7,\\\'et\\\':0,\\\'tt\\\':2,\\\'c\\\':\\\'ohcvrc\\\'}\',\'pid\':\'Np_V1_16519_16519_000000\',\'mid\':\'22686\'}',
                                        'type': 1,
                                        'newEntrant': false,
                                        'percentVolumeChange': {
                                          'displayText': {
                                            'value': '-100.00%'
                                          },
                                          'value': -100
                                        },
                                        'previousNewsVolume': {
                                          'displayText': {
                                            'value': '82'
                                          },
                                          'value': 82
                                        }
                                      },
                                      {
                                        'code': 'bmblb',
                                        'currentTimeFrameNewsVolume': {
                                          'displayText': {
                                            'value': '0'
                                          },
                                          'value': 0
                                        },
                                        'descriptor': 'Bumble Bee Foods, LLC',
                                        'searchContextRef': '{\'pn\':\'Np\',\'sct\':\'TrendingItemSearchContext\',\'j\':\'{\\\'tf\\\':-7,\\\'et\\\':0,\\\'tt\\\':2,\\\'c\\\':\\\'bmblb\\\'}\',\'pid\':\'Np_V1_16519_16519_000000\',\'mid\':\'22686\'}',
                                        'type': 1,
                                        'newEntrant': false,
                                        'percentVolumeChange': {
                                          'displayText': {
                                            'value': '-100.00%'
                                          },
                                          'value': -100
                                        },
                                        'previousNewsVolume': {
                                          'displayText': {
                                            'value': '75'
                                          },
                                          'value': 75
                                        }
                                      },
                                      {
                                        'code': 'amiass',
                                        'currentTimeFrameNewsVolume': {
                                          'displayText': {
                                            'value': '0'
                                          },
                                          'value': 0
                                        },
                                        'descriptor': 'AIA Group Ltd.',
                                        'searchContextRef': '{\'pn\':\'Np\',\'sct\':\'TrendingItemSearchContext\',\'j\':\'{\\\'tf\\\':-7,\\\'et\\\':0,\\\'tt\\\':2,\\\'c\\\':\\\'amiass\\\'}\',\'pid\':\'Np_V1_16519_16519_000000\',\'mid\':\'22686\'}',
                                        'type': 1,
                                        'newEntrant': false,
                                        'percentVolumeChange': {
                                          'displayText': {
                                            'value': '-100.00%'
                                          },
                                          'value': -100
                                        },
                                        'previousNewsVolume': {
                                          'displayText': {
                                            'value': '75'
                                          },
                                          'value': 75
                                        }
                                      },
                                      {
                                        'code': 'univin',
                                        'currentTimeFrameNewsVolume': {
                                          'displayText': {
                                            'value': '1'
                                          },
                                          'value': 1
                                        },
                                        'descriptor': 'Universal Investment GmbH',
                                        'searchContextRef': '{\'pn\':\'Np\',\'sct\':\'TrendingItemSearchContext\',\'j\':\'{\\\'tf\\\':-7,\\\'et\\\':0,\\\'tt\\\':2,\\\'c\\\':\\\'univin\\\'}\',\'pid\':\'Np_V1_16519_16519_000000\',\'mid\':\'22686\'}',
                                        'type': 1,
                                        'newEntrant': false,
                                        'percentVolumeChange': {
                                          'displayText': {
                                            'value': '-99.42%'
                                          },
                                          'value': -99.42
                                        },
                                        'previousNewsVolume': {
                                          'displayText': {
                                            'value': '171'
                                          },
                                          'value': 171
                                        }
                                      },
                                      {
                                        'code': 'chkfil',
                                        'currentTimeFrameNewsVolume': {
                                          'displayText': {
                                            'value': '445'
                                          },
                                          'value': 445
                                        },
                                        'descriptor': 'Chick Fil A Incorporated',
                                        'searchContextRef': '{\'pn\':\'Np\',\'sct\':\'TrendingItemSearchContext\',\'j\':\'{\\\'tf\\\':-7,\\\'et\\\':0,\\\'tt\\\':2,\\\'c\\\':\\\'chkfil\\\'}\',\'pid\':\'Np_V1_16519_16519_000000\',\'mid\':\'22686\'}',
                                        'type': 1,
                                        'newEntrant': false,
                                        'percentVolumeChange': {
                                          'displayText': {
                                            'value': '-87.31%'
                                          },
                                          'value': -87.31
                                        },
                                        'previousNewsVolume': {
                                          'displayText': {
                                            'value': '3,506'
                                          },
                                          'value': 3506
                                        }
                                      }
                                ]";


            JsonReader jsonreader = new JsonTextReader(new StringReader(entities)); 
            IList<NewsEntityNewsVolumeVariation> entityCollection = new JsonSerializer().Deserialize<IList<NewsEntityNewsVolumeVariation>>(jsonreader); 

            TrendingNewsModel model = new TrendingNewsModel {
                                                               //packageType = "trendingTopEntitiesPackage",  
                                                               //packageType = "TrendingDownPackage",  
                                                               packageType = "TrendingUpPackage",  
                                                               trendingEntities = entityCollection               
                                                            } ;                                                              
            
            return View("TrendingNews", model);
        }
















    }
}
