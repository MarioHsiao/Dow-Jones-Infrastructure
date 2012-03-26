using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;
using DowJones.Web.Mvc.Razor.RegistrationAttributes;
using Microsoft.VisualStudio.Shell;
using VSLangProj80;

namespace DowJones.Web.Mvc.Razor
{
    [ComVisible(true)]
    [Guid("98B6471D-B3EC-48B9-805E-37D1F7D9C0A1")]
    [CodeGeneratorRegistration(typeof(RazorViewComponentClassGenerator), "C# Razor Generator (.cshtml)", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true)]
    [CodeGeneratorRegistration(typeof(RazorViewComponentClassGenerator), "C# Razor View Component Generator (.csrazor)", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(RazorViewComponentClassGenerator))]
    public class RazorViewComponentClassGenerator : BaseCodeGeneratorWithSite
    {
#pragma warning disable 0414
        //The name of this generator (use for 'Custom Tool' property of project item)
        internal static string name = "RazorViewComponentClassGenerator";
#pragma warning restore 0414

        private static readonly string RazorClassGeneratorVersion = 
            typeof(RazorViewComponentClassGenerator).Assembly.GetName().Version.ToString();

        public CodeGeneratorOptions CodeGeneratorOptions
        {
            get
            {
                return _codeGeneratorOptions ??
                    new CodeGeneratorOptions
                           {
                               BlankLinesBetweenMembers = false,
                               BracingStyle = "C"
                           };
            }
            set { _codeGeneratorOptions = value; }
        }
        private CodeGeneratorOptions _codeGeneratorOptions;

        /// <summary>
        /// Function that builds the contents of the generated file based on the contents of the input file
        /// </summary>
        /// <param name="inputFileContent">Content of the input file</param>
        /// <returns>Generated file as a byte array</returns>
        protected override byte[] GenerateCode(string inputFileContent)
        {
            try
            {
                var templateGenerator = GetRazorTemplateGenerator();

                GeneratorResults results = templateGenerator.GenerateRazorTemplate(inputFileContent);

                ReportGenerationErrors(results);

                // Report that we are 1/2 done
                UpdateProgress(50);

                AddGeneratedClassAttribute(results.GeneratedCode);

                CodeDomProvider provider = GetCodeProvider();

                var viewComponent = templateGenerator.GenerateViewComponent(results, provider, CodeGeneratorOptions);
                
                UpdateProgress(100);

                return viewComponent.GetGeneratedCodeBytes();
            }
            catch (Exception e)
            {
                GeneratorError(4, e.ToString(), 1, 1);
                //Returning null signifies that generation has failed
                return null;
            }
        }

        private RazorViewComponentTemplateGenerator GetRazorTemplateGenerator()
        {
            // Get the root folder of the project
            string appRoot = Path.GetDirectoryName(GetProject().FullName);

            if (appRoot == null)
                throw new ApplicationException("Invalid application root folder!");

            // Determine the project-relative path
            string projectRelativePath = InputFilePath.Substring(appRoot.Length);

            // Turn it into a virtual path by prepending ~ and fixing it up
            string virtualPath = VirtualPathUtility.ToAppRelative("~" + projectRelativePath);

            var generator = new RazorViewComponentTemplateGenerator(virtualPath, InputFilePath, FileNameSpace);
            
            return generator;
        }

        private void UpdateProgress(uint progressPercentage)
        {
            if (CodeGeneratorProgress != null)
            {
                CodeGeneratorProgress.Progress(progressPercentage, 100);
            }
        }

        private void ReportGenerationErrors(GeneratorResults results)
        {
            foreach (RazorError error in results.ParserErrors)
            {
                GeneratorError(4, error.Message, (uint)error.Location.LineIndex + 1,
                               (uint)error.Location.CharacterIndex + 1);
            }
        }


        private static void AddGeneratedClassAttribute(CodeCompileUnit generatedCode)
        {
            CodeNamespace ns = generatedCode.Namespaces[0];
            CodeTypeDeclaration generatedType = ns.Types[0];

            generatedType.CustomAttributes.Add(
                new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(GeneratedCodeAttribute)),
                    new CodeAttributeArgument(new CodePrimitiveExpression("RazorViewComponentClassGenerator")),
                    new CodeAttributeArgument(new CodePrimitiveExpression(RazorClassGeneratorVersion))));

            string lastGeneratedTimestamp = string.Format("Last Generated Timestamp: {0:MM/dd/yyyy hh:mm tt}", DateTime.Now);
            generatedType.Comments.Add(new CodeCommentStatement(lastGeneratedTimestamp));
        }
    }
}