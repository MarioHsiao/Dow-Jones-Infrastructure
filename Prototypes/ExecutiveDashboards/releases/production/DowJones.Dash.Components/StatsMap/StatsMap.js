/*!
 * StatsMap
 */

DJ.UI.StatsMap = DJ.UI.Component.extend({

    defaults: {
        debug: false,
        cssClass: 'statMap',
        articleVolumeTextColor: '#FFF', // Article Volume Text Color
        variationPercentageTextColor: '#000', // Variation Percentage Text Color
        circleFillColor: 'rgba(0,0,0,.5)', // Circle Fill Color
        innerCircleFillColor: 'rgba(0,0,0,.7)', // Circle Stroke Color
        circleStrokeWidth: 3, // Circle Stroke Width (Value in Pixels)
        textMaxSize: 30, // Text Max Size (Value in Px)
        textMinSize: 9, // Text Min Size (Value in Px)
        smallVersionTextSize: 7, // The text size that will trigger the small version
        showTooltips: true, //Show Text Labels (true/false)
        tooltipAlign: 'right', //Alignment of tooltip (right/left/center),
        width: 298,
        height: 198,
        imageMapPath: "dash/content/images/world_map.png"
    },

    mapSize: {
        width: 298,             //width of the component 
        height: 198,
        circleMaxRadius: 32,    //The maximum radius that a circle can have
        circleMinRadius: 7      //The minimum radius that a circle can have
    },

    mapConfig: {
        bubbles: {
            // NN - bubbles provide coordinates in percentage (posX/Y) for the bubble circles
            US: {
                id: "NAMZ",
                name: '<%= Token("northAmerica") %>',
                posX: 20,
                posY: 30
            },
            EURZ: {
                id: "EURZ",
                name: '<%= Token("europe") %>',
                posX: 50.25,
                posY: 32.55
            },
            ASIAZ: {
                id: "ASIAZ",
                name: '<%= Token("asia") %>',
                posX: 82,
                posY: 32,
                tooltipAlign: 'left'
            },
            AU: {
                id: "AUSNZ",
                name: '<%= Token("countryName9Aus") %>',
                posX: 87.125,
                posY: 81,
                tooltipAlign: 'left'
            },
            RU: {
                id: "RUSS",
                name: '<%= Token("s2bubbleRussia") %>',
                posX: 70,
                posY: 16,
                tooltipAlign: 'left'
            },
            AFRICAZ: {
                id: "AFRICAZ",
                name: '<%= Token("africa") %>',
                posX: 52,
                posY: 60
            },
            IN: {
                id: "INDSUBZ",
                name: '<%= Token("countryName9Ind") %>',
                posX: 70,
                posY: 46,
                tooltipAlign: 'left'
            }
        },
        mapImgWidth: 988, // Original Map Image Width
        mapImgHeight: 530 // Original Map Image Height            
    },

    init: function (element, meta) {
        // Call the base constructor
        this._super(element, $.extend({ name: "StatsMap" }, meta));

        this.bubbles = {};

        var o = this.options,
            mc = this.mapConfig;



        this.worldMapRenderer = new Highcharts.Renderer(this.element, o.width, o.height);

        this.map = {
            /* Put the world map at the bottom right of the container */
            x: Math.max(o.width - mc.mapImgWidth, 0),
            y: Math.max(o.height - mc.mapImgHeight, 0),

            /* World map size should not exceed container's size */
            width: Math.min(o.width, mc.mapImgWidth),
            height: Math.min(o.height, mc.mapImgHeight)
        };

        this.worldMapRenderer.image(o.imageMapPath,
            this.map.x, this.map.y, this.map.width, this.map.height).add();

        this.setData(this.data);
    },

    renderMap: function (data) {
        var i, len;

        if (!data || !data.length)
            return;

        this._clearbubbles();

        this.map.highestValue = _.max(data, function (item) {
            return item.value;
        });

        for (i = 0, len = data.length; i < len; i++) {
            try {
                this.addBubble(data[i]);
            }
            catch (e) {
                $dj.error("statsMap::Exception: ", e.message);
            }
        }
    },

    addBubble: function (bubbleData) {
        var el = this.$element,
            o = this.options,
            id = bubbleData.id,
            bubbleMapConfig = this.mapConfig.bubbles[id];

        if (!bubbleMapConfig) {
            $dj.warn('Unknown region:', bubbleData.id);
            return;
        }

        var posX = this.map.x + Math.ceil((bubbleMapConfig.posX * this.map.width) / 100), // The horizontal position of the circle
            posY = this.map.y + Math.ceil((bubbleMapConfig.posY * this.map.height) / 100), // The vertical position of the circle
            articleVolume = bubbleData.value, // The volume of the bubble
            circleRadius,
            tooltipAlign = bubbleMapConfig.tooltipAlign || o.tooltipAlign;

        circleRadius = ((articleVolume * o.circleMaxRadius) / this.map.highestValue);
        if (circleRadius < o.circleMinRadius) {
            circleRadius = o.circleMinRadius;
        }


        // Create a group (Highchart function)
        this.bubbles[id] =
            this.worldMapRenderer
            .g('bubble')
            .css({ 'cursor': 'pointer' })
            .add();

        // Draw a circle and add it to the previously created Highchart Group
        var circle = this.worldMapRenderer.circle(posX, posY, Math.ceil(circleRadius + o.circleStrokeWidth)).attr({
            fill: bubbleData.temperature
        }).add(this.bubbles[id]);


        // Tooltip (Note: the highcharts renderer does not support class names, hence inline CSS rules)
        if (o.showTooltips == true) {
            var tooltip = this.worldMapRenderer.g().add(this.bubbles[id]);

            var tooltipAvText = this.worldMapRenderer.text('articleVolumeText  articleText', posX, posY).css({
                fontSize: '11px',
                "font-family": 'Arial',
                'text-shadow': 'none',
                color: "#444",
                'font-weight': 'bold'
            }).add(tooltip);

            // Calculate tooltip dimensions
            var box1 = tooltipAvText.getBBox();

            var bw = box1.width;
            var bh = box1.height;

            var by = box1.y;
            var bx = box1.x;

            if ('left' == tooltipAlign) {

                tooltipAvText.attr('x', box1.x - bw);
                bx -= bw;
            } else if ('center' == tooltipAlign) {
                tooltipAvText.attr('x', box1.x - (bw / 2));
                bx -= (bw / 2);
            }

            var rectbox = this.worldMapRenderer.rect(bx - 5, by - 5, bw + 10, bh + 10, 5).attr({
                fill: 'rgba(255,255,255,0.8)'
            }).add(tooltip);

            tooltipAvText.toFront();
            tooltip.hide();
            $(this.bubbles[id]).data('tooltip', tooltip);
        }

    },


    _clearbubbles: function () {
        for (var bubble in this.map.bubbles) {
            this.bubbles[bubble].destroy();
            delete this.map.bubbles[bubble];
        }
    },

    _initializeDelegates: function () {
        this._delegates = $.extend(this._delegates, {
            setData: $dj.delegate(this, this.setData)
        });
    },

    _initializeElements: function (ctx) {
        this.$element.html(this.templates.container());

        this.$element.addClass('dj-stat-map').css({
            width: this.options.width,
            height: this.options.height
        });

        // make IE image resize a little better
        ctx.find('img').css("-ms-interpolation-mode", "bicubic");
    },

    _initializeEventHandlers: function () {
        $dj.subscribe('data.PageLoadDetailsByCountry', this._delegates.setData);
    },

    setData: function (data) {
        if (!data)
            return;

        this.renderMap(this._mapData(data));
    },

    _mapData: function (data) {
        if (!data)
            return;
        var bubbleData = [];

        for (var i = 0, len = data.length; i < len; i++) {
            bubbleData.push({
                id: this._mapRegion(data[i].country_id),
                value: data[i].Avg,
                temperature: this._getTemperature(data[i].Avg)
            });
        }
        return bubbleData;
    },

    _mapRegion: function (id) {
        // 17		--Australia
        // 32		--Brazil
        // 39		--Canada
        // 49		--China
        // 57		--Germany
        // 68		--Spain
        // 75		--France
        // 77		--United Kingdom
        // 103		--India
        // 111		--Japan
        // 184		--Russian Federation
        // 191		--Singapore
        // 204		--Swaziland
        // 223		--United States
        // 238		--South Africa
        switch (id) {
            case 17: return 'AU';
            case 32: return 'BR';
            case 39: return 'CA';
            case 49: return 'CN';
            case 57: return 'DE';
            case 68: return 'ES';
            case 75: return 'FR';
            case 77: return 'UK';
            case 103: return 'IN';
            case 111: return 'JP';
            case 184: return 'RU';
            case 191: return 'SG';
            case 204: return 'SZ';
            case 223: return 'US';
            case 238: return 'ZA';
            default:
                return;
        }
    },

    _getTemperature: function (num) {
        if (num <= 5000)
            return '#60E89E';

        if (num <= 7000)
            return '#DBC566';

        return '#E8676C';
    }
});


// Declare this class as a jQuery plugin
$.plugin('dj_StatsMap', DJ.UI.StatsMap);
