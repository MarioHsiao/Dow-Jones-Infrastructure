using System;
using System.Text.RegularExpressions;
using DowJones.Extensions;

namespace DowJones.Web.UI
{
    public class DoTJsClientTemplateParser : IClientTemplateParser
    {
        // changing defaults to underscore style 
        // {{= }} 	=>	<%= %>		for interpolation
        // {{ }} 	=>	<% %>		for evaluation
        // {{! }} 	=>	<%! %>		for interpolation with encoding
        // {{# }} 	=>	<%# %>		for compile-time evaluation/includes and partials
        // {{## #}} =>	<%## #%>	for compile-time defines
        private const string DefaultEvaluatePattern = @"<%([\s\S]+?)%>";
        private const string DefaultInterpolatePattern = @"<%=([\s\S]+?)%>";
        private const string DefaultEncodePattern = @"<%-([\s\S]+?)%>";
        private const string DefaultUsePattern = @"<%#([\s\S]+?)%>";
        private const string DefaultDefinePattern = @"<%##\s*([\w\.$]+)\s*(\ = |=)([\s\S]+?)#%>";
        private const string DefaultConditionalStartPattern = @"<%\?([\s\S]+?)%>";
        private const string DefaultConditionalEndPattern = @"<%\?%>";
        private const string DefaultVarName = "self";
        private const bool DefaultStrip = true;
        private const bool DefaultAppend = true;

        private const string DefaultFunctionTemplate = "function {0}({1}){{{2}}}";


        #region Private - Please don't peek!

        private string Interpolate(Match m)
        {
            var code = m.Groups[1].Captures[0].Value;
            return CStart 
                    + code.Replace(@"\'", "'").Replace(@"\\", @"\").ReplaceWith(@"[\r\t\n]", " ") 
                    + CEnd;
        }

        private static string Evaluate(Match m)
        {
            var code = m.Groups[1].Captures[0].Value;
            return "';" 
                    + code.Replace(@"\'", "'").Replace(@"\\", @"\").ReplaceWith(@"[\r\t\n]", " ") 
                    + "out+='";
        }

        private string Encode(Match m)
        {
            var code = m.Groups[1].Captures[0].Value;
            return CStart
                   + code.Replace(@"\'", "'").Replace(@"\\", @"\").ReplaceWith(@"[\r\t\n]", " ")
                   + ").toString().replace(/&(?!\\w+;)/g, '&#38;').split('<').join('&#60;').split('>').join('&#62;').split('"
                   + '"'
                   + "').join('&#34;').split("
                   + '"'
                   + "'"
                   + '"'
                   + ").join('&#39;').split('/').join('&#47;'"
                   + CEnd;
        }

        private static string ConditionalStart(Match m)
        {
            var expression = m.Groups[1].Captures[0].Value;
            var code = "if(" + expression + "){";
            return "';" 
                    + code.Replace(@"\'", "'").Replace(@"\\", @"\").ReplaceWith(@"[\r\t\n]", " ") 
                    + "out+='";
        }

        private static string ConditionalEnd(Match m)
        {
            const string End = "';}out+='";
            return End;
        }

        protected string CStart
        {
            get
            {
                // optimal choice depends on platform/size of templates
                return Append ? "'+(" : "';out+=(";
            }
        }

        protected string CEnd
        {
            get { return Append ? ")+'" : ");out+='"; }
        }

        #endregion

        #region ..:: ctor and Public members ::..

        public DoTJsClientTemplateParser()
        {
            EvaluatePattern = DefaultEvaluatePattern;
            InterpolatePattern = DefaultInterpolatePattern;
            EncodePattern = DefaultEncodePattern;
            UsePattern = DefaultUsePattern;
            DefinePattern = DefaultDefinePattern;
            ConditionalStartPattern = DefaultConditionalStartPattern;
            ConditionalEndPattern = DefaultConditionalEndPattern;
            VarName = DefaultVarName;
            StripWhitespace = DefaultStrip;
            Append = DefaultAppend;

        }

        /// <summary>
        /// Gets or sets the regex pattern for interpolation with encoding.
        /// </summary>
        /// <value>
        /// The encode pattern.
        /// </value>
        public string EncodePattern { get; set; }

        /// <summary>
        /// Gets or sets the regex pattern for compile-time evaluation/includes and partials.
        /// </summary>
        /// <value>
        /// The use pattern.
        /// </value>
        public string UsePattern { get; set; }

        /// <summary>
        /// Gets or sets the regex pattern for compile-time defines.
        /// </summary>
        /// <value>
        /// The define pattern.
        /// </value>
        public string DefinePattern { get; set; }

        /// <summary>
        /// Gets or sets the conditional start.
        /// </summary>
        /// <value>
        /// The conditional start.
        /// </value>
        public string ConditionalStartPattern { get; set; }

        /// <summary>
        /// Gets or sets the conditional end.
        /// </summary>
        /// <value>
        /// The conditional end.
        /// </value>
        public string ConditionalEndPattern { get; set; }

        /// <summary>
        /// To change the default variable name, modify setting 'VarName'. 
        /// For example, if you set 'VarName' to "foo, bar" you will be able to pass 2 data instances and refer to them from the template by foo and bar.
        /// </summary>
        /// <value>
        /// The name of the var.
        /// </value>
        public string VarName { get; set; }

        /// <summary>
        /// To control whitespace use 'StripWhitespace' option, <c>true</c> - to strip, <c>false</c> - to preserve.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [strip whitespace]; otherwise, <c>false</c>.
        /// </value>
        public bool StripWhitespace { get; set; }

        /// <summary>
        /// This is performance optimization setting. It allows to tweak performance, depending on the javascript engine used and size of the template, it may produce better results with append set to <c>false</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if append; otherwise, <c>false</c>.
        /// </value>
        public bool Append { get; set; }

        #endregion

        #region IClientTemplateParser Members

        /// <summary>
        /// Gets or sets the regex pattern for evaluation.
        /// </summary>
        /// <value>
        /// The evaluate pattern.
        /// </value>
        public string EvaluatePattern { get; set; }

        /// <summary>
        /// Gets or sets the regex pattern for interpolation.
        /// </summary>
        /// <value>
        /// The interpolate pattern.
        /// </value>
        public string InterpolatePattern { get; set; }

        /// <summary>
        /// Parses an client template written with the 
        /// dotJS library implementation. Uses underscore client template syntax for backward compatibility
        /// </summary>
        /// <remarks>
        /// The <param name="functionName" /> parameter is optional.
        /// If not provided, the function will return an anonymous
        /// function that can be assigned to a variable.
        /// </remarks>
        /// <returns>
        /// A parsed client template. This is a JavaScript function in the format:
        /// function [functionName](context) { /* template */ }
        /// </returns>
        public string Parse(string html, string functionName = null)
        {
            const string WhiteSpacePattern = @"\s*<!\[CDATA\[\s*|\s*\]\]>\s*|[\r\n\t]|(\/\*[\s\S]*?\*\/)";

            // resolve defs not supported. not required in our scenario
            // var str = (!string.IsNullOrEmpty(UsePattern) || !string.IsNullOrEmpty(DefinePattern)) ? ResolveDefs(html) : html;

            var str = html;

            
            if (StripWhitespace)
                str = str.ReplaceWith(WhiteSpacePattern, String.Empty);

            // yes you can combine many str statements togetehr, but know that debugging becomes increasingly difficult
            str = str.Replace(@"\", @"\\")
                .Replace(@"'", @"\'")
                .ReplaceWith(InterpolatePattern, Interpolate)
                .ReplaceWith(EncodePattern, Encode)
                .ReplaceWith(ConditionalEndPattern, ConditionalEnd)
                .ReplaceWith(ConditionalStartPattern, ConditionalStart)
                .ReplaceWith(EvaluatePattern, Evaluate);
            str = "var out='"  + str + "';return out;";
            str = str.Replace(@"\n", @"\\n")
                .Replace(@"\t", @"\\t")
                .Replace(@"\r", @"\\r")
                .Split(@"out\+='';").Join("")
                .Split(@"var out='';out\+=").Join("var out=");

            var compiledTemplate = DefaultFunctionTemplate.FormatWith(functionName, VarName, str);

            return compiledTemplate;
        }

        #endregion


    }
}
