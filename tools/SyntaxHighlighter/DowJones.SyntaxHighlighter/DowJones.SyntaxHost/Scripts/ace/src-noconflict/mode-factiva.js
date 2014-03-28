ace.define('ace/mode/factiva', ['require', 'exports', 'module', 'ace/lib/oop', 'ace/mode/text', 'ace/tokenizer', 'ace/mode/factiva_highlight_rules', 'ace/mode/behaviour'], function (require, exports, module) {


    var oop = require("../lib/oop");
    var TextMode = require("./text").Mode;
    var Tokenizer = require("../tokenizer").Tokenizer;
    var FactivaHighlightRules = require("./factiva_highlight_rules").FactivaHighlightRules;
    var Behaviour = require("./behaviour").Behaviour;

    var Mode = function () {
        TextMode.call(this);
        var highlighter = new FactivaHighlightRules();
        this.$tokenizer = new Tokenizer(highlighter.getRules());
        this.$highlightRules = FactivaHighlightRules;
        this.$behaviour = new Behaviour();
        //this.$keywordList = ["and"];
    };

    oop.inherits(Mode, TextMode);

    (function () {
        this.$id = "ace/mode/factiva";
    }).call(Mode.prototype);

    exports.Mode = Mode;
}); ace.define('ace/mode/factiva_highlight_rules', ['require', 'exports', 'module', 'ace/lib/oop', 'ace/lib/lang', 'ace/mode/text_highlight_rules'], function (require, exports, module) {


    var oop = require("../lib/oop");
    var lang = require("../lib/lang");
    var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;

    var FactivaHighlightRules = function () {

        var keywordMapper = this.createKeywordMapper({
            "keyword": "lock-on-active|date-effective|date-expires|no-loop|" +
                       "auto-focus|activation-group|agenda-group|ruleflow-group|" +
                       "entry-point|duration|package|import|dialect|salience|enabled|" +
                       "attributes|rule|extend|when|then|template|query|declare|" +
                       "function|global|eval|not|in|or|and|exists|forall|accumulate|" +
                       "collect|from|action|reverse|result|end|over|init"
        }, "identifier");

        this.$rules = {
            "start": [
                {
                    token: "constant.character.asterisk",
                    regex: "[\\*]"
                }, {
                    token: 'constant.character.count',
                    regex: '\\batleast[0-9]+\\b'
                }, {
                    token: 'constant.character.proximity',
                    regex: '\\bnear[0-9]*\\b'
                }, {
                    token: 'constant.character.adjancency',
                    regex: '\\badj[0-9]*\\b'
                }, {
                    token: 'constant.character.wordcount',
                    regex: '\\bwc[<|>][1-9][0-9]*\\b'
                }, {
                    token: 'constant.character.within',
                    regex: 'w/[1-9][0-9]*\\b'
                }, {
                    token: 'keyword.operator',
                    regex: '\\b(?:and|or|not)\\b'
                }, {
                    token: "paren.lparen",
                    regex: "[\\(]"
                }, {
                    token: "paren.rparen",
                    regex: "[\\)]"
                }, {
                    token: ["keyword.fii", "keyword.equals"],
                    regex: "\\b(co|fds|in|ns|re|rst|la|sc)(=)"
                }, {
                    token: ["keyword.artcode", "keyword.equals"],
                    regex: "\\b(?:ip|an|by|art|clm|ct|cx|cr|dln|de|ed|hd|hl|hlp|lp|pg|pub|rf|se|sn|td|vol)="
                }, {
                    token: "phrase",           // " string
                    regex: '".*?"'
                }, {
                    token: "text",
                    regex: "\\s+"
                }
            ]
        };
    };

    oop.inherits(FactivaHighlightRules, TextHighlightRules);

    exports.FactivaHighlightRules = FactivaHighlightRules;
});
