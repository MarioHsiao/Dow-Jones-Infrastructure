using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.UI
{

    [TestClass]
    public class ClientTemplateProcessorTests : UnitTestFixtureBase<IClientTemplateParser>
    {
        protected IClientTemplateParser Parser
        {
            get { return UnitUnderTest; }
        }

        [TestMethod]
        public void ParseBasicTemplate()
        {
            const string html = "<% if (x.contentCategoryDescriptor === \"external\") { %><span class=\"fi\"><%= x.headlineUrl %></span><% } %>";
            const string expectedTemplate = "function (obj) { var __p=[],print=function(){__p.push.apply(__p,arguments);}; with(obj||{}){__p.push(''); if (x.contentCategoryDescriptor === \"external\") { __p.push('<span class=\"fi\">', x.headlineUrl ,'</span>'); } __p.push('');}return __p.join(''); }";

            var actualTemplate = Parser.Parse(html);
            Assert.AreEqual(expectedTemplate.Trim(), actualTemplate);
        }

        [TestMethod]
        public void ParseComplexTemplate()
        {
            const string html = "<ul class=\"article-list\">\r\n    <% for (var i = 0, len = Math.min(headlines.length, options.maxNumHeadlinesToShow); i < len; i++) { \r\n            x = headlines[i]; \r\n            showSource = options.showSource && x.sourceCode && x.sourceDescriptor;\r\n            showPublicationDateTime = options.showSource && options.showPublicationDateTime;\r\n            showAuthor = options.showAuthor && x.author;\r\n    %>\r\n    <li class=\"dj_entry\">\r\n        <div class=\"article-wrap\">\r\n            <% if (showSource || showPublicationDateTime) { %>\r\n            <div class=\"article-meta\">\r\n                <% if (showSource) { %>\r\n                    <span class=\"article-source\" rel=\"<%= x.sourceCode %>\"><%= x.sourceDescriptor %></span>\r\n                <% } \r\n                   if (showSource && showPublicationDateTime) %>\r\n                    <span> - </span>\r\n                <% } \r\n                   if (showPublicationDateTime) { %>\r\n                    <span class=\"date-stamp\"><%= x.publicationDateTimeDescriptor %></span>\r\n                <% } %>\r\n            </div>\r\n            <% } %>\r\n            <h4 class=\"article-headline\">\r\n                <a href=\"<%= x.headlineUrl %>\" class=\"article-view-trigger\" title=\"<%= x.toolTip %>\">\r\n                    <%= x.title %></a>\r\n                <% if (x.contentCategoryDescriptor === \"external\") { %>\r\n                    <span class=\"fi\"></span>\r\n                <% } %>\r\n            </h4>\r\n            <% if (showAuthor) { %>\r\n            <div class=\"article-meta\">\r\n                <span class=\"byline\">By:</span><span class=\"author\"><%= x.author.value %></span>\r\n            </div>\r\n            <% } \r\n             /* if we have a snippet and mode is anything but none, render the first snippet only if mode is hybrid; else render all snippets if mode is inline */ \r\n              if (x.snippets && (options.displaySnippets === 1 || (options.displaySnippets === 2 && index === 0))) { \r\n                  for (var j = 0, cnt = x.snippets.length; j < cnt; j++) { %>\r\n                        <p class=\"article-snip\">\r\n                            <span class=\"dj_text\" rel=\"\"><%= x.snippets[j] %></span>\r\n                        </p>\r\n            <%    }\r\n              } %>\r\n        </div>\r\n    </li>\r\n    <% } %>\r\n</ul>\r\n";
            const string expectedTemplate = "function (obj) { var __p=[],print=function(){__p.push.apply(__p,arguments);}; with(obj||{}){__p.push('<ul class=\"article-list\">\\r\\n    '); for (var i = 0, len = Math.min(headlines.length, options.maxNumHeadlinesToShow); i < len; i++) {               x = headlines[i];               showSource = options.showSource && x.sourceCode && x.sourceDescriptor;              showPublicationDateTime = options.showSource && options.showPublicationDateTime;              showAuthor = options.showAuthor && x.author;      __p.push('\\r\\n    <li class=\"dj_entry\">\\r\\n        <div class=\"article-wrap\">\\r\\n            '); if (showSource || showPublicationDateTime) { __p.push('\\r\\n            <div class=\"article-meta\">\\r\\n                '); if (showSource) { __p.push('\\r\\n                    <span class=\"article-source\" rel=\"', x.sourceCode ,'\">', x.sourceDescriptor ,'</span>\\r\\n                '); }                      if (showSource && showPublicationDateTime) __p.push('\\r\\n                    <span> - </span>\\r\\n                '); }                      if (showPublicationDateTime) { __p.push('\\r\\n                    <span class=\"date-stamp\">', x.publicationDateTimeDescriptor ,'</span>\\r\\n                '); } __p.push('\\r\\n            </div>\\r\\n            '); } __p.push('\\r\\n            <h4 class=\"article-headline\">\\r\\n                <a href=\"', x.headlineUrl ,'\" class=\"article-view-trigger\" title=\"', x.toolTip ,'\">\\r\\n                    ', x.title ,'</a>\\r\\n                '); if (x.contentCategoryDescriptor === \"external\") { __p.push('\\r\\n                    <span class=\"fi\"></span>\\r\\n                '); } __p.push('\\r\\n            </h4>\\r\\n            '); if (showAuthor) { __p.push('\\r\\n            <div class=\"article-meta\">\\r\\n                <span class=\"byline\">By:</span><span class=\"author\">', x.author.value ,'</span>\\r\\n            </div>\\r\\n            '); }                /* if we have a snippet and mode is anything but none, render the first snippet only if mode is hybrid; else render all snippets if mode is inline */                 if (x.snippets && (options.displaySnippets === 1 || (options.displaySnippets === 2 && index === 0))) {                     for (var j = 0, cnt = x.snippets.length; j < cnt; j++) { __p.push('\\r\\n                        <p class=\"article-snip\">\\r\\n                            <span class=\"dj_text\" rel=\"\">', x.snippets[j] ,'</span>\\r\\n                        </p>\\r\\n            ');    }                } __p.push('\\r\\n        </div>\\r\\n    </li>\\r\\n    '); } __p.push('\\r\\n</ul>\\r\\n');}return __p.join(''); }";

            var actualTemplate = Parser.Parse(html);
            Assert.AreEqual(expectedTemplate.Trim(), actualTemplate);
        }

        [TestMethod]
        public void ShouldNotSpitOutInvalidJavaScriptForTemplatesThatContainSingleQuotes()
        {
            const string clientTemplateText = @"<span>C'est la vie</span><% if(x === 'multiMedia') return true;%>";
            const string expectedTemplate = @"function (obj) { var __p=[],print=function(){__p.push.apply(__p,arguments);}; with(obj||{}){__p.push('<span>C\'est la vie</span>'); if(x === 'multiMedia') return true;__p.push('');}return __p.join(''); }";

            var template = Parser.Parse(clientTemplateText);

            //Assert.IsTrue(
            //    template.Contains(
            //        clientTemplateText.Replace("'", @"\'"))
            //);

            Assert.AreEqual(expectedTemplate.Trim(), template);
        }



        protected override IClientTemplateParser CreateUnitUnderTest()
        {
            return new UnderscoreClientTemplateParser();
        }
    }
}
