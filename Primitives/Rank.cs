using System.Collections.Generic;
using System.Collections.Immutable;

namespace PlayingCards.Primitives
{


	/// <summary>
	/// Struct for the Rank concept in playing cards.
	/// </summary>
	/// <remarks>
	/// <para>The struct assumes usage of the French ranks (<em>Ace (A), 2-10, Jack (J), Queen (Q), King (K)</em>), also in that order when sorted by internal value.</para>
	/// <para>A struct instance holds an internal integer value and can be explicitly casted back and forth.
	/// For a <see cref="Rank"/> instance that is not presented as class constants, an explicit cast should be used.</para>
	/// </remarks>
	public struct Rank
    { 
        #region Public constants
        //====================//

        /// <summary>
		/// Public constant for a dummy instance representing any rank.
		/// </summary>
		/// <remarks>
		/// Note: Is always equal to any other <see cref="Rank"/> instance and should not be used in &lt;, &lt;=, &gt;, &gt;= comparision.
		/// </remarks>
        public static readonly Rank ANY_RANK;

        /// <summary>
		/// Public constant representing the Ace. (Internal value = 1)
		/// </summary>
        public static readonly Rank A_RANK;
        /// <summary>
		/// Public constant representing the Jack. (Internal value = 11)
		/// </summary>
        public static readonly Rank J_RANK;
        /// <summary>
		/// Public constant representing the Queen. (Internal value = 12)
		/// </summary>
        public static readonly Rank Q_RANK;
        /// <summary>
		/// Public constant representing the Queen. (Internal value = 13)
		/// </summary>
        public static readonly Rank K_RANK;

        /// <summary>
		/// Public constant for total count of ranks.
		/// As of June 2021, this value stays at 13, potentially interminably.
		/// </summary>
        public const int RANK_COUNT = 13;

        /// <summary>
		/// Public constant for an immutable list of every Rank, in increasing order.
		/// </summary>
        public static readonly ImmutableList<Rank> RANK_LIST;


        static Rank()
        {
            ANY_RANK = new Rank();
            ANY_RANK.m_value = 999;

			A_RANK = new Rank(1);
			J_RANK = new Rank(11);
			Q_RANK = new Rank(12);
			K_RANK = new Rank(13);

			List<Rank> temp = new List<Rank>();
			for (int i = (int)A_RANK; i <= (int)K_RANK; ++i)
                temp.Add(i);

			RANK_LIST = ImmutableList<Rank>.Empty.AddRange(temp);
		}
        //======================================================================//
        #endregion


        #region Fields
        //==========//
        private int m_value;
        //======================================================================//
        #endregion


        #region Constructors
        //================//

        private Rank(int value)
        {
            if (value < 0 || value > 13) value = 1;
            this.m_value = value;
        }

        //======================================================================//
        #endregion


        #region Overloaded methods
        //======================//

        /// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns><see langword="true"/> if <paramref name="obj"/> and this instance
		/// are the same type and represent the same value; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>Returns the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>The fully qualified type name</returns>
        public override string ToString()
        {
            switch (m_value)
            {
                case 1:
                    return "A";
                case 11:
                    return "J";
                case 12:
                    return "Q";
                case 13:
                    return "K";
                default:
                    return m_value.ToString();
            }
        }
        //======================================================================//
        #endregion


        #region Overloaded operators
        //========================//

        /// <summary>
		/// Overloaded &lt; operator. Performs &lt; check on two <see cref="Rank"/> instances.
		/// Should not be used with <see cref="Rank.ANY_RANK"/>.
		/// </summary>
		/// <param name="a">Left-hand <see cref="Rank"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="Rank"/> instance operand</param>
		/// <returns><see langword="true"/> if the left operand's internal value is smaller than the right's internal value, otherwise <see langword="false"/>.</returns>
        public static bool operator <(Rank a, Rank b)
        {
            return a.m_value < b.m_value;
        }

        /// <summary>
		/// Overloaded &lt; operator. Performs &gt; check on two <see cref="Rank"/> instances.
		/// Should not be used with <see cref="Rank.ANY_RANK"/>.
		/// </summary>
		/// <param name="a">Left-hand <see cref="Rank"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="Rank"/> instance operand</param>
		/// <returns><see langword="true"/> if the left operand's internal value is higher than the right's internal value, otherwise <see langword="false"/>.</returns>
        public static bool operator >(Rank a, Rank b)
        {
            return a.m_value > b.m_value;
        }

        /// <summary>
		/// Overloaded &lt; operator. Performs &gt;= check on two <see cref="Rank"/> instances.
		/// Should not be used with <see cref="Rank.ANY_RANK"/>.
		/// </summary>
		/// <param name="a">Left-hand <see cref="Rank"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="Rank"/> instance operand</param>
		/// <returns><see langword="true"/> if the left operand's internal value is higher than or equal
		/// to the right's internal value, otherwise <see langword="false"/>.</returns>
        public static bool operator >= (Rank a, Rank b)
        {
            return a.m_value >= b.m_value;
        }
        
        /// <summary>
		/// Overloaded &lt; operator. Performs &lt;= check on two <see cref="Rank"/> instances.
		/// Should not be used with <see cref="Rank.ANY_RANK"/>.
		/// </summary>
		/// <param name="a">Left-hand <see cref="Rank"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="Rank"/> instance operand</param>
		/// <returns><see langword="true"/> if the left operand's internal value is smaller than or equal
		/// to the right's internal value, otherwise <see langword="false"/>.</returns>
        public static bool operator <= (Rank a, Rank b)
        {
            return a.m_value <= b.m_value;
        }

        /// <summary>
		/// Overloaded == operator. Performs equivalent check on two <see cref="Rank"/> instances.
		/// Can be used with <see cref="Rank.ANY_RANK"/>.
		/// </summary>
		/// <param name="a">Left-hand <see cref="Rank"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="Rank"/> instance operand</param>
		/// <returns><see langword="true"/> if the two instances are equal, otherwise <see langword="false"/>.</returns>
        public static bool operator ==(Rank a, Rank b)
        {
            return a.m_value == b.m_value;
        }

        /// <summary>
		/// Overloaded != operator. Performs unequivalent check on two <see cref="Rank"/> instances.
		/// Can be used with <see cref="Rank.ANY_RANK"/>.
		/// </summary>
		/// <param name="a">Left-hand <see cref="Rank"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="Rank"/> instance operand</param>
		/// <returns><see langword="true"/> if the two instances are not equal, otherwise <see langword="false"/>.</returns>
        public static bool operator !=(Rank a, Rank b)
        {
            return a.m_value != b.m_value;
        }

        /// <summary>
		/// <para>Overloaded != operator. Performs a subtraction on two <see cref="Rank"/> instances.
		/// Should not be used with <see cref="Rank.ANY_RANK"/>.</para>
		/// Wrapping is assumed. <see cref="Rank.A_RANK"/> - <see cref="Rank.K_RANK"/> would equals 1;
		/// </summary>
		/// <param name="a">Left-hand <see cref="Rank"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="Rank"/> instance operand</param>
		/// <returns><see langword="true"/> if the two instances are not equal, otherwise <see langword="false"/>.</returns>
		/// <remarks>Internally, a subtraction of the internal values followed by a modulo by <see cref="Rank.RANK_COUNT"/> is performed.</remarks>
        public static int operator -(Rank a, Rank b)
        {
            return (a.m_value - b.m_value + RANK_COUNT) % RANK_COUNT;
        }

        /// <summary>
		/// Explicit cast to an <see langword="int"/>
		/// </summary>
		/// <param name="a">A <see cref="Rank"/> instance</param>
        public static explicit operator int(Rank a)
        {
            return a.m_value;
        }

        /// <summary>
		/// Implicit cast to an <see cref="Rank"/>
		/// </summary>
		/// <param name="a">A <see langword="int"/> instance</param>
        public static implicit operator Rank(int a)
        {
			return new Rank(a);
		}

		/// <summary>
		/// Explicit cast of a <see cref="string"/> object to <see cref="Rank"/>.
		/// </summary>
		/// <remarks>>
		/// Subceptible values for <paramref name="value"/> are: "1" to "10", "A", "J", "Q", "K", "Any", "any".
		/// </remarks>
		/// <param name="value"></param>
		public static explicit operator Rank(string value)
		{
			switch(value)
			{
				case "A":
					return Rank.A_RANK;
				case "J":
					return Rank.J_RANK;
				case "Q":
					return Rank.Q_RANK;
				case "K":
					return Rank.K_RANK;
				case "Any":
				case "any":
					return Rank.ANY_RANK;
				default:
					try
					{
						var res = int.Parse(value);
						if (!(0 < res && res <= 10))
							throw new System.Exception();
						return (Rank)res;
					}
					catch
					{
						throw new System.NotImplementedException();
					}
			}
		}
        //======================================================================//
        #endregion
    }
}