namespace DowJones.Factiva.Currents.Website.Models.PageService
{
	public class PageServiceResponse
	{
		public NewsPageServiceResult NewsPageServiceResult { get; set; }

		public SummaryNewsPageModuleServiceResult SummaryNewsPageModuleServiceResult { get; set; }

		public RadarNewsPageModuleServiceResult RadarNewsPageModuleServiceResult { get; set; }

		public CustomTopicsNewsPageModuleServiceResult CustomTopicsNewsPageModuleServiceResult { get; set; }
	}
}