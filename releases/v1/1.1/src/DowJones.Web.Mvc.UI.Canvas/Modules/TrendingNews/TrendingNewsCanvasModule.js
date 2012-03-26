/*!
* TrendingNewsCanvasModule
*   e.g. , "this._imageSize" is generated automatically.
*
*   
*  Getters and Setters are generated automatically for every Client Property during init;
*   e.g. if you have a Client Property called "imageSize" on server side code
*        get_imageSize() and set_imageSize() will be generated during init.
*  
*  These can be overriden by defining your own implementation in the script. 
*  You'd normally override the base implementation if you have extra logic in your getter/setter 
*  such as calling another function or validating some params.
* 
*/

(function ($) {

    DJ.UI.TrendingNewsCanvasModule = DJ.UI.AbstractCanvasModule.extend({

        selectors:{
            allTrendingEntities: "h4.industry-item-title > a"

        },

        events:{
            trendingEntityClickEvent:"trendingEntityClickEvent.dj.TrendingNewsCanvasModule"      
        },
        init: function (element, meta) {
            var $meta = $.extend({ name: "TrendingNewsCanvasModule" }, meta);

            // Call the base constructor
            this._super(element, $meta);
            var me = this;

            me.topFive = $(".top-five-mentioned ol", me.$element);
            me.trendingUp = $(".trending-up ul", me.$element);   //Enum = 1
            me.trendingDown = $(".trending-down ul", me.$element);   //Enum=2
            me.allTrendingElements = $(me.selectors.allTrendingElements,me.$element);

            me.scopeSelectors = $(".scope .dj_btn", me.$element);

            /*initialize timePeriod*/
            me.scopeSelectors
                .addClass("no-bg")
                .each(function () {
                    var scopeItem = this,
                        scopeItemID = $(scopeItem).attr('id');
                    
                    if ($(scopeItem).parent().hasClass('trending-timePeriod')) {

                        scopeItemID = scopeItemID.slice(-1); //extract the number out of the ID ('trendingTP-#' - 'trendingTP-')
                        if (scopeItemID == me.options.timePeriod) {
                            $(scopeItem).removeClass("no-bg");
                        }

                    } else if ($(scopeItem).parent().hasClass('trending-entityType')) {

                        scopeItemID = scopeItemID.slice(-1); //extract the number out of the ID ('trendingENT-#' - 'trendingENT-')
                        if (scopeItemID == me.options.timePeriod) {
                            $(scopeItem).removeClass("no-bg");
                        }

                    }
                })
                .live('click', function () {
                    var scopeItemID = $(this).attr('id');

                    if ($(this).hasClass('no-bg') && $(this).parent().hasClass('trending-timePeriod')) {
                        scopeItemID = scopeItemID.slice(-1); //extract the number out of the ID ('trendingTP-#' - 'trendingTP-')
                        me._onTimePeriod(this, scopeItemID);
                    } else if ($(this).hasClass('no-bg') && $(this).parent().hasClass('trending-entityType')) {
                        scopeItemID = scopeItemID.slice(-1); //extract the number out of the ID ('trendingENT-#' - 'trendingENT-')
                        me._onEntityType(this, scopeItemID);
                    }

                    return false;
                });
        },
       

        /*
        * Public methods
        */

        // TODO: Public Methods here

        getData: function () {
            this._super();
            $dj.proxy.invoke({
                
                url:this.options.dataServiceUrl,
                queryParams: {
                    "pageID": this._canvas.get_canvasId(),
                    "moduleID": this.get_moduleId(),
                    "parts": "topEntities|TrendingUp|trendingDown",
                    "timeFrame": this._getTimeFrame(this.options.timePeriod),
                    "entityType": this._getEntityType(this.options.entityType)
                },
                controlData: this._canvas.get_ControlData(),
                preferences: this._canvas.get_Preferences(),
                onSuccess: this._delegates.OnServiceCallSuccess,
                onError: this._delegates.OnServiceCallError

            });
        },

        fireOnSaveAndCloseEditArea: function (e) {
            var editorProps = this._editor.buildProperties();
            this._publish('swap', editorProps);
        },

        /*
        * Private methods
        */

        _initializeDelegates: function() {
            this._super();
            $.extend(this._delegates, {
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this, this.showErrorMessage),
                OnTrendingEntityClick:$dj.delegate(this,this._onTrendingItemClick)
            });
            
        },


        _onTrendingItemClick:function(event,data){
            $dj.debug("publising OnTrendingItemClick");
            var searchContext = event.target.getAttribute("searchContext"),
            title= event.target.innerHTML,  
            $currentVolume= $(event.target).parent().siblings('span.news-volume-current'),//this is not available for top 5 Mentioned
            currentVolumeCount ;
            if($currentVolume.size()>0){
               currentVolumeCount = parseInt($currentVolume.html(),10);
            }
            this._publish(this.events.trendingEntityClickEvent,{"searchContext":searchContext,"title":title,"target":event.target, "modulePart": $(event.target).closest("div").find("h3").text() + ' - ' +title,"headlineCount":currentVolumeCount });
        },

        _onTimePeriod: function (e, id) {
            this.options.timePeriod = id;
            $(".trending-timePeriod .dj_btn", this.$element).addClass("no-bg");
            $(e).removeClass("no-bg");
            this.getData();
        },

        _onEntityType: function (e, id) {
            this.options.entityType = id;
            $(".trending-entityType .dj_btn", this.$element).addClass("no-bg");
            $(e).removeClass("no-bg");
            this.getData();
        },


        _getTimeFrame: function (timePeriod) {
            var timeFrame;
            switch (timePeriod) {
                case "0":
                    timeFrame = 'lastweek';
                    break;
                case "1":
                    timeFrame = 'lastmonth';
                    break;
                case "2":
                    timeFrame = 'threemonths';
                    break;
                default:
                    timeFrame = 'lastweek';
                    break;
            }
            return timeFrame

        },

        _getEntityType: function (eType) {
            var entityType;
            switch (eType) {
                case "0":
                    entityType = "companies";
                    break;
                case "1":
                    entityType = "people";
                    break;
                case "2":
                    entityType = "subjects";
                    break;
                default:
                    entityType = "companies";
                    break;
            }

            return entityType;

        },

        _bindOnItemClick: function (el) {        
        
        $(el).bind('click',this._delegates.OnTrendingEntityClick);
        
        },

        _onSuccess: function (data) {

          var errors = $dj.getError(data);
          if(errors){
           this.showErrorMessage(errors);
          }
          else {

          this._initializeModuleParts();
          
          if (data && data.partResults && data.partResults.length > 0) {
                
                for (var i = 0; i < data.partResults.length; i++) {
                                                     
                    if (data.partResults[i].package && data.partResults[i].packageType) {

                        var trendType = data.partResults[i].packageType;
                        error = $dj.getError(data.partResults[i]);
                        switch (trendType) {

                          /*Is there a neater way to add searchContext - to discuss with Jess/Hrusi*/
                            case "TrendingTopEntitiesPackage":
                               
                                if(error) {
                                    this._showModulePartError(error,this.topFive);                                   
                                }
                                else{ 
                                    var entities = data.partResults[i].package.topNewsVolumeEntities;
                                    _.each(entities, function (entity, key) {
                                        this.append(
                                     '<li><h4 class="industry-item-title"><a href="javascript:void(0)" class="popup-trigger" searchContext=' +entity.searchContextRef +'>' + entity.descriptor +
                                     '</a></h4><p><span class="item-total">' + entity.currentTimeFrameNewsVolume.displayText.value + '</span> <span class="item-total-type">'+"<%=Token("articlesLabel")%>" +'</span></p></li>'
                                     )
                                    }, this.topFive);
                                }                                      
                                break;
                            case "TrendingUpPackage": //TODO get the HTML as a template

                                if(error){
                                    this._showModulePartError(error,this.trendingUp);
                                }
                                else{
                                     var entities = data.partResults[i].package.trendingEntities;
                                     _.each(entities, function (entity, key) {
                                    var percentOrNew ;
                                    if(entity.percentVolumeChange)
                                    {
                                        percentOrNew =  entity.percentVolumeChange.displayText.value + "<%=Token("vol")%>" ;
                                    }
                                    else
                                    {
                                        percentOrNew  = "<%=Token("newEntry")%>" ;
                                    }
                                    
                                    this.append(
                                    '<li><h4 class="industry-item-title"><a href="javascript:void(0)" class="popup-trigger" searchContext=' + entity.searchContextRef +'>' + entity.descriptor + '</a> </h4>' +
                                    '<p><span class="news-volume-previous">' + entity.previousNewsVolume.displayText.value + '</span><span class="fi fi_arrow-green-small"></span>' +
                                    '<span class="news-volume-current">' + entity.currentTimeFrameNewsVolume.displayText.value + '</span><span class="news-volume-percentage increase">' + percentOrNew +'</span></p></li>');
                                }, this.trendingUp);
                                }
                                break;
                            case "TrendingDownPackage":
                                                               
                                if(error) {
                                    this._showModulePartError(error,this.trendingDown);
                                }
                                else{
                                     var entities = data.partResults[i].package.trendingEntities;
                                      _.each(entities, function (entity, key) {
                                    this.append(
                                    '<li><h4 class="industry-item-title"><a href="javascript:void(0)" class="popup-trigger" searchContext=' + entity.searchContextRef +'>' + entity.descriptor +
                                    '</a></h4><p><span class="news-volume-previous">' + entity.previousNewsVolume.displayText.value + '</span><span class="fi fi_arrow-red-small"></span>' +
                                    '<span class="news-volume-current">' + entity.currentTimeFrameNewsVolume.displayText.value + '</span><span class="news-volume-percentage decrease">' +
                                        entity.percentVolumeChange.displayText.value + "<%=Token("vol")%>" + '</span></p></li>');
                                     }, this.trendingDown);
                                }
                               
                                
                                break;

                            default:
                                this.showErrorMessage(data);

                        }
                        
                    }
                    
                }
                var entities = $(this.selectors.allTrendingEntities,this.$element);
                this._bindOnItemClick(entities);
                this.showContentArea();
            }
            
            else {
                this.showErrorMessage(data);
            }
          }           
        },

        _initializeModuleParts:function(){

             this.topFive.html("");
             this.trendingUp.html("");
              this.trendingDown.html("");
        },
        
        _showModulePartError:function(error,$modulePart){

            var formattedError= $dj.formatError(error);
            $modulePart.html(formattedError);
        },


        EOF: null


    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_TrendingNewsCanvasModule', DJ.UI.TrendingNewsCanvasModule);

    $dj.debug('Registered DJ.UI.TrendingNewsCanvasModule as dj_TrendingNewsCanvasModule');

})(jQuery);

//Should this code go here ?? Debatable- Ram

$(document).ready(function () {
    $('div.dj_Canvas').keyup(function (e) {

        if (e.keyCode == 27) {
            $dj.hide();
        }
    });
});


//Here is the subscription code$dj.debugEnabled=true;
/*

var canvas = $("#FactivaCanvas-0").findComponent(DJ.UI.AbstractCanvas);
canvas.subscribe(canvas.events.trendingEntityClickEvent,function(sender,args){
console.log("subscribe:" + args.innerData.innerHTML);
});

*/
