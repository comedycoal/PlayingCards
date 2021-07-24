using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PlayingCards.Extensions
{
    /// <summary>
    /// Extension class for System.Collections.Generic.List
    /// </summary>
    public static partial class ListExtensions
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
    }
}