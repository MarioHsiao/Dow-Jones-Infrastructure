var defaultQUnitDirectory = "../../../lib/qunit";
var defaultFrameworkScriptDirectory = "../../DowJones.Web.Mvc/Resources/js";

var frameworkScriptDirectory = window["frameworkScriptDirectory"] || defaultFrameworkScriptDirectory;
var qunitDirectory = window["qunitDirectory"] || defaultQUnitDirectory;

var include = function (src) {
    if (console.log) { console.log('Including script file ', src); }
	document.writeln('<script src="' + src + '"></script>');
};

function setTargetScript(scriptFile) {
    include(scriptFile);
    var fullTitle = scriptFile.replace(/(\.\.\/)*/, '');
    DJ.jQuery("#qunit-header").text(fullTitle);

    window['sandbox'] = DJ.jQuery('#sandbox');
}

function createjQueryTestElement(id) {
    id = id || "test-element";
    return DJ.jQuery('<div id="' + id + '"></div>').appendTo(DJ.jQuery('#qunit-fixture'));
}


include(frameworkScriptDirectory + "/jquery.js");
include(frameworkScriptDirectory + "/require.js");
include(frameworkScriptDirectory + "/jquery-ui.core.js");
include(frameworkScriptDirectory + "/jquery-ui.effects.js");
include(frameworkScriptDirectory + "/jquery-ui.interactions.js");
include(frameworkScriptDirectory + "/jquery-ui.widgets.js");
include(frameworkScriptDirectory + "/underscore.js");
include(frameworkScriptDirectory + "/common.js");
include(frameworkScriptDirectory + "/dj-jquery-ext.js");
include(frameworkScriptDirectory + "/pubsub.js");
include(frameworkScriptDirectory + "/composite-component.js");
include(qunitDirectory + "/qunit.js");

document.writeln('<script>');
document.writeln('  $ = DJ.jQuery;');
document.writeln('  _ = DJ.underscore;');
document.writeln('  $dj = DJ.$dj;');
document.writeln('</script>');
    
document.writeln('<link rel="stylesheet" href="' + qunitDirectory + '/qunit.css" type="text/css" />');
document.writeln('    <h1 id="qunit-header"></h1>');
document.writeln('    <h2 id="qunit-banner"></h2>');
document.writeln('    <div id="qunit-testrunner-toolbar"></div>');
document.writeln('    <h2 id="qunit-userAgent"></h2>');
document.writeln('    <ol id="qunit-tests"></ol>');
document.writeln('    <div id="qunit-fixture">This is the "sandbox area" for tests - cleared after every test</div>');
