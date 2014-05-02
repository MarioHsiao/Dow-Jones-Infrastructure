
    DJ.UI.ModuleSearchBuilder = DJ.UI.Component.extend({
  
        init: function (element, meta) {
            var $meta = $.extend({ name: "ModuleSearchBuilder" }, meta);
            this._super(element, $meta);
        }

    });

    // Declare this class as a jQuery plugin
    $.plugin('dj_ModuleSearchBuilder', DJ.UI.ModuleSearchBuilder);
