/*!  jquery counter plugin
* Author: Hrusikesh Panda
* Version: 0.4
*
* Provides a counter like animation while updating a number field.
*
* 0.4: Even faster accel logic, fix for midway stop issue
* 0.3: Faster Accel logic
* 0.2: Simpler acceleration logic
* 0.1: Basic functionality
*/
(function ($) {
    $.fn.counter = function (target, callback) {
        var render = function(elem, value) {
            elem.text(value.toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ","));
        };
        
        var count = function (elem, stop, currentValue) {
            var timer;
            var speed = 15,
                $el = $(elem),
                step, diff;


            stop = parseInt(stop, 10);

            currentValue = currentValue || parseInt($el.text().replace(',', ''), 10);
            if (isNaN(currentValue)) currentValue = 0;

            diff = stop - currentValue;

            if (stop !== currentValue) {
                if (Math.abs(diff / speed) > 0.001) {

                    step = currentValue + diff / speed;

                    render($el, step);
                    
                    timer = setTimeout(function () {
                        count(elem, stop, step);
                    }, speed);
                    $el.data('timer', timer);
                }
                else
                    render($el, stop); 
            }
            else if ($.isFunction(callback)) callback();
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