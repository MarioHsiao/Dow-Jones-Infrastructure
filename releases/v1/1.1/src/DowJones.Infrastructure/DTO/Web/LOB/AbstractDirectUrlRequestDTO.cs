using System;
using DowJones.Utilities.Attributes;

namespace DowJones.Utilities.DTO.Web.LOB
{
	/// <summary>
	/// Summary description for AbstractDirectUrlRequestDTO.
	/// </summary>
	public abstract class AbstractDirectUrlRequestDTO : AbstractRequestDTO
	{
		[ParameterName("userid")] public string userId;
		[ParameterName("longuserid")] public string longUserId;
		[ParameterName("userpassword")] public string userPassword;
		[ParameterName("namespace")] public string @namespace;
		[ParameterName("accountid")] public string accountId;
		[ParameterName("xsid")] public string xsid;
		[ParameterName("cc")] public string cc;
		[ParameterName("cookie")] public string cookie;
	}
}
