/* @dependency ServiceProxy.js */
/* Operational Data Service Manager, tracks data. */

$dj.registerNamespace('DJ');

(function ($, DJ) {
    DJ.ODSManager = DJ.ODSManager || {

        defaults: {
            operationalserviceURL: '<%= AppSetting("OperationalDataSource", "http://fdevweb3/api/dashboard/OperationalData/1.0/json") %>'
        },

        recordODSData: function (data) {
            var me = DJ.ODSManager,
            items = data || [],
            jsonobj = { operationData: { items: items} };

            $.ajax({
                url: me.defaults.operationalserviceURL,
                type: 'POST',
                data: jsonobj,
                //dataType: 'json',
                //contentType: 'application/json',
                success: $dj.delegate(me, me._success),
                error: $dj.delegate(me, me._failure),
                complete: $dj.delegate(me, me._complete)
            });

        },

        _success: function (data) {
            $dj.debug("Call to the Operational Data Service successfull");
            return data;
        },

        _failure: function (jqXHR) {
            $dj.debug(jqXHR);
            $dj.debug('Call to the Operational Data Service failed, Invalid response');
        },

        _complete: function (jqXHR, textStatus) {
            $dj.debug('Call to the Operational Data Service completed. Results:', jqXHR, textStatus);
        }
    };

    $.extend($dj, { recordODSData: DJ.ODSManager.recordODSData });
}(jQuery, DJ));