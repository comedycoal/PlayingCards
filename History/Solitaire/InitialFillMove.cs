using System;
using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.Utilities;
using PlayingCards.Component;
using PlayingCards.Component.Solitaire;

namespace PlayingCards.History.Solitaire
{
    /// <summary>
    /// Class encapsulate the "filling" card-dealing action.
    /// </summary>
    public class InitialFillMove : IMove
    {
		private List<IMove> m_moves;

		/// <summary>
		/// Constructs a <see cref="InitialFillMove"/>. Executing this action fills <paramref name="receivers"/> to its <see cref="SolitairePile.InitialCount"/> 
		/// </summary>
		/// <param name="src">The source pile.</param>
		/// <param name="receivers">List of destination piles and their respective <see cref="SolitairePile.InitialCount"/>.</param>
		public InitialFillMove(IPile<Card> src, List<Tuple<IPile<Card>, int>> receivers) : base()
        {
			m_moves = new List<IMove>();
			MakeMoves(src, receivers);
		}

		private void MakeMoves(IPile<Card> src, List<Tuple<IPile<Card>, int>> receivers)
		{
			foreach (var tuple in receivers)
			{
				var transferData = src.Peek(tuple.Item2);
				var transfer = new ReversedOrderMove(tuple.Item1, transferData);
				m_moves.Add(transfer);
			}
		}

        /// <inheritdoc cref="IMove.Execute"/>
        public virtual void Execute()
        {
			foreach (var item in m_moves)
				item.Execute();
        }

        /// <inheritdoc cref="IMove.Undo"/>
        /// <remarks>Normal gameplay generally avoids undoing this move.</remarks>
        public virtual void Undo()
        {
			foreach (var item in m_moves.GetReversedEnumerator())
				item.Undo();
        }
    }
}
