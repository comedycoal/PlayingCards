using System.Collections.Generic;

namespace PlayingCards.History
{
    /// <summary>
    /// An <see cref="IHistory{T}"/> implementation to record scored moves.
    /// </summary>
	public class ScoredHistory : IHistory<ScoredMove>
    {
        private List<ScoredMove> m_historyQueue;
        private int m_historyPointerIndex;
        private int m_maxUndo;

        /// <summary>
        /// Constructs a <see cref="ScoredHistory"/> instance.
        /// </summary>
        /// <param name="maxUndo">Maximum number of undos allows in the queue.</param>
        public ScoredHistory(int maxUndo= int.MaxValue)
        {
            m_historyQueue = new List<ScoredMove>();
            m_historyPointerIndex = -1;
            m_maxUndo = maxUndo;
        }

        /// <inheritdoc cref="IHistory{T}.CanUndo"/>
        public bool CanUndo => m_historyQueue.Count - m_historyPointerIndex + 1 <= m_maxUndo;

        /// <inheritdoc cref="IHistory{T}.CanRedo"/>
        public bool CanRedo => m_historyPointerIndex < m_historyQueue.Count - 1;

        /// <inheritdoc cref="IHistory{T}.Execute"/>
        public void Execute(ScoredMove move)
        {
            if (CanRedo)
                m_historyQueue.RemoveRange(m_historyPointerIndex + 1, m_historyQueue.Count - m_historyPointerIndex - 1);
            if (move != null)
                move.Execute();
                m_historyQueue.Add(move);
                ++m_historyPointerIndex;
        }

        /// <inheritdoc cref="IHistory{T}.Undo"/>
        public void Undo()
        {
            if (!CanUndo)
                return;
            m_historyQueue[m_historyPointerIndex].Undo();
            --m_historyPointerIndex;
        }

        /// <inheritdoc cref="IHistory{T}.Redo"/>
        public void Redo()
        {
            if (!CanRedo)
                return;
            m_historyQueue[m_historyPointerIndex].Execute();
            ++m_historyPointerIndex;
        }
    }
}
