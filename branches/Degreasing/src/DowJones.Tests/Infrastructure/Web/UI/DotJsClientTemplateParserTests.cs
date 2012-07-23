using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Web.UI;

namespace DowJones.Infrastructure.Web.UI
{
    [TestClass]
    public class DotJsClientTemplateParserTests : UnitTestFixtureBase<IClientTemplateParser>
    {
        protected IClientTemplateParser Parser
        {
            get { return UnitUnderTest; }
        }

        [TestMethod]
        public void ParseBasicTemplate()
        {
            const string html = "<h1><%=self.title%></h1>";
            const string expectedTemplate = "function (self){var out='<h1>'+(self.title)+'</h1>';return out;}";

            var actualTemplate = Parser.Parse(html);
            Assert.AreEqual(expectedTemplate.Trim(), actualTemplate);
        }

        [TestMethod]
        public void ParseComplexTemplate()
        {
            const string html = "<ul class=\"article-list\">\r\n	<%  var showSource, showPublicationDateTime, showAuthor, title, x, multimediaMode;\r\n		for (var i = 0, len = Math.min(headlines.length, options.maxNumHeadlinesToShow); i < len; i++) { \r\n			x = headlines[i]; \r\n			showSource = options.showSource && x.sourceCode && x.sourceDescriptor;\r\n			showPublicationDateTime = options.showPublicationDateTime && x.publicationDateTimeDescriptor;\r\n			showAuthor = options.showAuthor && x.authors && x.authors.length > 0;\r\n			title = (options.showTruncatedTitle && $.trim(x.truncatedTitle)) || x.title;\r\n			multimediaMode = options.multimediaMode && (x.contentCategoryDescriptor && x.contentCategoryDescriptor === 'multimedia');\r\n	%>\r\n	<li class=\"dj_entry\">\r\n		<div class=\"article-wrap\">\r\n			<h4 class=\"article-headline\">\r\n				<a href=\"<%= x.headlineUrl %>\" class=\"article-view-trigger\">\r\n					<%= title %></a>\r\n			</h4>\r\n			<% if (showSource || showPublicationDateTime) { %>\r\n			<div class=\"article-meta\">\r\n				<% if (showSource) { \r\n					if (options.sourceClickable) { %>\r\n						<span class=\"article-source source-clickable\" rel=\"<%= x.sourceCode %>\"><%= x.sourceDescriptor %></span><br />\r\n				 <% } else { %>\r\n						<span class=\"article-source\"><%= x.sourceDescriptor %></span><br />\r\n				 <% } %>\r\n				<% } \r\n				   if (showPublicationDateTime) { %>\r\n					<span class=\"date-stamp\"><%= x.publicationDateTimeDescriptor %></span>\r\n				<% } %>\r\n			</div>\r\n			<% } \r\n			   if (showAuthor) { %>\r\n				<div class=\"article-meta\">\r\n				 <% \r\n					if ( options.authorClickable && x.codedAuthors) {\r\n						for (var j = 0, jCnt = x.codedAuthors.length; j < jCnt ; j++) {\r\n							var author = x.codedAuthors[j]; \r\n							for (var k = 0, kCnt = author.items.length; k < kCnt ; k++) { \r\n								var item = author.items[k];\r\n								switch(item.entityTypeDescriptor.toLowerCase()) {\r\n									case \"person\":\r\n									case \"author\": %>\r\n										<span class=\"article-author article-clickable\" rel=\"<%= item.guid %>\"><%= item.value %></span>                                    \r\n									<%  break;\r\n									case \"textual\":\r\n									default: %>\r\n										<span class=\"article-author\"><%= item.value %></span>\r\n									<%  break;\r\n								}\r\n							}       \r\n						} \r\n					} \r\n					else { %>\r\n						<span class=\"author\"><%= x.authors.join(', ') %></span>\r\n				 <% } %>\r\n				</div>\r\n			<% } \r\n			/* if we have a snippet and mode is anything but none, render the first snippet only if mode is hybrid; else render all snippets if mode is inline --> */ \r\n			 if (x.snippets && (options.displaySnippets === 1 || (options.displaySnippets === 2 && i === 0))) { \r\n				for (var j = 0, cnt = x.snippets.length; j < cnt; j++) { %>\r\n					  <p class=\"article-snip\">\r\n						  <span class=\"dj_text\" rel=\"\"><%= x.snippets[j] %></span>\r\n					  </p>\r\n			<%  } } %>\r\n			<% if (multimediaMode) { %>\r\n				<div class=\"article-meta\">\r\n					<span class=\"fi fi_<%= x.contentSubCategoryDescriptor %>\"> </span>\r\n					<span class=\"media-length\">[<%= x.mediaLength %>]</span>\r\n				</div>\r\n			<% } %>\r\n		</div>\r\n	</li>\r\n	<% } %>\r\n</ul>";
            const string expectedTemplate = "function (self){var out='<ul class=\"article-list\">';  var showSource, showPublicationDateTime, showAuthor, title, x, multimediaMode;for (var i = 0, len = Math.min(headlines.length, options.maxNumHeadlinesToShow); i < len; i++) { x = headlines[i]; showSource = options.showSource && x.sourceCode && x.sourceDescriptor;showPublicationDateTime = options.showPublicationDateTime && x.publicationDateTimeDescriptor;showAuthor = options.showAuthor && x.authors && x.authors.length > 0;title = (options.showTruncatedTitle && $.trim(x.truncatedTitle)) || x.title;multimediaMode = options.multimediaMode && (x.contentCategoryDescriptor && x.contentCategoryDescriptor === 'multimedia');out+='<li class=\"dj_entry\"><div class=\"article-wrap\"><h4 class=\"article-headline\"><a href=\"'+( x.headlineUrl )+'\" class=\"article-view-trigger\">'+( title )+'</a></h4>'; if (showSource || showPublicationDateTime) { out+='<div class=\"article-meta\">'; if (showSource) { if (options.sourceClickable) { out+='<span class=\"article-source source-clickable\" rel=\"'+( x.sourceCode )+'\">'+( x.sourceDescriptor )+'</span><br /> '; } else { out+='<span class=\"article-source\">'+( x.sourceDescriptor )+'</span><br /> '; }  }    if (showPublicationDateTime) { out+='<span class=\"date-stamp\">'+( x.publicationDateTimeDescriptor )+'</span>'; } out+='</div>'; }    if (showAuthor) { out+='<div class=\"article-meta\"> '; if ( options.authorClickable && x.codedAuthors) {for (var j = 0, jCnt = x.codedAuthors.length; j < jCnt ; j++) {var author = x.codedAuthors[j]; for (var k = 0, kCnt = author.items.length; k < kCnt ; k++) { var item = author.items[k];switch(item.entityTypeDescriptor.toLowerCase()) {case \"person\":case \"author\": out+='<span class=\"article-author article-clickable\" rel=\"'+( item.guid )+'\">'+( item.value )+'</span>                                    ';  break;case \"textual\":default: out+='<span class=\"article-author\">'+( item.value )+'</span>';  break;}}       } } else { out+='<span class=\"author\">'+( x.authors.join(', ') )+'</span> '; } out+='</div>'; }   if (x.snippets && (options.displaySnippets === 1 || (options.displaySnippets === 2 && i === 0))) { for (var j = 0, cnt = x.snippets.length; j < cnt; j++) { out+='  <p class=\"article-snip\">  <span class=\"dj_text\" rel=\"\">'+( x.snippets[j] )+'</span>  </p>';  } }  if (multimediaMode) { out+='<div class=\"article-meta\"><span class=\"fi fi_'+( x.contentSubCategoryDescriptor )+'\"> </span><span class=\"media-length\">['+( x.mediaLength )+']</span></div>'; } out+='</div></li>'; } out+='</ul>';return out;}";

            var actualTemplate = Parser.Parse(html);
            Assert.AreEqual(expectedTemplate.Trim(), actualTemplate);
        }

        protected override IClientTemplateParser CreateUnitUnderTest()
        {
            return new DoTJsClientTemplateParser();
        }
    }
}
