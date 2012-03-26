/*!  PubSub Manager */

$dj.registerNamespace('DJ.Platform');

DJ.Platform.PubSubManager = Class.extend({

    init: function () {
        this._pubSubCache = {};
        $dj.debug('PubSubManager initialized');
    },

    publish: function (eventName, args) {

        if (!this._pubSubCache || !eventName) { $dj.debug('PubSubManager.publish: Event not found.', eventName, args); return; }

        $dj.debug('PubSubManager.publish: Publishing "', eventName, '" with following arguments:', args);

        if (this._pubSubCache[eventName]) {
            _.each(this._pubSubCache[eventName], function (handler) {
                if (handler && typeof handler === 'function') {
                    handler.apply(this, [args]);
                    $dj.debug('PubSubManager.publish: Published to "', handler.name || 'anonymous function', '"');
                }
            });
        }
    },

    subscribe: function (eventName, handler) {

        if (!eventName) { $dj.debug('PubSubManager.subscribe: Event Name cannot be null or empty'); return; }

        if (!handler || typeof handler !== 'function') { $dj.debug('PubSubManager.subscribe: Not a valid handler'); return; }

        $dj.debug('PubSubManager.subscribe: Subscribing to "'
                    , eventName
                    , '" with "'
                    , handler.name || 'anonymous function', '"');

        this._pubSubCache[eventName] = this._pubSubCache[eventName] || [];
        this._pubSubCache[eventName].push(handler);
        return [eventName, handler];
    }
});

$dj._pubSubManager = $dj._pubSubManager || new DJ.Platform.PubSubManager();
$dj.publish = $dj.publish || $dj.delegate($dj._pubSubManager, $dj._pubSubManager.publish);
$dj.subscribe = $dj.subscribe || $dj.delegate($dj._pubSubManager, $dj._pubSubManager.subscribe);


