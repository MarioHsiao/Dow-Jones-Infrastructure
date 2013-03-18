/*!  Dow Jones Error Manager  */

$dj.registerNamespace('DJ');

DJ.ErrorManager = {

    errorMessages: {
        "-1": "<%= Token('errorForUserMinus1') %>",
        "585327": "<%= Token('operationAborted') %>"
    },

    formatError: function (error, message) {
        if (error === null || error === undefined)
            return message;

        var code = "-1";
        var message = message || error.message || error.Message;


        if (error && (error.code || error.Code))
            code = JSON.stringify(error.code || error.Code);
        else
            code = error || code;


        if (message === undefined)
            message = DJ.ErrorManager.errorMessages[code];

        if ((message === undefined) || (message === null) || (message.length < 1))
            message = DJ.ErrorManager.errorMessages["-1"];


        return "<%= Token('error') %> " + code + ": " + message;
    },

    getError: function (resp) {
        /// <summary>
        /// Extracts error information from an object.
        /// Will accept any object, but expects XHRs or objects with code, returnCode, 
        /// message, and/or statusMessage properties.
        /// </summary>
        /// <returns>
        /// The extracted error information or null if no error info was found
        /// </returns>

        if (!resp) {
            return null;
        }

        var isErrorObject = function(obj) {
            return obj && !$.isFunction(obj);
        };


        var error = (isErrorObject(resp.error)) ? resp.error : null;
        var code = resp.code || resp.returnCode || resp.Code;
        var message = resp.message || resp.statusMessage || resp.Message;
        var statusCode = parseInt(resp.status);
        var hasStatusCode = !isNaN(statusCode) && statusCode != 0;

        // If there was no error object defined,
        // see if the response text contains JSON w/ an error
        if (!error) {
            var body = resp.responseText;
            if (hasStatusCode && body && body.length > 0) {
                try { error = JSON.parse(body); }
                catch (err) { $dj.debug("Couldn't parse response body JSON (not unexpected)"); }
            }
        }

        // If error has an error, assume the inner one is the 
        // real error details and parse that instead
        if (error && isErrorObject(error.error || error.Error))
            return DJ.ErrorManager.getError(error.error || error.Error);

        // Is this an XHR with a 3xx, 4xx, or 5xx?
        if (!error && !code && hasStatusCode) {
            var statusCodeGroup = Math.floor(statusCode / 100);
            if (statusCodeGroup > 2 && statusCodeGroup < 6) {
                code = "21090" + statusCodeGroup.toString();
            }
        }

        if (code) {
            error = { code: code, message: message };
        }

        if (error) {
            var intCode = parseInt(error.code);
            if (intCode !== NaN) {
                error.code = intCode;
            }
        }

        return error;
    },

    registerErrors: function (errors) {
        $.extend(DJ.ErrorManager.errorMessages, errors);
    }

};


$.extend($dj, {
    formatError: DJ.ErrorManager.formatError,
    getError: DJ.ErrorManager.getError
});