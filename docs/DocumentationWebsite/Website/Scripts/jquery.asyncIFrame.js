(function ($) {

    $.asyncIFrame = function (elem, options) {

        var plugin = this,
            $element = $(elem);


        var detectBrowser = function () {
            var userAgent = navigator.userAgent.toLowerCase(),
                knownBrowsers = ["chrome", "firefox", "msie"],
                browser = "gecko";

            for (var i = 0; i < knownBrowsers.length; i++) {
                if ((new RegExp(knownBrowsers[i])).test(userAgent)) {
                    browser = knownBrowsers[i];
                    break;
                }
            }

            return browser;
        };
        
        var createIFrame = function () {
            var iframe = $('<iframe>').addClass(options.cssClass);

            if (typeof options.onLoad === 'function')
                iframe.load(options.onLoad);

            return iframe;
        };

        var renderers = {
            gecko: function () {
                var url = options.url,
                    iframe = createIFrame();
                $element.append(iframe);
                iframe.attr('src', url);
            },
            
            chrome: function () {
                renderers.trulyDynamic();
            },
            
            msie: function () {
                renderers.gecko();
            },
            
            firefox: function () {
                renderers.trulyDynamic();
            },
            
            trulyDynamic: function () {
                var url = options.url;
                
                $.get(url, function (data) {
                    var iframe = createIFrame().appendTo($element),
                        doc = iframe[0].contentWindow.document;
                    doc.open().write(data);
                    doc.close();
                });
            },
            
            pseudoDynamic: function () {
                var url = options.url,
                    iframe = createIFrame().appendTo($element),
                    doc = iframe[0].contentWindow.document;
                
                doc.open().write('<body onload="setTimeout(function(){window.location=\'' + url + '\'}, 10)" />');
                doc.close();
            }
        };

        var renderIFrame = function () {
            var browser = detectBrowser();

            // render browser specific iframe
            renderers[browser]();
        };
        
        plugin.init = function () {
            renderIFrame();
        };
        
        plugin.init();

    };
    
    $.fn.asyncIFrame = function (options) {
        return this.each(function () {
            if (undefined == $(this).data('asyncIFrame')) {
                var plugin = new $.asyncIFrame(this, options);
                $(this).data('asyncIFrame', plugin);
            }
        });
    };
})(jQuery);