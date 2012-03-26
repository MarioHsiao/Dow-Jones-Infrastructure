using DowJones.Infrastructure;
using DowJones.Token;

namespace DowJones.Web.Mvc.UI.Components.RealtimeHeadlineList
{
    public class RealtimeHeadlineListTokens : AbstractTokenBase
    {
        #region << Accessors >>

        public string controlTitleTkn { get; set; }
        public string queueTkn { get; set; }
        public string viewAllTkn { get; set; }

        #endregion

        public RealtimeHeadlineListTokens()
        {
            controlTitleTkn = GetTokenByName("controlTitleTkn");
            queueTkn = GetTokenByName("queueTkn");
            viewAllTkn = GetTokenByName("viewAllTkn");
        }
    }
}
