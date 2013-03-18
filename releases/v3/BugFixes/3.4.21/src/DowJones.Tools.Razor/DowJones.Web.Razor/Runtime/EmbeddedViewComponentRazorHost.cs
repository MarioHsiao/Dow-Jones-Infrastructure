using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser;
using System.Web.WebPages.Razor;
using DowJones.Web.Razor.Keywords.ClientPlugin;

namespace DowJones.Web.Razor.Runtime
{
    public class EmbeddedViewComponentRazorHost : WebPageRazorHost
    {
        public const string SectionMethodName = "DefineSection";
        public const string ExecuteMethodName = "ExecuteTemplate";
        public const string DefaultBaseClassName = "DowJones.Web.Mvc.UI.ViewComponentBase";

        public static readonly IEnumerable<string> ExcludedNamespaces = new[] {
                "System.Web.Helpers",
                "System.Web.WebPages",
                "System.Web.WebPages.Html",
            };

        public static readonly IEnumerable<string> IncludedNamespaces = new[] {
                "DowJones.Web.Mvc.Extensions",
            };


        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        public EmbeddedViewComponentRazorHost(string virtualPath)
            : base(virtualPath)
        {
            DefaultNamespace = DetermineNamespace(virtualPath);
            DefaultBaseClass = DefaultBaseClassName;
            DefaultDebugCompilation = false;
            GeneratedClassContext = GetGeneratedClassContext();
            StaticHelpers = true;
        }

        public EmbeddedViewComponentRazorHost(string virtualPath, string physicalPath)
            : base(virtualPath, physicalPath)
        {
            DefaultBaseClass = DefaultBaseClassName;
            DefaultNamespace = DetermineNamespace(virtualPath);
            GeneratedClassContext = GetGeneratedClassContext();
            DefaultDebugCompilation = false;
            StaticHelpers = true;
        }
        // ReSharper restore DoNotCallOverridableMethodsInConstructor


        private static GeneratedClassContext GetGeneratedClassContext()
        {
            return new GeneratedClassContext(
                ExecuteMethodName,
                GeneratedClassContext.DefaultWriteMethodName,
                GeneratedClassContext.DefaultWriteLiteralMethodName,
                null, null, null, SectionMethodName);
        }

        public override MarkupParser CreateMarkupParser()
        {
            return new HtmlMarkupParser();
        }

        public override RazorCodeGenerator DecorateCodeGenerator(RazorCodeGenerator incomingCodeGenerator)
        {
            if (incomingCodeGenerator is CSharpRazorCodeGenerator)
            {
                return new ViewComponentCSharpRazorCodeGenerator(incomingCodeGenerator.ClassName, incomingCodeGenerator.RootNamespaceName, incomingCodeGenerator.SourceFileName, incomingCodeGenerator.Host);
            }
            if (incomingCodeGenerator is VBRazorCodeGenerator)
            {
                return new MvcVBRazorCodeGenerator(incomingCodeGenerator.ClassName, incomingCodeGenerator.RootNamespaceName, incomingCodeGenerator.SourceFileName, incomingCodeGenerator.Host);
            }
            return base.DecorateCodeGenerator(incomingCodeGenerator);
        }

        public override ParserBase DecorateCodeParser(ParserBase incomingCodeParser)
        {
            if (incomingCodeParser is CSharpCodeParser)
            {
                return new ViewComponentCSharpRazorCodeParser();
            }
            if (incomingCodeParser is VBCodeParser)
            {
                return new MvcVBRazorCodeParser();
            }
            return base.DecorateCodeParser(incomingCodeParser);
        }

        private static string DetermineNamespace(string virtualPath)
        {
            virtualPath = virtualPath.Replace(Path.DirectorySeparatorChar, '/');
            virtualPath = GetDirectory(virtualPath);
            IEnumerable<string> source = virtualPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (!source.Any())
            {
                return "ASP";
            }
            return ("ASP." + string.Join(".", source));
        }

        protected override string GetClassName(string virtualPath)
        {
            return ParserHelpers.SanitizeClassName(Path.GetFileNameWithoutExtension(virtualPath));
        }

        private static string GetDirectory(string virtualPath)
        {
            int length = virtualPath.LastIndexOf('/');
            if (length != -1)
            {
                return virtualPath.Substring(0, length);
            }
            return string.Empty;
        }

        public override void PostProcessGeneratedCode(CodeCompileUnit codeCompileUnit, CodeNamespace generatedNamespace, CodeTypeDeclaration generatedClass, CodeMemberMethod executeMethod)
        {
            base.PostProcessGeneratedCode(codeCompileUnit, generatedNamespace, generatedClass, executeMethod);

            RemoveWebMatrixNamespaces(generatedNamespace);

            AddDowJonesNamespaces(generatedNamespace);

            EnsureClientPluginName(generatedClass);

            RemoveApplicationInstanceProperty(generatedClass);
        }

        private static void AddDowJonesNamespaces(CodeNamespace codeNamespace)
        {
            var namespaces = IncludedNamespaces.Select(x => new CodeNamespaceImport(x));
            codeNamespace.Imports.AddRange(namespaces.ToArray());
        }

        private static void EnsureClientPluginName(CodeTypeDeclaration generatedClass)
        {
            bool hasClientPluginNameProperty =
                generatedClass.Members.Cast<CodeTypeMember>()
                    .Any(x => x.Name == ClientPluginNameProperty.PropertyName);

            if (hasClientPluginNameProperty)
                return;

            generatedClass.Members.Add(new ClientPluginNameProperty((string)null));
        }

        private static void RemoveApplicationInstanceProperty(CodeTypeDeclaration generatedClass)
        {
            CodeMemberProperty applicationInstanceProperty =
                generatedClass.Members.OfType<CodeMemberProperty>().SingleOrDefault(
                    p => "ApplicationInstance".Equals(p.Name));

            if (applicationInstanceProperty != null)
            {
                generatedClass.Members.Remove(applicationInstanceProperty);
            }
        }

        private static void RemoveWebMatrixNamespaces(CodeNamespace codeNamespace)
        {
            List<CodeNamespaceImport> imports =
                codeNamespace.Imports.OfType<CodeNamespaceImport>()
                    .Where(import => !ExcludedNamespaces.Contains(import.Namespace))
                    .ToList();

            codeNamespace.Imports.Clear();

            imports.ForEach(import => codeNamespace.Imports.Add(import));
        }
    }
}
