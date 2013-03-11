using System;

namespace EMG.widgets.ui.Modules.Compression
{
    internal sealed class Quadruplet<TFirst, TSecond, TThird, TFourth> 
    {
        #region // Private membres

        /// <summary>
        /// The _first.
        /// </summary>
        private readonly TFirst first;

        /// <summary>
        /// The _forth.
        /// </summary>
        private readonly TFourth fourth;

        /// <summary>
        /// The _second.
        /// </summary>
        private readonly TSecond second;

        /// <summary>
        /// The _third.
        /// </summary>
        private readonly TThird third;

        #endregion

        #region // Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Quadruplet{TFirst,TSecond,TThird,TFourth}"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="third">The third.</param>
        /// <param name="fourth">The fourth.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public Quadruplet(TFirst first, TSecond second, TThird third, TFourth fourth)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }

            if (second == null)
            {
                throw new ArgumentNullException("second");
            }

            if (third == null)
            {
                throw new ArgumentNullException("third");
            }

            if (fourth == null)
            {
                throw new ArgumentNullException("fourth");
            }

            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
        }

        #endregion

        #region // Properties

        /// <summary>
        /// Gets First.
        /// </summary>
        public TFirst First
        {
            get { return first; }
        }

        /// <summary>
        /// Gets Second.
        /// </summary>
        public TSecond Second
        {
            get { return second; }
        }

        /// <summary>
        /// Gets Third.
        /// </summary>
        public TThird Third
        {
            get { return third; }
        }

        /// <summary>
        /// Gets Forth.
        /// </summary>
        public TFourth Fourth
        {
            get { return fourth; }
        }

        #endregion

        #region // Public methods

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">The current object.</param>
        /// <returns>A boolean indicating equality.</returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            var other = obj as Quadruplet<TFirst, TSecond, TThird, TFourth>;
            return (other != null) &&
                   other.first.Equals(first) && other.second.Equals(second) && other.third.Equals(third) && other.fourth.Equals(fourth);
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>A integer representing the hash code.</returns>
        public override int GetHashCode()
        {
            var a = first.GetHashCode();
            var c = third.GetHashCode();

            var ab = ((a << 5) + a) ^ second.GetHashCode();
            var cd = ((c << 5) + a) ^ third.GetHashCode();

            return ((ab << 5) + ab) ^ cd.GetHashCode();
        }

        #endregion
    }
}
