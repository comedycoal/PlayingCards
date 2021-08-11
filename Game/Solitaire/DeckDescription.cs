using System;
using System.Collections.Generic;
using System.Linq;

using PlayingCards.Primitives;

namespace PlayingCards.Game.Solitaire
{
	/// <summary>
	/// Class describing a <see cref="Deck"/> object and can instantiate one when necessary.
	/// </summary>
	/// <remarks>
	/// A description of a deck consists of a "type" and a "count".
	/// <list type="bullet">
	/// <item>"type" can be "full", "hearts", "diamonds", "clubs", "spades", or any comma-seperated combinations of.
	/// A <see cref="Deck"/> instantiated will consists of cards fitting these descriptions.
	/// </item>
	/// <item>"count" dictates the amount of duplication each set of cards inferred from "type" is included in the <see cref="Deck"/>.</item>
	/// </list>
	/// </remarks>
	public class DeckDescription
	{
		private string m_type;
		private int m_count;

		/// <summary>
		/// Constructs a <see cref="DeckDescription"/>, without instantiating a <see cref="Deck"/> object.
		/// </summary>
		/// <param name="typeString">Type string signifies the component(s) of the deck.</param>
		/// <param name="count">The amount of duplication.</param>
		public DeckDescription(string typeString, int count)
		{
			m_type = typeString;
			m_count = count;
		}

		/// <summary>
		/// Readonly property. Retrieves the type string signifies the component(s) of the deck.
		/// </summary>
		public string Type => m_type;

		/// <summary>
		/// Readonly property. Retrieves the amount of duplication "type" receives.
		/// </summary>
		public int Count => m_count;

		/// <summary>
		/// Instantiate a <see cref="Deck"/> object based on the descriptions.
		/// </summary>
		/// <returns></returns>
		public Deck CreateDeck()
		{
			var items = m_type.Split(',').ToList();
			List<Suit> suits = new List<Suit>();
			foreach(var item in items)
			{
				try
				{
					Suit suit = (Suit)item;
					suits.Add(suit);
				}
				catch
				{
					if (item == "full")
					{
						suits.AddRange(Suit.FULL_SUITS_LIST);
						continue;
					}
					throw new NotImplementedException();
				}
			}

			return Deck.GetSelectedSuitDeck(suits);
		}
	}
}
