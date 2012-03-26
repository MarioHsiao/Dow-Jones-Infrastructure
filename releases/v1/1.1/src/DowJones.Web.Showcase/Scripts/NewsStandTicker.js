$(function () {

    setTimeout("SetTickerData()", 500);

});

function SetTickerData() {

    var tickerData = {

        "topNewsVolumeEntities": [{ "code": "Apple", "currentTimeFrameNewsVolume": { "displayText": { "value": "21" }, "value": 21 }, "descriptor": "Apple", "type": 3 },

        { "code": "Cisco", "currentTimeFrameNewsVolume": { "displayText": { "value": "32" }, "value": 32 }, "descriptor": "Cisco", "type": 3 },

        { "code": "IBM", "currentTimeFrameNewsVolume": { "displayText": { "value": "52" }, "value": 52 }, "descriptor": "IBM", "type": 3 },

        { "code": "Moodys", "currentTimeFrameNewsVolume": { "displayText": { "value": "12" }, "value": 12 }, "descriptor": "Moodys", "type": 3 },

        { "code": "Google", "currentTimeFrameNewsVolume": { "displayText": { "value": "25" }, "value": 25 }, "descriptor": "Google", "type": 3 },

        { "code": "Barclays", "currentTimeFrameNewsVolume": { "displayText": { "value": "34" }, "value": 34 }, "descriptor": "Barclays", "type": 3 },

        { "code": "Fedex", "currentTimeFrameNewsVolume": { "displayText": { "value": "16" }, "value": 16 }, "descriptor": "Fedex", "type": 3 },

        { "code": "Microsoft", "currentTimeFrameNewsVolume": { "displayText": { "value": "19" }, "value": 19 }, "descriptor": "Microsoft", "type": 3 },

        { "code": "Infosys", "currentTimeFrameNewsVolume": { "displayText": { "value": "15" }, "value": 15 }, "descriptor": "Infosys", "type": 3 },

        { "code": "Verizon", "currentTimeFrameNewsVolume": { "displayText": { "value": "72" }, "value": 72 }, "descriptor": "Verizon", "type": 3 },

        { "code": "Netflix", "currentTimeFrameNewsVolume": { "displayText": { "value": "22" }, "value": 22 }, "descriptor": "Netflix", "type": 3 },

        { "code": "S&P", "currentTimeFrameNewsVolume": { "displayText": { "value": "42" }, "value": 42 }, "descriptor": "S&P", "type": 3 },

        { "code": "Dow Jones", "currentTimeFrameNewsVolume": { "displayText": { "value": "52" }, "value": 52 }, "descriptor": "Dow Jones", "type": 3 },

        { "code": "Samsung", "currentTimeFrameNewsVolume": { "displayText": { "value": "26" }, "value": 26 }, "descriptor": "Samsung", "type": 3}]

    };

    $(".NewsStandTicker").findComponent('dj_NewsStandTicker').setData(tickerData);

}

