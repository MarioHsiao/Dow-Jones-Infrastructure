/// <reference path="..\..\..\..\DowJones.Web.Mvc\Resources\js\jquery.js" />
/// <reference path="..\..\..\..\DowJones.Web.Mvc\Resources\js\common.js" />
/// <reference path="..\..\..\..\DowJones.Web.Mvc\Resources\js\ServiceProxy.js" />
(function ($) {

    DJ.UI.TopNewsCanvasModule = DJ.UI.AbstractCanvasModule.extend({

        selectors: {
            topnewsTitles: 'h3.module-col-title',
            viewAllBtn: 'ul.view-all-btn a.dashboard-control',
            portalHeadlineList: 'div.dj_headlineListContainer'
        },

        events: {
            topNewsViewAllClick: 'topNewsViewAll.dj.TopNewsCanvasModule',
            topNewsHeadlineClick: 'topNewsHeadline.dj.TopNewsCanvasModule'
        },


        init: function (element, meta) {

            var $meta = $.extend({
                name: "TopnewsModule"
            }, meta);

            // Call the base constructor
            this._super(element, $meta);

            // get all control plugins and preserve a reference to avoid subsequent lookups
            // specifying scope + [tag.selector] pattern for ultra fast lookups
            this.topnewsTitles = $(this.selectors.topnewsTitles, this.$element);
            this.portalHeadlineLists = $(this.selectors.portalHeadlineList, this.$element);
            this.viewAllBtns = $(this.selectors.viewAllBtn, this.$element);

            //Build delegates
            $.extend(this._delegates, {
                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
                OnServiceCallError: $dj.delegate(this, this.showErrorMessage),
                OnPortalHeadlineClick: $dj.delegate(this, this._onHeadlineClick)
            });

        },

        //        _initializeDelegates: function () {
        //            this._super();

        //            $.extend(this._delegates, {

        //                OnServiceCallSuccess: $dj.delegate(this, this._onSuccess),
        //                OnServiceCallError: $dj.delegate(this, this.showErrorMessage)
        //            });
        //        },

        getData: function () {
            this._super();
            $dj.proxy.invoke({
                url: this.options.dataServiceUrl,
                queryParams: {
                    "pageid": this._canvas.get_canvasId(),
                    "moduleid": this.get_moduleId(),
                    "firstResultToReturn": 0,
                    "maxResultsToReturn": this.options.maxHeadlinesToReturn,
                    "timeFrame": 'LastMonth',
                    "parts": 'EditorsChoice|VideoAndAudio|OpinionAndAnalysis'
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

        _onSuccess: function (data) {

            var errors = $dj.getError(data);
            if (errors) {
                this.showErrorMessage(errors);
            }
            else {

                if (data && data.partResults && data.partResults.length > 0) {
                    var containerIndex = 0;
                    _.each(data.partResults, function (partResult) {

                        // find the position of the Portal Headline List
                        var controlIndex = containerIndex; //parseInt(partResult.id);

                        // get hold of the component by the index
                        // make sure this collection is populated during init
                        var phl = this.portalHeadlineLists[controlIndex];
                        var title = this.topnewsTitles[controlIndex];
                        if (title) {
                            $(title).html(partResult.package.title || partResult.packageType);
                        }
                        if (partResult.returnCode == 0) {
                            

                            var headlines = partResult.package.portalHeadlineListDataResult.resultSet;
                            if (phl) {
                                var comp = $(phl).findComponent(DJ.UI.PortalHeadlineList);
                                comp.bindOnSuccess(headlines);
                                comp.$element.bind(comp.events.headlineClick, this._delegates.OnPortalHeadlineClick);
                            }
                        } else {
                            if (phl) {
                                $(phl).findComponent(DJ.UI.PortalHeadlineList).bindOnError({
                                    'code': partResult.returnCode,
                                    'message': partResult.statusMessage
                                });
                                $(phl).show();
                            }
                        }

                        this._bindViewAllBtns(controlIndex, partResult);
                        containerIndex++;

                    }, this);

                    this.showContentArea();
                } else {
                    this.showErrorMessage(data);
                }
                
            }

           

        },

        _bindViewAllBtns: function (idx, partResult) {
            // wire up the view all button
            var viewAllBtn = this.viewAllBtns[idx];
            if (partResult.returnCode !== 0 ||
                    (partResult.package && partResult.package.portalHeadlineListDataResult &&
                    partResult.package.portalHeadlineListDataResult.resultSet &&
                    partResult.package.portalHeadlineListDataResult.resultSet.count &&
                    partResult.package.portalHeadlineListDataResult.resultSet.count.value < this.options.maxHeadlinesToReturn)
                ) {
                $(viewAllBtn).hide();
                return;
            }

            // wire up the view all button
               $(viewAllBtn).unbind('click').click($dj.delegate(this, function (e) {
                this._publish(this.events.topNewsViewAllClick, {
                    "searchContext": partResult.package.viewAllSearchContext,
                    "modulePart": (partResult.package.title || partResult.packageType)
                });
                e.stopPropagation();
                $dj.debug('Published ' + this.events.topNewsViewAllClick);
                return false;
            })).removeClass('hidden').show();
        },

//        _onError: function (erroData, jqXHR, serverMessage) {
//            _.each(this.viewAllBtns, function (elem) {
//                $(elem).hide();
//            });
//            this.showErrorMessage(erroData);
//        },

        _onHeadlineClick: function (sender, data) {
            this._publish(this.events.topNewsHeadlineClick, data);
            $dj.debug('Published ' + this.events.topNewsHeadlineClick);
        },

        EOF: null

    });

    $.plugin('dj_TopNewsCanvasModule', DJ.UI.TopNewsCanvasModule);

    $dj.debug('Registered DJ.UI.TopNewsCanvasModule as dj_TopNewsCanvasModule');

})(jQuery);
