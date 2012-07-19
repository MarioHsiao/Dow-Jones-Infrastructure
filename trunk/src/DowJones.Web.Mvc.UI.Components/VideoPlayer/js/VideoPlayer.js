
DJ.UI.VideoPlayer = DJ.UI.Component.extend({

    defaults: {
        width: 500,
        height: 340,
        playList: null,
        autoPlay: false
    },

    templates: { playList: '<a href="${url}"><div>${title}</div><span><em>${duration} min</em></span></a>' },

    events: {
        onFullScreenExit: 'fullScreenExit.dj.videoPlayer',
        onBeforeFullScreen: 'onBeforeFullScreen.dj.videoPlayer'
    },

    selectors: {
        videoPlayer: 'div.video-player',
        playListContainer: 'div.playlist-container',
        playListWrap: 'div.playlist-wrap',
        playListItems: 'div.playlist-items',
        playListEntries: 'div.entries',
        playListNav: 'div.playlist-nav',
        playListBtn: 'a.playlist-btn',
        prevBtn: 'a.prev-btn',
        nextBtn: 'a.next-btn'
    },

    init: function (element, meta) {

        // start base constructor
        var $meta = $.extend({ name: "VideoPlayer" }, meta);
        this._super(element, $meta);
        // end base constructor

        this.playerId = this.$element.attr("id") + "_player";

        this.player = $(this.selectors.videoPlayer, this.$element).attr('id', this.playerId);

        this.options.width = (this.options.width || this.defaults.width);
        this.options.height = (this.options.height || this.defaults.height);

        if (this.data) {

            this.data = $.isArray(this.data) ? this.data : (this.data) ? [this.data] : [];

            if (this.data.length > 0) {
                this.play(this.data);
            }
        }
    },

    _initializePlayer: function () {

        if (this.data && this.data.length > 0) {

            this.playList = this._buildPlayList();
            this.playerConfig = {
                clip: {
                    autoPlay: this.options.autoPlay,
                    medium: this.hasAudio ? 'audio' : 'video'
                },
                plugins: {
                    controls: {
                        autoHide: (this.hasAudio ? false : true),
                        height: (this.options.container ? 20 : 30),
                        fullscreen: (this.hasAudio ? false : true),
                        url: encodeURIComponent(this.options.controlBarPath)
                    }
                },
                playlist: this.playList,
                key: this.options.playerKey,
                logo: null,
                play: {
                    label: null,
                    replayLabel: "<%= Token('playAgain') %>",
                    fadeSpeed: 200,
                    rotateSpeed: 50
                }
            };

            if (this.iDevices.iPad) {
                this.playerConfig.onLoad = function() {
                    if (this.getState() != 3) { //Not Playing
                        this.play(0);
                    }
                };
            }

            if (this.hasRTMP) {
                this.playerConfig.plugins.rtmp = { url: this.options.rtmpPluginPath };
            }

            if (this.options.container) {
                this.fPlayer = $f(
                        this.options.container,
                        { src: this.options.playerPath, wmode: 'opaque' },
                        this.playerConfig
                    ).ipad({ medium: this.hasAudio ? 'audio' : 'video' });

                this.fPlayer.play();
            }
            else {

                this.player.css({ "width": (this.options.width || this.defaults.width), "height": (this.options.height || this.defaults.height) });

                this.playerConfig.onFullscreenExit = $dj.delegate(this, function () {
                    this.$element.triggerHandler(this.events.onFullScreenExit);
                });

                this.playerConfig.onBeforeFullscreen = $dj.delegate(this, function () {
                    this.$element.triggerHandler(this.events.onBeforeFullScreen);
                });

                if (this.playList.length == 1)//Single clip - No playlist
                {
                    this.fPlayer = $f(
                            this.playerId,
                            { src: this.options.playerPath, wmode: 'opaque' },
                            this.playerConfig
                        ).ipad({ medium: this.hasAudio ? 'audio' : 'video' });
                }
                else//Multiple clips - Show playlist
                {
                    var playListContainer = $(this.selectors.playListContainer, this.$element);
                    var playListWrap = $(this.selectors.playListWrap, playListContainer);
                    var playListEntries = $(this.selectors.playListEntries, playListWrap);
                    var playListNav = $(this.selectors.playListNav, playListContainer);
                    var playListItems = $(this.selectors.playListItems, playListWrap);
                    this.entriesId = this.playerId + "_entries";

                    playListEntries.attr("id", this.entriesId).empty().css({ "left": "0px" }).html(this.templates.playList);
                    playListContainer.show();

                    this.fPlayer = $f(
                            this.playerId,
                            { src: this.options.playerPath, wmode: 'opaque' },
                            this.playerConfig
                        )
                        .ipad({ medium: this.hasAudio ? 'audio' : 'video' }).playlist('#' + this.entriesId);

                    playListWrap.show().css("top", (this.player.height() - 37 - playListItems.height())).hide().find("a.prev, a.next").removeClass("disabled"); //37 is the flowplayer control bar height

                    playListItems.removeData().scrollable({ itemsToScroll: 3, initialIndex: 0 });

                    $(this.selectors.playListBtn, playListNav).unbind('click').bind('click', function (e) {
                        if (playListWrap.is(":visible"))
                            playListWrap.fadeOut(300);
                        else
                            playListWrap.fadeIn(300);

                        e.stopPropagation();
                    });

                    this.playListNextBtn = $(this.selectors.nextBtn, playListNav).click($dj.delegate(this, this._onPlayListNext)).removeClass("disabled");
                    this.playListPrevBtn = $(this.selectors.prevBtn, playListNav).click($dj.delegate(this, this._onPlayListPrev)).removeClass("disabled");
                }
            }
        }


    },

    _onPlayListNext: function (e) {
        this._playClip(true);
        return false;
    },

    _onPlayListPrev: function (next, entriesId) {
        this._playClip(false);
        return false;
    },

    _playClip: function (next) {

        if ((next && this.playListNextBtn.hasClass("disabled")) || (!next && this.playListPrevBtn.hasClass("disabled")))
            return false;

        var clips = $('#' + this.entriesId + ' > a');
        var isPlaying = clips.filter("a.playing, a.paused, a.progress").length > 0;
        if (isPlaying) {//Buffering, Playing or Paused
            var me = this;
            clips.each(function (i, clip) {
                var $clip = $(clip);
                if ($clip.hasClass('playing') || $clip.hasClass('paused') || $clip.hasClass('progress')) {
                    if (next) {
                        $clip.next().click();
                        me.playListPrevBtn.removeClass("disabled");
                        if ($clip.next().index() == me.playList.length - 1 || $clip.next().index() < 0)
                            me.playListNextBtn.addClass("disabled");
                    }
                    else {
                        $clip.prev().click();
                        me.playListNextBtn.removeClass("disabled");
                        if ($clip.prev().index() <= 0)
                            me.playListPrevBtn.addClass("disabled");
                    }
                    return false;
                }
                return false;
            });
        }
        else {
            clips.eq(0).click();
        }
        return false;
    },

    _padOs: function (str, l, s) {
        return (l -= str.length) > 0
                ? (s = new Array(Math.ceil(l / s.length) + 1).join(s)).substr(0, s.length) + str + s.substr(0, l - s.length)
                : str;
    },

    _buildPlayList: function () {
        if (this.data && this.data.length > 0) {
            var fpPlayList = [], clip, me = this;
            this.hasAudio = false; this.hasRTMP = false;
            $.each(this.data, function (i, item) {
                if (item && item.url && item.duration != 0) {
                    clip = {};
                    if ((item.url.indexOf('rtmp') > -1 || item.url.indexOf('RTMP') > -1) && item.streamer && item.file) {
                        me.hasRTMP = true;
                        clip.provider = 'rtmp';
                        clip.netConnectionUrl = item.streamer;
                        clip.url = item.file;
                    }
                    else {
                        clip.url = item.url;
                    }

                    if (item.medium == "audio" || item.medium == "AUDIO") {
                        me.hasAudio = true;
                        if (item.thumbNail)
                            clip.coverImage = { url: item.thumbNail, scaling: 'orig' };
                    }
                    clip.medium = item.medium;
                    clip.title = item.title;
                    //clip.duration = Math.floor(item.duration / 60) + "." + me._padOs((item.duration % 60).toFixed(), 2, "0");
                    fpPlayList.push(clip);
                }
            });
            return fpPlayList;
        }
    },

    play: function (list, container) {
        this.data = $.isArray(list) ? list : [list];
        this.options.container = container;
        this._initializePlayer();
    },

    stop: function () {
        try {
            if (this.fPlayer)
                this.fPlayer.stop();
        } catch (e) { }
    },

    setHeight: function (height) {
        this.options.height = height;
        if (this.player)
            this.player.css("height", height);
    },

    setWidth: function (width) {
        this.options.width = width;
        if (this.player)
            this.player.css("width", width);
    },

    getHeight: function (height) {
        return (this.options.height || this.defaults.height);
    },

    getWidth: function (width) {
        return (this.options.width || this.defaults.width);
    }
});

DJ.UI.VideoPlayer.prototype.iDevices = DJ.UI.VideoPlayer.prototype.iDevices || {
    iPad: (navigator.userAgent.indexOf('iPad') !== -1)
};

// Declare this class as a jQuery plugin
$.plugin('dj_VideoPlayerControl', DJ.UI.VideoPlayer);


$dj.debug('Registered DJ.UI.VideoPlayer (extends DJ.UI.Component)');
