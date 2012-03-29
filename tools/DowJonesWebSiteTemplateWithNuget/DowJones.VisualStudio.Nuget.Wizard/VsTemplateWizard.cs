using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using DowJones.VisualStudio.Nuget.Wizard.NugetInternal;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TemplateWizard;
using NuGet;


namespace DowJones.VisualStudio.Nuget.Wizard
{
    [Export(typeof(IVsHPTemplateWizard))]
    public class VsHPTemplateWizard : IVsHPTemplateWizard
    {
        private VsTemplateWizardInstallerConfiguration _configuration;
        private Project _project;
        private ProjectItem _projectItem;
        //private IPackagePathResolver _pathResolver;
        //private IVsProjectSystem _projectSystem;
        //private IPackageRepository _localRepository;

        //[ImportingConstructor]
        //public VsHPTemplateWizard(IPackagePathResolver pathResolver, IProjectSystem projectSystem, IPackageRepository localRepository)
        //{
        //    _pathResolver = pathResolver;
        //    _projectSystem = projectSystem;
        //    _localRepository = localRepository;
        //}

        //[ImportingConstructor]
        //public VsHPTemplateWizard(IVsProjectSystem projectSystem)
        //{
        //    _projectSystem = projectSystem;
        //}

        private DTE Dte { get; set; }


        private VsTemplateWizardInstallerConfiguration GetConfigurationFromVsTemplateFile(string vsTemplatePath)
        {
            XDocument document = LoadDocument(vsTemplatePath);

            return GetConfigurationFromXmlDocument(document, vsTemplatePath);
        }

        internal VsTemplateWizardInstallerConfiguration GetConfigurationFromXmlDocument(XDocument document,
                                                                                        string vsTemplatePath,
                                                                                        object vsExtensionManager = null)
        {
            IEnumerable<VsTemplateWizardPackageInfo> packages = Enumerable.Empty<VsTemplateWizardPackageInfo>();
            string repositoryPath = null;

            // Ignore XML namespaces since VS does not check them either when loading vstemplate files.
            XElement packagesElement = document.Root.ElementsNoNamespace("WizardData")
                .ElementsNoNamespace("packages")
                .FirstOrDefault();

            if (packagesElement != null)
            {
                RepositoryType repositoryType = GetRepositoryType(packagesElement);
                packages = GetPackages(packagesElement).ToList();

                if (packages.Count() > 0)
                {
                    repositoryPath = GetRepositoryPath(packagesElement, repositoryType, vsTemplatePath,
                                                       vsExtensionManager);
                }
            }

            return new VsTemplateWizardInstallerConfiguration(repositoryPath, packages);
        }

        private IEnumerable<VsTemplateWizardPackageInfo> GetPackages(XElement packagesElement)
        {
            var declarations = (from packageElement in packagesElement.ElementsNoNamespace("package")
                                let id = packageElement.GetOptionalAttributeValue("id")
                                let version = packageElement.GetOptionalAttributeValue("version")
                                select new { id, version }).ToList();

            SemanticVersion semVer;
            var missingOrInvalidAttributes = from declaration in declarations
                                             where
                                                 String.IsNullOrWhiteSpace(declaration.id) ||
                                                 String.IsNullOrWhiteSpace(declaration.version) ||
                                                 !SemanticVersion.TryParse(declaration.version, out semVer)
                                             select declaration;

            if (missingOrInvalidAttributes.Count() > 0)
            {
                ShowErrorMessage(
                    VsResources.TemplateWizard_InvalidPackageElementAttributes);
                throw new WizardBackoutException();
            }

            return from declaration in declarations
                   select new VsTemplateWizardPackageInfo(declaration.id, declaration.version);
        }

        private string GetRepositoryPath(XElement packagesElement, RepositoryType repositoryType, string vsTemplatePath,
                                         object vsExtensionManager)
        {
            switch (repositoryType)
            {
                case RepositoryType.Template:
                    return Path.GetDirectoryName(vsTemplatePath);
                case RepositoryType.Extension:
                    string repositoryId = packagesElement.GetOptionalAttributeValue("repositoryId");
                    if (repositoryId == null)
                    {
                        ShowErrorMessage(VsResources.TemplateWizard_MissingExtensionId);
                        throw new WizardBackoutException();
                    }


                    var extensionManagerShim = new ExtensionManagerShim(vsExtensionManager);
                    string installPath;
                    if (!extensionManagerShim.TryGetExtensionInstallPath(repositoryId, out installPath))
                    {
                        ShowErrorMessage(String.Format(VsResources.TemplateWizard_InvalidExtensionId,
                                                       repositoryId));
                        throw new WizardBackoutException();
                    }
                    return Path.Combine(installPath, "Packages");
            }
            // should not happen
            return null;
        }

        private RepositoryType GetRepositoryType(XElement packagesElement)
        {
            string repositoryAttributeValue = packagesElement.GetOptionalAttributeValue("repository");
            switch (repositoryAttributeValue)
            {
                case "extension":
                    return RepositoryType.Extension;
                case "template":
                case null:
                    return RepositoryType.Template;
                default:
                    ShowErrorMessage(String.Format(VsResources.TemplateWizard_InvalidRepositoryAttribute,
                                                   repositoryAttributeValue));
                    throw new WizardBackoutException();
            }
        }

        internal virtual XDocument LoadDocument(string path)
        {
            return XDocument.Load(path);
        }

        private void ProjectFinishedGenerating(Project project)
        {
            _project = project;
        }

        private void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            _projectItem = projectItem;
        }

        private void RunFinished()
        {
            if (_projectItem != null && _project == null)
            {
                _project = _projectItem.ContainingProject;
            }

            Debug.Assert(_project != null);
            Debug.Assert(_configuration != null);
            if (_configuration.Packages.Count() > 0)
            {
                var failedPackages = new List<VsTemplateWizardPackageInfo>();

                string repositoryPath = _configuration.RepositoryPath;

                var solutionPath = Dte.Solution.Properties.Item("Path").Value;
                var packagesPath = Path.Combine(Path.GetDirectoryName(solutionPath), "pacakages");
                var packageRepository = new AggregateRepository(
                new[] { PackageRepositoryFactory.Default.CreateRepository("https://go.microsoft.com/fwlink/?LinkID=230477"),
                        PackageRepositoryFactory.Default.CreateRepository( @"\\fsegdevw1.win.dowjones.net\WebSites\nuget\main\Packages")});

                var localRepository = new LocalPackageRepository(packagesPath);
                var packagePathResolver = new DefaultPackagePathResolver(packagesPath);
                var physicalFileSystem = new PhysicalFileSystem(packagesPath);
                var projectSystem = new VsProjectSystem(_project, physicalFileSystem);

                var packageManager = new PackageManager(packageRepository,
                    packagePathResolver,
                    physicalFileSystem);

                foreach (VsTemplateWizardPackageInfo package in _configuration.Packages)
                {
                    try
                    {
                        Dte.StatusBar.Text = String.Format(CultureInfo.CurrentCulture,
                                                           VsResources.TemplateWizard_PackageInstallStatus, package.Id,
                                                           package.Version);
                        packageManager.InstallPackage(package.Id, package.Version, false, false);


                        ProjectManager projectManager = new ProjectManager(packageRepository,
                                                                           packagePathResolver, projectSystem,
                                                                           localRepository);

                        //projectManager.AddPackageReference(package.Id);
                        //var dw = new DependentsWalker(localRepository);
                        //var deps = dw.GetDependents(localRepository.FindPackage(package.Id, package.Version));
                        //foreach (var dep in deps)
                        //{
                        //    _project.Object.References.Add(dep.)
                        //}
                        projectManager.UpdatePackageReference(package.Id);


                    }
                    catch (InvalidOperationException)
                    {
                        failedPackages.Add(package);
                    }
                }

                if (failedPackages.Count() > 0)
                {
                    var errorString = new StringBuilder();
                    errorString.AppendFormat(VsResources.TemplateWizard_FailedToInstallPackage, repositoryPath);
                    errorString.AppendLine();
                    errorString.AppendLine();
                    errorString.Append(String.Join(Environment.NewLine, failedPackages.Select(p => p.Id + "." + p.Version)));
                    ShowErrorMessage(errorString.ToString());
                }

            }
        }

        private void RunStarted(object automationObject, WizardRunKind runKind, object[] customParams)
        {
            if (runKind != WizardRunKind.AsNewProject && runKind != WizardRunKind.AsNewItem)
            {
                ShowErrorMessage(VsResources.TemplateWizard_InvalidWizardRunKind);
                throw new WizardBackoutException();
            }

            Dte = (DTE)automationObject;
            var vsTemplatePath = (string)customParams[0];
            _configuration = GetConfigurationFromVsTemplateFile(vsTemplatePath);
        }

        internal virtual void ShowErrorMessage(string message)
        {
            //MessageHelper.ShowErrorMessage(message, VsResources.TemplateWizard_ErrorDialogTitle);
        }

        void IWizard.BeforeOpeningFile(ProjectItem projectItem)
        {
            // do nothing
        }

        void IWizard.ProjectFinishedGenerating(Project project)
        {
            ProjectFinishedGenerating(project);
        }

        void IWizard.ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            ProjectItemFinishedGenerating(projectItem);
        }

        void IWizard.RunFinished()
        {
            RunFinished();
        }

        void IWizard.RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary,
                                WizardRunKind runKind, object[] customParams)
        {
            // TODO REVIEW alternatively could get body of WizardData element from replacementsDictionary["$wizarddata$"] instead of parsing vstemplate file.
            RunStarted(automationObject, runKind, customParams);
        }

        bool IWizard.ShouldAddProjectItem(string filePath)
        {
            // always add all project items
            return true;
        }

        #region Nested type: ExtensionManagerShim

        private class ExtensionManagerShim
        {
            private static Type _iInstalledExtensionType;
            private static Type _iVsExtensionManagerType;
            private static PropertyInfo _installPathProperty;
            private static Type _sVsExtensionManagerType;
            private static MethodInfo _tryGetInstalledExtensionMethod;
            private static bool _typesInitialized;

            private readonly object _extensionManager;

            public ExtensionManagerShim(object extensionManager)
            {
                InitializeTypes();
                _extensionManager = extensionManager ?? Package.GetGlobalService(_sVsExtensionManagerType);
            }

            private static void InitializeTypes()
            {
                if (_typesInitialized)
                {
                    return;
                }

                try
                {
                    Assembly extensionManagerAssembly = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(a => a.FullName.StartsWith("Microsoft.VisualStudio.ExtensionManager,"))
                        .First();
                    _sVsExtensionManagerType =
                        extensionManagerAssembly.GetType("Microsoft.VisualStudio.ExtensionManager.SVsExtensionManager");
                    _iVsExtensionManagerType =
                        extensionManagerAssembly.GetType("Microsoft.VisualStudio.ExtensionManager.IVsExtensionManager");
                    _iInstalledExtensionType =
                        extensionManagerAssembly.GetType("Microsoft.VisualStudio.ExtensionManager.IInstalledExtension");
                    _tryGetInstalledExtensionMethod = _iVsExtensionManagerType.GetMethod("TryGetInstalledExtension",
                                                                                         new[]
                                                                                             {
                                                                                                 typeof (string),
                                                                                                 _iInstalledExtensionType
                                                                                                     .
                                                                                                     MakeByRefType()
                                                                                             });
                    _installPathProperty = _iInstalledExtensionType.GetProperty("InstallPath", typeof(string));
                    if (_installPathProperty == null || _tryGetInstalledExtensionMethod == null ||
                        _sVsExtensionManagerType == null)
                    {
                        throw new Exception();
                    }

                    _typesInitialized = true;
                }
                catch
                {
                    // if any of the types or methods cannot be loaded throw an error. this indicates that some API in
                    // Microsoft.VisualStudio.ExtensionManager got changed.
                    throw new WizardBackoutException(VsResources.TemplateWizard_ExtensionManagerError);
                }
            }

            public bool TryGetExtensionInstallPath(string extensionId, out string installPath)
            {
                installPath = null;
                var parameters = new object[] { extensionId, null };
                var result = (bool)_tryGetInstalledExtensionMethod.Invoke(_extensionManager, parameters);
                if (!result)
                {
                    return false;
                }
                object extension = parameters[1];
                installPath = _installPathProperty.GetValue(extension, index: null) as string;
                return true;
            }
        }

        #endregion

        #region Nested type: RepositoryType

        private enum RepositoryType
        {
            /// <summary>
            /// Cache location relative to the template (inside the same folder as the vstemplate file)
            /// </summary>
            Template,
            /// <summary>
            /// Cache location relative to the VSIX that packages the project template
            /// </summary>
            Extension,
        }

        #endregion
    }
}