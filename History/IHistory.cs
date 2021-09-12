namespace PlayingCards.History
{
    /// <summary>
    /// Interface for a history queue used to track moves during a game.
    /// </summary>
    /// <typeparam name="T">A type implementing <see cref="IMove"/>.</typeparam>
	public interface IHistory<T> where T : IMove
	{
        /// <summary>
        /// Readonly property. Indicates whether there is a action/move to undo.
        /// </summary>
        public bool CanUndo { get; }

        /// <summary>
        /// Readonly property. Indicates whether there is a action/move that can be executed again.
        /// </summary>
        public bool CanRedo { get; }

		/// <summary>
		/// Execute and add <paramref name="move"/> to the stack. Override all actions that had been undid.
		/// </summary>
		/// <param name="move">A <see cref="IMove"/> implemented object.</param>
		public void Execute(T move);

        /// <summary>
        /// Undo the most recent action/move.
        /// </summary>
        public void Undo();

        /// <summary>
        /// Execute the oldest undid action/move.
        /// </summary>
        public void Redo();

        public void FullClear();
        
	}
}
