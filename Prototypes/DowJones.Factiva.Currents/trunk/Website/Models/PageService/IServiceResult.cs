using DowJones.Preferences;
using DowJones.Session;
using IAudit = DowJones.Pages.IAudit;

namespace DowJones.Factiva.Currents.Website.Models.PageService
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
