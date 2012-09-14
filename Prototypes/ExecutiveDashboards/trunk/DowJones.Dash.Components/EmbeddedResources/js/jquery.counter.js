/*!  jquery counter plugin
* Author: Hrusikesh Panda
* Version: 0.1
*
* Provides a counter like animation while updating a number field.
*/
(function ($) {
    $.fn.counter = function (target, callback) {
        var timer;
        
        var _tens = function (number) {
            /// figure out how many tens are in the source
            number = Math.abs(number);
            return number === 0 ? 0 : Math.floor(Math.log(number) / Math.log(10));
        }
        var _stepSize = function (src) {
            /// find the step size based on how big the number is.
            /// returns 1 (< 10), 9 (<99), 99 (<999) and so on...
            /// takes direction into account (up or down)
            var direction = src > 0 ? -1 : 1;
            var accel = Math.pow(10, _tens(src) - 1) - 1;

            return Math.max(accel, 1) * direction;
        };
        var _count = function (elem, stop) {

            var speed = 20,
                $el = $(elem),
                start = parseInt($el.text(), 10),
                stop = parseInt(stop, 10),
                step = start < stop ? 1 : -1;
            if (start !== stop) {

                // slow down when approaching target
                if (Math.abs(start - stop) < 5) speed = speed * 10;

                // accelerate if the distance is too wide
                step = _stepSize(start - stop);

                $el.text(start + step);
                timer = setTimeout(function () {
                    _count(elem, stop, callback);
                }, speed);
            } else if ($.isFunction(callback)) callback();
        };

        return this.each(function () {
            if (timer) {
                clearTimeout(timer);
                timer = null;
            }
            
            _count(this, target, callback);
        });
    }
})(jQuery);