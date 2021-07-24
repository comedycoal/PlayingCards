using System;
using System.Collections.Generic;

namespace PlayingCards.Primitives
{

	/// <summary>
	/// Class encapsulating a playing card.
	/// </summary>
	/// <remarks>
	/// <para><see cref="Card"/> assumes usage of the French-suited deck.</para>
	/// <para>Data consists of a <see cref="Suit"/> instance and a <see cref="Rank"/> instance.</para>
	/// </remarks>
	public class Card
	{
		#region Public constants
		//====================//

		/// <summary>
		/// <para>Class public constant representing a non-existing card.</para>
		/// </summary>
		/// <remarks>
		/// <para>It is an invalid card and should be taken into account when creating methods around cards.</para>
		/// <para>Can be used as a replacement for <see langword="null"/>, although <see langword="null"/> is still valid.</para>
		/// <para>Differences from <see cref="EMPTY"/> are mostly syntactical, but should be distinguished. 
		/// For example in a Solitaire game, moves including a <see cref="EMPTY"/> count toward score, but ones with <see cref="NONE"/> do not.</para>
		/// </remarks>
		public static readonly Card NONE;

		/// <summary>
		/// <para>Class public constant representing a faced-down card.</para>
		/// </summary>
		/// <remarks>
		/// <para>A faced-down card typically means its <see cref="Suit"/> and <see cref="Rank"/> data is not accessible and should not be retrived at all.</para>
		/// </remarks>
		public static readonly Card FACED_DOWN;

		/// <summary>
		/// <para>Class public constant representing the "empty" state of a container.</para>
		/// </summary>
		/// <remarks>
		/// <para>Is potentially used with container objects to indicate its empty state.</para>
		/// <para>Differences from <see cref="NONE"/> are mostly syntactical, but should be distinguished. 
		/// For example in a Solitaire game, moves including a <see cref="EMPTY"/> count toward score, but ones with <see cref="NONE"/> do not.</para>
		/// </remarks>
		public static readonly Card EMPTY;

		static Card()
		{
			NONE = new Card();
			NONE.m_suit = Suit.ANY_SUIT;
			NONE.m_rank = Rank.ANY_RANK;

			FACED_DOWN = new Card();
			FACED_DOWN.m_suit = Suit.ANY_SUIT;
			FACED_DOWN.m_rank = Rank.ANY_RANK;

			EMPTY = new Card();
			EMPTY.m_suit = Suit.ANY_SUIT;
			EMPTY.m_rank = Rank.ANY_RANK;
		}
		//======================================================================//
		#endregion


		#region Fields
		//==========//

		private Suit m_suit;
		private Rank m_rank;
		//======================================================================//
		#endregion


		#region Constructors
		//================//

		/// <summary>
		/// Default empty constructor.
		/// <para>Contructs a new <see cref="Card"/>, which is an "Ace of Hearts".</para>
		/// </summary>
		public Card() : this(Suit.HEARTS, Rank.A_RANK) { }

		/// <summary>
		/// Contructs a new <see cref="Card"/> with a suit and a rank.
		/// </summary>
		/// <param name="suit">A <see cref="Suit"/> instance.</param>
		/// <param name="rank">A <see cref="Rank"/> instance.</param>
		public Card(Suit suit, Rank rank)
		{
			m_suit = suit;
			m_rank = rank;
		}

		/// <summary>
		/// Copy constructor.
		/// <para>Contructs a new <see cref="Card"/> that is a clone of <paramref name="other"/>.</para>
		/// </summary>
		/// <param name="other">A <see cref="Card"/> instance.</param>
		public Card(Card other) : this(other.m_suit, other.m_rank) { }

		//======================================================================//
		#endregion


		#region Static methods
		//==================//

		/// <summary>
		/// Static method, examine if a <see cref="Card"/> instance is valid (not class constants or null).
		/// </summary>
		/// <param name="card">A <see cref="Card"/> instance</param>
		/// <returns><see langword="true"/> if a <paramref name="card"/> is valid; otherwise <see langword="false"/>.</returns>
		public static bool IsValid(Card card)
		{
			return card != null && card.Valid;
		}
		//======================================================================//
		#endregion


		#region New properties
		//==================//

		/// <summary>
		/// Readonly property. Retrives the suit of the card as a <see cref="Suit"/> instance.
		/// </summary>
		/// <value>Gets the internal <see cref="m_suit"/> field.</value>
		public Suit CardSuit => m_suit;

		/// <summary>
		/// Readonly property. Retrives the rank of the card as a <see cref="Rank"/> instance.
		/// </summary>
		/// <value>Gets the internal <see cref="m_rank"/> field.</value>
		public Rank CardRank => m_rank;

		/// <summary>
		/// Readonly property. Retrives whether this is a valid instance of <see cref="Card"/>
		/// </summary>
		public bool Valid => this != EMPTY && this != NONE && this != FACED_DOWN;
		//======================================================================//
		#endregion


		#region Overloaded methods
		//======================//

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>The fully qualified type name</returns>
		public override string ToString()
		{
			if (this == Card.EMPTY)
				return "EMPTY";
			else if (this == Card.FACED_DOWN)
				return "?";
			else if (this == Card.NONE)
				return "NONE";

			return m_rank.ToString() + /*" of " +*/ m_suit.ToString();
		}

		//======================================================================//
		#endregion


		#region New methods
		//===============//
		//======================================================================//
		#endregion


		#region Overloaded operators
		//========================//

		/// <summary>
		/// Expicit cast to a <see cref="List&lt;Card&gt;"/> with only one element.
		/// </summary>
		/// <param name="card">A <see cref="Card"/> instance.</param>
		public static explicit operator List<Card>(Card card)
		{
			return new List<Card> { card };
		}
		//======================================================================//
		#endregion
	}
}