using DowJones.Session;
using Factiva.Gateway.Utils.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    public interface IModuleList
    {
        void Populate(ControlData controlData, IModuleListRequest request, IPreferences preferences);

    }
}
