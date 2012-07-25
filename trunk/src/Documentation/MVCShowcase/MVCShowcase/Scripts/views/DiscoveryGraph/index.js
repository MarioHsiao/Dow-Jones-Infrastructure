﻿var discoveryGraph, orientation = 'horizontal';

function makeInteractive() {
    $('#horz').click(function () {
        $('#scrollable').attr("disabled", false);

        $('#sortable').attr("disabled", true);
        $('#sortable').attr("checked", false);

        orientation = 'horizontal';

    });
    
    $('#vert').click(function () {
        $('#sortable').attr("disabled", false);

        $('#scrollable').attr("checked", false);

        orientation = 'vertical';

    });
    
    $('#apply').click(function () {
        discoveryGraph.changeOrientation(orientation);

        if ($('#scrollable')[0].checked) {
            discoveryGraph.scrollable();
        } else if ($('#sortable')[0].checked) {

            discoveryGraph.sortable();
        }
    });
}

function init() {
    discoveryGraph = $("#DiscoveryGraph-0").findComponent(DJ.UI.DiscoveryGraph);
    discoveryGraph.options.sortable = true;
    discoveryGraph.options.scrollable = true;
    discoveryGraph.setData(getStubData());
};

// wrapped in a function so that code folding can hide it!
function getStubData() {
    var data = {
        "discovery": {
            "companyNewsEntities": {
                "title": "Companies",
                "entities": [
                  {
                      "code": "mcrost",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "3,319"
                          },
                          "value": 3319
                      },
                      "descriptor": "Microsoft Corp",
                      "type": 1,
                      "typeDescriptor": "Company"
                  },
                  {
                      "code": "goog",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "3,232"
                          },
                          "value": 3232
                      },
                      "descriptor": "Google Inc",
                      "type": 1,
                      "typeDescriptor": "Company"
                  },
                  {
                      "code": "whoz",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "2,301"
                          },
                          "value": 2301
                      },
                      "descriptor": "World Health Organization",
                      "type": 1,
                      "typeDescriptor": "Company"
                  },
                  {
                      "code": "usfda",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "2,186"
                          },
                          "value": 2186
                      },
                      "descriptor": "Food and Drug Administration",
                      "type": 1,
                      "typeDescriptor": "Company"
                  },
                  {
                      "code": "ibm",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "2,085"
                          },
                          "value": 2085
                      },
                      "descriptor": "International Business Machines Corp",
                      "type": 1,
                      "typeDescriptor": "Company"
                  }
                ]
            },
            "industryNewsEntities": {
                "title": "Industries",
                "entities": [
                  {
                      "code": "iinv",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "1,296"
                          },
                          "value": 1296
                      },
                      "descriptor": "Investing/Securities",
                      "type": 4,
                      "typeDescriptor": "Industry"
                  },
                  {
                      "code": "i3302",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 809
                          },
                          "value": 809
                      },
                      "descriptor": "Computers/Electronics",
                      "type": 4,
                      "typeDescriptor": "Industry"
                  },
                  {
                      "code": "i330202",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 800
                          },
                          "value": 800
                      },
                      "descriptor": "Software",
                      "type": 4,
                      "typeDescriptor": "Industry"
                  },
                  {
                      "code": "iconssv",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 799
                          },
                          "value": 799
                      },
                      "descriptor": "Consumer Services",
                      "type": 4,
                      "typeDescriptor": "Industry"
                  },
                  {
                      "code": "i3303",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 650
                          },
                          "value": 650
                      },
                      "descriptor": "Networking",
                      "type": 4,
                      "typeDescriptor": "Industry"
                  }
                ]
            },
            "personNewsEntities": {
                "title": "Persons",
                "entities": [
                  {
                      "code": 12302325,
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 38
                          },
                          "value": 38
                      },
                      "descriptor": "Carol A Bartz",
                      "type": 2,
                      "typeDescriptor": "Person"
                  },
                  {
                      "code": 12399685,
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 34
                          },
                          "value": 34
                      },
                      "descriptor": "Scott Thompson",
                      "type": 2,
                      "typeDescriptor": "Person"
                  },
                  {
                      "code": 12403950,
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 26
                          },
                          "value": 26
                      },
                      "descriptor": "Mike Lazaridis",
                      "type": 2,
                      "typeDescriptor": "Person"
                  },
                  {
                      "code": 48199442,
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 26
                          },
                          "value": 26
                      },
                      "descriptor": "Bill A Ackman",
                      "type": 2,
                      "typeDescriptor": "Person"
                  },
                  {
                      "code": 47859360,
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 24
                          },
                          "value": 24
                      },
                      "descriptor": "Thorsten Heins",
                      "type": 2,
                      "typeDescriptor": "Person"
                  }
                ]
            },
            "regionNewsEntities": {
                "title": "Regions",
                "entities": [
                  {
                      "code": "usa",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "6,440"
                          },
                          "value": 6440
                      },
                      "descriptor": "United States",
                      "type": 6,
                      "typeDescriptor": "Region"
                  },
                  {
                      "code": "usca",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "2,127"
                          },
                          "value": 2127
                      },
                      "descriptor": "California",
                      "type": 6,
                      "typeDescriptor": "Region"
                  },
                  {
                      "code": "usw",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "2,107"
                          },
                          "value": 2107
                      },
                      "descriptor": "Western U.S.",
                      "type": 6,
                      "typeDescriptor": "Region"
                  },
                  {
                      "code": "use",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 854
                          },
                          "value": 854
                      },
                      "descriptor": "Northeast U.S.",
                      "type": 6,
                      "typeDescriptor": "Region"
                  },
                  {
                      "code": "usny",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": 707
                          },
                          "value": 707
                      },
                      "descriptor": "New York",
                      "type": 6,
                      "typeDescriptor": "Region"
                  }
                ]
            },
            "subjectNewsEntities": {
                "entities": [
                  {
                      "code": "ccat",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "4,500"
                          },
                          "value": 4500
                      },
                      "descriptor": "Corporate/Industrial News",
                      "type": 5,
                      "typeDescriptor": "NewsSubject"
                  },
                  {
                      "code": "m11",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "2,805"
                          },
                          "value": 2805
                      },
                      "descriptor": "Equity Markets",
                      "type": 5,
                      "typeDescriptor": "NewsSubject"
                  },
                  {
                      "code": "gcat",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "2,570"
                          },
                          "value": 2570
                      },
                      "descriptor": "Political/General News",
                      "type": 5,
                      "typeDescriptor": "NewsSubject"
                  },
                  {
                      "code": "nrmf",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "1,902"
                          },
                          "value": 1902
                      },
                      "descriptor": "Routine Market/Financial News",
                      "type": 5,
                      "typeDescriptor": "NewsSubject"
                  },
                  {
                      "code": "c1522",
                      "currentTimeFrameNewsVolume": {
                          "displayText": {
                              "value": "1,495"
                          },
                          "value": 1495
                      },
                      "descriptor": "Share Price Movement/Disruptions",
                      "type": 5,
                      "typeDescriptor": "NewsSubject"
                  }
                ],
                "title": "Subjects"
            }
        }
    };
    return data;
}