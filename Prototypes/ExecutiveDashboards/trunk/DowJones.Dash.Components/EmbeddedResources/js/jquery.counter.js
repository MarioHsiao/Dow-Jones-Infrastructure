/*!  jquery counter plugin
* Author: Hrusikesh Panda
* Version: 0.3
*
* Provides a counter like animation while updating a number field.
*
* 0.3: Faster Accel logic
* 0.2: Simpler acceleration logic
* 0.1: Basic functionality
*/
(function ($) {
    $.fn.counter = function (target, callback) {
        var count = function (elem, stop) {
            var timer;
            var braking = 15,
                $el = $(elem),
                start = parseInt($el.text().replace(',', ''), 10),
                step;

            stop = parseInt(stop, 10);

            if (isNaN(start)) start = 0;

            if (start !== stop) {
                // accelerate if the distance is too wide
                //step = (stop - start) / braking;
                
                step = start + (stop - start) / braking;
               
                // slow down when approaching target
                if (Math.abs(stop - step) < 5) braking = braking * 10;

                $el.text(Math.ceil((step)).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                timer = setTimeout(function () {
                    count(elem, stop, callback);
                }, braking);
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