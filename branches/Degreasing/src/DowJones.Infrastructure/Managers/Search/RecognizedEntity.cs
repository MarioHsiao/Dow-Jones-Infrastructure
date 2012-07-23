using System.Collections.Generic;

namespace DowJones.Managers.Search
{
    public class RecognizedEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchTerm { get; set; }
        public string Type { get; set; }
    }

    public class RecognizedEntities
    {
        public IEnumerable<RecognizedEntity> Entities { get; set; }
        public string SearchText { get; set; }
        public string SpellCorrection { get; set; }
    }
}