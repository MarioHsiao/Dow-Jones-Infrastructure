/*jshint browser:true */
    
(function ($) {

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

    $(function () {

        // scroll subnav upto a certain point and then fix it to top
        setupNavScroll('.sidebar-nav');

        // syntax highlighting, Dave!
        setupPrettyPrint();

        // target tables generated via markdown
        prettifyMarkdownTables();

        // highlight Children in sidebar nav as we scroll
        setupScrollSpy(50);

        // activate first nav item
        $(".sidebar-nav > ul.nav > li").not(".nav-header").first().addClass("active");

    });

}(window.jQuery));