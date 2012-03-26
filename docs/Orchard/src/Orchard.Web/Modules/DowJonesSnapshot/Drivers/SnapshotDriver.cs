using DowJonesSnapshot.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;

namespace DowJonesSnapshot.Drivers
{
    public class SnapshotDriver : ContentPartDriver<SnapshotPart>
    {
    protected override DriverResult Display(
        SnapshotPart part, string displayType, dynamic shapeHelper)
    {
      return ContentShape("Parts_Snapshot",
          () => shapeHelper.Parts_Snapshot(
              Name: part.ComponentName,
              Data: part.ComponentData ));
    }

    //GET
    protected override DriverResult Editor(SnapshotPart part, dynamic shapeHelper)
    {
        return ContentShape("Parts_Snapshot_Edit",
          () => shapeHelper.EditorTemplate(
              TemplateName: "Parts/Snapshot",
              Model: part,
              Prefix: Prefix));
    }
 
    //POST
    protected override DriverResult Editor(SnapshotPart part, IUpdateModel updater, dynamic shapeHelper)
    {
      updater.TryUpdateModel(part, Prefix, null, null);
      return Editor(part, shapeHelper);
    }
  }
}
