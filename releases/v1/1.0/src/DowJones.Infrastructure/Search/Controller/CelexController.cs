using DowJones.Utilities.Search.Core;
using DowJones.Utilities.Search.SearchBuilder;

namespace DowJones.Utilities.Search.Controller
{
	internal class CelexController
	{
        public DocumentNumberType DocumentNumberType = DocumentNumberType.Unspecified;
		public string DocumentNumber;
		public int Year;
		public Sector[] Sectors;
	}
}
