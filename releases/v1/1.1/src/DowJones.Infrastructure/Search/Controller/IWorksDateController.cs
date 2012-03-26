using DowJones.Utilities.Search.Core;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Utilities.Search.Controller
{
	internal class IWorksDateController
	{
        public DateFormat DateFormat = DateFormat.MMDDCCYY;
        public SearchTwoDateQualifier DateQualifier = SearchTwoDateQualifier.ThreeMonths;
		//public string Before;
		public int After;
		public string Equal;
		public DateRange Range;
	}
}
