namespace PlayingCards.Component.Solitaire
{
	/// <summary>
	/// Pubic interface providing a meditative set of operations that <see cref="SolitairePile"/> instances
	/// may use to communicate with each others and to infer information unbeknowned to them.
	/// </summary>
	public interface IGameContext
	{
		/// <summary>
		/// Resolves an <see cref="IdentifierToken"/> and returns a reference to it. 
		/// </summary>
		/// <param name="sender">The sender pile.</param>
		/// <param name="token">The token to resolve.</param>
		/// <returns><see langword="null"/> if the resolve fails, else a <see cref="SolitairePile"/> 
		/// reference associated with <paramref name="token"/>.</returns>
		public SolitairePile ResolveAssociation(SolitairePile sender, IdentifierToken token);
	}
}
