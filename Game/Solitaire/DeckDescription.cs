using System;
using System.Collections.Generic;
using System.Linq;

using PlayingCards.Primitives;

namespace PlayingCards.Game.Solitaire
{
	public class DeckDescription
	{
		private string m_type;
		private int m_count;

		public DeckDescription(string typeString, int count)
		{
			m_type = typeString;
			m_count = count;
		}

		public string Type => m_type;
		public int Count => m_count;

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
