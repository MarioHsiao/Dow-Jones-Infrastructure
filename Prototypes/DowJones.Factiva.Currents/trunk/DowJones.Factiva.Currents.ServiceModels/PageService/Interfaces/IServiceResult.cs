using DowJones.Pages;
using DowJones.Preferences;
using DowJones.Session;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces
{
    public interface IServiceResult : IAudit
    {
        string StatusMessage { get; set; }

        Audit Audit { get; set;  }
    }

    public interface IValidate<in T> where T : IValidate
    {
        void Validate(IControlData controlData, T request, IPreferences preferences);
    }

	public interface IValidate
	{
		bool IsValid();
	}
}
