using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.Component;
using PlayingCards.Component.Solitaire;

namespace PlayingCards.History.Solitaire
{
    /// <summary>
    /// Class encapsulate the "distributive" card-dealing action.
    /// </summary>
    public class DistributeMove : IMove
    {
        private List<IMove> m_moves;

        /// <summary>
        /// Constructs a <see cref="DistributeMove"/> from <paramref name="src"/> to <paramref name="receivers"/>, each with <paramref name="rounds"/> cards.
        /// </summary>
        /// <param name="src">The source pile.</param>
        /// <param name="receivers">A list of receivers.</param>
        /// <param name="rounds">the number of cards each pile receives.</param>
        public DistributeMove(IPile<Card> src, List<IPile<Card>> receivers, int rounds = 1) : base()
        {
			m_moves = new List<IMove>();
			MakeMoves(src, receivers, rounds);
		}

		private void MakeMoves(IPile<Card> src, List<IPile<Card>> receivers, int rounds)
		{
			for (int round = 0; round < rounds; ++rounds)
				foreach (var pile in receivers)
				{
					var data = pile.Peek(1);
					var transfer = new TransferMove(pile, data);
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
        public virtual void Undo()
        {
			foreach (var item in m_moves)
				item.Undo();
		}
    }
}
