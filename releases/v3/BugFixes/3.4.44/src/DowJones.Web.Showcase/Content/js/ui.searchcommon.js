if (typeof (SearchCommon) == 'undefined')
	SearchCommon = {};

SearchCommon.loadingText = "<%= Token('loading') %>...";

SearchCommon.newsFilterCategory = {
	company: 1,
	executive: 2,
	author: 3,
	industry: 4,
	subject: 5,
	region: 6,
	source: 7,
	dateRange: 8,
	keyword: 9
};

SearchCommon.FolderTypes = {
    news: "m",
    author: "a",
    newAuthor: "n" 
};

SearchCommon.saveAsTopic = function (searchRequest, topicData) {

	var $dvSaveAsTopic = $("#dvSaveAsTopic");
	var stC = $dvSaveAsTopic.find("div.dj_TopicEditor:first").findComponent(DJ.UI.TopicEditor);

	if (!$dvSaveAsTopic.data("eventsBound")) {
		//Save
		$dj.subscribe(stC.events.onSaveClick, function (requestObj) {
			if (!requestObj.isValid) {
				alert(requestObj.error);
				return;
			}

			var searchRequest = requestObj.searchRequest;

			//Remove unwanted properties from the request
			requestObj.searchRequest = undefined;
			requestObj.isValid = undefined;

			//Make ajax call to save the topic using requestObj
			$dj.progressIndicator.display(SearchCommon.loadingText);
			DJGlobal.AJAX({
				httpMethod: "POST",
				data: {
					topicProperties: JSON.stringify(requestObj),
					searchRequest: JSON.stringify(searchRequest),
					kind: "freeText"
				},
				url: DJGlobal.getControllerActionUrl("Topics", requestObj.topicId ? "UpdateTopic" : "CreateTopic"),
				success: function (response) {
					$dj.progressIndicator.hide();
					$().overlay.hide('#dvSaveAsTopic');
					alert(requestObj.topicId ? "<%= Token('topicUpdatedSuccessfully') %>" : "<%= Token('topicCreatedSuccessfully') %>");
				},
				failure: function (response) {
					$dj.progressIndicator.hide();
					DJGlobal.showError(response.Error);
				}
			});
		});
		//Cancel
		$dj.subscribe(stC.events.onCancelClick, function () { $().overlay.hide('#dvSaveAsTopic'); });
		$dvSaveAsTopic.data("eventsBound", true);
	}

	SearchCommon.showTopicModal(searchRequest, topicData);
}

SearchCommon.showTopicModal = function (searchRequest, topicData) {
	searchRequest = searchRequest || {};//SearchRequest should always be there.
	topicData = topicData || {};
	var $dvSaveAsTopic = $("#dvSaveAsTopic")
	var stC = $dvSaveAsTopic.find("div.dj_TopicEditor:first").findComponent(DJ.UI.TopicEditor);

    //Set the modal title
    $("#dvSaveAsTopicTitle").html(topicData.topicId ? "<%= Token('editTopic') %>" : "<%= Token('saveAsTopic') %>");

	//Set the filters here using searchRequest object
	topicData.freeText = searchRequest.FreeText;

	//Channel Filters
	topicData.filters = {
		company: SearchCommon.getUIFilterBySearchFilter(searchRequest.Company),
		author: SearchCommon.getUIFilterBySearchFilter(searchRequest.Author),
		executive: SearchCommon.getUIFilterBySearchFilter(searchRequest.Executive),
		subject: SearchCommon.getUIFilterBySearchFilter(searchRequest.Subject),
		industry: SearchCommon.getUIFilterBySearchFilter(searchRequest.Industry),
		region: SearchCommon.getUIFilterBySearchFilter(searchRequest.Region)
	};

	//News Filters
	if (searchRequest.Filters && searchRequest.Filters.length) {
	    topicData.newsFilters = SearchCommon.getUINewsFiltersBySBNewsFilters(searchRequest.Filters);
	}

	stC.setData(topicData);

	$dvSaveAsTopic.overlay({ closeOnEsc: true, onShow: function () { stC.setFocusOnTopicName(); } });
}

SearchCommon.saveAsAlert = function (searchRequest, alertData) {

	var $dvSaveAsAlert = $("#dvSaveAsAlert");
	var stC = $dvSaveAsAlert.find("div.dj_AlertEditor:first").findComponent(DJ.UI.AlertEditor);

	if (!$dvSaveAsAlert.data("eventsBound")) {
		//Save
		$dj.subscribe(stC.events.onSaveClick, function (requestObj) {
			if (!requestObj.isValid) {
				alert(requestObj.error);
				return;
			}
			
			var searchRequest = requestObj.searchRequest;

			//Remove unwanted properties from the request
			requestObj.searchRequest = undefined;
			requestObj.isValid = undefined;

			//Make ajax call to save the topic using requestObj
			$dj.progressIndicator.display(SearchCommon.loadingText);
			DJGlobal.AJAX({
				httpMethod: "POST",
				data: {
					alertProperties: JSON.stringify(requestObj),
					searchRequest: JSON.stringify(searchRequest),
					kind: "freeText"
				},
				url: DJGlobal.getControllerActionUrl("Test", requestObj.alertId ? "UpdateAlert" : "CreateAlert"),
				success: function (response) {
					$dj.progressIndicator.hide();
					$().overlay.hide('#dvSaveAsAlert');
					alert(requestObj.topicId ? "<%= Token('alertUpdatedSuccessfully') %>" : "<%= Token('alertCreatedSuccessfully') %>");
				},
				failure: function (response) {
					$dj.progressIndicator.hide();
					DJGlobal.showError(response.Error);
				}
			});
			
		});
		//Cancel
		$dj.subscribe(stC.events.onCancelClick, function () { $().overlay.hide('#dvSaveAsAlert'); });
		$dvSaveAsAlert.data("eventsBound", true);
	}

	SearchCommon.showAlertModal(searchRequest, alertData);
}

SearchCommon.showAlertModal = function (searchRequest, alertData) {
    searchRequest = searchRequest || {}; // SearchRequest should always be there;
    alertData = alertData || {}; // alertData should be set, and contains FolderType;
    var $dvSaveAsAlert = $("#dvSaveAsAlert")
    var stC = $dvSaveAsAlert.find("div.dj_AlertEditor:first").findComponent(DJ.UI.AlertEditor);

    //Set the modal title
    switch (alertData.folderType) {
        case SearchCommon.FolderTypes.news:
            $("#dvSaveAsAlertTitle").html(alertData.alertId ? "<%= Token('updateNewsAlert') %>" : "<%= Token('saveAsNewsAlert') %>");
            break;
        case SearchCommon.FolderTypes.author:
            $("#dvSaveAsAlertTitle").html(alertData.alertId ? "<%= Token('updateAuthorAlert') %>" : "<%= Token('saveAsAuthorAlert') %>");
            break;
        case SearchCommon.FolderTypes.newAuthor:
            $("#dvSaveAsAlertTitle").html(alertData.alertId ? "<%= Token('updateNewAuthorAlert') %>" : "<%= Token('saveAsNewAuthorAlert') %>");
            break;
    }

    //Set the filters here using searchRequest object
    alertData.freeText = searchRequest.FreeText;

    //Channel Filters
    alertData.filters = {
        company: SearchCommon.getUIFilterBySearchFilter(searchRequest.Company),
        author: SearchCommon.getUIFilterBySearchFilter(searchRequest.Author),
        executive: SearchCommon.getUIFilterBySearchFilter(searchRequest.Executive),
        subject: SearchCommon.getUIFilterBySearchFilter(searchRequest.Subject),
        industry: SearchCommon.getUIFilterBySearchFilter(searchRequest.Industry),
        region: SearchCommon.getUIFilterBySearchFilter(searchRequest.Region)
    };

    //News Filters
    if (searchRequest.Filters && searchRequest.Filters.length) {
        alertData.newsFilters = SearchCommon.getUINewsFiltersBySBNewsFilters(searchRequest.Filters);
    }

    stC.setData(alertData);

    $dvSaveAsAlert.overlay({ closeOnEsc: true, onShow: function () { stC.setFocusOnAlertName(); } });
}

SearchCommon.getUIFilterBySearchFilter = function (searchFilter) {
    var filter, f;
    if (searchFilter) {
        filter = {};
        if (searchFilter.Include && searchFilter.Include.length > 0) {
            f = [];
            $.each(searchFilter.Include, function () {
                f.push({ code: this.Code, desc: this.Name });
            });
            filter.include = f;
        }
        if (searchFilter.Exclude && searchFilter.Exclude.length > 0) {
            f = [];
            $.each(searchFilter.Exclude, function () {
                f.push({ code: this.Code, desc: this.Name });
            });
            filter.exclude = f;
        }
        filter.operator = searchFilter.Operator;
    }
    return filter;
}

SearchCommon.getUINewsFiltersBySBNewsFilters = function (newsFilters) {
    if (newsFilters && newsFilters.length) {
        var f = {};
        $.each(newsFilters, function (key, val) {
            switch (this.Category) {
                case SearchCommon.newsFilterCategory.company:
                    {
                        (f.company || []).push({ code: this.Code, desc: this.Name });
                        break;
                    }
                case SearchCommon.newsFilterCategory.author:
                    {
                        (f.author || []).push({ code: this.Code, desc: this.Name });
                        break;
                    }
                case SearchCommon.newsFilterCategory.executive:
                    {
                        (f.executive || []).push({ code: this.Code, desc: this.Name });
                        break;
                    }
                case SearchCommon.newsFilterCategory.subject:
                    {
                        (f.subject || []).push({ code: this.Code, desc: this.Name });
                        break;
                    }
                case SearchCommon.newsFilterCategory.industry:
                    {
                        (f.industry || []).push({ code: this.Code, desc: this.Name });
                        break;
                    }
                case SearchCommon.newsFilterCategory.region:
                    {
                        (f.region || []).push({ code: this.Code, desc: this.Name });
                        break;
                    }
                case SearchCommon.newsFilterCategory.keyword:
                    {
                        (f.keyword || []).push(this.Code);
                        break;
                    }
            }
        });
        return f;
    }
}
