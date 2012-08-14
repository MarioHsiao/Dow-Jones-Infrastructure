    DJ.UI.RegionalMap = DJ.UI.Component.extend({
        /*
        * Properties
        */
        defaults: {
            debug: false,
            cssClass: 'RegionalMap',
            articleVolumeTextColor: '#FFF', // Article Volume Text Color
            variationPercentageTextColor: '#000', // Variation Percentage Text Color
            circleFillColor: 'rgba(0,0,0,.5)', // Circle Fill Color
            innerCircleFillColor: 'rgba(0,0,0,.7)', // Circle Stroke Color
            circleStrokeWidth: 3, // Circle Stroke Width (Value in Pixels)
            textMaxSize: 30, // Text Max Size (Value in Px)
            textMinSize: 9, // Text Min Size (Value in Px)
            smallVersionTextSize: 7, // The text size that will trigger the small version
            showTooltips: true, //Show Text Labels (true/false)
            tooltipAlign: 'right' //Alignment of tooltip (right/left/center)                       
        },

        mapsize:{
            small:{ 
                width:298, //width of the component 
                height:198,
                circleMaxRadius: 32, //The maximum radius that a circle can have
                circleMinRadius: 7  //The minimum radius that a circle can have
            },
            large:{
                width: 988, 
                height: 530,
                circleMaxRadius: 70, //The maximum radius that a circle can have
                circleMinRadius: 15  //The minimum radius that a circle can have 
            }  
        }, 
        
        events: {
            regionClick: "regionClick.dj.RegionalMap"
        },

        mapConfig: {
            regions:{
                // NN - regions provide coordinates in percentage (posX/Y) for the region circles
                NAMZ:{
                    id: "NAMZ",
                    name: "<%= Token("northAmerica") %>",
                    posX: 20,
                    posY: 30
                },
                CAMZ:{
                    id: "CAMZ",
                    name: "<%= Token("centralAmerica") %>",
                    posX: 21.5,
                    posY: 52.27
                },
                SAMZ:{
                    id: "SAMZ",
                    name: "<%= Token("southAmerica") %>",
                    posX: 31.5,
                    posY: 75.09
                },
                EURZ:{
                    id: "EURZ",
                    name: "<%= Token("europe") %>",
                    posX: 50.25,
                    posY: 32.55
                },
                MEASTZ: {
                    id: "MEASTZ",
                    name: "<%= Token("middleEast") %>",
                    posX: 57.75,
                    posY: 44.186
                },
                ASIAZ:{
                    id: "ASIAZ",
                    name: "<%= Token("asia") %>",
                    posX: 82,
                    posY: 32,
                    tooltipAlign: 'left'
                },
                AUSNZ:{
                    id: "AUSNZ",
                    name: "<%= Token("countryName9Aus") %>",
                    posX: 87.125,
                    posY: 81,
                    tooltipAlign: 'left'
                },
                RUSS:{
                    id: "RUSS",
                    name: "<%= Token("s2regionRussia") %>",
                    posX: 70,
                    posY: 16,
                    tooltipAlign: 'left'
                },
                AFRICAZ:{
                    id: "AFRICAZ",
                    name: "<%= Token("africa") %>",
                    posX: 52,
                    posY: 60
                },
                INDSUBZ:{
                    id: "INDSUBZ",
                    name: "<%= Token("countryName9Ind") %>",
                    posX: 70,
                    posY: 46,
                    tooltipAlign: 'left'
                }
            },
            mapImgWidth: 988, // Original Map Image Width
            mapImgHeight: 530 // Original Map Image Height            
        },
        worldMapRenderer: '', // Highcharts renderer object
        regionCircles: {}, // Highcharts groups
        highestValue: 0, // The Highest ArticleVolume value among the regions
        _mapsize: "large", //local default variable used.
        
        // Localization/Templating tokens
        tokens: {
            articles: "<%= Token("articlesLabel") %>",
            change: "<%= Token("changeLowerCase") %>"
        },

        /*
        * Initialization (constructor)
        */
        init: function (element, meta) {
            var $meta = $.extend({ name: "RegionalMap" }, meta);

            // Call the base constructor
            this._super(element, $meta);
            this.regionCircles = {};

            var el = $(this.element),
                o = this.options,
                mc = this.mapConfig,
                regionID;
            
            //Set the height/width based on size
            this._mapsize         = (o.size == 0)?"large":"small";
            o.width               = this.mapsize[this._mapsize].width;
            o.height              = this.mapsize[this._mapsize].height;
            o.circleMaxRadius     = this.mapsize[this._mapsize].circleMaxRadius;
            o.circleMinRadius     = this.mapsize[this._mapsize].circleMinRadius;

            el.addClass('dj-regionalmap').css({
                width: o.width,
                height: o.height                
            });

            this.worldMapRenderer = new Highcharts.Renderer(el[0], o.width, o.height);            
            
            //NN - Put the world map at the bottom right of the container
            this.mapX = Math.max(o.width - mc.mapImgWidth, 0);
            this.mapY = Math.max(o.height - mc.mapImgHeight, 0);
            
            //NN - World map size should not exceed container's size
            this.mapWidth = Math.min(o.width, mc.mapImgWidth);
            this.mapHeight = Math.min(o.height, mc.mapImgHeight);
            
            //this.worldMapRenderer.image('<%= WebResource("DowJones.Web.Mvc.UI.Components.RegionalMap.images.world_map.gif") %>', this.mapX, this.mapY, this.mapWidth, this.mapHeight).add();
            //$dj.debug(o.imageMapPath);
            //accesssing embedded resource from the model.
            this.worldMapRenderer.image(o.imageMapPath, this.mapX, this.mapY, this.mapWidth, this.mapHeight).add();

            // NN - what is this for?
            el.find('img').css("-ms-interpolation-mode","bicubic");       
            
            this.renderMap();
        },
        
        renderMap: function(){
            var self = this,
                el = $(self.element),
                o = self.options,
                i, regionID;
            
            if (self.data && self.data.regionNewsVolume) {
                for (regionID in self.regionCircles){
                    self.regionCircles[regionID].destroy();
                    delete self.regionCircles[regionID];
                }
            
                self.highestValue = 0;
                
                for (i = 0; i < self.data.regionNewsVolume.length; i++) {
                    var regionData = self.data.regionNewsVolume[i];
                    if(regionData.currentTimeFrameNewsVolume.value > self.highestValue){
                        self.highestValue = regionData.currentTimeFrameNewsVolume.value;
                    }
                }
            
                for (i = 0; i < self.data.regionNewsVolume.length; i++) {
                    try
                    {
                        if(self.mapConfig.regions[self.data.regionNewsVolume[i].code] && 
                            self.data.regionNewsVolume[i].currentTimeFrameNewsVolume.value > 0)
                            self.addRegion(self.data.regionNewsVolume[i]);
                        else
                            $dj.debug("RegionalMapComponent:: "+self.data.regionNewsVolume[i].code+" is not found in regional mapConfig");
                    }
                    catch(e){
                        $dj.debug("RegionalMapComponent::Exception: "+e.message);
                    }
                } 
            }
        },
        
        /**
         * Draw circles on the map and add related DOM events
         *
         * @param    regionData                    Article volume data for the rgion
         */
        addRegion: function(regionData){
            var self = this,
                el = $(self.element),
                o = self.options,
                id = regionData.code,
                regionMapConfig = self.mapConfig.regions[id],
                name = regionMapConfig.name, // The text that will be displayed on top of the region circle
                posX = self.mapX + Math.ceil((regionMapConfig.posX * self.mapWidth) / 100), // The horizontal position of the circle
                posY = self.mapY + Math.ceil((regionMapConfig.posY * self.mapHeight) / 100), // The vertical position of the circle
                articleVolume = regionData.currentTimeFrameNewsVolume.value, // The article volume of the region
                articleVolumeText = self.formatArticleCount(articleVolume), // The article volume of the region display text
                variationPercentage = regionData.percentVolumeChange?regionData.percentVolumeChange.value:0, // The variation percentage
                variationPercentageText = (variationPercentage > 0 ? "+" : "") + (regionData.percentVolumeChange?regionData.percentVolumeChange.displayText.value:""), // The variation percentage display text
                circleRadius, smallTextSize, largeTextSize,
                tooltipAlign = regionMapConfig.tooltipAlign || o.tooltipAlign,
                i;
            
            circleRadius =  ((articleVolume*o.circleMaxRadius)/self.highestValue);
			if(circleRadius < o.circleMinRadius){
				    circleRadius = o.circleMinRadius;
			}

            smallTextSize = (((articleVolume*o.textMaxSize)/self.highestValue)/2.5);
			if(smallTextSize < o.textMinSize){
				smallTextSize = o.textMinSize;
			}
			
			largeTextSize = (((articleVolume*o.textMaxSize)/self.highestValue));
			if(largeTextSize < o.textMinSize){
				largeTextSize = o.textMinSize;
			}

            // Create a group (Highchart function)
            self.regionCircles[id] =
                self.worldMapRenderer
                .g()
                .css({'cursor' : 'pointer'})
                .on('click', function(e) {
//                    if(o.size == 1)//Mini regional map
//                        return;


                    var x = posX + $(self.element).offset().left;
                    var y = posY + $(self.element).offset().top;
                    var offset =- (circleRadius+o.circleStrokeWidth);

                    //In IE "this" poins to window in this handler so we have to find the shape element and pass it as element in the handler parameters
                    el.triggerHandler(self.events.regionClick, { sender: self, searchContext: regionData.searchContextRef , element: ((this == window) ? $(e.target).closest('div').children('shape')[1] : this), title:name, regionCode:id,positionX:x,positionY:y,offset:offset});
                    
                    for (i in self.regionCircles){
                        $(self.regionCircles[i]).data('innerCircle').hide();
                        if(o.showTooltips == true){
                            $(self.regionCircles[i]).data('tooltip').hide();
                        }
                    }
                    $(self.regionCircles[id]).data('innerCircle').show();
                    if(o.showTooltips == true){
                        $(self.regionCircles[id]).data('tooltip').show();
                    }
                    if(!$.browser.msie) {
                        self.regionCircles[id].toFront();
                    }
                                        
                    preventEventPropagation(e);
                })
                .on('mouseover', function(e) {
                    if($dj.isCalloutVisible())
                        return;

                    // ON MOUSE OVER
                    for (i in self.regionCircles){
                        $(self.regionCircles[i]).data('innerCircle').hide();
                        if(o.showTooltips == true){

                            $(self.regionCircles[i]).data('tooltip').hide();
                        }
                    }
                    $(self.regionCircles[id]).data('innerCircle').show();
                    if(o.showTooltips == true){
                        $(self.regionCircles[id]).data('tooltip').show();
                    }
                    if(!$.browser.msie) {
                        self.regionCircles[id].toFront();
                    }
                    
                    preventEventPropagation(e);
                })
                .on('mouseout', function(e) {
                    if($dj.isCalloutVisible())
                        return;
                    // ON MOUSE OUT
                    for (i in self.regionCircles){
                        $(self.regionCircles[i]).data('innerCircle').hide();
                        if(o.showTooltips == true){
                            $(self.regionCircles[i]).data('tooltip').hide();
                        }
                    }
                    preventEventPropagation(e);
                })
                .add();
            
           // Inner circle (Mouse Interaction)										
			var innerCircle = self.worldMapRenderer.circle(posX, posY, Math.ceil(circleRadius)).attr({
				fill: o.innerCircleFillColor
			}).add(self.regionCircles[id]).hide();
			$(self.regionCircles[id]).data('innerCircle',innerCircle);
			
			
			// Draw a circle and add it to the previously created Highchart Group
			var circle = self.worldMapRenderer.circle(posX, posY, Math.ceil(circleRadius+o.circleStrokeWidth)).attr({
																fill: o.circleFillColor
															}).add(self.regionCircles[id]);

            var articleText = articleVolume == 1 ? "<%= Token("articleSingular") %>" : "<%= Token("articlePlural") %>";
                			
			// Tooltip (DEV Note : Unfortunately, the highchart renderer does not support class names, we need to provide inline CSS rules)
			if(o.showTooltips == true){
				var tooltip = self.worldMapRenderer.g().add(self.regionCircles[id]);
				
				var tooltipAvText = self.worldMapRenderer.text(articleVolumeText+' '+articleText,posX,posY).css({
					fontSize: '11px',
					"font-family": 'arial',
					'text-shadow' : 'none',
					color: "#444",
					'font-weight': 'bold'
				}).add(tooltip);
					
                // Calculate tooltip dimensions
				var box1 = tooltipAvText.getBBox();
				
				var bw =  box1.width;
				var bh =  box1.height;

				var by =  box1.y;
				var bx =  box1.x;
				
				if( 'left' == tooltipAlign ) {
							
					tooltipAvText.attr( 'x', box1.x - bw );
					bx -= bw;
				} else if( 'center' == tooltipAlign ){
					tooltipAvText.attr( 'x', box1.x - ( bw/2 ) );
					bx -= (bw/2);
				}
								
				var rectbox = self.worldMapRenderer.rect(bx - 5, by - 5, bw + 10, bh + 10, 5).attr({
					fill: 'rgba(255,255,255,0.8)'
				}).add(tooltip);
				
				tooltipAvText.toFront();
				tooltip.hide();
				$(self.regionCircles[id]).data('tooltip',tooltip);

//				var tooltipVText = self.worldMapRenderer.text((variationPercentage > 0?"+":"")+Math.round(variationPercentage)+' % <%= Token("change") %>',posX,posY+12).css({
//						fontSize: '11px',
//						"font-family": 'arial',
//						'text-shadow' : 'none',
//						color: "#333",
//						'font-weight': ''
//					}).add(tooltip);
//									
//				// Calculate tooltip dimensions
//				var box1 = tooltipAvText.getBBox();
//				var box2 = tooltipVText.getBBox();
//				
//				var bw =  (box1.width > box2.width) ? box1.width : box2.width;
//				var bh =  (box1.height + box2.height);

//				var by =  (box1.y < box2.y) ? box1.y : box2.y;
//				var bx =  (box1.x < box2.x) ? box1.x : box2.x;
//				
//				if( 'left' == tooltipAlign ) {
//							
//					tooltipAvText.attr( 'x', box1.x - bw );
//					tooltipVText.attr( 'x', box2.x - bw );
//					bx -= bw;
//				} else if( 'center' == tooltipAlign ){
//					tooltipAvText.attr( 'x', box1.x - ( bw/2 ) );
//					tooltipVText.attr( 'x', box2.x - ( bw/2 ) );
//					bx -= (bw/2);
//				}
//								
//				var rectbox = self.worldMapRenderer.rect(bx - 5, by - 5, bw + 10, bh + 10, 5).attr({
//					fill: 'rgba(255,255,255,0.8)'
//				}).add(tooltip);
//				
//				tooltipAvText.toFront();
//				tooltipVText.toFront();
//				tooltip.hide();
//				$(self.regionCircles[id]).data('tooltip',tooltip);
			}

			// Draw the text object and add it to the previously created Highchart Group
			if(o.showTextLabels == true){
				
                var avTextShadow = self.worldMapRenderer.text(articleVolumeText, 100, 100).css({
					color: '#333',
					fontSize: largeTextSize+'px'
				}).add(self.regionCircles[id]);

				var avText = self.worldMapRenderer.text(articleVolumeText, 100, 100).css({
					color: o.articleVolumeTextColor,
					fontSize: largeTextSize+'px'
				}).add(self.regionCircles[id]);	

                var arTextShadow = self.worldMapRenderer.text(articleText, 100, 100).css({
					color: '#333',
					fontSize: smallTextSize+'px'
				}).add(self.regionCircles[id]);

				var arText = self.worldMapRenderer.text(articleText, 100, 100).css({
					color: o.articleVolumeTextColor,
					fontSize: smallTextSize+'px'
				}).add(self.regionCircles[id]);	

                // VARIATION PERENTAGE (x+%)	
				var vSymbol = '';
				if(variationPercentage>0){
					vSymbol = '+';
				}				
				var vpText = self.worldMapRenderer.text(vSymbol+Math.round(variationPercentage)+'% <% = Token("newChange") %>', 100, 100).css({
					color: o.variationPercentageTextColor,
					fontSize: '10px',
					fontWeight: 'bold',
                    'text-shadow':'none'
				}).add(self.regionCircles[id]);	
				
				var arTextBox = arText.getBBox();	
				var avTextBox = avText.getBBox();
				var vpTextBox = vpText.getBBox();
				
				// SET THE TEXT POSITION
				arText.attr({x:(posX-(arTextBox.width/2)), y: (posY+arTextBox.height)});
                arTextShadow.attr({x:(posX-(arTextBox.width/2)), y: (posY+arTextBox.height)+1});
				avText.attr({x:(posX-(avTextBox.width/2)), y: (posY)});
                avTextShadow.attr({x:(posX-(avTextBox.width/2)), y: (posY)+1});
				vpText.attr({x:(posX-(vpTextBox.width/2)), y: (posY+circleRadius+o.circleStrokeWidth+vpTextBox.height)});
			}

            var preventEventPropagation = function(e)
            {
                e = e || window.event;
                if(e){
                    if (e.stopPropagation) {
                        e.stopPropagation();
                    }
                    else {
                        e.cancelBubble = true;
                    }
                }
            }
        },    
		
		/**
		*  Add (k) symbol to integer > 10 000
		*
		*  @param	val		Integer
		*/
	 	formatArticleCount: function(val) {
			val = val+''; // coerce to string
			if (val < 1000) {
				return val; // return the same number
			} else if (val < 10000) { // place a comma between
				return val.charAt(0) + ',' + val.substring(1);
			} else { // divide and format
				return Math.floor(val/1000)+'k';
			}
		},     

        /*
        * Public methods
        */

        // TODO: Public Methods here

        setData: function(regionalMapNewsVolumePackage) {
            this.data = regionalMapNewsVolumePackage;
            this.renderMap();
        },

        /*
        * Private methods
        */

        // DEMO: Overriding the base _paint method:
        _paint: function () {
            // "this._super()" is available in all overridden methods
            // and refers to the base method.
            this._super();

            //alert('TODO: implement RegionalMap._paint!');
        },

		redraw: function() {
			this.renderMap();
		}
    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_RegionalMap', DJ.UI.RegionalMap);
