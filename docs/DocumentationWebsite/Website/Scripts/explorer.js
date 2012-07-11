/*jshint browser:true */

(function ($) {
    function setupDataViewer() {
        $('.dataViewer')
            .click(function () {
                var $this = $(this),
                    useCache = $this.data('cached') || false;

                if (useCache) {
                    $this.next('.cache-container').modal();
                }
                else {
                    $this.button('loading');
                    $.get($this.data('url'), null, function (data) {
                        // create the container and append it to the element
                        $('<div>')
                            .addClass("cache-container hide")
                            .append(prettyPrintOne(data))
                            .insertAfter($this)
                            .modal();

                        $this.data('cached', true);

                        $this.button('reset');
                    });
                }

                return false;
            })
            .each(function (i, el) {
                var p = $(el).parent();
                p.prev('pre').append(p.detach().css({ float: "right" }));
            });
    }

    function setupDemoFrame() {
        var liveDemo = $('#livedemo'),
            blocker = $('div.blocker', liveDemo),
            iframe = blocker.find('iframe'),
            demoUrl = iframe.data('url');

        if (demoUrl) {
            blocker.block({
                message: $('#loader'),
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });

            iframe.load(function () {
                var actualHeight = iframe[0].contentDocument.height;
                iframe.height(actualHeight + 'px');
                blocker.unblock();
            }).attr('src', demoUrl);
        }
    }

    function setupScrollSpy() {
        var offset = $('.sidebar-nav').offset().top;
        $('body')
            .attr('data-target', '.sidebar-nav')
            .attr('data-spy', 'scroll')
            .scrollspy({ offset: offset, target: '.sidebar-nav' });
    }

    function setupNavScroll(nav) {
        var $win = $(window),
            $nav = $(nav),
            paddingToOffset = parseInt($('body').css("padding-top"), 10);

        $nav.on('click', 'li > a', function () {
            var el = $(this),
                target = $(el.attr('href'));
            
            $nav.find('li').removeClass('active');
            el.parent('li').addClass('active');
            setTimeout(function () { $win.scrollTop(target.offset().top - paddingToOffset - 10); }, 10);
            return false;
        });

        var width = $nav.width();
        $nav.addClass('subnav-fixed').width(width + 'px');

    }

    function setupPrettyPrint() {
        /// <summary>
        /// Find all "pre code"
        /// elements on the page and add the "prettyprint" style. If at least one 
        /// prettyprint element was found, call the Google Prettify prettyPrint() API.
        /// </summary>
        var found = false;
        $("pre code").parent().each(function () {
            if (!$(this).hasClass("prettyprint")) {
                $(this).addClass("prettyprint");
                found = true;
            }
        });

        if (found) window.prettyPrint();
    }

    function prettifyMarkdownTables() {
        /// <summary>
        /// target tables generated via markdown but ignore tables that already have style
        /// (styled via explicit html table tags)
        /// </summary>
        $(".content table").each(function () {
            if (!$(this).hasClass("table")) {
                $(this).addClass("table table-striped table-bordered table-condensed");
            }
        });
    }

    function switchView() {
        var $this = $(this);

        //  First remove class "active" from currently active tab
        $(".contentWell .child-link").removeClass('active');

        /* Now make current tab "active" */
        $this.addClass("active")
             .parents(".level-0").find(".level-1").hide();   /*  Hide all tab content */

        //  Here we get the href value of the selected tab
        var selectedTab = $(this).data("ref");

        //  Show the selected tab content
        $(selectedTab).fadeIn();

    }

    $(function () {
        // activate first nav item
        $(".sidebar-nav > ul.nav > li:first").addClass("active");

        // scroll subnav upto a certain point and then fix it to top
        setupNavScroll('.sidebar-nav');

        // syntax highlighting, Dave!
        setupPrettyPrint();

        // target tables generated via markdown
        prettifyMarkdownTables();

        // highlight Children in sidebar nav as we scroll
        //setupScrollSpy();

        setupDemoFrame();

        //  When user clicks on view switcher button
        $(".contentWell .child-link").click(switchView);

        setupDataViewer();

    });

}(window.jQuery));