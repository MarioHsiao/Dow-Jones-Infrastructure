/*!  jquery counter plugin
* Author: Hrusikesh Panda
* Version: 0.1
*
* Provides a counter like animation while updating a number field.
*/
(function ($) {
    $.fn.counter = function (target, callback) {
        var tens = function (number) {
            /// figure out how many tens are in the source
            number = Math.abs(number);
            return number === 0 ? 0 : Math.floor(Math.log(number) / Math.log(10));
        }
        var stepSize = function (src) {
            /// find the step size based on how big the number is.
            /// returns 1 (< 10), 9 (<99), 99 (<999) and so on...
            /// takes direction into account (up or down)
            var direction = src > 0 ? -1 : 1;
            var accel = Math.pow(10, tens(src) - 1) - 1;

            return Math.max(accel, 1) * direction;
        };
        var count = function (elem, stop) {
            var timer;
            var speed = 20,
                $el = $(elem),
                start = parseInt($el.text().replace(',', ''), 10),
                step;

            stop = parseInt(stop, 10);
            
            if (start !== stop) {

                // slow down when approaching target
                if (Math.abs(start - stop) < 5) speed = speed * 10;

                // accelerate if the distance is too wide
                step = stepSize(start - stop);

                $el.text((start + step).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                timer = setTimeout(function () {
                    count(elem, stop, callback);
                }, speed);
                $el.data('timer', timer);
            } else if ($.isFunction(callback)) callback();
        };

        return this.each(function () {
            var timer = parseInt($(this).data('timer'), 10);
            if (timer) {
                clearTimeout(timer);
                $(this).removeData('timer');
            }
            
            count(this, target, callback);
        });
    }
})(jQuery);