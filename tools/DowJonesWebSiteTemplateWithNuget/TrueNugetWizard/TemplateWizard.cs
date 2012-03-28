using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;
using DowJones.VisualStudio.Nuget.Wizard;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TemplateWizard;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;


namespace DowJones.VisualStudio.Nuget
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
           Justification = "This class is referenced via .vstemplate files from a VS project template")]
    internal sealed class TemplateWizard : IWizard
    {
        [Import]
        internal  IVsHPTemplateWizard Wizard { get; set; }


        //private Project _project;
        //private ProjectItem _projectItem;
        //private DTE DTE { get; set; }


        private void Initialize(object automationObject)
        {
            using (var serviceProvider = new ServiceProvider((IServiceProvider)automationObject))
            {
                var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
                using (var container = new CompositionContainer(componentModel.DefaultExportProvider))
                {
                    container.ComposeParts(this);
                }
            }
        }

        void IWizard.BeforeOpeningFile(ProjectItem projectItem)
        {
            Wizard.BeforeOpeningFile(projectItem);
        }

        void IWizard.ProjectFinishedGenerating(Project project)
        {
            Wizard.ProjectFinishedGenerating(project);
            //_project = project;
        }

        void IWizard.ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            Wizard.ProjectItemFinishedGenerating(projectItem);
            //_projectItem = projectItem;

        }

        void IWizard.RunFinished()
        {
            Wizard.RunFinished();
        }

        void IWizard.RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            //DTE = (DTE)automationObject;
            //var vsTemplatePath = (string)customParams[0];
            Initialize(automationObject);
            Wizard.RunStarted(automationObject, replacementsDictionary, runKind, customParams);
            
        }

        bool IWizard.ShouldAddProjectItem(string filePath)
        {
            return Wizard.ShouldAddProjectItem(filePath);
        }
    }
}
