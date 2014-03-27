ace.define('ace/mode/factiva', ['require', 'exports', 'module', 'ace/lib/oop', 'ace/mode/text', 'ace/tokenizer', 'ace/mode/lucene_highlight_rules'], function (require, exports, module) {


    var oop = require("../lib/oop");
    var TextMode = require("./text").Mode;
    var Tokenizer = require("../tokenizer").Tokenizer;
    var FactivaHighlightRules = require("./lucene_highlight_rules").FactivaHighlightRules;

    var Mode = function () {
        this.$tokenizer = new Tokenizer(new FactivaHighlightRules().getRules());
    };

    oop.inherits(Mode, TextMode);

    (function () {
        this.$id = "ace/mode/factiva";
    }).call(Mode.prototype);

    exports.Mode = Mode;
}); ace.define('ace/mode/lucene_highlight_rules', ['require', 'exports', 'module', 'ace/lib/oop', 'ace/lib/lang', 'ace/mode/text_highlight_rules'], function (require, exports, module) {


    var oop = require("../lib/oop");
    var lang = require("../lib/lang");
    var TextHighlightRules = require("./text_highlight_rules").TextHighlightRules;

    var FactivaHighlightRules = function () {
        this.$rules = {
            "start": [
                {
                    token: "constant.character.asterisk",
                    regex: "[\\*]"
                }, {
                    token: 'constant.character.count',
                    regex: 'atleast[0-9]+\\b'
                }, {
                    token: 'constant.character.proximity',
                    regex: 'near[0-9]*\\b'
                }, {
                    token: 'constant.character.wordcount',
                    regex: 'wc[<|>][1-9][0-9]*\\b'
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
                },{
                    token: "keyword.traditional.region",
                    regex: "re+="
                }, {
                    token: "keyword.traditional.ticker",
                    regex: "co+="
                }, {
                    token: "keyword.traditional.factiva_code",
                    regex: "fds+="
                }, {
                    token: "keyword.traditional.industry",
                    regex: "in+="
                }, {
                    token: "keyword.traditional.language",
                    regex: "la+="
                }, {
                    token: "keyword.traditional.subject",
                    regex: "ns+="
                }, {
                    token: "keyword.traditional.source_restrictor",
                    regex: "rst+="
                }, {
                    token: "keyword.traditional.source",
                    regex: "sc+="
                }, {
                    token: "keyword.traditional.ip",
                    regex: "ip+="
                }, {
                    token: "string",           // " string
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
