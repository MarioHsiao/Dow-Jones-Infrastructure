// This was ripped off directly from jQuery UI. Note that there is an important bug fix in
// $.ui.position.fit.top
jQuery(function($) {
    $.ui = $.ui || {};

    var horizontalPositions = /left|center|right/,
        horizontalDefault = "center",
        verticalPositions = /top|center|bottom/,
        verticalDefault = "center",
        _position = $.fn.position,
        _offset = $.fn.offset;

    $.fn.position = function( options ) {
        if ( !options || !options.of ) {
            return _position.apply( this, arguments );
        }

        // make a copy, we don't want to modify arguments
        options = $.extend( {}, options );

        var target = $( options.of ),
            collision = ( options.collision || "flip" ).split( " " ),
            offset = options.offset ? options.offset.split( " " ) : [ 0, 0 ],
            targetWidth,
            targetHeight,
            basePosition;

        if ( options.of.nodeType === 9 ) {
            targetWidth = target.width();
            targetHeight = target.height();
            basePosition = { top: 0, left: 0 };
        } else if ( options.of.scrollTo && options.of.document ) {
            targetWidth = target.width();
            targetHeight = target.height();
            basePosition = { top: target.scrollTop(), left: target.scrollLeft() };
        } else if ( options.of.preventDefault ) {
            // force left top to allow flipping
            options.at = "left top";
            targetWidth = targetHeight = 0;
            basePosition = { top: options.of.pageY, left: options.of.pageX };
        } else {
            targetWidth = target.outerWidth();
            targetHeight = target.outerHeight();
            basePosition = target.offset();
        }

        // force my and at to have valid horizontal and veritcal positions
        // if a value is missing or invalid, it will be converted to center 
        $.each( [ "my", "at" ], function() {
            var pos = ( options[this] || "" ).split( " " );
            if ( pos.length === 1) {
                pos = horizontalPositions.test( pos[0] ) ?
                    pos.concat( [verticalDefault] ) :
                    verticalPositions.test( pos[0] ) ?
                        [ horizontalDefault ].concat( pos ) :
                        [ horizontalDefault, verticalDefault ];
            }
            pos[ 0 ] = horizontalPositions.test( pos[0] ) ? pos[ 0 ] : horizontalDefault;
            pos[ 1 ] = verticalPositions.test( pos[1] ) ? pos[ 1 ] : verticalDefault;
            options[ this ] = pos;
        });

        // normalize collision option
        if ( collision.length === 1 ) {
            collision[ 1 ] = collision[ 0 ];
        }

        // normalize offset option
        offset[ 0 ] = parseInt( offset[0], 10 ) || 0;
        if ( offset.length === 1 ) {
            offset[ 1 ] = offset[ 0 ];
        }
        offset[ 1 ] = parseInt( offset[1], 10 ) || 0;

        if ( options.at[0] === "right" ) {
            basePosition.left += targetWidth;
        } else if (options.at[0] === horizontalDefault ) {
            basePosition.left += targetWidth / 2;
        }

        if ( options.at[1] === "bottom" ) {
            basePosition.top += targetHeight;
        } else if ( options.at[1] === verticalDefault ) {
            basePosition.top += targetHeight / 2;
        }

        basePosition.left += offset[ 0 ];
        basePosition.top += offset[ 1 ];
        
        return this.each(function() {
            var elem = $( this ),
                elemWidth = elem.outerWidth(),
                elemHeight = elem.outerHeight(),
                position = $.extend( {}, basePosition );

            if ( options.my[0] === "right" ) {
                position.left -= elemWidth;
            } else if ( options.my[0] === horizontalDefault ) {
                position.left -= elemWidth / 2;
            }

            if ( options.my[1] === "bottom" ) {
                position.top -= elemHeight;
            } else if ( options.my[1] === verticalDefault ) {
                position.top -= elemHeight / 2;
            }

            $.each( [ "left", "top" ], function( i, dir ) {
                if ( $.ui.position[ collision[i] ] ) {
                    $.ui.position[ collision[i] ][ dir ]( position, {
                        targetWidth: targetWidth,
                        targetHeight: targetHeight,
                        elemWidth: elemWidth,
                        elemHeight: elemHeight,
                        offset: offset,
                        my: options.my,
                        at: options.at
                    });
                }
            });

            if ( $.fn.bgiframe ) {
                elem.bgiframe();
            }
            elem.offset( $.extend( position, { using: options.using } ) );
        });
    };

    $.ui.position = {
        fit: {
            left: function( position, data ) {
                var win = $( window ),
                    over = position.left + data.elemWidth - win.width() - win.scrollLeft();
                position.left = over > 0 ? position.left - over : Math.max( 0, position.left );
            },
            top: function( position, data ) {
                var win     = $(window),
                    wHeight = win.height(),
                    wScroll = win.scrollTop(),
                    under   = position.top + data.elemHeight - wHeight - wScroll,
                    over    = wScroll - position.top;
                
                if (under > 0 && over < 0) {
                    position.top = position.top - under;
                }
                else if (under < 0 && over > 0) {
                    position.top = position.top + over;
                }
            }
        },
        flip: {
            left: function( position, data ) {
                if ( data.at[0] === "center" ) {
                    return;
                }
                var win = $( window ),
                    over = position.left + data.elemWidth - win.width() - win.scrollLeft(),
                    myOffset = data.my[ 0 ] === "left" ?
                        -data.elemWidth :
                        data.my[ 0 ] === "right" ?
                            data.elemWidth :
                            0,
                    offset = -2 * data.offset[ 0 ];
                position.left += position.left < 0 ?
                    myOffset + data.targetWidth + offset :
                    over > 0 ?
                        myOffset - data.targetWidth + offset :
                        0;
            },
            top: function( position, data ) {
                if ( data.at[1] === "center" ) {
                    return;
                }
                var win = $( window ),
                    over = position.top + data.elemHeight - win.height() - win.scrollTop(),
                    myOffset = data.my[ 1 ] === "top" ?
                        -data.elemHeight :
                        data.my[ 1 ] === "bottom" ?
                            data.elemHeight :
                                    0,
                    atOffset = data.at[ 1 ] === "top" ?
                        data.targetHeight :
                        -data.targetHeight,
                    offset = -2 * data.offset[ 1 ];
                position.top += position.top < 0 ?
                    myOffset + data.targetHeight + offset :
                    over > 0 ?
                        myOffset + atOffset + offset :
                        0;
            }
        }
    };

    // offset setter from jQuery 1.4
    if ( !$.offset.setOffset ) {
        $.offset.setOffset = function( elem, options ) {
            // set position first, in-case top/left are set even on static elem
            if ( /static/.test( $.curCSS( elem, "position" ) ) ) {
                elem.style.position = "relative";
            }
            var curElem   = $( elem ),
                curOffset = curElem.offset(),
                curTop    = parseInt( $.curCSS( elem, "top",  true ), 10 ) || 0,
                curLeft   = parseInt( $.curCSS( elem, "left", true ), 10)  || 0,
                props     = {
                    top:  (options.top  - curOffset.top)  + curTop,
                    left: (options.left - curOffset.left) + curLeft
                };
            
            if ( 'using' in options ) {
                options.using.call( elem, props );
            } else {
                curElem.css( props );
            }
        };

        $.fn.offset = function( options ) {
            var elem = this[ 0 ];
            if ( !elem || !elem.ownerDocument ) { return null; }
            if ( options ) { 
                return this.each(function() {
                    $.offset.setOffset( this, options );
                });
            }
            return _offset.call( this );
        };
    }
});
// end of jQuery UI position()


// popup code
jQuery(function($) {
    var $messenger     = $(window)
        ,$doc          = $(document)
        ,$popup        = $('#popup')
        ,$header       = $popup.find('.dj-popup-header')
        ,$title        = $popup.find('.dj-popup-title')
        ,$body         = $popup.find('.dj-popup-body')
        ,$arrow        = $popup.find('.dj-popup-arrow')
        ,aw            = parseInt($arrow.css('width') || 0)
        ,cw            = parseInt($body.css('width') || 0)
        ,ch            = parseInt($body.css('height') || 0)
        ,tpl           = parseInt($title.css('padding-left') || 0)
        ,tpr           = parseInt($title.css('padding-right') || 0)
        ,tml           = parseInt($title.css('margin-left') || 0)
        ,tmr           = parseInt($title.css('margin-right') || 0)
        ,arrowClasses  = ['dj-popup-arrow-left', 'dj-popup-arrow-right']
        ,stateClasses  = ''
        ,opts          = {}
        ,panel         = {}
        ,trig          = {};
    
    $doc.click(function() {
        hide();
    });
    $popup.click(function() {
        return false;
    });
    $doc.keyup(function(e) {
        if (e.keyCode == 27) {
            hide();
        }
    });
    
    var closeEvents = ['module/edit/open', 'module/edit/close', 'module/reload'];
    $messenger.bind(closeEvents.join(' '), function(ev, $module, $edit, $content) {
        hide();
    });
    
    function show(isUpdate) {
        var $trig = $(trig),
            title = opts.title || '&nbsp;',
            body  = opts.body || '&nbsp;',
            my    = opts.my || 'right center',
            at    = opts.at || 'left center',
            col   = opts.colision || 'flip fit',
            off   = opts.offset || '-' + aw + ' 0',
            args  = {of:$trig, my:my, at:at, collision:col, offset:off, using:positionArrow};
        
        setSize();

        $title.html(title).attr('title', title);
        $body.html(body);
        $popup.addClass('visible').position(args);
        updateState(opts.state || '');
        
        trig.popupShowing = true;
            
        if (isUpdate === false && typeof opts.open == 'function') {
            opts.open(panel);
        }
        
        function positionArrow(pPos) {
            $popup.offset(pPos);
            $arrow.removeClass(arrowClasses.join(' '));
            
            var tPos    = $trig.offset(),
                tH      = $trig.height(),
                pH      = $popup.height(),
                aH      = $arrow.height(),
                minYoff = pPos.top + $title.height() + aH/2,
                maxYoff = pPos.top + pH - 10 - aH/2,
                aYoff   = tPos.top + tH/2;
            
            // all good, show the arrow
            if (aYoff > minYoff && aYoff < maxYoff) {
                var top        = aYoff-pPos.top-aH/2,
                    classIdx   = (tPos.left < pPos.left) ? 0 : 1,
                    arrowClass = arrowClasses[classIdx];
                
                $arrow.css('top', top).addClass(arrowClass);
            }
        }
    }
    
    function hide() {
        if (typeof opts.close == 'function') {
            opts.close();
        }
        
        trig.popupShowing = false;
        
        updateState('');
        $popup.removeClass('visible');
    }
    
    function updateState(classes) {
        $popup.removeClass(stateClasses).addClass(classes);
        
        stateClasses = classes;
    }
    
    function setSize() {
        var w = opts.width || cw,
            h = opts.height || ch;

        $title.width(w);
        $header.width(w + tpl + tpr + tml + tmr);
        $body.width(w).height(h);
    }
    
    $.fn.extend({
        popup:function(options) {
            if (this.length < 1) { return; }
            
            if ($popup.hasClass('visible') == true) {
                hide();
            }
            
            opts  = options;
            trig  = this[0];
            panel = (function(self) {
                return {
                    update: function(props) {
                        if (self.popupShowing !== true) { return; }
                        
                        opts = $.extend(options, props);
                        show(true);
                    }
                }
            })(this[0]);
            
            show(false);
        }
    });
});


