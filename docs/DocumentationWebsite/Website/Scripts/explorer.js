/*jshint browser:true */

(function ($) {
    function setupDemoFrame() {
        var liveDemo = $('#livedemo'),
            demoLoaderDiv = $('div.showcase', liveDemo),
            demoUrl = $('#liveDemoUrl').val();

        if (demoUrl) {
            liveDemo.asyncIFrame({
                url: demoUrl,
                cssClass: 'showcase hide',
                onLoad: function () {
                    demoLoaderDiv.hide('fast', 'linear', function () {
                        var iframe = $('iframe.showcase', liveDemo),
                            actualHeight = iframe[0].contentDocument.height;
                        iframe.height(actualHeight + 'px').removeClass('hide').show('slow', 'linear');
                    });
                }
            });
        }
    }

    function setupScrollSpy(offset) {
        $('body')
            .attr('data-target', '.sidebar-nav')
            .attr('data-spy', 'scroll')
            .scrollspy({ offset: offset });
    }

    function setupNavScroll(nav) {
        // fix nav on scroll
        var $win = $(window),
            $nav = $(nav),
            navTop = $(nav).length && $(nav).offset().top - 40,
            isFixed = 0;
        processScroll();
        $nav.on('click', function () {
            if (!isFixed) {
                setTimeout(function () { $win.scrollTop($win.scrollTop() - 47); }, 10);
            }
        });

        $win.on('scroll', processScroll);

        function processScroll() {
            var scrollTop = $win.scrollTop();
            if (scrollTop >= navTop && !isFixed) {
                isFixed = 1;
                $nav.addClass('subnav-fixed');
            } else if (scrollTop <= navTop && isFixed) {
                isFixed = 0;
                $nav.removeClass('subnav-fixed');
            }
        }
    }

    // Find all </code><pre><code> 
    // elements on the page and add the "prettyprint" style. If at least one 
    // prettyprint element was found, call the Google Prettify prettyPrint() API.
    function setupPrettyPrint() {
        $("pre code").parent().each(function () {
            if (!$(this).hasClass("prettyprint")) {
                $(this).addClass("prettyprint");
            }
        });

        window.prettyPrint();
    }

    // target tables generated via markdown but ignore tables that already have style
    // (styled via explicit html table tags)
    function prettifyMarkdownTables() {
        $(".content table").each(function () {
            if (!$(this).hasClass("table")) {
                $(this).addClass("table table-striped table-bordered table-condensed");
            }
        });
    }

    function switchView() {
        var childLinks = $(".contentWell .child-link");
        var childTabs = $(this).parents(".level-0").find(".level-1");

        //  First remove class "active" from currently active tab
        $(childLinks).removeClass('active');

        //  Now add class "active" to the selected/clicked tab
        $(this).addClass("active");

        //  Hide all tab content
        $(childTabs).hide();

        //  Here we get the href value of the selected tab
        var selected_tab = $(this).attr("href");

        //  Show the selected tab content
        $(selected_tab).fadeIn();

        //  At the end, we add return false so that the click on the link is not executed
        return false;
    }

    $(function () {

        // scroll subnav upto a certain point and then fix it to top
        setupNavScroll('.sidebar-nav');

        // syntax highlighting, Dave!
        setupPrettyPrint();

        // target tables generated via markdown
        prettifyMarkdownTables();

        // highlight Children in sidebar nav as we scroll
        setupScrollSpy(50);

        setupDemoFrame();

        //  When user clicks on view switcher button
        $(".contentWell .child-link").click(switchView);
    });

}(window.jQuery));