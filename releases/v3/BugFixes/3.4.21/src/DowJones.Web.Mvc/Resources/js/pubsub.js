/*!  PubSub Manager */

(function ($, $dj, _) {

    DJ.PubSubManager = function () {
        var _pubSubCache = {};

        return {
            publish: function (eventName, args) {

                if (!_pubSubCache || !eventName) {
                    $dj.warn('PubSubManager.publish: Event not found.', eventName, args);
                    return;
                }

                $dj.info('PubSubManager.publish: Publishing "', eventName, '" with following arguments:', args);

                if (_pubSubCache[eventName]) {
                    _.each(_pubSubCache[eventName], function (handler) {
                        if (handler &&
                            typeof handler === 'function') {
                            setTimeout((function (s) {
                                return function () {
                                    s.apply(this, [args]);
                                    $dj.info('PubSubManager.publish: Published to "', s.name || 'anonymous function', '"');
                                };
                            } (handler)));
                        }
                    });
                }
            },

            subscribe: function (eventName, handler) {

                if (!eventName) {
                    $dj.warn('PubSubManager.subscribe: Event Name cannot be null or empty');
                    return;
                }

                if (!handler || typeof handler !== 'function') {
                    $dj.warn('PubSubManager.subscribe: Not a valid handler');
                    return;
                }

                $dj.info('PubSubManager.subscribe: Subscribing to "',
                            eventName,
                            '" with "',
                            handler.name || 'anonymous function', '"');

                _pubSubCache[eventName] = _pubSubCache[eventName] || [];
                _pubSubCache[eventName].push(handler);
                return [eventName, handler];
            }
        };
    };


    $dj._pubSubManager = $dj._pubSubManager || new DJ.PubSubManager();
    $dj.publish = $dj.publish || $dj.delegate($dj._pubSubManager, $dj._pubSubManager.publish);
    $dj.subscribe = $dj.subscribe || $dj.delegate($dj._pubSubManager, $dj._pubSubManager.subscribe);

    $.extend(DJ, {
        publish: $dj.publish,
        subscribe: $dj.subscribe
    });
})(DJ.jQuery, DJ.$dj, DJ.underscore);