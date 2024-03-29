﻿using System.Collections.Generic;
using System.Collections.Immutable;

namespace PlayingCards.Primitives
{
	/// <summary>
	/// Struct for the suit concept in playing cards.
	/// </summary>
	/// <remarks>
	/// <para>The struct assumes usage of the French suits (<em>Hearts, Diamonds, Clubs, Spades</em>), also in that order when sorted by internal value.</para>
	/// <para>A struct instance holds an internal integer value and can be explicitly casted back and forth.</para>
	/// </remarks>
	public struct Suit
    {
        #region Public constants
        //====================//

        /// <summary>
		/// Public constant for a dummy instance representing any suit.
		/// </summary>
		/// <remarks>
		/// Note: Is always equal to any other <see cref="Suit"/> instance.
		/// </remarks>
        public static readonly Suit ANY_SUIT;
        /// <summary>
		/// Public constant representing the Hearts suit. (Internal value = 1)
		/// </summary>
        public static readonly Suit HEARTS;
        /// <summary>
		/// Public constant representing the Diamonds suit. (Internal value = 2)
		/// </summary>
        public static readonly Suit DIAMONDS;
        /// <summary>
		/// Public constant representing the Clubs suit. (Internal value = 3)
		/// </summary>
        public static readonly Suit CLUBS;
        /// <summary>
		/// Public constant representing the Spades suit. (Internal value = 4)
		/// </summary>
        public static readonly Suit SPADES;

        /// <summary>
		/// Public constant for total count of suits.
		/// As of June 2021, this value stays at 4, potentially interminably.
		/// </summary>
        public const int SUIT_COUNT = 4;

        /// <summary>
		/// Public constant for an immutable list of every suit, in increasing order.
		/// </summary>
        public static readonly ImmutableList<Suit> FULL_SUITS_LIST;

        /// <summary>
		/// Public constant for an immutable list of every suit considered RED in color, in increasing order.
		/// </summary>
        public static readonly ImmutableList<Suit> RED_SUITS_LIST;

        /// <summary>
		/// Public constant for an immutable list of every suit considered BLACK in color, in increasing order.
		/// </summary>
        public static readonly ImmutableList<Suit> BLACK_SUITS_LIST;

        static Suit()
        {
            ANY_SUIT = new Suit();
            ANY_SUIT.m_value = 999;

			HEARTS = new Suit(1);
			DIAMONDS = new Suit(2);
			CLUBS = new Suit(3);
			SPADES = new Suit(4);
			
			ImmutableList<Suit> temp = ImmutableList<Suit>.Empty;
			RED_SUITS_LIST = temp.AddRange(new List<Suit> { HEARTS, DIAMONDS });
			BLACK_SUITS_LIST = temp.AddRange(new List<Suit> { CLUBS, SPADES });
			FULL_SUITS_LIST = RED_SUITS_LIST.AddRange(BLACK_SUITS_LIST);
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

        private Suit(int value)
        {
            if (value > 4 || value < 1) value = 1;
            this.m_value = value;
        }
        //======================================================================//
        #endregion


        #region New properties
        //==================//

        /// <summary>
		/// Readonly property. Retrives a <see cref="bool"/> representing whether the suit is considered Red.
		/// </summary>
        public bool IsRed => (m_value == 1 || m_value == 2);
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
		/// 
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
                    return "H";
                case 2:
                    return "D";
                case 3:
                    return "C";
                case 4:
                    return "S";
                default:
                    return "?";
            }
        }
        //======================================================================//
        #endregion


        #region Overloaded operators
        //========================//

        /// <summary>
		/// Overloaded == operator. Performs equivalent check on two <see cref="Suit"/> instances.
		/// </summary>
		/// <param name="a">Left-hand <see cref="Suit"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="Suit"/> instance operand</param>
		/// <returns><see langword="true"/> if the two instances are equal, otherwise <see langword="false"/>.</returns>
        public static bool operator ==(Suit a, Suit b)
        {
            return a.m_value == ANY_SUIT.m_value || a.m_value == ANY_SUIT.m_value || a.m_value == b.m_value;
        }
        
        /// <summary>
		/// Overloaded != operator. Performs unequivalent check on two <see cref="Suit"/> instances.
		/// </summary>
		/// <param name="a">Left-hand <see cref="Suit"/> instance operand</param>
		/// <param name="b">Right-hand <see cref="Suit"/> instance operand</param>
		/// <returns><see langword="true"/> if the two instances are not equal, otherwise <see langword="false"/>.</returns>
        public static bool operator !=(Suit a, Suit b)
        {
            return !(a == b);
        }
        
        /// <summary>
		/// Explicit cast to an <see langword="int"/>
		/// </summary>
		/// <param name="a">A <see cref="Suit"/> instance</param>
        public static explicit operator int(Suit a)
        {
            return a.m_value;
        }

        /// <summary>
		/// Cast to a <see cref="Suit"/>
		/// </summary>
		/// <param name="a">An <see langword="int"/> </param>
        public static implicit operator Suit(int a)
        {
			return new Suit(a);
		}


		/// <summary>
		/// Explicit cast of a <see cref="string"/> object to <see cref="Suit"/>.
		/// </summary>
		/// <remarks>>
		/// Subceptible values for <paramref name="value"/> are: "Hearts", "Diamonds", "Clubs", "Spades", "Any", or their lower-case variant.
		/// </remarks>
		/// <param name="value"></param>
		public static explicit operator Suit(string value)
		{
			switch (value)
			{
				case "hearts":
				case "Hearts":
					return Suit.HEARTS;
				case "diamonds":
				case "Diamonds":
					return Suit.DIAMONDS;
				case "clubs":
				case "Clubs":
					return Suit.CLUBS;
				case "spades":
				case "Spades":
					return Suit.SPADES;
				case "any":
				case "Any":
					return Suit.ANY_SUIT;
				default:
					throw new System.NotImplementedException();
			}
		}
		//======================================================================//
		#endregion
	}
}