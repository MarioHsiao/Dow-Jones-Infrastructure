In project `DowJones.Web.Mvc.UI.Components`

* Create a new folder - `SampleComponent`

* Under folder `SampleComponent`, create file `SampleComponent.js`:

		DJ.UI.SampleComponent = DJ.UI.Component.extend({
			// Set selectors to be used within the component
			selectors: {
				textOne: '.textOne',
				textTwo: '.textTwo'
			},
        
			// Component options with default values defined
			options: {
				textColor: "blue",
				textSize: "15px"
			},
        
			// Events that this component can raise
			events: {
				textOneClick: "textOneClick.dj.SampleComponent",
				textTwoClick: "textTwoClick.dj.SampleComponent"
			},

			// This method is called when the component is initialized
			init: function (element, meta) {
				var $meta = $.extend({ name: "SampleComponent" }, meta);

				// Call the base constructor. Required.
				this._super(element, $meta);

				// call databind if the data was already passed
				if (this.data)
					this.bindOnSuccess(this.data);
			},

			// This method will be called from base constructor (DJ.UI.Component)
			// Can be used to initialize various elements to be used within this component, if applicable.
			_initializeElements: function () {
			},

			// This method will be called from base constructor, after _initializeElements
			// Event handlers for this component should be defined here.
			_initializeEventHandlers: function () {
				var me = this;

				this.$element.delegate(this.selectors.textOne, 'click', function () {
					me.publish(me.events.textOneClick, { data: me.data });
					// prevent browser from handling the click
					return false;
				});

				this.$element.delegate(this.selectors.textTwo, 'click', function () {
					me.publish(me.events.textTwoClick, { data: me.data });
					// prevent browser from handling the click
					return false;
				});
			},

			// This is where the data is processed and rendered as HTML
			bindOnSuccess: function (data) {
				this.$element.html(this.templates.success({data: data, options: this.options}));
			},

			// This method will be called by the component users when an error happens.
			// Error is of structure: { Error: { Code: "123", Message: "Message for 123" } }
			bindOnError: function (data) {
				this.$element.html(this.templates.error(data));
			},

			EOF: null

		});

		// Declare this class as a jQuery plugin
		$.plugin('dj_SampleComponent', DJ.UI.SampleComponent);
		$dj.debug('Registered DJ.UI.SampleComponent (extends DJ.UI.Component)');