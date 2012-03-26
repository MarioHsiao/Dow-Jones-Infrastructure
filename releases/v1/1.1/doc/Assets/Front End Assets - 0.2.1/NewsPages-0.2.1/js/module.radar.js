/*
 * jQuery radar v.0.2
 * 
 * Author: Blagovest Dachev
 * Copyright 2010 Dow Jones & Company Inc.
 */
jQuery(function($) {
    var $divs  = $('div'),
        $radar = $divs.filter('.radar-module');

    $radar.delegate('.radar-box .radar-node', 'mouseenter', function() {
        var $trail = getInTrailNodes.call(this);
        
        $trail.addClass('radar-node-intrail');
    });
    $radar.delegate('.radar-box .radar-node', 'mouseleave', function() {
        var $trail = getInTrailNodes.call(this);
        
        $trail.removeClass('radar-node-intrail');
    });
    
    function getInTrailNodes() {
        var $this     = $(this),
            $td       = $this.closest('td'),
            $tr       = $td.closest('tr'),
            $page     = $tr.closest('table'),
            $tds      = $tr.children().filter('td'),
            $trs      = $page.find('tr'),
            tdIdx     = $tds.index($td),
            trIdx     = $trs.index($tr),
            trail     = $tds.slice(0, tdIdx+1).get(),
            $subjects = $radar.find('.radar-subject');
        
        $trs.slice(0, trIdx+1).each(function() {
            var td = $(this).children().filter('td').get(tdIdx);
            trail.push(td);
        });
        
        trail.push($subjects.get(trIdx));
            
        return $(trail).children().filter('.radar-node');
    }
});