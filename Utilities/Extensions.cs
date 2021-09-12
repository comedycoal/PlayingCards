using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PlayingCards.Utilities
{
    /// <summary>
    /// Extension class for System.Collections.Generic.List
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Retrieves a reversed Enumerator for object implemented IExentedList interface
        /// </summary>
        /// <typeparam name="T">type of list elements</typeparam>
        /// <param name="list">IExentedList object</param>
        /// <returns>An IEnumerable for reversed list traversal</returns>
        public static IEnumerable<T> GetReversedEnumerator<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i >= 0; --i)
            {
                yield return list[i];
            }
        }
        
        /// <summary>
		/// Retrieves a <see cref="List&lt;T&gt;"/> instance as a copy of this <see cref="ImmutableList&lt;T&gt;"/> instance
		/// </summary>
		/// <typeparam name="T">Typename</typeparam>
		/// <param name="list">A <see cref="ImmutableList&lt;T&gt;"/> instance</param>
		/// <returns>A <see cref="List&lt;T&gt;"/> instance</returns>
        public static List<T> ToList<T>(this ImmutableList<T> list)
        {
			var a = new List<T>();
			a.AddRange(list);
			return a;
		}

		/// <summary>
		/// Add dictionary entries from <paramref name="other"/> into <paramref name="dict"/>.
		/// </summary>
		/// <remarks>
		/// Entries are added sequentially and duplicated entries are skipped entirely.
		/// <paramref name="other"/> is unchanged after merging.
		/// </remarks>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dict">The <see cref="SortedDictionary{TKey, TValue}"/> receiving the records.</param>
		/// <param name="other">The <see cref="SortedDictionary{TKey, TValue}"/> being merged.</param>
		public static void MergeWith<TKey, TValue>(this SortedDictionary<TKey, TValue> dict, SortedDictionary<TKey, TValue> other)
		{
			foreach(var keyValuePair in other)
			{
				dict.TryAdd(keyValuePair.Key, keyValuePair.Value);
			}
		}
    }
}