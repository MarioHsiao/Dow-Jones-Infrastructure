(function ($) {

    $.ODSTracker = function (options) {

        var options = $.extend({
            operationalserviceURL: 'http://fdevweb3/api/Private/2.0/OperationalData/json'
        }, options || {});

        var operationdata = function () {
            var _operationJSONObject = { operationdata: { items: { item: []}} };

            if (option.data && options.data.length > 0) {
                $.each(options.data, function (key, value) {
                    _operationJSONObject.recordoperationrequest.operationdata.items.item.push({ name: key, value: value });
                });
            }
            return _operationJSONObject;
            //FCS_OD_RequestIP
        };

        var jsonobject = {
            OperationData: {
                Items: [{
                    Name: 'String content',
                    Value: 'String content'
                }]
            }
        };

        var results = {
            _success: function (data) {
                $dj.debug("Call to the Operational Data Service successfull");
                return data;
            },
            _failure: function (jqXHR) {
                $dj.debug(jqXHR);
                console.log(jqXHR);
                $dj.debug('Call to the Operational Data Service failed, Invalid response');
            },
            _complete: function (jqXHR, textStatus) {
                $dj.debug('Call to the Operational Data Service completed. Results:', jqXHR, textStatus);
            }
        };

        $dj.debug(jsonobject);

        $.ajax({
            url: options.operationalserviceURL,
            type: 'POST',
            data: jsonobject,
            dataType: 'json',
            contentType: 'application/json',
            success: $dj.delegate(this, results._success),
            error: $dj.delegate(this, results._failure),
            complete: $dj.delegate(this, results._complete)
        });

    } //end of ODSTracker.

})(jQuery)




