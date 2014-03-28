ace.define('ace/mode/factiva', ['require', 'exports', 'module', 'ace/lib/oop', 'ace/mode/text', 'ace/tokenizer', 'ace/mode/factiva_highlight_rules', 'ace/mode/behaviour/fstyle'], function (require, exports, module) {


    var oop = require("../lib/oop");
    var TextMode = require("./text").Mode;
    var Tokenizer = require("../tokenizer").Tokenizer;
    var FactivaHighlightRules = require("./factiva_highlight_rules").FactivaHighlightRules;
    var Behaviour = require("./behaviour").Behaviour;
    var FstyleBehaviour = require("./behaviour/fstyle").FstyleBehaviour;

    var Mode = function () {
        TextMode.call(this);
        var highlighter = new FactivaHighlightRules();
        this.$tokenizer = new Tokenizer(highlighter.getRules());
        this.$highlightRules = FactivaHighlightRules;
        this.$behaviour = new FstyleBehaviour();
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
                    token: 'constant.character.proximity',
                    regex: '\\N[1-9][0-9]*\\b'
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


ace.define('ace/mode/behaviour/fstyle', ['require', 'exports', 'module', 'ace/lib/oop', 'ace/mode/behaviour', 'ace/token_iterator', 'ace/lib/lang'], function (require, exports, module) {


    var oop = require("../../lib/oop");
    var Behaviour = require("../behaviour").Behaviour;
    var TokenIterator = require("../../token_iterator").TokenIterator;
    var lang = require("../../lib/lang");

    var SAFE_INSERT_IN_TOKENS =
        ["text", "paren.rparen"];
    var SAFE_INSERT_BEFORE_TOKENS =
        ["text", "paren.rparen"];

    var context;
    var contextCache = {}
    var initContext = function (editor) {
        var id = -1;
        if (editor.multiSelect) {
            id = editor.selection.id;
            if (contextCache.rangeCount != editor.multiSelect.rangeCount)
                contextCache = { rangeCount: editor.multiSelect.rangeCount };
        }
        if (contextCache[id])
            return context = contextCache[id];
        context = contextCache[id] = {
            autoInsertedBrackets: 0,
            autoInsertedRow: -1,
            autoInsertedLineEnd: "",
            maybeInsertedBrackets: 0,
            maybeInsertedRow: -1,
            maybeInsertedLineStart: "",
            maybeInsertedLineEnd: ""
        };
    };

    var FstyleBehaviour = function () {
        
        this.add("parens", "insertion", function (state, action, editor, session, text) {
            if (text == '(') {
                initContext(editor);
                var selection = editor.getSelectionRange();
                var selected = session.doc.getTextRange(selection);
                if (selected !== "" && editor.getWrapBehavioursEnabled()) {
                    return {
                        text: '(' + selected + ')',
                        selection: false
                    };
                } else if (FstyleBehaviour.isSaneInsertion(editor, session)) {
                    FstyleBehaviour.recordAutoInsert(editor, session, ")");
                    return {
                        text: '()',
                        selection: [1, 1]
                    };
                }
            } else if (text == ')') {
                initContext(editor);
                var cursor = editor.getCursorPosition();
                var line = session.doc.getLine(cursor.row);
                var rightChar = line.substring(cursor.column, cursor.column + 1);
                if (rightChar == ')') {
                    var matching = session.$findOpeningBracket(')', { column: cursor.column + 1, row: cursor.row });
                    if (matching !== null && FstyleBehaviour.isAutoInsertedClosing(cursor, line, text)) {
                        FstyleBehaviour.popAutoInsertedClosing();
                        return {
                            text: '',
                            selection: [1, 1]
                        };
                    }
                }
            }
        });

        this.add("parens", "deletion", function (state, action, editor, session, range) {
            var selected = session.doc.getTextRange(range);
            if (!range.isMultiLine() && selected == '(') {
                initContext(editor);
                var line = session.doc.getLine(range.start.row);
                var rightChar = line.substring(range.start.column + 1, range.start.column + 2);
                if (rightChar == ')') {
                    range.end.column++;
                    return range;
                }
            }
        });
    };


    FstyleBehaviour.isSaneInsertion = function (editor, session) {
        var cursor = editor.getCursorPosition();
        var iterator = new TokenIterator(session, cursor.row, cursor.column);
        if (!this.$matchTokenType(iterator.getCurrentToken() || "text", SAFE_INSERT_IN_TOKENS)) {
            var iterator2 = new TokenIterator(session, cursor.row, cursor.column + 1);
            if (!this.$matchTokenType(iterator2.getCurrentToken() || "text", SAFE_INSERT_IN_TOKENS))
                return false;
        }
        iterator.stepForward();
        return iterator.getCurrentTokenRow() !== cursor.row ||
            this.$matchTokenType(iterator.getCurrentToken() || "text", SAFE_INSERT_BEFORE_TOKENS);
    };

    FstyleBehaviour.$matchTokenType = function (token, types) {
        return types.indexOf(token.type || token) > -1;
    };

    FstyleBehaviour.recordAutoInsert = function (editor, session, bracket) {
        var cursor = editor.getCursorPosition();
        var line = session.doc.getLine(cursor.row);
        if (!this.isAutoInsertedClosing(cursor, line, context.autoInsertedLineEnd[0]))
            context.autoInsertedBrackets = 0;
        context.autoInsertedRow = cursor.row;
        context.autoInsertedLineEnd = bracket + line.substr(cursor.column);
        context.autoInsertedBrackets++;
    };

    FstyleBehaviour.recordMaybeInsert = function (editor, session, bracket) {
        var cursor = editor.getCursorPosition();
        var line = session.doc.getLine(cursor.row);
        if (!this.isMaybeInsertedClosing(cursor, line))
            context.maybeInsertedBrackets = 0;
        context.maybeInsertedRow = cursor.row;
        context.maybeInsertedLineStart = line.substr(0, cursor.column) + bracket;
        context.maybeInsertedLineEnd = line.substr(cursor.column);
        context.maybeInsertedBrackets++;
    };

    FstyleBehaviour.isAutoInsertedClosing = function (cursor, line, bracket) {
        return context.autoInsertedBrackets > 0 &&
            cursor.row === context.autoInsertedRow &&
            bracket === context.autoInsertedLineEnd[0] &&
            line.substr(cursor.column) === context.autoInsertedLineEnd;
    };

    FstyleBehaviour.isMaybeInsertedClosing = function (cursor, line) {
        return context.maybeInsertedBrackets > 0 &&
            cursor.row === context.maybeInsertedRow &&
            line.substr(cursor.column) === context.maybeInsertedLineEnd &&
            line.substr(0, cursor.column) == context.maybeInsertedLineStart;
    };

    FstyleBehaviour.popAutoInsertedClosing = function () {
        context.autoInsertedLineEnd = context.autoInsertedLineEnd.substr(1);
        context.autoInsertedBrackets--;
    };

    FstyleBehaviour.clearMaybeInsertedClosing = function () {
        if (context) {
            context.maybeInsertedBrackets = 0;
            context.maybeInsertedRow = -1;
        }
    };



    oop.inherits(FstyleBehaviour, Behaviour);

    exports.FstyleBehaviour = FstyleBehaviour;
});
