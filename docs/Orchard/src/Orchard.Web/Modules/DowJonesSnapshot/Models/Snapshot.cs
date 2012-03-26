using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace DowJonesSnapshot.Models
{
    public class SnapshotRecord : ContentPartRecord
    {
        public virtual string ComponentName { get; set; }
        public virtual string ComponentData { get; set; }
    }

    public class SnapshotPart : ContentPart<SnapshotRecord>
    {
        [Required]
        public string ComponentName
        {
            get { return Record.ComponentName ; }
            set { Record.ComponentName = value; }
        }

        [Required]
        public string ComponentData
        {
            get { return Record.ComponentData; }
            set { Record.ComponentData = value; }
        }
    }
}