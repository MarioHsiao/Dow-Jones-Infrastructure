/*!
* CompaniesSparline
* Composite Control class
*
*/

(function ($) {

	DJ.UI.CompaniesSparklinesComponent = DJ.UI.CompositeComponent.extend({

		/*
		* Properties
		*/

		// Default options
		defaults: {
			debug: false,
			cssClass: 'CompaniesSparklines'
			// ,name: value     // add more defaults here separated by comma
		},

		eventNames: {
			pillClicked: 'pillClick.dj.CompanySparkline',
			sparklineClicked: 'sparklineClick.dj.CompaniesSparkline'
		},

		// Localization/Templating tokens
		tokens: {
		// name: value     // add more defaults here separated by comma
	},

	_initializeEventHandlers: function () {
		this.subscribe(this.eventNames.pillClicked, $dj.delegate(this, this.subscribeEvent));
	},


	subscribeEvent: function (data) {
		$dj.debug("SubscribeEvent: -- > ", data);
		this.publish(this.eventNames.sparklineClicked, data);
	},

	init: function (element, meta) {
		var $meta = $.extend({ name: "CompaniesSparklines" }, meta);

		// Call the base constructor
		this._super(element, $meta);

		if ($.isArray(this.data) && this.data.length > 0 && o.IsClientMode === true) {
			this.renderPills();
		}
		else {
			this.getData();
		}
	},

	setData: function (dataResult) {
		this.data = dataResult;
		this.renderPills();
	},

	getData: function () {
		var self = this,
                o = self.options,
                el = $(self.element);

		if (o.companies &&
                $.isArray(o.companies) && o.companies.length > 0) {
			var tUrl = (o.dataServiceUrl.indexOf("?") <= 0) ? o.dataServiceUrl + "?c=" + o.companies.join(",") : o.dataServiceUrl + "&c=" + o.companies.join(",");
			$dj.debug(tUrl);
			$.ajax({
				url: tUrl,
				type: 'GET',
				success: $dj.delegate(this, this._getDataOnSuccess),
				error: $dj.delegate(this, this._getDataOnFailure)
			});
		}
	},

	_getDataOnSuccess: function (dataObj) {
		var self = this,
                o = self.options,
                el = $(self.element);

		if (dataObj.error) {
			this._getDataOnFailure();
			return;
		}
		self.data = dataObj;
		this.renderPills();
		$dj.debug("success");
	},

	_getDataOnFailure: function () {
		var self = this,
                o = self.options,
                el = $(self.element);
		el.hide();
	},

	renderPills: function () {
		var self = this,
                o = self.options,
                el = $(self.element);

		el.empty();

		if (self.data && self.data.length > 0) {
			$.each(self.data, $dj.delegate(this, this.generatePillDelegate));
		}
	},

	generatePillDelegate: function (i, dataObj) {
		var self = this,
                o = self.options,
                el = $(self.element);

		var tId = self.id + "child-" + i;
		if (dataObj.closePrices &&
                dataObj.closePrices.length &&
                dataObj.closePrices.length > 0) {
			el.append("<div id=\"" + tId + "\" class=\"dj_CompanySparkline dj_sparkline\"></div>");
			this.generatePill(tId, dataObj);
		}
		else if (dataObj.status === 0 && dataObj.name != null) {
			el.append("<div class=\"dj_sparkline CompanySparklin dj_sparkline-name\">" + dataObj.name + "</div>");
		}
	},

	generatePill: function (id, dataObj) {
		var self = this,
                o = self.options,
                el = $(self.element);

		var baseOptions = {
			baselineSeriesColor: o.baselineSeriesColor,
			seriesColorForIncrease: o.seriesColorForIncrease,
			seriesColorForDecrease: o.seriesColorForDecrease
		};

		try { $("#" + id).dj_CompanySparkline({ options: baseOptions, data: dataObj }).setOwner(this); } catch (ex) { $dj.debug(ex); }


	},

	dispose: function () {
		this._super();
	}
});


// Declare this class as a jQuery plugin
$.plugin('dj_CompaniesSparklinesComponent', DJ.UI.CompaniesSparklinesComponent);


})(jQuery);