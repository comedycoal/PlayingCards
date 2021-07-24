namespace PlayingCards.History
{
	/// <summary>
	/// Class represnting a <see cref="IMove"/> by wrapping one <see langword="delegate"/>
	/// as execution method and another as undo method.
	/// </summary>
	/// <remarks>
	/// The delegates wrapped should be and <see cref="System.Action"/> as no returns value should be expected.
	/// </remarks>
	public class GenericMove : IMove
	{
		private System.Delegate m_executeMethod;
		private System.Delegate m_undoMethod;
		private object[] m_args;

		///<inheritdoc cref="IMove.Execute"/>
		public void Execute()
		{
			m_executeMethod.DynamicInvoke(m_args);
		}

		///<inheritdoc cref="IMove.Undo"/>
		public void Undo()
		{
			m_undoMethod.DynamicInvoke(m_args);
		}

		/// <summary>
		/// Constructs a <see cref="GenericMove"/>. Both executing and reversed delegate should takes identical arguments <paramref name="args"/>.
		/// </summary>
		/// <param name="execute">Executing delegate, should be an <see cref="System.Action"/>.</param>
		/// <param name="undo">Reversed delegate of <paramref name="execute"/>, should be an <see cref="System.Action"/>.</param>
		/// <param name="args">Arguments for both delegates</param>
		public GenericMove(System.Delegate execute, System.Delegate undo, params object[] args)
		{
			m_executeMethod = execute;
			m_undoMethod = undo;
			m_args = args;
		}
	}

}