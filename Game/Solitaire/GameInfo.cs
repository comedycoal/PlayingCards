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
	/// Class providing metatdata and layout description of a solitaire game,
	/// without instatiating any piles and other statistics.
	/// </summary>
	/// <remarks>
	/// <see cref="GameInfo"/> provides a data-only view of a solitaire game,
	/// no real references are made and can instantiate identical <see cref="SolitaireGame"/>s at will.
	/// </remarks>
	public class GameInfo
	{
		private Metadata m_metadata;
		private List<PileInfo> m_piles;

		/// <summary>
		/// Constructs a <see cref="GameInfo"/> instance with metadata and layout descriptions.
		/// </summary>
		/// <param name="metadata">The metadata description of the game.</param>
		/// <param name="piles">The layout description of the game, in form of a list of <see cref="PileInfo"/>.</param>
		public GameInfo(Metadata metadata, List<PileInfo> piles)
		{
			m_metadata = metadata;
			m_piles = piles;
		}

		/// <summary>
		/// Instantiate a <see cref="SolitaireGame"/> instance.
		/// </summary>
		/// <returns>A <see cref="SolitaireGame"/> instance based on the game description.</returns>
		public SolitaireGame CreateGame()
		{
			throw new NotImplementedException();
		}
	}
}
