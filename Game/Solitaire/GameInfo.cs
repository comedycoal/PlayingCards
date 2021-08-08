using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

using PlayingCards.Primitives;
using PlayingCards.Component.Solitaire;
using PlayingCards.Game.Solitaire.XmlSerialization;

namespace PlayingCards.Game.Solitaire
{
	/// <summary>
	/// Class providing metatdata and layout description of a solitaire game.
	/// </summary>
	/// <remarks>
	/// Capable of spawning <see cref="SolitaireGame"/> instances.
	/// </remarks>
	public class GameInfo
	{
		private Metadata m_metadata;
		private List<PileInfo> m_piles;

		public GameInfo(Metadata metadata, List<PileInfo> piles)
		{
			m_metadata = metadata;
			m_piles = piles;
		}

		public SolitaireGame CreateGame()
		{
			throw new NotImplementedException();
		}
	}
}
