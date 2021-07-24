using System;
using System.Collections.Generic;
using System.Text;

using PlayingCards.Primitives;

namespace PlayingCards.Game.Solitaire
{
	public partial class SolitaireGame
	{
		public struct Metadata
		{
			public string Name;
			public string Description;
			public Deck GameDeck;
			public WinCondition GameWinCondition;

			public string Summary()
			{
				throw new NotImplementedException();
			}
		}
	}
}
