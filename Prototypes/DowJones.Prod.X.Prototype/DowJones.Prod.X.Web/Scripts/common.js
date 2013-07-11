var common = common || {};

if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    };
}
if (!Date.prototype.toISOString) {
    (function () {

        function pad(number) {
            var r = String(number);
            if (r.length === 1) {
                r = '0' + r;
            }
            return r;
        }

        Date.prototype.toISOString = function () {
            return this.getUTCFullYear()
              + '-' + pad(this.getUTCMonth() + 1)
              + '-' + pad(this.getUTCDate())
              + 'T' + pad(this.getUTCHours())
              + ':' + pad(this.getUTCMinutes())
              + ':' + pad(this.getUTCSeconds())
              + '.' + String((this.getUTCMilliseconds() / 1000).toFixed(3)).slice(2, 5)
              + 'Z';
        };

    }());
}

var d = new Date("2013-06-17T15:50:32.652425Z");
if (!d || +d !== 1307000069000) {
    Date.fromISO = function (s) {
        var day, tz,
            rx = /^(\d{4}\-\d\d\-\d\d([tT ][\d:\.]*)?)([zZ]|([+\-])(\d\d):(\d\d))?$/,
            p = rx.exec(s) || [];
        if (p[1]) {
            day = p[1].split(/\D/);
            for (var i = 0, L = day.length; i < L; i++) {
                day[i] = parseInt(day[i], 10) || 0;
            }
            ;
            day[1] -= 1;
            day = new Date(Date.UTC.apply(Date, day));
            if (!day.getDate()) return NaN;
            if (p[5]) {
                tz = (parseInt(p[5], 10) * 60);
                if (p[6]) tz += parseInt(p[6], 10);
                if (p[4] == '+') tz *= -1;
                if (tz) day.setUTCMinutes(day.getUTCMinutes() + tz);
            }
            return day;
        }
        return NaN;
    };
}
else {
    Date.fromISO = function (s) {
        return new Date(s);
    };
}

common.init = function () {
    $('#secondary ul dropdown-menu').dropdown();
}