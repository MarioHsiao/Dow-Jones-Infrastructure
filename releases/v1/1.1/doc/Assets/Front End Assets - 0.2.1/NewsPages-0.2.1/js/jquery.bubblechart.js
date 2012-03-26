/*
 * jQuery bubbleChart v.0.4
 * 
 * Author: Philippe Arcand
 * Copyright 2010 Dow Jones & Company Inc.
 *
 * Depends:
 *   jquery.ui.core.js
 *   jquery.ui.widget.js
 *
 * Require:
 *	 Highcharts Framework
 */
(function( $, undefined ) {
	var popupVisible = false;
	 
	$.widget("ui.bubbleChart", {
		options: {
				height: 300,
				width: 320,
				textColor:'#FFF',
				variationPercentageTextColor:'#aaa',
				circleFillColor:'rgba(113,187,219,0.8)',
				circleStrokeColor:'rgba(113,187,219,0.8)',
				circleStrokeWidth:1,
				circleMaxRadius: 50,
				circleMinRadius: 20,
				textSize: 12,
				textMaxSize: 30,
				textMinSize: 1,
				highestValue: 0,
				onBubbleSelect: function(){},
				items:[]
		},
		
		bubbleChartRenderer: '',
		bubbleGroups: new Object,
	
		_init: function(){
			var self = this,
			el = self.element;
			o = self.options;
			
			self.bubbleGroups = new Object;
			
			el.addClass('dj-bubblechart');
			
			self.bubbleChartRenderer = new Highcharts.Renderer($(el)[0], o.width, o.height);
			
			self.render();
		},
		
		destroy: function() {
			$.Widget.prototype.destroy.apply(this, arguments);
   		},
		
		render: function(){
			var self = this,
			el = self.element;
			o = self.options;
			
			self.highestValue = 0;
			for (var i in o.items){
				if(o.items[i].size>self.highestValue){
					self.highestValue = o.items[i].size;
				}
			}
			
			o.items = o.items.sort(function(a,b){return stableSort(a,b,'size')}).reverse();
						
			for (var i in o.items){
				var circleRadius =  Math.ceil((o.items[i].size*o.circleMaxRadius)/self.highestValue);
				if(circleRadius < o.circleMinRadius){
					circleRadius = o.circleMinRadius;
				}
				
				var posTaken = true;
				var loop = 0
				while(posTaken == true || loop <10){
					var posX = rand(circleRadius, (o.width)-circleRadius);
					var posY = rand(circleRadius, (o.height)-circleRadius);
					
					posTaken = self.isPositionTaken(posX, posY);
					loop++;
				}				
				
				o.items[i].posX = posX;
				o.items[i].posY = posY;
				o.items[i].radius = circleRadius;
				
				self.addBubble(o.items[i].id, o.items[i].name, posX, posY, circleRadius);
			}
		},
		
		isPositionTaken: function(posX, posY){
			posTaken = false;
			for (var i in o.items){
				itmLeft = o.items[i].posX-o.items[i].radius;
				itmRight = o.items[i].posX+o.items[i].radius;
				itmTop = o.items[i].posY-o.items[i].radius;
				itmBottom = o.items[i].posY+o.items[i].radius;
				if((posX >itmLeft && posX<itmRight) && (posY >itmTop && posY<itmBottom)){
					posTaken = true;
				}
			}
			return posTaken;
		},
		
		addBubble: function(id, name, posX, posY, circleRadius){
			var self = this,
			el = self.element;
			o = self.options;
			
			var formattedName = '';
			var words = name.split(" ");
			var lines = new Array();
			
			var currentLine = 0;
			lines[0] = '';
			
			for(i=0;i<words.length;i++){
				lines[currentLine] += words[i];
				//formattedName += words[i];
				if(words[i].length>3 && i+1<words.length){
					currentLine++;
					lines[currentLine] = '';
				}
				else if(i+1<words.length){
					lines[currentLine]+=	' ';
				}
			}
						
			self.bubbleGroups[id] = self.bubbleChartRenderer.g().attr({cursor: 'pointer'}).css({'cursor' : 'pointer'}).on('click', function(e) {
																o.onBubbleSelect(e);
																for (var i in self.bubbleGroups){
																	self.changeElementOpacity(self.bubbleGroups[i].element, .3);
																}
																self.changeElementOpacity(self.bubbleGroups[id].element, 1);
																
																if(!$.browser.msie) {
																	self.bubbleGroups[id].toFront();
																}
																
																var css  = {width:1, height:1, position:'absolute', top:posY, left:posX};
																var dot = $('<div></div>').css(css);
																
																$(el).append(dot)
																
																$(dot).popup({
																	title  : name,
																	body   : '',
																	state  : 'dj-loading',
																	width  : 300,
																	height : 200,
																	open   : function(panel) {
																		popupVisible = true;
																		for (var i in self.bubbleGroups){
																			self.changeElementOpacity(self.bubbleGroups[i].element, .3);
																		}
																		self.changeElementOpacity(self.bubbleGroups[id].element, 1);
																		if(!$.browser.msie) {
																			self.bubbleGroups[id].toFront();
																		}
																		
																		setTimeout(function() {
																			panel.update({
																				body  : 'content body returned by the server',
																				state : 'dj-loaded'
																			});
																		}, 1000);
																	},
																	close : function() {
																		$(dot).remove();
																		popupVisible = false;
																		for (var i in self.bubbleGroups){
																			self.changeElementOpacity(self.bubbleGroups[i].element, 1);
																		}
																	}
																});
																
																preventEventPropagation(e);
															}).on('mouseover', function(e) {
																if(popupVisible == false){
																	for (var i in self.bubbleGroups){
																		self.changeElementOpacity(self.bubbleGroups[i].element, .3);
																	}
																	self.changeElementOpacity(self.bubbleGroups[id].element, 1);
																	if(!$.browser.msie) {
																		self.bubbleGroups[id].toFront();
																	}
																}
																preventEventPropagation(e);
															}).on('mouseout', function(e) {
																if(popupVisible == false){
																	for (var i in self.bubbleGroups){
																		self.changeElementOpacity(self.bubbleGroups[i].element, 1);
																	}
																}
																preventEventPropagation(e);
															}).add();

			self.bubbleChartRenderer.circle(posX, posY, circleRadius).attr({
																fill: o.circleFillColor
																
															}).add(self.bubbleGroups[id]);
					
			var textInitialY = posY-((lines.length*o.textSize)/2)+o.textSize;
			
			for(i=0;i<lines.length;i++){
				var shadow = self.bubbleChartRenderer.text(lines[i], posX, textInitialY+(i*o.textSize)).css({color: "#1f7599", 'font-size':o.textSize, 'line-height': o.textSize}).add(self.bubbleGroups[id]);
				var text = self.bubbleChartRenderer.text(lines[i], posX, textInitialY+(i*o.textSize)-1).css({color: o.textColor, 'font-size':o.textSize, 'line-height': o.textSize}).add(self.bubbleGroups[id]);
				var box = text.getBBox();
				text.attr({x:(box.x-(box.width/2))})
				var shadowBox = shadow.getBBox();
				shadow.attr({x:(shadowBox.x-(shadowBox.width/2))})
			}
					
		},
		
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
		},
		
		showDetailsWindow: function(posX,posY, name){
			var self = this,
			el = self.element;
			o = self.options;
			
			$(el).find(".dj-details-window .title").text(name);
			$(el).find(".dj-details-window").addClass("visible");
			$(el).find(".dj-details-window").css("margin-left",(posX-$(el).find(".dj-details-window").width())-$(el).find(".dj-details-window .outer-arrow-right").width()+'px');
			$(el).find(".dj-details-window").css("margin-top",(posY-($(el).find(".dj-details-window").height()/2))+'px');
			$(el).find(".dj-details-window .outer-arrow-right").css("margin-left", $(el).find(".dj-details-window").width()+'px');
			$(el).find(".dj-details-window .outer-arrow-right").css("margin-top", ($(el).find(".dj-details-window").height()/2)-($(el).find(".dj-details-window .outer-arrow-right").height()/2)+'px');
		},
		
		hideDetailsWindow: function(){
			var self = this,
			el = self.element;
			o = self.options;
			
			$(el).find(".dj-details-window").removeClass("visible");
			
			if(!$(el).find(".dj-details-window").hasClass("visible")){
				for (var i in self.bubbleGroups){
					self.changeElementOpacity(self.bubbleGroups[i].element, 1);	
				}
			}
		}

	});
})( jQuery );

function preventEventPropagation(e){
	if (e && e.stopPropagation) //if stopPropagation method supported
		e.stopPropagation()
	else
		event.cancelBubble=true
}	

function stableSort(a,b,attributename){
	if ($(a).attr(attributename) < $(b).attr(attributename)) return -1;  
  	if ($(a).attr(attributename) > $(b).attr(attributename)) return 1;  
	if($(a).attr(attributename) == $(b).attr(attributename)){
		return $(a).index() > $(b).index() ? 1 : -1; 
	}
}	

function rand(min, max){		
	var randomNum = Math.random() * (max-min); 

	// Round to the closest integer and return it
	return(Math.round(randomNum) + min); 
}			