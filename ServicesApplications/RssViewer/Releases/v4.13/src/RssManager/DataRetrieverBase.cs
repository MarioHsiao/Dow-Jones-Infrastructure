using System;

namespace FactivaRssManager
{
	/// <summary>
	/// Summary description for DataRetrieverBase.
	/// </summary>
	public class DataRetriever
	{
		protected string _remoteHost = "";
		public  DataRetriever(string remoteHost)
		{
			_remoteHost = remoteHost;
		}
	}
}
