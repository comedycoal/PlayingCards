using System;
using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.Component;
using PlayingCards.Extensions;

namespace PlayingCards.History.Solitaire
{

	/// <summary>
	/// A <see cref="IMove"/> that execute a transfer. Each counts toward score.
	/// </summary>
	public class TransferMove : IMove 
	{
		/// <summary>
		/// Destination container.
		/// </summary>
		protected IPile<Card> m_dest;

		/// <summary>
		/// Transfer data.
		/// </summary>
		protected TransferData<Card> m_transferData;

		/// <summary>
		/// Execution state.
		/// </summary>
		protected bool m_executed;


		/// <summary>
		/// Constructs a Transfer move consisting or a source and a destination, a move index and an optional
		/// aditional move to execute along.
		/// </summary>
		/// <param name="dest">The destination container.</param>
		/// <param name="transferData"></param>

		public TransferMove(IPile<Card> dest, TransferData<Card> transferData)
		{
			m_dest = dest;
			m_transferData = transferData;
			m_executed = false;
		}


		///<inheritdoc cref="IMove.Execute"/>
		public virtual void Execute()
		{
			var list = m_transferData.Source.Extract(m_transferData.Count);
			m_dest.Add(list);
			foreach (var item in m_transferData.Actions) item?.Execute();
			m_executed = true;
		}

		///<inheritdoc cref="IMove.Undo"/>
		public virtual void Undo()
		{
			m_executed = false;
			foreach (var item in m_transferData.Actions.GetReversedEnumerator()) item?.Undo();
			List<Card> list = m_dest.Extract(m_transferData.Count);
			m_transferData.Source.Add(list);
		}
	}
}