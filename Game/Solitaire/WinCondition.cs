namespace PlayingCards.Game.Solitaire
{
	/// <summary>
	/// Enumeration signaling how a SolitaireGame should consider a win.
	/// </summary>
	public enum WinCondition
	{
		/// <summary>
		/// Signals that the game is won when all foundations' <see cref="PlayingCards.Component.Solitaire.IFoundation.Filled"/> are <see langword="true"/>.
		/// </summary>
		FILL_FOUNDATION = 104,

		/// <summary>
		/// Signals that the game is won when the tableau is cleared.
		/// </summary>
		CLEAR_TABLEAU = 200,

		/// <summary>
		/// Singals a custom winning method defined in a inherited class of <see cref="SolitaireGame"/>
		/// </summary>
		CUSTOM = 999
	}
}
