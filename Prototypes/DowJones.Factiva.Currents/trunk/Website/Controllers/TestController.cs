using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DowJones.Formatters;
using DowJones.Models.Common;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Factiva.Currents.Website.Controllers
{
	public class TestController : ControllerBase
	{
		//
		// GET: /Test/

		public ActionResult Index()
		{

			var newsVolume = new[] {
				new NewsEntityNewsVolumeVariation
				{
					Code = "AFRICAZ",
					CurrentTimeFrameNewsVolume = new WholeNumber(7191)
				},
				new NewsEntityNewsVolumeVariation
				{
					Code = "APACZ",
					CurrentTimeFrameNewsVolume = new WholeNumber(82528)
				},
				new NewsEntityNewsVolumeVariation
				{
					Code = "AUSTR",
					CurrentTimeFrameNewsVolume = new WholeNumber(9352)
				},
				new NewsEntityNewsVolumeVariation
				{
					Code = "CAMZ",
					CurrentTimeFrameNewsVolume = new WholeNumber(6163)
				},
				new NewsEntityNewsVolumeVariation
				{
					Code = "EURZ",
					CurrentTimeFrameNewsVolume = new WholeNumber(149062)
				},
				new NewsEntityNewsVolumeVariation
				{
					Code = "INDIA",
					CurrentTimeFrameNewsVolume = new WholeNumber(10601)
				},
				new NewsEntityNewsVolumeVariation
				{
					Code = "MEASTZ",
					CurrentTimeFrameNewsVolume = new WholeNumber(13638)
				},
				new NewsEntityNewsVolumeVariation
				{
					Code = "NAMZ",
					CurrentTimeFrameNewsVolume = new WholeNumber(352527)
				},
				new NewsEntityNewsVolumeVariation
				{
					Code = "RUSS",
					CurrentTimeFrameNewsVolume = new WholeNumber(9469)
				},
				new NewsEntityNewsVolumeVariation
				{
					Code = "SAMZ",
					CurrentTimeFrameNewsVolume = new WholeNumber(10057)
				}	

			};
			return View(newsVolume);
		}

	}
}
