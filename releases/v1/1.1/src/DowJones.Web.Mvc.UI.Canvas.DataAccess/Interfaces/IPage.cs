using DowJones.Session;
using Factiva.Gateway.Utils.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    public interface IPage
    {
        void Populate(ControlData controlData, IPageRequest request, IPreferences preferences);

    }
}
