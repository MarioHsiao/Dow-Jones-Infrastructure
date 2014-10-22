

//
// create closure
//
(function($) {
  //
  // plugin definition
  //
  $.fn.headlines = function(options, data, literals) {
     debug(this, 'hilight selection count: ' + this.size());
     // build main options before element iteration
     var opts = $.extend({}, $.fn.headlines.defaults, options);
     var dfts = $.extend({}, $.fn.headlines.literals, literals);
     // iterate and reformat each matched element
     return this.each( function() {
       $this = $(this);
       // build element specific options
       var o = $.meta ? $.extend({}, opts, $this.data()) : opts;
       // update element styles
       $this.css({
          backgroundColor: o.background,
          color: o.foreground
       });
       //var markup = $this.html();
       // call our format function
       ///markup = $.fn.headlines.format(markup);
       paint($this);
       
     } );
  };
    //
    // private function for debugging
    //
    function debug($obj, msg) {
        if (window.console && window.console.log)
            window.console.log(msg);
    };
    
    //
    // private function for painting headlines
    //
    function paint($obj){
        debug($obj,"painting");
        $obj.html("<div> painting headlines </div>");
    };
    
    //
    // define and expose our format function
    //
    $.fn.headlines.format = function(txt) {
        return '<strong>' + txt + ' Loading...</strong>';
    };
    
    //
    // plugin defaults
    //
    $.fn.headlines.defaults = {
        foreground: '#CCC',
        background: '#555',
        maxNumHeadlinesToShow: 10,
        displayDeleteTrigger: false,
        displayClipTrigger: false,
        displayMoreLikeThisTrigger: true,
        displayByLines: true,
        displayInputCheckBoxes: false,
        displayDuplicates: true,
        displayAccessionNumbers: false,
        displayEntitiesInHeadlines: false,
        displayEntitiesInSnippets : true,
        enableHeadlineDraging: false,
        dropZoneId : "",        
        baseClassName : ""
    };
    
    //
    // plugin defaults
    //
    $.fn.headlines.literals = {
        of : ' ${of}',
        publication : '${publication}',
        website : '${website}',
        picture : '${picture}'
        
        
        
    };
    //
    // end of closure
    //
})(jQuery);
