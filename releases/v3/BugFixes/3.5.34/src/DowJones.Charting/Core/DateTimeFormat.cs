namespace DowJones.Charting.Core
{
    internal class DateTimeFormat : IGeneratesITXML
    {
        #region Implementation of IGeneratesITXML

        //<cit:date-time-format override-input-format="false" input-format="%m/%d/%y" override-output-format="false" output-format="%m/%d/%y" override-month-names="true" month-names="${sJan},${sFeb},${sMar},${sApr},${sMay},${sJun},${sJul},${sAug},${sSep},${sOct},${sNov},${sDec}" override-day-names="true" day-names="${sSun},${sMon},${sTue},${sWed},${sThu},${sFri},${sSat}" override-am-pm-names="false" am-pm-names="AM,PM" override-first-day-of-week="false" first-day-of-week="Sunday" time-plot-major="%Y^Q%Q %y^%b^%a^%I%p^%I:%M%p" time-plot-minor="Q%Q^%b^%d^%I%p^%M" time-category-major="%b" time-category-minor="%d" time-category-minor-every="every-day"/>

        public string ToITXML()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
