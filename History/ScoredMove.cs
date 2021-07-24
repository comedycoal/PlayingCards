namespace PlayingCards.History
{
	/// <summary>
	/// Wrapper to add a score to a <see cref="IMove"/>
	/// </summary>
	/// <remarks>
	/// Inherited from <see cref="IMove"/>, <see cref="ScoredMove"/> can be used as type parameter for <see cref="IHistory{T}"/>
	/// </remarks>
	public class ScoredMove : IMove
	{
		private int m_score;
		private IMove m_move;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="move"></param>
		/// <param name="score"></param>
		public ScoredMove(IMove move, int score=0)
		{
			m_move = move;
			m_score = score;
		}


		/// <summary>
		/// Readonly property. Retrieves the score associated with the move.
		/// </summary>
		public int Score => m_score;

		///<inheritdoc cref="IMove.Execute()"/>
		public void Execute()
		{
			m_move?.Execute();
		}

		///<inheritdoc cref="IMove.Undo()"/>
		public void Undo()
		{
			m_move?.Undo();
		}
	}
}