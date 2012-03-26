﻿using System.Web.Mvc.Razor;
using System.Web.Razor.Parser;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Text;
using DowJones.Web.Mvc.Razor.ClientPluginName;
using DowJones.Web.Mvc.Razor.ClientResources.Spans;

namespace DowJones.Web.Mvc.Razor
{
    public class ViewComponentCSharpRazorCodeParser : MvcCSharpRazorCodeParser
    {
        public ViewComponentCSharpRazorCodeParser()
        {
            RazorKeywords.Add("ClientPlugin", WrapSimpleBlockParser(BlockType.Directive, ParseClientPluginStatement));
            RazorKeywords.Add("ClientTemplate", WrapSimpleBlockParser(BlockType.Directive, ParseClientTemplateResourceStatement));
            RazorKeywords.Add("FrameworkResource", WrapSimpleBlockParser(BlockType.Directive, ParseFrameworkResourceStatement));
            RazorKeywords.Add("ScriptResource", WrapSimpleBlockParser(BlockType.Directive, ParseScriptResourceStatement));
            RazorKeywords.Add("StylesheetResource", WrapSimpleBlockParser(BlockType.Directive, ParseStylesheetResourceStatement));
        }

        private bool ParseClientPluginStatement(CodeBlockInfo block)
        {
            string clientPluginName = ParseFullLineStatement();
            End(new ClientPluginSpan(Context, clientPluginName));
            return false;
        }

        private bool ParseFrameworkResourceStatement(CodeBlockInfo block) 
        {
            string resourceLine = ParseFullLineStatement();
            End(new FrameworkResourceSpan(Context.CurrentLocation, resourceLine));
            return false;
        }

        private bool ParseClientTemplateResourceStatement(CodeBlockInfo block) 
        {
            string resourceLine = ParseFullLineStatement();
            End(new ClientTemplateResourceSpan(Context.CurrentLocation, resourceLine));
            return false;
        }

        private bool ParseScriptResourceStatement(CodeBlockInfo block) 
        {
            string resourceLine = ParseFullLineStatement();
            End(new ScriptResourceSpan(Context.CurrentLocation, resourceLine));
            return false;
        }

        private bool ParseStylesheetResourceStatement(CodeBlockInfo block) 
        {
            string resourceLine = ParseFullLineStatement();
            End(new StylesheetResourceSpan(Context.CurrentLocation, resourceLine));
            return false;
        }


        private string ParseFullLineStatement()
        {
            SourceLocation currentLocation = CurrentLocation;
            bool flag = RequireSingleWhiteSpace();
            AcceptedCharacters acceptedCharacters = flag ? AcceptedCharacters.None : AcceptedCharacters.Any;
            End(MetaCodeSpan.Create(Context, false, acceptedCharacters));
            
            Context.AcceptWhiteSpace(false);
            
            string fullLineText = null;
            if (ParserHelpers.IsIdentifierStart(CurrentCharacter))
            {
                using (Context.StartTemporaryBuffer())
                {
                    Context.AcceptUntil(ParserHelpers.IsNewLine);
                    fullLineText = Context.ContentBuffer.ToString().Trim();
                    Context.AcceptTemporaryBuffer();
                }
                Context.AcceptNewLine();
            }
            else
            {
                OnError(currentLocation, "Error parsing full line statement");
            }

            return fullLineText;
        }
    }
}