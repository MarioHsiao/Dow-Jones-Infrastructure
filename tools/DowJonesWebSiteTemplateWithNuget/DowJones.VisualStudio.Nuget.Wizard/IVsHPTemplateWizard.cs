using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TemplateWizard;

namespace DowJones.VisualStudio.Nuget.Wizard
{
    [ComImport]
    [Guid("FE13F055-98D1-4269-B2FE-77A08A4B03CF")]
    public interface IVsHPTemplateWizard : IWizard
    {
    }
}