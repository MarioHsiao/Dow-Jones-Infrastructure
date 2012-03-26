using System;
using System.Collections;

namespace DowJones.Utilities.Search.SearchBuilder.Core.ISO8601
{
    /// <summary>
    /// Class that represents and Iso8601Week
    /// </summary>
    /// 
    public abstract class DateComparer : IComparer
    {
        public enum SortDirections
        {
            Descending = 0,
            Ascending,
        }

        protected SortDirections _sortDirection = SortDirections.Ascending;

        public SortDirections SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        #region Constructor

        public DateComparer(SortDirections direction)
        {
            _sortDirection = direction;
        }

        #endregion

        protected abstract int Compare_Ascending(object x, object y);
        protected abstract int Compare_Descending(object x, object y);

        #region IComparer Members

        public int Compare(object x, object y)
        {
            switch (_sortDirection)
            {
                case SortDirections.Descending:
                    return Compare_Descending(x, y);
                default:
                    return Compare_Ascending(x, y);
            }
        }

        #endregion
    }

    public class Iso8601WeekComparer : DateComparer
    {
        #region Constructor

        public Iso8601WeekComparer(SortDirections direction)
            : base(direction)
        {
        }

        #endregion

        #region DateComparer Compare Related Private Methods

        protected override int Compare_Descending(Object x, Object y)
        {
            Iso8601Week _x = x as Iso8601Week;
            Iso8601Week _y = y as Iso8601Week;
            if (_x == null || _y == null)
            {
                throw new ArgumentException("Both x and y must be of type Iso8601Week.");
            }
            if (_x.IsoWeek.Year > _y.IsoWeek.Year)
            {
                return -1;
            }
            else
            {
                if (_x.IsoWeek.Year == _y.IsoWeek.Year)
                {
                    // check the week portion
                    if (_x.IsoWeek.Week > _y.IsoWeek.Week)
                    {
                        return -1;
                    }
                    else
                    {
                        if (_x.IsoWeek.Week == _y.IsoWeek.Week)
                            return 0;
                        else
                            return 1;
                    }
                }
                else return 1;
            }
        }

        protected override int Compare_Ascending(Object x, Object y)
        {
            Iso8601Week _x = x as Iso8601Week;
            Iso8601Week _y = y as Iso8601Week;
            if (_x == null || _y == null)
            {
                throw new ArgumentException("Both x and y must be of type Iso8601Week.");
            }
            if (_x.IsoWeek.Year > _y.IsoWeek.Year)
            {
                return 1;
            }
            else
            {
                if (_x.IsoWeek.Year == _y.IsoWeek.Year)
                {
                    // check the week portion
                    if (_x.IsoWeek.Week > _y.IsoWeek.Week)
                    {
                        return 1;
                    }
                    else
                    {
                        if (_x.IsoWeek.Week == _y.IsoWeek.Week)
                            return 0;
                        else
                            return -1;
                    }
                }
                else return -1;
            }
        }

        #endregion
    }

    public class DateCounterComparer : DateComparer
    {
        #region Constructor

        public DateCounterComparer(SortDirections direction)
            : base(direction)
        {
        }

        #endregion

        #region DateComparer Compare Related Private Methods

        protected override int Compare_Ascending(object x, object y)
        {
            DateCounter _x = x as DateCounter;
            DateCounter _y = y as DateCounter;
            if (_x == null || _y == null)
            {
                throw new ArgumentException("Both x and y must be of type Iso8601Week.");
            }
            return _x.CurrentDate.CompareTo(_y.CurrentDate);
        }

        protected override int Compare_Descending(object x, object y)
        {
            DateCounter _x = x as DateCounter;
            DateCounter _y = y as DateCounter;
            if (_x == null || _y == null)
            {
                throw new ArgumentException("Both x and y must be of type Iso8601Week.");
            }
            return _y.CurrentDate.CompareTo(_x.CurrentDate);
        }

        #endregion
    }

    public interface IUpdateItem
    {
        string GetKey();
        void UpdateHitCount(int hits);
    }

    public class DateCounter : IComparable, IUpdateItem
    {
        protected DateTime currentDate;
        private int hitCount = 0;

        #region Properties

        public DateTime CurrentDate
        {
            get { return currentDate; }
        }

        public int HitCount
        {
            get { return hitCount; }
            set { hitCount = value; }
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            DateCounter temp = (DateCounter)obj;
            return currentDate.CompareTo(temp.currentDate);
        }

        #endregion

        #region << Constructor >>

        public DateCounter(DateTime dt)
        {
            currentDate = dt;
        }

        #endregion

        #region IUpdateItem Members

        public string GetKey()
        {
            return currentDate.ToString("MM-dd-yyyy");
        }

        public void UpdateHitCount(int hits)
        {
            hitCount += hits;
        }

        #endregion
    }

    public class MonthCounter : DateCounter
    {
        public MonthCounter(DateTime dt)
            : base(dt)
        {
            currentDate = new DateTime(dt.Year, dt.Month, 1);
        }
    }

    public class Iso8601Week : IComparable, IUpdateItem
    {
        private int numOfWeeksInYear;
        private IsoWeek isoWeek;
        private DateTime startDate;
        private DateTime endDate;
        private int hitCount = 0;

        #region << Properties >>

        public int HitCount
        {
            get { return hitCount; }
            set { hitCount = value; }
        }

        public IsoWeek IsoWeek
        {
            get { return isoWeek; }
            set { isoWeek = value; }
        }

        public int NumOfWeeksInYear
        {
            get { return numOfWeeksInYear; }
            set { numOfWeeksInYear = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        #endregion

        #region << Constructor >>

        public Iso8601Week(int year, int week)
        {
            numOfWeeksInYear = Iso8601Date.GetNumIso8601WeeksInYear(year);
            isoWeek = new IsoWeek(year, week);
            startDate = Iso8601Date.GetIso8601Week(year, week);
            endDate = startDate.AddDays(6);
        }

        public Iso8601Week(DateTime dt)
        {
            isoWeek = Iso8601Date.ISOWeekNumber(dt);
            startDate = Iso8601Date.GetIso8601Week(isoWeek.Year, isoWeek.Week);
            endDate = startDate.AddDays(6);
            numOfWeeksInYear = Iso8601Date.GetNumIso8601WeeksInYear(isoWeek.Year);
        }

        public Iso8601Week(int year, int month, int day)
            : this(new DateTime(year, month, day))
        {
        }

        #endregion

        #region << Overrides >>

        public override string ToString()
        {
            return string.Format("StartDate:{0} EndDate:{1} NumOfWeeksInYear:{2}", startDate.ToShortDateString(), endDate.ToShortDateString(), numOfWeeksInYear);
        }

        #endregion

        #region << IComparable Members >>

        public int CompareTo(object obj)
        {
            Iso8601Week temp = (Iso8601Week)obj;
            if (isoWeek.Year > temp.isoWeek.Year)
            {
                return -1;
            }
            else
            {
                if (isoWeek.Year == temp.IsoWeek.Year)
                {
                    // check the week portion
                    if (isoWeek.Week > temp.IsoWeek.Week)
                    {
                        return -1;
                    }
                    else
                    {
                        if (isoWeek.Week == temp.IsoWeek.Week)
                            return 0;
                        else
                            return 1;
                    }
                }
                else return 1;
            }
        }

        #endregion

        #region IUpdateItem Members

        public string GetKey()
        {
            return string.Format("{0} {1}", isoWeek.Year, isoWeek.Week);
        }

        public void UpdateHitCount(int hits)
        {
            hitCount += hits;
        }

        #endregion
    }

    public class IsoWeek
    {
        public int Week;
        public int Year;

        public IsoWeek(int year, int week)
        {
            Year = year;
            Week = week;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Year, Week);
        }
    }

    public class Iso8601Date
    {
        /// <summary>
        /// Static Method to check LeepYear
        /// </summary>
        /// <param name="yyyy"></param>
        /// <returns></returns>
        private static bool IsLeapYear(int yyyy)
        {
            return (yyyy % 4 == 0 && yyyy % 100 != 0) || (yyyy % 400 == 0);
        }

        /// <summary>
        /// Static Method to return the number of ISO 8601 weeks in any given year.
        /// </summary>
        /// <param name="yyyy"><see cref="int"/>Year</param>
        /// <returns><see cref="int"/>Number of ISO 8601 weeks.</returns>
        public static int GetNumIso8601WeeksInYear(int yyyy)
        {
            var thisYearStart = GetIso8601WeekStartDate(yyyy, 1);
            var nextYearStart = GetIso8601WeekStartDate(yyyy + 1, 1);
            var days = nextYearStart.Subtract(thisYearStart);
            return Convert.ToInt32(days.TotalDays / 7);
        }

        /// <summary>
        /// Static Method to return the DateTime of start date of given week.
        /// </summary>
        /// <param name="yyyy"><see cref="int"/>Year</param>
        /// <param name="ww"><see cref="int"/>Week</param>
        /// <returns><see cref="DateTime"/>return the DateTime of start date of given week</returns>
        public static DateTime GetIso8601Week(int yyyy, int ww)
        {
            var thisYearStart = GetIso8601WeekStartDate(yyyy, 1);
            var nextYearStart = GetIso8601WeekStartDate(yyyy + 1, 1);
            var days = nextYearStart.Subtract(thisYearStart);

            var max = Convert.ToInt32(days.TotalDays / 7);
            const int min = 1;
            if (ww < min || ww > max)
            {
                throw new ArgumentOutOfRangeException("ww", string.Format("ww is out of range for year {0}.", yyyy));
            }
            return GetIso8601WeekStartDate(yyyy, ww);
        }

        /// <summary>
        /// Static Method to return the DateTime of start date of given week.
        /// </summary>
        /// <param name="yyyy"><see cref="int"/>Year</param>
        /// <param name="ww"><see cref="int"/>Week</param>
        /// <returns><see cref="DateTime"/>return the DateTime of start date of given week</returns>
        private static DateTime GetIso8601WeekStartDate(int yyyy, int ww)
        {
            if (ww <= 0)
            {
                throw new ArgumentOutOfRangeException("ww", ww, "Weeks must be in the range of 1 or greater");
            }

            // Find the Jan1WeekDay for year 
            var i = (yyyy - 1) % 100;
            var j = (yyyy - 1) - i;
            var k = i + i / 4;
            var Jan1WeekDay = 1 + (((((j / 100) % 4) * 5) + k) % 7);

            //initialized to first of year
            var FirsDayOfFirstWeek = new DateTime(yyyy, 1, 1);

            // algorithm used to calculate 1 day based
            if (Jan1WeekDay > 4)
            {
                var deltaDays = (8 - Jan1WeekDay) % 7;
                FirsDayOfFirstWeek = FirsDayOfFirstWeek.AddDays(deltaDays);
            }
            else
            {
                var deltaDays = -((6 + Jan1WeekDay) % 7);
                FirsDayOfFirstWeek = FirsDayOfFirstWeek.AddDays(deltaDays);
            }

            return FirsDayOfFirstWeek.AddDays((ww - 1) * 7);
        }

        /// <summary>
        /// Static Method to return Date based on year and a given day in year
        /// </summary>
        /// <param name="yyyy">Year</param>
        /// <param name="dd">Day</param>
        /// <returns></returns>
        public static DateTime GetDate(int yyyy, int dd)
        {
            var max = 365;
            const int min = 1;
            if (IsLeapYear(yyyy))
            {
                max = 366;
            }
            // see if dd false into proper range
            if (dd < min || dd > max)
            {
                throw new ArgumentOutOfRangeException("dd", "dd is out of range for year " + yyyy);
            }
            // initialized to first of year
            var FirstDayOfYear = new DateTime(yyyy, 1, 1);
            return FirstDayOfYear.AddDays(dd - 1);
        }

        /// <summary>
        /// Static Method to return ISO WeekNumber (1-53) for a given year
        /// </summary>
        /// <param name="dt"><see cref="DateTime"/> Date</param>
        /// <returns><see cref="int"/>return ISO WeekNumber (1-53)</returns>
        public static IsoWeek ISOWeekNumber(DateTime dt)
        {
            // Set Year
            var yyyy = dt.Year;

            // Set Month
            var mm = dt.Month;

            // Set Day
            var dd = dt.Day;

            // Declare other required variables
            int DayOfYearNumber;
            int Jan1WeekDay;
            int WeekNumber = 0, WeekDay;


            int i, j, k, l;
            int[] Mnth = new int[12] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };

            int YearNumber;


            // Set DayofYear Number for yyyy mm dd
            DayOfYearNumber = dd + Mnth[mm - 1];

            // Increase of Dayof Year Number by 1, if year is leapyear and month is february
            if (IsLeapYear(yyyy) && mm > 1)
                DayOfYearNumber += 1;

            // Find the Jan1WeekDay for year 
            i = (yyyy - 1) % 100;
            j = (yyyy - 1) - i;
            k = i + i / 4;
            Jan1WeekDay = 1 + (((((j / 100) % 4) * 5) + k) % 7);

            // Calcuate the WeekDay for the given date
            l = DayOfYearNumber + (Jan1WeekDay - 1);
            WeekDay = 1 + ((l - 1) % 7);

            // Find if the date falls in YearNumber set WeekNumber to 52 or 53
            if ((DayOfYearNumber <= (8 - Jan1WeekDay)) && (Jan1WeekDay > 4))
            {
                YearNumber = yyyy - 1;
                if ((Jan1WeekDay == 5) || ((Jan1WeekDay == 6) && (Jan1WeekDay > 4)))
                    WeekNumber = 53;
                else
                    WeekNumber = 52;
            }
            else
                YearNumber = yyyy;


            // Set WeekNumber to 1 to 53 if date falls in YearNumber
            if (YearNumber == yyyy)
            {
                int m = 365;
                if (IsLeapYear(yyyy))
                    m = 366;
                if ((m - DayOfYearNumber) < (4 - WeekDay))
                {
                    YearNumber = yyyy + 1;
                    WeekNumber = 1;
                }
            }

            if (YearNumber == yyyy)
            {
                int n = DayOfYearNumber + (7 - WeekDay) + (Jan1WeekDay - 1);
                WeekNumber = n / 7;
                if (Jan1WeekDay > 4)
                    WeekNumber -= 1;
            }

            return new IsoWeek(YearNumber, WeekNumber);
        }


        /// <summary>
        /// Static Method to Calculate WeekDay (Monday=1...Sunday=7)
        /// </summary>
        /// <param name="dt"><see cref="DateTime"/> Date </param>
        /// <returns><see cref="int"/> Corresponding to (Monday=1...Sunday=7)</returns>
        public static int WeekDay(DateTime dt)
        {
            // Set Year
            int yyyy = dt.Year;

            // Set Month
            int mm = dt.Month;

            // Set Day
            int dd = dt.Day;

            // Declare other required variables
            int DayOfYearNumber;
            int Jan1WeekDay;
            int WeekDay;


            int i, j, k, l;
            int[] Mnth = new int[12] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };


            // Set DayofYear Number for yyyy mm dd
            DayOfYearNumber = dd + Mnth[mm - 1];

            // Increase of Dayof Year Number by 1, if year is leapyear and month is february
            if (IsLeapYear(yyyy) && mm == 2)
                DayOfYearNumber += 1;

            // Find the Jan1WeekDay for year 
            i = (yyyy - 1) % 100;
            j = (yyyy - 1) - i;
            k = i + i / 4;
            Jan1WeekDay = 1 + (((((j / 100) % 4) * 5) + k) % 7);

            // Calcuate the WeekDay for the given date
            l = DayOfYearNumber + (Jan1WeekDay - 1);
            WeekDay = 1 + ((l - 1) % 7);

            return WeekDay;
        }


        /// <summary>
        /// Static Method to Display date in ISO Format (Year - WeekNumber - WeekDay)
        /// </summary>
        /// <param name="dt"><see cref="DateTime"/> Date</param>
        /// <returns><see cref="string"/></returns>
        public static string DisplayISODate(DateTime dt)
        {
            string str;
            int year, weekday, weeknumber;

            year = dt.Year;
            weeknumber = ISOWeekNumber(dt).Week;
            weekday = WeekDay(dt);

            str = year.ToString("d0") + "-" + weeknumber.ToString("d0")
                + "-" + weekday.ToString("d0");
            return str;
        }
    }
}