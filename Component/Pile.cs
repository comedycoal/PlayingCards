using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using PlayingCards.History;
using PlayingCards.Component;

namespace PlayingCards.Component
{
	/// <summary>
	/// Represents a Stack-like data structure with array-like indexing.
	/// </summary>
	/// <typeparam name="T">Specifies the type of elements in the pile.</typeparam>
	public class Pile<T> : IEnumerable<T>, IReadOnlyCollection<T>, ICollection<T>, IPile<T>
	{
		private List<T> m_list;

		/// <summary>
		/// Constructs an empty <see cref="Pile{T}"/> container object.
		/// </summary>
		public Pile()
		{
			m_list = new List<T>();
		}

		/// <summary>
		/// Constructs a <see cref="Pile{T}"/> container object and initialize with elements from <paramref name="enumerable"/>.
		/// </summary>
		/// <param name="enumerable">A <see cref="IEnumerable{T}"/></param>
		public Pile(IEnumerable<T> enumerable) : this()
		{
			m_list.AddRange(enumerable);
		}


		/// <inheritdoc cref="IReadOnlyCollection{T}.Count"/>
		public int Count => m_list.Count;

		/// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
		public bool IsReadOnly => ((ICollection<T>)m_list).IsReadOnly;

		/// <inheritdoc cref="ICollection{T}.Add"/>
		public void Add(T item) => m_list.Add(item);

		/// <inheritdoc cref="ICollection{T}.Clear"/>
		public void Clear() => m_list.Clear();

		/// <inheritdoc cref="ICollection{T}.Contains"/>
		public bool Contains(T item) => m_list.Contains(item);

		/// <inheritdoc cref="ICollection{T}.CopyTo"/>
		public void CopyTo(T[] array, int arrayIndex) => m_list.CopyTo(array, arrayIndex);

		/// <inheritdoc cref="ICollection{T}.Remove"/>
		public bool Remove(T item) => m_list.Remove(item);

		/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
		public IEnumerator<T> GetEnumerator() => m_list.GetEnumerator();

		/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
		IEnumerator IEnumerable.GetEnumerator() => m_list.GetEnumerator();

		/// <inheritdoc cref="IPile{T}.Peek"/>
		public TransferData<T> Peek(int count)
		{
			return new TransferData<T>(this, m_list[Count - count], count, default(IMove));
		}

		/// <inheritdoc cref="IPile{T}.Extract"/>
		public List<T> Extract(int count)
		{
			List<T> a = m_list.GetRange(Count - count, count);
			m_list.RemoveRange(Count - count, count);
			return a;
		}

		/// <inheritdoc cref="IPile{T}.Add"/>
		public void Add(List<T> items)
		{
			m_list.AddRange(items);
		}

		/// <inheritdoc cref="List{T}.this[int]" path="//*[not(or(self::summary,self::param))]"/>
		/// <summary>
		/// (Access only) Get element at <paramref name="index"/>.
		/// </summary>
		/// <param name="index">Zero based index to get.</param>
		/// <returns><typeparamref name="T"/> object at <paramref name="index"/>.</returns>
		public T this[int index]
		{
			get { return m_list[index]; }
		}
	}
}
