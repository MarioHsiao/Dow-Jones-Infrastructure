/*
 * jQuery regionalMap
 * 
 * Author: Philippe Arcand
 * Copyright 2010 Dow Jones & Company Inc.
 *
 * Depends:
 *   jquery.ui.core.js
 *   jquery.ui.widget.js
 *	 popup.js
 *
 * Require:
 *	 Highcharts Framework
 */
(function( $, undefined ) {
    var popupVisible = false;
    
	$.widget("ui.regionalMap", {
		options: {
				articleVolumeTextColor:'#FFF', // Article Volume Text Color
				variationPercentageTextColor:'#aaa', // Variation Percentage Text Color
				circleFillColor:'rgba(0,0,0,.5)', // Circle Fill Color
				circleStrokeColor:'rgba(0,0,0,.6)', // Circle Stroke Color
				circleStrokeWidth:1, // Circle Stroke Width (Value in Pixels)
				circleMaxRadius: 70, // The maximum radius that a circle can have
				circleMinRadius: 5, // The minimum radius that a circle can have
				textMaxSize: 30, // Text Max Size (Value in Px)
				textMinSize: 1, // Text Min Size (Value in Px)
				showSidebar:true, // Show Filter Sidebar (true/false)
				showTextLabels:true, //Show Text Labels (true/false)
				width:988, // Plugin Width
				height:430, // Plugin Height
				
				totalArticleVolume:0, // Total Article Volume (the sum of all regions)
				onRegionSelect: function(){}, // CallBack function. Called when the user clicks on a circle.
				onFilterChange: function(){}, // CallBack function. Called when the user check/uncheck a filter.
				
				filters: {}, // Filter List
				regions:{
					na:{
						id: "na",
						name: "North America",
						posX: 21.5,
						posY: 32.55,
						articleVolume: 0,
						variationPercentage: 0
						},
					ca:{
						id: "ca",
						name: "Central America",
						posX: 21.5,
						posY: 52.27,
						articleVolume: 0,
						variationPercentage: 0
						},
					sa:{
						id: "sa",
						name: "South America",
						posX: 31.5,
						posY: 75.09,
						articleVolume: 0,
						variationPercentage: 0
						},
					eur:{
						id: "eur",
						name: "Europe",
						posX: 50.25,
						posY: 32.55,
						articleVolume: 0,
						variationPercentage: 0
						},
					me: {
						id: "me",
						name: "Middle-East",
						posX: 57.75,
						posY: 44.186,
						articleVolume: 0,
						variationPercentage: 0
					},
					asi:{
						id: "asi",
						name: "Asia",
						posX: 80.25,
						posY: 34.88,
						articleVolume: 0,
						variationPercentage: 0
					},
					aus:{
						id: "aus",
						name: "Australia",
						posX: 87.125,
						posY: 81.39,
						articleVolume: 0,
						variationPercentage: 0
					}
				}
		},
		mapImgWidth: 800, // Map Image Width
		mapImgHeight: 430, // Map Image Height
		worldMapRenderer: '', // Highcharts renderer object
		regionCircles: new Object, // Highcharts groups
		highestValue: 0, // The Highest ArticleVolume value among the regions
		
		/**
		 * Plugin Initialization. (Executed automatically)
		 */
		_init: function(){
			var self = this,
			el = self.element;
			o = self.options;
			
			el.addClass('dj-regionalmap');
			el.css('width',o.width+'px');
			el.css('height',o.height+'px');
			
			self.worldMapRenderer = new Highcharts.Renderer($(el)[0], o.width, o.height);
			
			// CALCULATE THE MAP IMAGE SIZE
			self.mapWidth = o.width;
			self.mapHeight = o.height;
			self.mapX = o.width-self.mapImgWidth;
			self.mapY = o.height-self.mapImgHeight;
			if(o.width>self.mapImgWidth){
				self.mapWidth = self.mapImgWidth;
			}
			if(o.height>self.mapImgHeight){
				self.mapHeight = self.mapImgHeight;
			}
			if(self.mapX<0){
				self.mapX = 0;	
			}
			if(self.mapY<0){
				self.mapY = 0;	
			}
			self.worldMapRenderer.image('images/world_map.gif', self.mapX, self.mapY, self.mapWidth, self.mapHeight).add();
			
			el.find('img').attr("width",self.mapWidth);
			el.find('img').attr("height",self.mapHeight).css("-ms-interpolation-mode","bicubic");

			// FILTERS SIDEBAR CODE
			var sidebarClass = 'dj-regionalmap-sidebar';
			if(o.showSidebar == false) sidebarClass+=' hidden';
			
			el.prepend('<div class="'+sidebarClass+'"><div class="title">Article Topics</div><div class="dj-regionalmap-filters"></div></div>');
			
			$(el).find('.dj-regionalmap-filters').append('<div class="filter default"><label><input type="checkbox" articleVolume="'+o.totalArticleVolume+'" checked="true" disabled="true"> All News <span class="volume">'+o.totalArticleVolume+' articles</span></label></div><div class="separator"><!-- (IE 6.0 FIX) --></div>');

			for (var i in o.filters){
				self.addFilter(o.filters[i].name, o.filters[i].articleVolume);
			}
			
			$(el).find('.dj-regionalmap-filters .filter.custom label').disableSelection().bind("change", function(e){
				o.onFilterChange(e);
				
				$(el).find('.dj-regionalmap-filters .filter.default input').attr("checked", true);
				$(el).find('.dj-regionalmap-filters .filter.custom input').each(function(){
					if($(this).attr("checked") == true){
						$(el).find('.dj-regionalmap-filters .filter.default input').attr("checked", false);	
					}
				});
				e.stopPropagation();
			});
			
			
			self.renderMap();
		},
		
		/**
		 * Render the map
		 */
		renderMap: function(){
			var self = this,
			el = self.element;
			o = self.options;
			
			
			for (var i in self.regionCircles){
				self.regionCircles[i].destroy();
			}
			
			self.highestValue = 0;
			for (var i in o.regions){
				if(o.regions[i].articleVolume>self.highestValue){
					self.highestValue = o.regions[i].articleVolume;
				}
			}
			
			for (var i in o.regions){
				self.addRegion(i, o.regions[i].name, o.regions[i].posX, o.regions[i].posY, o.regions[i].articleVolume, o.regions[i].variationPercentage);
			}
			for (var i in self.regionCircles){
				self.changeElementOpacity(self.regionCircles[i].element, 1);
			}
		},
		
		/**
		 * Add a filter to the sidebar
		 *
		 * @param	name					The filter name
		 * @param	articleVolume			The article volume associated with that filter
		 */
		addFilter: function(name, articleVolume){
			var self = this,
			el = self.element;
			o = self.options;
			
			$(el).find('.dj-regionalmap-filters').append('<div class="filter custom"><label><input type="checkbox" articleVolume="'+articleVolume+'"/> '+name+' <span class="volume">'+articleVolume+' articles</span></label></div>');
		},
		
		/**
		 * Draw circles on the map and add related DOM events
		 *
		 * @param	id						The region unique identifier
		 * @param	name					The text that will be displayed on top of the region circle
		 * @param	posX					The horizontal position of the circle
		 * @param	posY					The vertical position of the circle
		 * @param	articleVolume			The article volume of the region
		 * @param	variationPercentage		The variation percentage
		 */
		addRegion: function(id, name, posX, posY, articleVolume, variationPercentage){
			var self = this,
			el = self.element;
			o = self.options;
			
			posX = self.mapX+Math.ceil((posX*self.mapWidth)/100);
			posY = self.mapY+Math.ceil((posY*self.mapHeight)/100);	
			
			var circleRadius =  ((articleVolume*o.circleMaxRadius)/self.highestValue);
			if(circleRadius < o.circleMinRadius){
				circleRadius = o.circleMinRadius;
			}
			
			var smallTextSize = (((articleVolume*o.textMaxSize)/self.highestValue)/2.5);
				if(smallTextSize < o.textMinSize){
				smallTextSize = o.textMinSize;
			}
			
			var largeTextSize = (((articleVolume*o.textMaxSize)/self.highestValue));
				if(largeTextSize < o.textMinSize){
				largeTextSize = o.textMinSize;
			}
			
			// Create a group (Highchart function)
			self.regionCircles[id] = self.worldMapRenderer.g().attr({cursor: 'pointer'}).css({'text-shadow' : '0px 1px 0px #000', 'cursor' : 'pointer'}).on('click', function(e) {
                // ON CLICK
				o.onRegionSelect(e);
                for (var i in self.regionCircles){
                    self.changeElementOpacity(self.regionCircles[i].element, .3);
                }
                self.changeElementOpacity(self.regionCircles[id].element, 1);
                
                if(!$.browser.msie) {
                    self.regionCircles[id].toFront();
                }
                
                var css  = {width:1, height:1, position:'absolute', top:posY, left:posX},
                    $dot = $('<div></div>').css(css);
                
                el.append($dot);
                
				// POPUP CODE
                $dot.popup({
                    title  : name,
                    body   : '',
                    state  : 'dj-loading',
                    width  : 300,
                    height : 200,
                    open   : function(panel) {
                        popupVisible = true;
                        for (var i in self.regionCircles){
                            self.changeElementOpacity(self.regionCircles[i].element, .3);
                        }
                        self.changeElementOpacity(self.regionCircles[id].element, 1);
                        if(!$.browser.msie) {
                            self.regionCircles[id].toFront();
                        }
                        
                        setTimeout(function() {
                            panel.update({
                                body  : 'content body returned by the server',
                                state : 'dj-loaded'
                            });
                        }, 1000);
                    },
                    close : function() {
                        $dot.remove();
                        popupVisible = false;
                        for (var i in self.regionCircles){
                            self.changeElementOpacity(self.regionCircles[i].element, 1);
                        }
                    }
                });
                
                preventEventPropagation(e);
            }).on('mouseover', function(e) {
				// ON MOUSE OVER
                if(popupVisible == false){
                    for (var i in self.regionCircles){
                        self.changeElementOpacity(self.regionCircles[i].element, .3);
                    }
                    self.changeElementOpacity(self.regionCircles[id].element, 1);
                    if(!$.browser.msie) {
                        self.regionCircles[id].toFront();
                    }
                }
                preventEventPropagation(e);
            }).on('mouseout', function(e) {
				// ON MOUSE OUT
                if(popupVisible == false){
                    for (var i in self.regionCircles){
                        self.changeElementOpacity(self.regionCircles[i].element, 1);
                    }
                }
                preventEventPropagation(e);
            }).add();
			
			// Draw a circle and add it to the previously created Highchart Group
			self.worldMapRenderer.circle(posX, posY, circleRadius).attr({
																fill: o.circleFillColor,
																stroke: o.circleStrokeColor,
																'stroke-width':  o.circleStrokeWidth
															}).add(self.regionCircles[id]);
			
			// Draw the text object and add it to the previously created Highchart Group
			if(o.showTextLabels == true){
				// ARTICLE VOLUME
				var avText = self.worldMapRenderer.text(articleVolume, 100, 100).css({
					color: o.articleVolumeTextColor,
					fontSize: largeTextSize+'px'
				}).add(self.regionCircles[id]);	
				// "ARTICLES" LABEL
				var arText = self.worldMapRenderer.text('ARTICLES', 100, 100).css({
					color: o.articleVolumeTextColor,
					fontSize: smallTextSize+'px'
				}).add(self.regionCircles[id]);	
				// SLASH SIGN (/)
				var slashText = self.worldMapRenderer.text('/', 100, 100).css({
					color: o.articleVolumeTextColor,
					fontSize: largeTextSize+'px'
				}).add(self.regionCircles[id]);	
				// VARIATION PERENTAGE (x+%)		
				var vpText = self.worldMapRenderer.text('+'+variationPercentage+'%', 100, 100).css({
					color: o.variationPercentageTextColor,
					fontSize: smallTextSize+'px'
				}).add(self.regionCircles[id]);	
				// "CHANGE" LABEL
				var chText = self.worldMapRenderer.text('CHANGE', 100, 100).css({
					color: o.variationPercentageTextColor,
					fontSize: smallTextSize+'px'
				}).add(self.regionCircles[id]);	
				
				var arTextBox = arText.getBBox();	
				var avTextBox = avText.getBBox();
				var slashTextBox = slashText.getBBox();
				var vpTextTextBox = vpText.getBBox();
				
				// SET THE TEXT POSITION
				arText.attr({x:(posX-arTextBox.width), y: posY});
				avText.attr({x:(posX-avTextBox.width), y: (posY-arTextBox.height)});
				slashText.attr({x:(posX), y: (posY)});
				vpText.attr({x:(posX+slashTextBox.width), y: (posY)});
				chText.attr({x:(posX+slashTextBox.width), y: (posY+vpTextTextBox.height)});
			}
			
		},
		
		/**
		*  Changed Element Opacity
		*
		*  @param	elem		DOM element
		*  @param	opacity		Decimal value (0 to 1)
		*/
		changeElementOpacity: function(elem, opacity){
			if(!$.browser.msie) {
				$(elem).css({'opacity':opacity});
			}
			else{
				if(opacity!=1){
					$(elem).find("span").css({'opacity':0.4});
					$(elem).find("fill").attr('opacity', 0.1);
					$(elem).find("stroke").attr('opacity', 0.2);
				}
				else{
					$(elem).find("span").css({'opacity':1});
					$(elem).find("fill").attr('opacity', 0.5);
					$(elem).find("fill").attr('opacity', 0.6);
				}
				
			}	
		}
	});
})( jQuery );

/**
 *	Prevent Event Propagation (aka Event Bubbling)
 *
 *	@param	e	Event Object
 */
function preventEventPropagation(e){
	if (e && e.stopPropagation) //if stopPropagation method supported
		e.stopPropagation()
	else
		event.cancelBubble=true
}					