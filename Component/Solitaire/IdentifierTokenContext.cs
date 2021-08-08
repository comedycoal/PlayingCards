using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingCards.Component.Solitaire
{

	/// <summary>
	/// Class represents the context for enumeration of individual piles.
	/// </summary>
	/// <remarks>
	///	Enumeration for each pile type is one-indexed. Each pile type is regconized by a string.
	/// </remarks>
	public class IdentifierTokenContext
	{
		private SortedDictionary<string, int> m_counter;

		/// <summary>
		/// Constructs a new context, with all counters at zeros.
		/// </summary>
		public IdentifierTokenContext()
		{
			m_counter = new SortedDictionary<string, int>();
		}

		/// <summary>
		/// Adding a pile of type identified by <paramref name="name"/> 
		/// </summary>
		/// <remarks>
		/// Each call to <see cref="Add"/> increases the internal counter for the identifying string by one.
		/// </remarks>
		/// <param name="name">a pile identifier string.</param>
		/// <returns>The corresponding id for the newly added pile.</returns>
		public int Add(string name)
		{
			int value;
			var gotten = m_counter.TryGetValue(name, out value);
			if (!gotten)
				m_counter[name] = 0;

			m_counter[name] = m_counter[name] + 1;
			return value + 1;
		}

		/// <summary>
		/// Returns a suggested name for a pile identifying as <paramref name="name"/> with id <paramref name="id"/>
		/// </summary>
		/// <param name="name">Identifying string.</param>
		/// <param name="id">Id.</param>
		/// <returns><see langword="null"/> if no records are found, otherwise a <see cref="string"/> instance.</returns>
		public string GetSuggestedName(string name, int id)
		{
			int value;
			if (!m_counter.TryGetValue(name, out value) || m_counter[name] < id)
				return null;
			return name + (value > 1 ? value.ToString() : "");
		}
	}
}
