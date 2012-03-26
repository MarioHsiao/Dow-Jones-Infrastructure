using DowJones.Tools.Ajax;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Utilities.Formatters.Globalization;

namespace DowJones.Utilities.Ajax.RealtimeHeadlineList
{
    public class RealtimeHeadlineRequestDelegate : IAjaxRequestDelegate
    {
        /// <summary>
        /// Alert Context
        /// </summary>
        public string AlertContext;

        //Maximum headlines to display
        public int MaxHeadlinesToReturn;

        //Date Time Formating Preference
        public string DateTimeFormatingPreference;

        //Clock Type
        public ClockType ClockType;
    }

    public class RealtimeHeadlineResponseDelegate: AbstractAjaxResponseDelegate
    {
        /// <summary>
        /// The HeadlineList Data Result
        /// </summary>
        public HeadlineListDataResult headlineListDataResult;

        /// <summary>
        /// Alert Context
        /// </summary>
        public string AlertContext;

        /// <summary>
        /// Max Headlines to Return
        /// </summary>
        public int MaxHeadlinesToReturn;
    }
}
