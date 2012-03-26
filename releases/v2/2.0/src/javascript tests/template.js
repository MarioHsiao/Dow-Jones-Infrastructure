	var defaultQUnitDirectory = "../../../lib/qunit";
	var defaultFrameworkScriptDirectory = "../../DowJones.Web.Mvc/Resources/js"

	var frameworkScriptDirectory = window["frameworkScriptDirectory"] || defaultFrameworkScriptDirectory;
	var qunitDirectory = window["qunitDirectory"] || defaultQUnitDirectory;

    var include = function (src) {
        document.writeln('<script src="' + src + '"></script>');
    }

    function setTargetScript(scriptFile) {
        include(scriptFile);
        var fullTitle = scriptFile.replace(/(\.\.\/)*/, '');
        $("#qunit-header").text(fullTitle);

        window['sandbox'] = $('#sandbox');
    }

    function createjQueryTestElement(id) {
        id = id || "test-element";
        return $('<div id="'+id+'"></div>').appendTo($('#qunit-fixture'));
    }


    include(frameworkScriptDirectory + "/jquery.js");
    include(frameworkScriptDirectory+"/jquery-ui.core.js");
    include(frameworkScriptDirectory+"/jquery-ui.effects.js");
    include(frameworkScriptDirectory+"/jquery-ui.interactions.js");
    include(frameworkScriptDirectory + "/jquery-ui.widgets.js");
    include(frameworkScriptDirectory + "/underscore.js");
    include(frameworkScriptDirectory + "/common.js");
    include(qunitDirectory + "/qunit.js");

    document.writeln('<script type="text/javascript">$dj.debugEnabled=true;</script>');
    document.writeln('<link rel="stylesheet" href="' + qunitDirectory + '/qunit.css" type="text/css" />');
    document.writeln('<link rel="stylesheet" href="../../DowJones.Web.Mvc/Resources/css/base.css" type="text/css" />');
    document.writeln('    <h1 id="qunit-header"></h1>');
    document.writeln('    <h2 id="qunit-banner"></h2>');
    document.writeln('    <div id="qunit-testrunner-toolbar"></div>');
    document.writeln('    <h2 id="qunit-userAgent"></h2>');
    document.writeln('    <ol id="qunit-tests"></ol>');
    document.writeln('    <div id="qunit-fixture">This is the "sandbox area" for tests - cleared after every test</div>');
