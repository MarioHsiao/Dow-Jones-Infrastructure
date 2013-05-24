(function ($) {

    $.fn.ODSTracker.RecordData = function (options) {

        var options = $.extend({
            operationalserviceURL: 'http://privateapi.int.dowjones.com/api/1.0/operationaldata/'
        }, options || {});

        var results = {
            _operationdata: function () {
                var _operationObject = {};
                    _operationObject.recordoperationrequest.operationdata.items.item = [];
                    _operationObject.recordoperationrequest.operationdata.items.item.push({ name: 'Caller', value: 'Widgets' });
                return _operationObject;
            },
            _success: function (data) {
                $dj.debug("Call to the Operational Data Service is Successfull");
                return data;
            },
            _failure: function () {
                $dj.debug('Call to the Operational Data Service is failed, Invalid response');
            }
        };

        $.ajax({
            url: options.operationalserviceURL,
            type: 'POST',
            data: options.data,
            dataType: 'json',
            success: results.success,
            error: results.failure 
        }); 

        /* test using POST- XML request */
        /*$.ajax({
            url: options.operationalserviceURL,
            type: 'POST',
            data: results._operationdata,
            contentType: "text/xml",
            dataType: 'xml',
            processData: false,
            success: $dj.delegate(this, results._success),
            error: results.failure
        });*/

    } //end of ODSTracker.

})(jQuery)



