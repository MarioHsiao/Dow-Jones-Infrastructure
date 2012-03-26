using System;
using System.Collections;
using DowJones.Utilities.Managers.Search.Responses;

namespace DowJones.Utilities.Managers.Search.Comparers
{
    internal class AccessionNumberBasedContentItemComparer : IComparer
    {
        public enum SortDirections
        {
            PublicationDateChronological = 0,
            PublicationDateReverseChronological,
        }

        protected SortDirections _sortDirection = SortDirections.PublicationDateReverseChronological;

        public SortDirections SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        #region Constructor

        public AccessionNumberBasedContentItemComparer(SortDirections direction)
        {
            _sortDirection = direction;
        }
        #endregion

        protected static int Compare_PublicationDateChronological(object x, object y)
        {
            AccessionNumberBasedContentItem _x = x as AccessionNumberBasedContentItem;
            AccessionNumberBasedContentItem _y = y as AccessionNumberBasedContentItem;
            if (_x == null || _y == null)
            {
                throw new ArgumentException("Both x and y must be of type AccessionNumberBasedContentItem.");
            }
            return _x.CompareTo(_y);
        }
        protected static int Compare_PublicationDateReverseChronological(object x, object y)
        {
            AccessionNumberBasedContentItem _x = x as AccessionNumberBasedContentItem;
            AccessionNumberBasedContentItem _y = y as AccessionNumberBasedContentItem;
            if (_x == null || _y == null)
            {
                throw new ArgumentException("Both x and y must be of type AccessionNumberBasedContentItem.");
            }
            return _y.CompareTo(_x);
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            switch (_sortDirection)
            {
                case SortDirections.PublicationDateChronological:
                    return Compare_PublicationDateChronological(x, y);
                default:
                    return Compare_PublicationDateReverseChronological(x, y);
            }
        }

        #endregion
    }
}
