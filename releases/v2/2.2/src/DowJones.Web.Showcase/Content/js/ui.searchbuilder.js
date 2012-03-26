if (typeof (SearchBuilder) == 'undefined')
	SearchBuilder = {};

SearchBuilder.$sb = $("div.dj_SearchBuilder:first");

//Show loading area on Search Builder
SearchBuilder.$sb.showLoading();

SearchBuilder.Init = function () {

	SearchBuilder.sbC = SearchBuilder.$sb.findComponent(DJ.UI.SearchBuilder);

	$dj.subscribe(SearchBuilder.sbC.events.onSearchTextBoxEnter, function (args) {
		$("#sbSearchbtn").click();
	});

	$("#sbSearchbtn").click(function () {
		//Get the Search Builder request
		var sbRequest = SearchBuilder.sbC.getRequestObject();
		if (!sbRequest.isValid) {
			alert(sbRequest.error);
		}
		else {
			var $form = $("<form/>", { method: 'post', action: DJGlobal.getControllerActionUrl("Search", "Results") })
						.appendTo(document.body);
			DJGlobal.createHiddenField("kind", "freeText", $form[0]);
			DJGlobal.createHiddenField("request", JSON.stringify(sbRequest.request), $form[0]);
			$form.submit();
		}
	});

	//Save as memu
	$("#sbSaveAs").click(function (e) {
		var $elem = $(this), $saveAsMenu = $("#sbSaveAsMenu"), elemOffset = $elem.offset(), elemHeight = $elem.outerHeight(),
		menuHeight = $saveAsMenu.outerHeight(), t = elemOffset.top, l = elemOffset.left;
		if (($(window).height() + $(document).scrollTop() - elemOffset.top - elemHeight) > menuHeight) {
			t = t + elemHeight;
			$saveAsMenu.removeClass("dj_btn-up-arrow").addClass("dj_btn-down-arrow");
		}
		else {
			t = t - menuHeight;
			$saveAsMenu.removeClass("dj_btn-down-arrow").addClass("dj_btn-up-arrow");
		}
		$elem.addClass('active');
		$saveAsMenu.css({ top: t, left: l, position: "absolute" }).show();

		$(document).unbind('mousedown.sbSaveAs').bind('mousedown.sbSaveAs').click(function () {
			$("#sbSaveAs").removeClass('active');
			$('#sbSaveAsMenu').hide();
			$(document).unbind('mousedown.sbSaveAs');
		});
		e.stopPropagation();
	});

	$('#sbSaveAsMenu').delegate('.label', 'click', function () {
		var saveAs = $(this).data('saveas'), sbRequest;
		sbRequest = SearchBuilder.sbC.getRequestObject();
		if (!sbRequest.isValid) {
			alert(sbRequest.error);
		}
		else {
			switch (saveAs) {
				case "saveAsTopic":
					{
						SearchCommon.saveAsTopic(sbRequest.request, SearchBuilder.topicData);
						break;
					}
				case "saveAsNewsAlert":
					{
						SearchBuilder.alertData = SearchBuilder.alertData || {};
						SearchBuilder.alertData.folderType = SearchCommon.FolderTypes.news;
						SearchCommon.saveAsAlert(sbRequest.request, SearchBuilder.alertData);
						break;
					}
				case "saveAsAuthorAlert":
					{
						SearchBuilder.alertData = SearchBuilder.alertData || {};
						SearchBuilder.alertData.folderType = SearchCommon.FolderTypes.author;
						SearchCommon.saveAsAlert(sbRequest.request, SearchBuilder.alertData);
						break;
					}
				case "saveAsNewAuthorAlert":
					{
						SearchBuilder.alertData = SearchBuilder.alertData || {};
						SearchBuilder.alertData.folderType = SearchCommon.FolderTypes.newAuthor;
						SearchCommon.saveAsAlert(sbRequest.request, SearchBuilder.alertData);
						break;
					}
			}
		}
	});

	//Hide the loading area on Search Builder
	SearchBuilder.$sb.hideLoading();
}