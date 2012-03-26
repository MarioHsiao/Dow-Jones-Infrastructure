/*!
* EntityList
*/

(function ($) {

    DJ.UI.EntityList = DJ.UI.Component.extend({

        selectors: {
            entity: '.dj_pill',
            group: '.pill-group',
            groupEntities: '.pill-list'
        },


        init: function (element, meta) {
            this._super(element, $.extend({ name: "EntityList" }, meta));
        },


        addEntity: function (entity) {
            if (!entity)
                return;

            var context = { group: entity.group, code: entity.code, name: entity.name };

            this._debug('Adding entity ', context);

            var container =
                $(this.selectors.group, this.$element)
                    .filterByData('group', context.group);

            // If no group found, just use the $element
            if (!container.length)
                container = this.$element;

            var matchingEntities =
                $(this.selectors.entity, container)
                    .filterByData('code', context.code);

            if (matchingEntities.length) {
                this._debug('Entity ', context.code, ' already exists - skipping add');
                return;
            }

            var newEntity = this.templates.entity(context);
            container.append(newEntity);

            this.publish('entityAdded.dj.EntityList', context);
        },

        clear: function (context) {
            $(this.selectors.entity, context || this.$element).remove();
        },

        getEntities: function (context) {
            var elements = $(this.selectors.entity, context);

            var entities = _.map(elements, function (el) {
                var $el = $(el);
                return {
                    code: $el.data('code'),
                    group: $el.data('group') || $el.closest(this.selectors.group).data('group'),
                    name: $el.data('name')
                };
            }, this);

            return entities;
        },

        removeEntity: function (entity) {
            if (!entity)
                return;

            var code = entity.code ? entity.code : entity;

            this._debug('Removing entity ', code, ' (from param ', entity, ')');

            $(this.selectors.entity).filterByData('code', code).remove();

            this.publish('entityRemoved.dj.EntityList', code);
        },


        _initializeDelegates: function () {
            this._delegates = $.extend(this._delegates, {
                OnRemoveEntity: $dj.delegate(this, this._onRemoveEntity)
            });
        },

        _initializeElements: function () {
            this._entities = this.$element.find(this.selectors.entity);
        },

        _initializeEventHandlers: function (elements) {
            $('.remove', this.$element).live('click', this._delegates.OnRemoveEntity);
        },


        _onRemoveEntity: function (e) {
            var el = $(e.target).closest(this.selectors.entity);
            this.removeEntity(el.data('code'));
        },

        EOF: null  // Final property placeholder (without a comma) to allow easier moving of functions
    });


    // Declare this class as a jQuery plugin
    $.plugin('dj_EntityList', DJ.UI.EntityList);


})(jQuery);