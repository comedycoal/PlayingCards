using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.Component;
using PlayingCards.Component.Solitaire;

namespace PlayingCards.History.Solitaire
{
	/// <summary>
	/// <see cref="TransferMove"/> that reverse the order before moving to destination.
	/// </summary>
	public class ReversedOrderMove : TransferMove
    {
        /// <summary>
        /// Constructs a <see cref="ReversedOrderMove"/>.
        /// </summary>
        /// <inheritdoc cref="TransferMove.TransferMove"/>
        public ReversedOrderMove(IPile<Card> dest, TransferData<Card> transferData) : base(dest, transferData) { }

        /// <inheritdoc cref="TransferMove.Execute"/>
        public override void Execute()
        {
            List<Card> list = m_transferData.Source.Extract(m_transferData.Count);
            foreach (var item in m_transferData.Actions) item?.Execute();
            list.Reverse();
            m_dest.Add(list);
            m_executed = true;
        }

        ///<inheritdoc cref="TransferMove.Undo"/>
        public override void Undo()
        {
            List<Card> list = m_dest.Extract(m_transferData.Count);
            foreach (var item in m_transferData.Actions) item?.Undo();
            list.Reverse();
            m_transferData.Source.Add(list);
            m_executed = false;
        }
    }
}