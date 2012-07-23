using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Search.Controller
{
	public class LinquisticsController
	{
		public bool SpellCheckOn = true;
		public bool LemmatizationOn = true;
        public LinguisticsMode SymbolRecongnitionMode = LinguisticsMode.Suggest;
        public LinguisticsMode NameRecongnitionMode = LinguisticsMode.Suggest;
	}
}
