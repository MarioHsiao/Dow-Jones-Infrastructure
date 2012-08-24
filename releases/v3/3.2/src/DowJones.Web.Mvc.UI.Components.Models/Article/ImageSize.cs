using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Attributes;

namespace DowJones.Web.Mvc.UI.Components.Article
{
	public enum ImageSize
	{
		Unknown,	// default

		Logo,

		[AssignedToken("pictureSizeLarge")]
		Large,
		[AssignedToken("pictureSizeSmall")]
		Small,
		[AssignedToken("pictureSizeXSmall")]
		XSmall,
	}
}
