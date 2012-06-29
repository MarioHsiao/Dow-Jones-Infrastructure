/*jshint browser:true */

(function ($) {
    function setupDataViewer() {
        $('.dataViewer')
            .click(function () {
                var $this = $(this),
                    useCache = $this.data('cached') || false;
                               
                if(useCache) {
                    $this.next('.cache-container').modal();
                }
                else {
                    $this.button('loading');
                    $.get($this.data('url'), null, function (data) {
                        // modal body with prettified code
                        var modalBody = $('<div>').addClass('modal-body').append(prettyPrintOne(data));

                        // create the container and append it to the element
                        $('<div>')
                            .addClass("cache-container modal hide")
                            .append('<div class="modal-header"><button type="button" class="close" data-dismiss="modal">×</button><h3>Sample Data</h3></div>')
                            .append(modalBody)
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
            } });
        
            iframe.load(function () {
                var actualHeight = iframe[0].contentDocument.height;
                iframe.height(actualHeight + 'px');
                blocker.unblock();
            }).attr('src', demoUrl);
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

        if(found) window.prettyPrint();
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
        
        setupDataViewer();
    });

}(window.jQuery));