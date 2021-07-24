using System.Collections.Generic;

using PlayingCards.Primitives;
using PlayingCards.History;
using PlayingCards.Component.Solitaire;

namespace PlayingCards.Component
{
	/// <summary>
	/// Structs encapsulate data necessary to perform a transfer (via a <see cref="TransferData{T}"/> instance.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct TransferData<T>
	{
		/// <summary>
		/// Source of the transfer.
		/// </summary>
		public IPile<T> Source;

		/// <summary>
		/// Leading element of the transfer.
		/// </summary>
		public T First;

		/// <summary>
		/// Length of the transfer.
		/// </summary>
		public int Count;

		/// <summary>
		/// Nested moves to perform along-side (strictly speaking, right after) while still counts as one move. 
		/// </summary>
		public List<IMove> Actions;

		/// <summary>
		/// Constant dictates an invalid transfer data object.
		/// </summary>
		public static readonly TransferData<T> Null;

		static TransferData()
		{
			Null = new TransferData<T>(null, default, 0, default(IMove));
		}

		/// <summary>
		/// Constructs a <see cref="TransferData{T}"/> object.
		/// </summary>
		/// <param name="src">Source of the transfer.</param>
		/// <param name="first">Leading element of the transfer.</param>
		/// <param name="count">Length of the transfer.</param>
		/// <param name="moves">Nested moves to perform along-side (strictly speaking, right after) while still counts as one move. </param>
		public TransferData(IPile<T> src, T first, int count, List<IMove> moves)
		{
			Source = src;
			First = first;
			Count = count;
			Actions = moves ?? new List<IMove>();
		}

		/// <summary>
		/// Constructs a <see cref="TransferData{T}"/> object.
		/// </summary>
		/// <param name="src">Source of the transfer.</param>
		/// <param name="first">Leading element of the transfer.</param>
		/// <param name="count">Length of the transfer.</param>
		/// <param name="move">Nested move to perform along-side (strictly speaking, right after).</param>
		public TransferData(IPile<T> src, T first, int count, IMove move) : this(src, first, count, move == null ? new List<IMove>() : new List<IMove> { move }) { }

		/// <summary>
		/// Add action to execute alongside the transfer.
		/// </summary>
		/// <param name="move">An <see cref="IMove"/> instance.</param>
		public void AddAction(IMove move)
		{
			if (Actions is null)
				Actions = new List<IMove>();
			Actions.Add(move);
		}
	}

}