/*
*  Documentation file for Portal Headline List JS
*/

DJ.UI.PortalHeadlineList = DJ.UI.PortalHeadlineList.extend({
    /// <summary>
    /// Portal Headline List Component.
    /// </summary>
    
        selectors: {
            /// <value name="selectors" type="object" mayBeNull="true" locid="selectors">
            /// jQuery selectors that are used to lookup various elements within the component. 
            /// Defining them here also makes easy to manage changes to the actual selector names (think of why you define constants in C#).</value>
            /// <example>
            /// selectors {
            ///     source: 'span.source-clickable',
            ///     author: 'span.article-clickable',
            /// }
            /// </example>
        },

        options: {
            /// <value name="options" type="object" mayBeNull="true" locid="options">Client side Options and their default values</value>
            /// <example>
            /// options {
            ///     maxHeadlines: 5,
            ///     displaySnippets: true
            /// }
            /// </example>
        },

        events: {
            /// <value name="events" type="object" mayBeNull="true" locid="events">List of events that the component exposes</value>
            /// <example>headlineClick: "headlineClick.dj.PortalHeadlineList"</example>
            /// <remarks>Follows jQuery style namespacing - [eventName].[namespace]</remarks>
        },


        init: function (element, meta) {
            /// <summary>
            /// Initializes the component. This is similar to a constructor call.
            /// </summary>
            /// <param name="element">The DOM element container for the component.</param>
            /// <param name="meta">Dictionary of any associated metadata for the component as set from server side code. 
            /// This includes client side Options and Data.
            /// </param>
        },


        _initializeHeadlineList: function (data) {
            /// <summary>
            /// Initializes the headline elements by attaching <c>headlineInfo</c> using jQuery data bag.
            /// </summary>
            /// <param name="data">The headline data.</param>
        },


        _initializeElements: function (ctx) {
            /// <summary>
            /// Initializes the jQuery element handles.
            /// </summary>
            /// <param name="ctx" mayBeNull="false" type="DOM element or jQuery selector context">DOM Element or jQuery <a href="http://api.jquery.com/jquery/#selector-context">selector context</a>.</param>
            /// <code>
            ///     this.$selectAll = ctx.find(this.selectors.selectAll);
            ///     this.$options = ctx.find(this.selectors.headlineSelectOptions);
            /// </code>
            /// <remarks>
            /// By doing the jQuery lookup once and memoizing the result, the script performance
            /// is improved greatly.
            /// 
            /// If you're leveraging the framework, ctx is passed automatically during initialization of the component.
            ///
            /// This function is called automatically during initialization by the base class.
            /// </remarks>
        },


        _initializeEventHandlers: function () {
            /// <summary>
            /// Initializes the event handlers for events like headline click.
            /// </summary>
            /// <remarks>
            /// This function is called automatically during initialization by the base class.
            /// </remarks>
        },


        _renderSnippets: function (headline, tLi) {
            /// <summary>
            /// Renders the Snippet for each headline and initializes the tooltip plugin to display snippets as a tooltip.
            /// </summary>
            /// <param name="headline">The headline data.</param>
            /// <param name="tLi">DOM handle for the &lt;li&gt; item of the headline. </param>
        },


        bindOnSuccess: function (data) {
            /// <summary>
            /// Binds the on success template with the given data.
            /// </summary>
            /// <param name="data">The headline data.</param>
        },


        bindOnError: function (data) {
            /// <summary>
            /// Binds the on error template with the given data.
            /// </summary>
            /// <param name="data">Error data.</param>
        },


        showEditSection: function (show) {
            /// <summary>
            /// Shows the Add new content section 
            /// </summary>
            /// <param name="show">if set to <c>true</c> [show], otherwsie removes the section.</param>
        },


        getNoDataTemplate: function () {
            /// <summary>
            /// Returns the "No Data" template 
            /// </summary>
            /// <remarks>
            /// Uses the default template if none specified
            /// </remarks>
        },


        setNoDataTemplate: function (markup) {
            /// <summary>
            /// Sets the "No Data" template 
            /// </summary>
            /// <param name="markup">Markup for the alternate template.</param>
            /// <remarks>
            /// Uses the default template if none specified
            /// </remarks>

        },


        getErrorTemplate: function () {
            /// <summary>
            /// Returns the error template 
            /// </summary>
            /// <remarks>
            /// Uses the default template if none specified
            /// </remarks>

        },


        setErrorTemplate: function (markup) {
            /// <summary>
            /// Sets the error template 
            /// </summary>
            /// <param name="markup">Markup for the alternate template.</param>
            /// <remarks>
            /// Uses the default template if none specified
            /// </remarks>
        }

    });

