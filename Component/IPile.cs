using System.Collections.Generic;

namespace PlayingCards.Component
{
	/// <summary>
	/// Generic interface for a array/list-like container that can performs transfers 
	/// (from one container to another) in form of <see cref="List&lt;T&gt;"/> instances.
	/// </summary>
	/// <remarks>
	/// Such a container satisfies the following:
	/// <list type="bullet">
	/// <item>Its content can be extracted and added from only one direction, like a stack.</item>
	/// <item>Information about its size can be inferred.</item>
	/// <item>Provides a method to query if a subset of its content can be extracted.</item>
	/// <item>Provides a method to query if a specific content instance can be added.</item>
	/// <item>Whether a set of content instances can be added depending only on the first instance of the set and its size.</item>
	/// </list>
	/// </remarks>
	/// <typeparam name="T">Type of the container's content</typeparam>
	public interface IPile<T>
	{
		
		/// <summary>
		/// Readonly property. Retrieves the size of the container.
		/// </summary>
		public int Count { get; }

		/// <summary>
		/// Retrieves information necessary for a transfer starting at <paramref name="index"/>.
		/// </summary>
		/// <remarks>
		/// A transfer instance
		/// </remarks>
		/// <param name="index">index from which the transfer will perform.</param>
		/// <returns>A <see cref="TransferData{T}"/> instance</returns>
		public TransferData<T> Peek(int index);

		/// <summary>
		/// "Pop" a list of <paramref name="count"/> last items.
		/// </summary>
		/// <param name="count"></param>
		/// <returns>A <see cref="List&lt;T&gt;"/> instance containing extracted content.</returns>
		public List<T> Extract(int count);

		/// <summary>
		/// Add a list of elements, signifying the end of the transfer. No checks are performed.
		/// </summary>
		/// <param name="list">The transfer instance</param>
		public void Add(List<T> list);
	}
}
