using DowJones.Search.Core;

namespace DowJones.Search.Controller
{
	internal class CelexController
	{
        public DocumentNumberType DocumentNumberType = DocumentNumberType.Unspecified;
		public string DocumentNumber;
		public int Year;
		public Sector[] Sectors;
	}
}
