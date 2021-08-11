using System;
using System.Collections.Generic;
using System.Text;

using PlayingCards.Primitives;

namespace PlayingCards.Game.Solitaire
{
	/// <summary>
	/// Structs holding the metadata information of a solitaire game.
	/// </summary>
	/// <remarks>
	/// The information included is:
	/// <list type="bullet">
	/// <item>The name of the game.</item>
	/// <item>Description of the game: Origins, trivia, etc.</item>
	/// <item>A description of the deck used: A full deck, 2 decks, Spades only deck, etc.</item>
	/// <item>The goal of the game: Clearing the tableau, complete the foundations, etc.</item>
	/// </list>
	/// </remarks>
	public struct Metadata
	{
		/// <summary>
		/// The name of the game.
		/// </summary>
		public string Name;

		/// <summary>
		/// The string description of the game.
		/// </summary>
		public string Description;

		/// <summary>
		/// The description of the deck used.
		/// </summary>
		public DeckDescription GameDeck;

		/// <summary>
		/// The winning condition of the game.
		/// </summary>
		public WinCondition GameWinCondition;

		/// <summary>
		/// Gives a summary of the metadata.
		/// </summary>
		/// <returns>A string holding necessary information.</returns>
		public string Summary()
		{
			throw new NotImplementedException();
		}
	}
}

