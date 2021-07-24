namespace PlayingCards.History
{
	/// <summary>
	/// Interface for a reversible command/action.
	/// </summary>
	/// <remarks>
	/// The name "Move" is inspired by Solitaire Move statistics.
	/// </remarks>
	public interface IMove
	{
		/// <summary>
		/// Execute the command/action.
		/// </summary>
		public void Execute();

		/// <summary>
		/// Reverse/Undo the command/action.
		/// </summary>
		public void Undo();
	}

}