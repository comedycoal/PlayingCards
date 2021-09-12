using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.History;
using PlayingCards.Game;

namespace PlayingCards.Component.Solitaire
{
	/// <summary>
	/// Defines methods for cards dealing at game start and throughout gameplay.
	/// </summary>
	/// <remarks>
	/// The stock usually houses the entire playing deck at the start of the game, though not a <see cref="Deck"/> object itself
	/// (which is manages by a <see cref="IGame"/> implementations).
	/// </remarks>
	public interface IDealer
	{
		/// <summary>
		/// Receives a list of cards as its "initial" state.
		/// </summary>
		/// <param name="cards">A <see cref="List&lt;Card&gt;"/> object. Should be the return of a <see cref="Deck"/> object's methods.</param>
		public void ReceiveDeck(List<Card> cards);
		
		/// <summary>
		/// Craete an <see cref="IMove"/> that deals card to its associated pile(s).
		/// </summary>
		/// <param name="destinations">Destination piles.</param>
		/// <returns>An <see cref="IMove"/> implementation.</returns>
		public IMove CreateInitialDeal(IEnumerable<SolitairePile> destinations);

		public IMove CreateDealMove();
	}
}
